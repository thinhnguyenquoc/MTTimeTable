#include "stdafx.h"
#include"Solution.h"

void clsSolution::InitData(int i_Jury, int i_IntLect, int i_ExtLect, int Y, clsLecturer arr_Lecturer[MAXLECT], int i_ThreNumOfProj)
{
	this->iJury=i_Jury;
	this->iIntLect=i_IntLect;
	this->iExtLect=i_ExtLect;
	this->Y=Y;
	this->SetLectures(arr_Lecturer);
	this->iThreNumOfProj=i_ThreNumOfProj;

	//this->numUnFesibleJury=0;
	//this->vcrUnFesibleJury.clear();

//	this->InitData();//Initialize data
}
//Set list of lecturers
void clsSolution::SetLectures(clsLecturer arrLects[MAXLECT])
{
	for(int i=0;i<iIntLect + iExtLect;i++)
		arrLecturer[i]=arrLects[i];
}
void clsSolution::InitData()
{
	for(int i=0;i<iIntLect+iExtLect;i++)
		arrNumProjofLect[i]=0;
	for(int i=0;i<iJury;i++)
	{
		arrJury[i].IdJury=i;
		arrJury[i].InitData();
		arrIsChoosePeriod[i]=false;
	}
	iIDRoom=0;
	//Calculate the number of available period of each lecturer
	for(int i=0;i<iIntLect + iExtLect;i++)
	{
		arrThreNumProjOfLect[i]=this->arrLecturer[i].numAvailTime;
	}
	//cout<<endl<<"Number of available period of lecturers:"<<endl;
	//for(int i=0;i<iIntLect + iExtLect;i++)
	//	cout<<arrThreNumProjOfLect[i]<<"   ";

	numRoom=0; //number of room used
	numSession=0; //number of sesion of all lecturer
	exSession=0; //number of sessions of external lecturer
	makeSpan=0; //the max of finished time of solution

	//The number of unfesible jury
	this->numUnFesibleJury=0;
	this->vcrUnFesibleJury.clear();

}
//Reset data from executed and fesible solutions
void clsSolution::ResetData()
{
	for(int i=0;i<this->iIntLect + this->iExtLect;i++)
		this->arrLecturer[i].ResetData();
	for(int i=0;i<this->iJury;i++)
		this->arrJury[i].ResetData();
	numUnFesibleJury=0;//Number of unfesible jury
	vcrUnFesibleJury.clear();//list of unfesible jury in solution

	for(int i=0;i<iJury;i++)
		arrIsChoosePeriod[i]=false;

	iIDRoom=0;
	numRoom=0; //number of room used
	numSession=0; //number of sesion of all lecturer
	exSession=0;
	makeSpan=0; //the max of finished time of solution
}
//Start to schedule
bool clsSolution::DoScheduling()
{
	if(ChooseLecturerForJuries() == false)
		return false;
	if(ChoosePeriodForJuries() == false)
		return false;
	ChooseRoomForJuries();
	UpdateFinalState();
	CalculateObjectiveValue();
	return true;
}
//Print current solution
void clsSolution::PrintSolution()
{
	//Number of sessions that lecturer joined
	//cout<<endl<<"Number of sessions that lecturer joined:"<<endl;
	//for(int i=0;i<iIntLect + iExtLect;i++)
	//	cout<<arrLecturer[i].iSessionJoined<<"  ";
	cout<<"test";
}
//Write the solution result to file
void clsSolution::WriteFileSolutionResult(std::string sOutputFileName, string sLectFileName)
{
	ofstream myFile(sOutputFileName);
	if (!myFile)
		cout<<"cannot open output file";
	myFile<<iIntLect<<endl;
	myFile<<iExtLect<<endl;
	myFile<<iJury<<endl;
	myFile<<Y<<endl;
	for (int k=0;k<iIntLect+iExtLect;k++)//each lecturer
	{
		myFile<<arrLecturer[k].IdLect<<endl;//Id is index of the lecturers array
		myFile<<arrLecturer[k].IsExternalLect<<endl; //is external lecturer or not
		for (int h=0; h<iJury;h++)
			myFile<<"\t"<<arrLecturer[k].arrSpecialize[h];//role for each jury
		myFile<<endl;
		for (int h=0;h<Y;h++)
			myFile<<"\t"<<arrLecturer[k].arrAvailTime[h];//availability time of lecturer,=0: free; =1: unavailability, =2: unavailability because have joined a jury
		myFile<<endl;
		myFile<<arrLecturer[k].numAvailTime<<endl;//Number of availability time of lecturer
		myFile<<arrLecturer[k].Y<<endl;
		myFile<<arrLecturer[k].iSessionJoined<<endl;
	}
	for (int k=0;k<iJury;k++)//each jury
	{
		myFile<<arrJury[k].IdJury<<endl;//is the index of Jury array
		myFile<<"\t"<<arrJury[k].arrOtherMembers[2].IdLect<<"\t"<<arrJury[k].arrReviewers[0].IdLect<<"\t"<<arrJury[k].arrReviewers[1].IdLect<<"\t"<<arrJury[k].arrOtherMembers[0].IdLect<<"\t"<<arrJury[k].arrOtherMembers[1].IdLect<<endl;//list of members
		myFile<<arrJury[k].iNumExtLect<<endl; //number of external lecturers
		myFile<<arrJury[k].IdRoom<<endl;//Id of room for jury
		myFile<<arrJury[k].IdPeriod<<endl; //Id of period for jury
		for (int h=0;h<Y;h++)//available time
			myFile<<"\t"<<arrJury[k].arrAvailPeriod[h];
		myFile<<endl;
	}
	myFile<<iThreNumOfProj<<endl;//maximum number of jury that each lecturer could join.
	for (int h=0;h<iIntLect+iExtLect;h++)
		myFile<<"\t"<<arrNumProjofLect[h];//number of jury that each lecturer joined.
	myFile<<endl;
	for (int h=0;h<iIntLect+iExtLect;h++)
		myFile<<"\t"<<arrThreNumProjOfLect[h];//The maximum number of jury that each lecturer can be joined
	myFile<<endl;
	myFile<<numUnFesibleJury<<endl;//Number of unfesible jury
	for (int h=0;h<vcrUnFesibleJury.size();h++)
		myFile<<"\t"<<vcrUnFesibleJury[h];//list of unfesible jury in solution
	myFile<<endl;
	//additional variables
	for (int h=0; h<iJury;h++)
		myFile<<"\t"<<arrIsChoosePeriod[h];//array of flags to show whether period of jury is chosen or not.
	myFile<<endl;
	myFile<<iIDRoom<<endl; //this template variable is used to set Room for jury
	myFile<<numRoom<<endl;//number of room used
	myFile<<numSession<<endl; //number of sesion of all lecturer
	myFile<<makeSpan<<endl; //the max of finished time of solution
	myFile<<missingTimeslot<<endl;
	myFile.close();
}
void clsSolution::WriteProfSchedule(std::string sLectFileName)
{
	ofstream myFile(sLectFileName);
	if(!myFile)
		cout<<"Cannot open output file ";
	for (int k=0;k<iIntLect+iExtLect;k++)
	{
		myFile<<"Lecturer "<<k+1<<endl;
		vector<int> ListOfProfJoining= FindJuriesProfIsIn(k);
		int count=ListOfProfJoining.size();
		myFile<<"Jury";
		for (int i=0;i<count;i++)
			myFile<<"\t"<<ListOfProfJoining[i]+1;
		myFile<<endl;
		myFile<<"Role";
		for (int i=0;i<count;i++)
			myFile<<"\t"<<arrJury[ListOfProfJoining[i]].GetRoleOfLecturerInJury(k);
		myFile<<endl;
		myFile<<"Time";
		for (int i=0;i<count;i++)
			myFile<<"\t"<<arrJury[ListOfProfJoining[i]].IdPeriod;
		myFile<<endl;
		myFile<<"Room";
		for (int i=0;i<count;i++)
			myFile<<"\t"<<arrJury[ListOfProfJoining[i]].IdRoom;
		myFile<<endl;
		myFile<<endl;
	}
}
/*void clsSolution::WriteProfSchedule(std::string sOutputFileName)
{
	ofstream myFile(sOutputFileName);
	if(!myFile)
		cout<<"Cannot open output file ";
	for (int k=0;k<iIntLect+iExtLect;k++)
	{
		myFile<<"Lecturer "<<k+1<<endl;
		vector<int> ListOfProfJoining= FindJuriesProfIsIn(k);

		for(int i=0;i<iJury;i++)
			myFile<<"\t"<<"J"<<i+1;
		myFile<<endl;
		int arrOutput[MAXPERIOD][MAXJURY];
		for(int i=0;i<iJury;i++)
		for(int j=0;j<Y;j++)
		{
			if (arrJury[i].IdPeriod==j)
			{
				arrOutput[j][i]=arrJury[i].GetRoleOfLecturerInJury(k);
			}
			else
				arrOutput[j][i]=0;
		}
		for(int i=0;i<Y;i++)
	{
		myFile<<"Time"<<std::to_string((long long)(i));
		for(int j=0;j<iJury;j++)
			myFile<<"\t"<<arrOutput[i][j];
		myFile<<endl;

	}
		myFile<<endl;
	}
}*/
//Task 1: Choose lecturers for juries
bool clsSolution::ChooseLecturerForJuries()
{
	//Step 1: Choose advisor for juries
	if(ChooseAdvisor() == false)
		return false;
	//Step 4: Choose secretary for juries
	if(ChooseSecretaries() == false)
		return false;
	
	//Step 3: Choose reviewers for juries
	if(ChooseReviewers() == false)
		return false;
	//Step 2: Choose chair for juries
	if(ChooseChairs() == false)
		return false;
	if (ChooseNormalMembers()==false)
		return false;
	//result of Task 1
	//cout<<endl<<endl<<"Choose lecturers for juries:"<<endl;
	//for(int i=0;i<iJury;i++)
	//{
	//	cout<<arrJury[i].advisor.IdLect+1<<"  ";
	//	cout<<arrJury[i].arrReviewers[0].IdLect+1<<"  ";
	//	cout<<arrJury[i].arrReviewers[1].IdLect+1<<"  ";
	//	cout<<arrJury[i].arrOtherMembers[0].IdLect+1<<"  ";
	//	cout<<arrJury[i].arrOtherMembers[1].IdLect+1;
	//	cout<<endl;
	//}


	return true;
}
//Step 1-task 1: Choose advisor for juries
bool clsSolution::ChooseAdvisor()
{
	for(int i=0;i<iJury;i++)
	{
		int j;
		for(j=0;j<iIntLect+iExtLect;j++)
			if(arrLecturer[j].arrSpecialize[i] == 1)// =1: advisor
				break;
		if(j == iIntLect + iExtLect)
		{
			cout<<endl<<"Error: There is not advisor for jury: "<<i+1;
			return false;
		}
		else
		{
			arrJury[i].advisor[0]=arrLecturer[j];			
//			arrNumProjofLect[j]++;//increase the number of jury for lecture j
//			arrThreNumProjOfLect[j]--;//decrease the maximum number of jury lecturer could be joined
//			if(arrLecturer[j].IsExternalLect == true)
//				arrJury[i].iNumExtLect++;
		}
	}
	for(int i=0;i<iJury;i++)
	{
		int j;
		for(j=0;j<iIntLect+iExtLect;j++)
			if(arrLecturer[j].arrSpecialize[i] == 2)// =2: co advisor
				break;
		if(j == iIntLect + iExtLect)//no co advisor
		{
			clsLecturer nu;
			nu.IdLect=-1;
			arrJury[i].advisor[1]=nu;			
		}
		else
		{
			arrJury[i].advisor[1]=arrLecturer[j];			
//			arrNumProjofLect[j]++;//increase the number of jury for lecture j
//			arrThreNumProjOfLect[j]--;//decrease the maximum number of jury lecturer could be joined
//			if(arrLecturer[j].IsExternalLect == true)
//				arrJury[i].iNumExtLect++;
		}
	}
	return true;
}
//Step 2-task 1: Choose reviewers for juries
bool clsSolution::ChooseReviewers()
{
	//Choose fisrt reviewer
	for(int i=0;i<iJury;i++)
	{
		int thresholdNum=iThreNumOfProj;
		int IdFirstRevi, numRevi;
		numRevi=ChooseReviewerForOneJury_1(i,thresholdNum,IdFirstRevi);
		while(numRevi !=1 && thresholdNum < iJury)
		{
			thresholdNum++; //increase the threshold number of jury per lecturer
			numRevi=ChooseReviewerForOneJury_1(i,thresholdNum,IdFirstRevi);
		}
		if(numRevi !=1)
		{
			cout<<endl<<"Error: There is not enough candidate reviewers for jury "<<i+1;
			return false;
		}
		else
		{
			arrJury[i].arrReviewers[0]=arrLecturer[IdFirstRevi];
			arrNumProjofLect[IdFirstRevi]++;
			arrThreNumProjOfLect[IdFirstRevi]--;//decrease the maximum number of jury lecturer could be joined
			if(arrLecturer[IdFirstRevi].IsExternalLect == true)
				arrJury[i].iNumExtLect++;
		}
	}
	//Choose second reviewer
	for(int i=0;i<iJury;i++)
	{
		int thresholdNum=iThreNumOfProj;
		int IdSecondRevi, numRevi;
		int iCase=this->arrJury[i].iNumExtLect;	//Number of external lecturer
		numRevi=ChooseReviewerForOneJury_2(i,thresholdNum,IdSecondRevi, 1);//iCase=1
		while(numRevi !=1 && thresholdNum < iJury)
		{
			thresholdNum++; //increase the threshold number of jury per lecturer
			numRevi=ChooseReviewerForOneJury_2(i,thresholdNum,IdSecondRevi, 1);//iCase=1
		}
		if(numRevi !=1)
		{
			cout<<endl<<"Error: There is not enough candidate reviewers for jury "<<i+1;
			return false;
		}
		else
		{
			arrJury[i].arrReviewers[1]=arrLecturer[IdSecondRevi];
			arrNumProjofLect[IdSecondRevi]++;
			arrThreNumProjOfLect[IdSecondRevi]--;//decrease the maximum number of jury lecturer could be joined
			if(arrLecturer[IdSecondRevi].IsExternalLect == true)
				arrJury[i].iNumExtLect++;
		}
	}
	return true;
}
//Step 2a - task 1: Choose first reviewer for ONE jury
int clsSolution::ChooseReviewerForOneJury_1(int IDJury, int thresholdNumOfJury, int& firstReviewer)
{
	//find first reviewer
	int k;
	//find the first candidate
	for(k=0;k<iIntLect+iExtLect;k++) //from lecturer 1 to n
		if(arrLecturer[k].arrSpecialize[IDJury] == 3 && 
				arrNumProjofLect[k] < thresholdNumOfJury &&
				this->arrThreNumProjOfLect[k] > 0 &&
				arrJury[IDJury].IsLecturerJoinJury(k) == false)// =3: can be a reviewer, and this lecture do not joins over "threNumOfProj" jury
			break;
	if(k == iIntLect+iExtLect) //could not find any suiltable lecturers
		return 0;
	else
	{
		//find the best candidate: there is the most number of available PERIODS
		int numAvailPeriod=arrThreNumProjOfLect[k];
		int m=k;
		for(int i=k+1;i<iIntLect+iExtLect;i++) //from lecturer k+1 to n
			if(arrLecturer[i].arrSpecialize[IDJury] == 3 && 
				arrNumProjofLect[i] < thresholdNumOfJury &&
				this->arrThreNumProjOfLect[i] >0 &&
				arrJury[IDJury].IsLecturerJoinJury(i) == false &&
				arrThreNumProjOfLect[i] > numAvailPeriod)// =3: can be a reviewer, and this lecture do not joins over "threNumOfProj" jury
			{
				numAvailPeriod = arrThreNumProjOfLect[i];
				m=i;
			}

		firstReviewer=m;
		return 1;
	}
}
//Step 2b: Choose second reviewer for ONE jury, 
//bCase=0: Must choose internal lecturer, 
//bCase=2: Must choose external lecturer, 
//bCase=1: do not need more condition
int clsSolution::ChooseReviewerForOneJury_2(int IDJury, int thresholdNumOfJury, int& secondReviewer, int iCase)
{
	//find second reviewer
	int minIndex, maxIndex;
	switch (iCase)
	{
		case 0:
			minIndex=iIntLect;
			maxIndex=iIntLect+ iExtLect;//Must choose external lecturer
			break;
		case 1:
			minIndex=0;
			maxIndex=iIntLect+ iExtLect;//Choose in all lecturer
			break;
		case 2:
			minIndex=0;
			maxIndex=iIntLect;//Must choose internal lecturer
			break;
	}
	int k;
	//find the first candidate
	for(k=minIndex;k<maxIndex;k++)
		if(arrLecturer[k].arrSpecialize[IDJury] == 3 && 
				arrNumProjofLect[k] < thresholdNumOfJury &&
				this->arrThreNumProjOfLect[k] >0 &&
				arrJury[IDJury].IsLecturerJoinJury(k) == false)// =3: can be a reviewer, and this lecture do not joins over "threNumOfProj" jury
			break;
	if(k == maxIndex) //could not find any suiltable lecturers
		return 0;
	else
	{
		//find the best candidate: there is the most number of available PERIODS
		int numAvailPeriod=arrThreNumProjOfLect[k];
		int m=k;
		for(int i=k+1;i<maxIndex;i++) //from lecturer k+1 to n
			if(arrLecturer[i].arrSpecialize[IDJury] == 3 && 
				arrNumProjofLect[i] < thresholdNumOfJury &&
				this->arrThreNumProjOfLect[i] >0 &&
				arrJury[IDJury].IsLecturerJoinJury(i) == false &&
				arrThreNumProjOfLect[i] > numAvailPeriod)// =3: can be a reviewer, and this lecture do not joins over "threNumOfProj" jury
			{
				numAvailPeriod = arrThreNumProjOfLect[i];
				m=i;
			}
		secondReviewer=m;
		return 1;
	}
}
//Step 2: Choose chair for juries
bool clsSolution::ChooseChairs()
{
	for(int i=0;i<iJury;i++)
	{
		int thresholdNum=iThreNumOfProj;
		int IdChair, numMem;
		numMem=this->ChooseChairForOneJury(i, thresholdNum, IdChair, 0);//iCase =0
		while(numMem != 1 && thresholdNum < iJury)
		{
			thresholdNum++; //increase the threshold number of jury per lecturer
			numMem=this->ChooseChairForOneJury(i, thresholdNum, IdChair, 0);//iCase =0
		}
		if(numMem !=1)//Choose the lecture could be reviewer (iCase =3)
		{
			thresholdNum=iThreNumOfProj;
			numMem=this->ChooseChairForOneJury(i, thresholdNum, IdChair, 3);//iCase =3
			while(numMem != 1 && thresholdNum < iJury)
			{
				thresholdNum++; //increase the threshold number of jury per lecturer
				numMem=this->ChooseChairForOneJury(i, thresholdNum, IdChair, 3);//iCase =3
			}
		}
		//After travelling all cases
		if(numMem != 1)
		{
			cout<<endl<<"Error: There is not candidate who can be chair of jury "<<i+1;
			return false;
		}
		else//add lecturer to jury
		{
			arrJury[i].arrOtherMembers[0]=arrLecturer[IdChair];
			arrNumProjofLect[IdChair]++;
			arrThreNumProjOfLect[IdChair]--;//decrease the maximum number of jury lecturer could be joined
			if(arrLecturer[IdChair].IsExternalLect == true)
				arrJury[i].iNumExtLect++;
		}
	}
	return true;
}
//Step 2a: Choose chair for one jury, 
//iCase =0 (the lecturer can not be reviewer) or iCase = 3
//Must choose internal candidate
int clsSolution::ChooseChairForOneJury(int IDJury, int thresholdNumOfJury, int& chairID, int iCase)
{
	int minIndex, maxIndex;
	switch (iCase)
	{
		case 0:
			minIndex=iIntLect;
			maxIndex=iIntLect+ iExtLect;//Must choose external lecturer
			break;
		default:
			minIndex=0;
			maxIndex=iIntLect+ iExtLect;//Choose in all lecturer
			break;
	}
	int k;
	//find the first candidate
	for(k=minIndex; k<maxIndex;k++)
		if(arrLecturer[k].arrSpecialize[IDJury] == iCase && 
			arrJury[IDJury].IsLecturerJoinJury(k) == false &&
			arrNumProjofLect[k] < thresholdNumOfJury &&
			this->arrThreNumProjOfLect[k] >0)
		break;
	if(k == maxIndex) //could not find any suitable lecturers
		return 0;
	else
	{
		//find the best candidate: there is the most number of available PERIODS
		int numAvailPeriod=arrThreNumProjOfLect[k];
		int m=k;
		for(int i=k+1;i<maxIndex;i++) //from lecturer k+1 to n
			if(arrLecturer[i].arrSpecialize[IDJury] == iCase && 
				arrJury[IDJury].IsLecturerJoinJury(i) == false &&
				arrNumProjofLect[i] < thresholdNumOfJury &&
				this->arrThreNumProjOfLect[i] > 0 &&
				arrThreNumProjOfLect[i] > numAvailPeriod)//and this lecture do not joins over "threNumOfProj" jury
				{
					numAvailPeriod = arrThreNumProjOfLect[i];
					m=i;
				}
		chairID=m;
		return 1;
	}
}
//Step 4: Choose secretary for juries
bool clsSolution::ChooseSecretaries()
{
	for(int i=0;i<iJury;i++)
	{
		int thresholdNum=iThreNumOfProj;
		int IdSecret, numMem;
		int iCase=this->arrJury[i].iNumExtLect;	//Number of external lecturer
		numMem=this->ChooseSecretaryForOneJury(i,thresholdNum,IdSecret, 2);
		while(numMem != 1 && thresholdNum < iJury)
		{
			thresholdNum++; //increase the threshold number of jury per lecturer
			numMem=this->ChooseSecretaryForOneJury(i,thresholdNum,IdSecret, 2);
		}
		if(numMem !=1)
		{
			cout<<endl<<"Error: There is not enough secretary candidate for jury "<<i+1;
			return false;
		}
		else//add lecturer to jury
		{
			arrJury[i].arrOtherMembers[1]=arrLecturer[IdSecret];
			arrNumProjofLect[IdSecret]++;
			arrThreNumProjOfLect[IdSecret]--;//decrease the maximum number of jury lecturer could be joined
			if(arrLecturer[IdSecret].IsExternalLect == true)
				arrJury[i].iNumExtLect++;
		}
	}
	return true;
}
//Step 4a: Choose secretary for ONE jury, iCase =1, or iCase = 2 (number of external lecturers)
int clsSolution::ChooseSecretaryForOneJury(int IDJury, int thresholdNumOfJury, int& secretID, int iCase)
{
	int minIndex, maxIndex;
	switch (iCase)
	{
		case 1:
			minIndex=iIntLect;
			maxIndex=iIntLect+ iExtLect;//Must choose external lecturer
			break;
		case 2:
			minIndex=0;
			maxIndex=iIntLect;//Must choose internal lecturer
			break;
	}
	int k;
	for(k=minIndex; k<maxIndex;k++)
			if((arrLecturer[k].arrSpecialize[IDJury] == 0 || arrLecturer[k].arrSpecialize[IDJury] == 3) && 
				arrJury[IDJury].IsLecturerJoinJury(k) == false &&
				arrNumProjofLect[k] < thresholdNumOfJury &&
				this->arrThreNumProjOfLect[k] >0)
			break;
		if(k == maxIndex) //could not find any suiltable lecturers
			return 0;
		else
		{
			//find the best candidate: there is the most number of available PERIODS
			int numAvailPeriod=arrThreNumProjOfLect[k];
			int m=k;
			for(int i=k+1;i<maxIndex;i++) 
				if((arrLecturer[i].arrSpecialize[IDJury] == 0 || arrLecturer[i].arrSpecialize[IDJury] == 3) && 
					arrJury[IDJury].IsLecturerJoinJury(i) == false &&
					arrNumProjofLect[i] < thresholdNumOfJury &&
					this->arrThreNumProjOfLect[i] >0 &&
					arrThreNumProjOfLect[i] > numAvailPeriod)// this lecture do not joins over "threNumOfProj" jury
					{
						numAvailPeriod = arrThreNumProjOfLect[i];
						m=i;
					}
			secretID=m;
			return 1;
		}
}
//Step 5: Choose normal member for juries
bool clsSolution::ChooseNormalMembers()
{
	for(int i=0;i<iJury;i++)
	{
		int thresholdNum=iThreNumOfProj;
		int IdNormal, numMem;
		numMem=this->ChooseNormalMemberForOneJury(i,thresholdNum,IdNormal);
		while(numMem != 1 && thresholdNum < iJury)
		{
			thresholdNum++; //increase the threshold number of jury per lecturer
			numMem=this->ChooseNormalMemberForOneJury(i,thresholdNum,IdNormal);
		}
		if(numMem !=1)
		{
			cout<<endl<<"Error: There is not enough normal-menber candidate for jury "<<i+1;
			return false;
		}
		else//add lecturer to jury
		{
			arrJury[i].arrOtherMembers[2]=arrLecturer[IdNormal];
			arrNumProjofLect[IdNormal]++;
			arrThreNumProjOfLect[IdNormal]--;//decrease the maximum number of jury lecturer could be joined
			if(arrLecturer[IdNormal].IsExternalLect == true)
				arrJury[i].iNumExtLect++;
		}
	}
	return true;
}
//Step 5a: Choose normal member for ONE jury
int clsSolution::ChooseNormalMemberForOneJury(int IDJury, int thresholdNumOfJury, int& normalID)
{
	int k=0;
	for(k=0; k<iIntLect+iExtLect;k++)
			if((arrLecturer[k].arrSpecialize[IDJury] == 0 || arrLecturer[k].arrSpecialize[IDJury] == 3) && 
				arrJury[IDJury].IsLecturerJoinJury(k) == false &&
				arrNumProjofLect[k] < thresholdNumOfJury &&
				this->arrThreNumProjOfLect[k] >0)
			break;
	if(k == iIntLect+iExtLect) //could not find any suiltable lecturers
			return 0;
		else
		{
			//find the best candidate: there is the most number of available PERIODS
			int numAvailPeriod=arrThreNumProjOfLect[k];
			int m=k;
			for(int i=k+1;i<iIntLect+iExtLect;i++) 
				if((arrLecturer[i].arrSpecialize[IDJury] == 0 || arrLecturer[i].arrSpecialize[IDJury] == 3) && 
					arrJury[IDJury].IsLecturerJoinJury(i) == false &&
					arrNumProjofLect[i] < thresholdNumOfJury &&
					this->arrThreNumProjOfLect[i] >0 &&
					arrThreNumProjOfLect[i] > numAvailPeriod)// this lecture do not joins over "threNumOfProj" jury
					{
						numAvailPeriod = arrThreNumProjOfLect[i];
						m=i;
					}
			normalID=m;
			return 1;
		}
}

//Task 2: Choose period for juries
bool clsSolution::ChoosePeriodForJuries()
{
	//Step 1: Find the availability period for juries
	FindAvailabilityPeriodForJuries();
	//Step 2: Find suitable period for juries
	if(FindPeriodForJuries() == false)
		return false;
	//period for juries
	//cout<<endl<<"Set period for juries: "<<endl;
	//for(int i=0;i<iJury;i++)
	//{
	//	cout<<arrJury[i].IdPerizzaAzod+1<<"  ";
	//}

	return true;

}
//Step 1 - task 2: Find availability period for juries
void clsSolution::FindAvailabilityPeriodForJuries()//0 is available,1 is busy
{
	
	for(int IDJury=0;IDJury<iJury;IDJury++)
	{
		if (arrJury[IDJury].advisor[1].IdLect==-1)
		{
		for(int i=0;i<MAXPERIOD;i++)
			if(arrJury[IDJury].advisor[0].arrAvailTime[i] != 0 ||
				
					arrJury[IDJury].arrReviewers[0].arrAvailTime[i] != 0 ||
					arrJury[IDJury].arrReviewers[1].arrAvailTime[i] != 0 ||
					arrJury[IDJury].arrOtherMembers[0].arrAvailTime[i] != 0 ||
					arrJury[IDJury].arrOtherMembers[1].arrAvailTime[i] != 0 ||
						arrJury[IDJury].arrOtherMembers[2].arrAvailTime[i] != 0)
				arrJury[IDJury].arrAvailPeriod[i] = false;		
		}
		else //there is co advisor
		{
			for(int i=0;i<MAXPERIOD;i++)
			if(arrJury[IDJury].advisor[0].arrAvailTime[i] != 0 ||
				arrJury[IDJury].advisor[1].arrAvailTime[i] != 0 ||
					arrJury[IDJury].arrReviewers[0].arrAvailTime[i] != 0 ||
					arrJury[IDJury].arrReviewers[1].arrAvailTime[i] != 0 ||
					arrJury[IDJury].arrOtherMembers[0].arrAvailTime[i] != 0 ||
					arrJury[IDJury].arrOtherMembers[1].arrAvailTime[i] != 0 ||
						arrJury[IDJury].arrOtherMembers[2].arrAvailTime[i] != 0)
				arrJury[IDJury].arrAvailPeriod[i] = false;	
		}

	}
	//availability period for juries
	//cout<<endl<<"Availability period for juries:"<<endl;
	//for(int i=0;i<iJury;i++)
//	{
//		for(int j=0;j<MAXPERIOD;j++)
//			cout<<arrJury[i].arrAvailPeriod[j]<<"  ";
//		cout<<endl;
//	}

}
//Find juries that professor A is in
vector<int> clsSolution::FindJuriesProfIsIn(int ProfID)
{
	vector<int> ListOfProfJoining;
	for (int i=0;i<iJury;i++)
	{
		if (arrJury[i].IsLecturerJoinJury(ProfID)==true)
		{
			ListOfProfJoining.push_back(i);
		}
	}
	return ListOfProfJoining;
}
vector<int> clsSolution::FindJuriesProfIsIn(int ProfID,clsSolution Sol)
{
	vector<int> ListOfProfJoining;
	for (int i=0;i<iJury;i++)
	{
		if (Sol.arrJury[i].IsLecturerJoinJury(ProfID)==true)
		{
			ListOfProfJoining.push_back(i);
		}
	}
	return ListOfProfJoining;
}
bool clsSolution::CheckIfPeriodIsOK(int JuryID, int iPeriod)
{
	vector<int> JuryHasSamePeriod;
	for (int i=0;i<iJury;i++)
	{
		if (arrIsChoosePeriod[i]==true && arrJury[i].IdPeriod==iPeriod)
			JuryHasSamePeriod.push_back(i);
	}
	vector<int> LectInJury;
	vector<int> LectInJuryNeedTested;
	LectInJuryNeedTested.push_back(arrJury[JuryID].advisor[0].IdLect);
		LectInJuryNeedTested.push_back(arrJury[JuryID].arrReviewers[0].IdLect);
		LectInJuryNeedTested.push_back(arrJury[JuryID].arrReviewers[1].IdLect);
		LectInJuryNeedTested.push_back(arrJury[JuryID].arrOtherMembers[0].IdLect);
		LectInJuryNeedTested.push_back(arrJury[JuryID].arrOtherMembers[1].IdLect);
		if (arrJury[JuryID].advisor[1].IdLect!=-1)
			LectInJuryNeedTested.push_back(arrJury[JuryID].advisor[1].IdLect);
	for (int i=0;i<JuryHasSamePeriod.size();i++)
	{
		LectInJury.push_back(arrJury[JuryHasSamePeriod[i]].advisor[0].IdLect);
		LectInJury.push_back(arrJury[JuryHasSamePeriod[i]].arrReviewers[0].IdLect);
		LectInJury.push_back(arrJury[JuryHasSamePeriod[i]].arrReviewers[1].IdLect);
		LectInJury.push_back(arrJury[JuryHasSamePeriod[i]].arrOtherMembers[0].IdLect);
		LectInJury.push_back(arrJury[JuryHasSamePeriod[i]].arrOtherMembers[1].IdLect);
		if (arrJury[JuryHasSamePeriod[i]].advisor[1].IdLect!=-1)
			LectInJury.push_back(arrJury[JuryHasSamePeriod[i]].advisor[1].IdLect);
		for (int j=0;j<LectInJury.size();j++)
		{
			for (int k=0;k<LectInJuryNeedTested.size();k++)
			{
				if (LectInJury[j]==LectInJuryNeedTested[k])
					return false;
			}
		}
		LectInJury.clear();
	}
	//all members are available

	return true;


	return false;
}
int clsSolution::FindSoonestPeriod(int JuryID)
{
	int iPed;
	for (int i=0;i<MAXPERIOD;i++)
	{
		if (arrJury[JuryID].arrAvailPeriod[i]==true) //find a period that all professor are not busy (just according to busy schedule)
		{
			if (CheckIfPeriodIsOK(JuryID, i)==true)
			{
				iPed=i;
				break;
			}
		}
	}
	if (iPed<=this->Y)
		return iPed;
	return -1;
}
//Step 2 - task 2: Find suitable period for juries (1)
bool clsSolution::FindPeriodForJuries1()
{
	//Initialize period for juries
	for(int i=0;i<iJury;i++)
	{
		int iPerid;
		for(iPerid=0;iPerid<=this->Y;iPerid++)
			if(arrJury[i].arrAvailPeriod[iPerid]==true)
				break;
		if(iPerid == this->Y + 1)
		{
			this->numUnFesibleJury++;//increase number of unfesible jury
			this->vcrUnFesibleJury.push_back(i);
			cout<<endl<<"All periods of jury "<<i+1<<" is unvailable!";
			//return false;
		}
		else
			arrJury[i].IdPeriod=iPerid;	
	}
	if(this->numUnFesibleJury > 0) //if there is unfesible jury in the solution
		return false;
	//If juries coincide and have the same lecture, move it
	int minIDJury=FindMinPeriod();//ID of jury that has mininimal period.
	while(minIDJury != -1)
	{
//		cout<<endl<<"Jury has Minimal period: "<<minIDJury;
		//count the number of juries at the same period minPerid
		int countJury=0;
		int arrTempIDJury[MAXJURY];
		for(int i=0;i<iJury;i++)
			if(arrJury[i].IdPeriod == arrJury[minIDJury].IdPeriod)
			{
				arrTempIDJury[countJury]=i;//add jury to the template list
				countJury++;
			}
//		cout<<endl<<"Template list: "<<endl;
//		for(int i=0;i<countJury;i++)
//			cout<<arrTempIDJury[i]<<"  ";

		if(countJury == 1) //there is only one jury at the period
			arrIsChoosePeriod[minIDJury] = true;
		else //there are more than one jury at the period
			SolveCoincideJuries(arrTempIDJury, countJury);		
		minIDJury = FindMinPeriod();
	}
	//check whether there jury be set at period MAXPERIOD
	for(int i=0;i<iJury;i++)
		if(arrJury[i].IdPeriod >= this->Y)
		{
			cout<<endl<<"This solution is over makespan objective!";
			return false;
		}
	return true;
}
//Find period for juries (3) testing graph idea
bool clsSolution::FindPeriodForJuries2()
{

	return true;
}
//Step 2 - task 2: Find suitable period for juries (2)
bool clsSolution::FindPeriodForJuries()
{
	vector<int> ListCount;//number of juries that each professor joins
	for (int i=0;i<iIntLect+iExtLect;i++)
	{
		ListCount.push_back(FindJuriesProfIsIn(i).size());
	}
	vector<int> ListLect;//sort list of lecturers by the number of juries they join
	int checkLect[MAXLECT];
	for (int i=0;i<iIntLect+iExtLect;i++)
	{
		checkLect[i]=-1;
	}
	for (int i=0;i<iIntLect+iExtLect;i++)
	{
		int pos=-1;
		int max=-1;
		for (int j=0;j<iIntLect+iExtLect;j++)
		{
			if (ListCount[j]>max&&checkLect[j]==-1)
			{
				max=ListCount[j];
				pos=j;
			}
		}
		ListLect.push_back(pos);
		checkLect[pos]=0;
	}
	//New initialize period for juries
	for (int i=0;i<iIntLect+iExtLect;i++)
	{
		vector<int> ListOfProfJoining=FindJuriesProfIsIn(ListLect[i]);
		if (ListOfProfJoining.size()==1 && arrIsChoosePeriod[ListOfProfJoining[0]]==false)//there is only 1 jury that professor have to join
		{
			//choose the soonest timeslot available
			int iPerid = FindSoonestPeriod(ListOfProfJoining[0]);
			if(iPerid == -1)
			{
				this->numUnFesibleJury++;//increase number of unfesible jury
				this->vcrUnFesibleJury.push_back(ListOfProfJoining[0]);
				cout<<endl<<"All periods of jury "<<ListOfProfJoining[0]+1<<" is unvailable!";
				//return false;
			}
			else
				arrJury[ListOfProfJoining[0]].IdPeriod=iPerid;	
			if(this->numUnFesibleJury > 0) //if there is unfesible jury in the solution
				return false;
			arrIsChoosePeriod[ListOfProfJoining[0]]=true;
		}
		if (ListOfProfJoining.size()>1)//there are more than 1 jury
		{
			FindPeriodForJuries_Step1(ListOfProfJoining);
			FindPeriodForJuries_Step2(ListOfProfJoining);
		}

	
	}
	if(this->numUnFesibleJury > 0) //if there is unfesible jury in the solution
		return false;
	
	//check whether there jury be set at period MAXPERIOD
	for(int i=0;i<iJury;i++)
		if(arrJury[i].IdPeriod >= this->Y)
		{
			cout<<endl<<"This solution is over makespan objective!";
			return false;
		}
	return true;
}

bool clsSolution::FindPeriodForJuries_Step1(vector<int> ListOfProfJoining)
{
	for (int j=0;j<ListOfProfJoining.size();j++)
			{
				if (arrIsChoosePeriod[ListOfProfJoining[j]]==false)
				{
					int check=0;//check=1 if proffesor has already joined at least 1 jury
								
					for (int k=0;k<ListOfProfJoining.size();k++)
					{
						if (arrIsChoosePeriod[ListOfProfJoining[k]]==true)
						{
							check=1;
							break;
						}
					}
					//if professor hasn't joined any jury yet, choose the soonest one
					if (check==0)
					{
						int iPerid=FindSoonestPeriod(ListOfProfJoining[j]);
						if(iPerid == -1)
						{
							this->numUnFesibleJury++;//increase number of unfesible jury
							this->vcrUnFesibleJury.push_back(ListOfProfJoining[j]);
							cout<<endl<<"All periods of jury "<<ListOfProfJoining[j]+1<<" is unvailable!";
							//return false;
						}
						else
							arrJury[ListOfProfJoining[j]].IdPeriod=iPerid;	
						if(this->numUnFesibleJury > 0) //if there is unfesible jury in the solution
							return false;
						arrIsChoosePeriod[ListOfProfJoining[j]]=true;
					}
					//if professor has joined some juries, try to choose the next one or the previous one
					if (check==1)
					{
						for (int k=0;k<ListOfProfJoining.size();k++)
						{
							if (arrIsChoosePeriod[ListOfProfJoining[k]]==true)
							{
								//try the previous one
								if ((arrJury[ListOfProfJoining[j]].arrAvailPeriod[arrJury[ListOfProfJoining[k]].IdPeriod-1]==true)&&
									((arrJury[ListOfProfJoining[k]].IdPeriod/NUMSLOT)==((arrJury[ListOfProfJoining[k]].IdPeriod-1)/NUMSLOT))&&
									CheckIfPeriodIsOK(ListOfProfJoining[j],arrJury[ListOfProfJoining[k]].IdPeriod-1)==true)
								{
									arrJury[ListOfProfJoining[j]].IdPeriod=arrJury[ListOfProfJoining[k]].IdPeriod-1;
									arrIsChoosePeriod[ListOfProfJoining[j]]=true;
									break;
								}
								//try the next one
								if ((arrJury[ListOfProfJoining[j]].arrAvailPeriod[arrJury[ListOfProfJoining[k]].IdPeriod+1]==true)&&
									((arrJury[ListOfProfJoining[k]].IdPeriod/NUMSLOT)==((arrJury[ListOfProfJoining[k]].IdPeriod+1)/NUMSLOT))&&
									CheckIfPeriodIsOK(ListOfProfJoining[j],arrJury[ListOfProfJoining[k]].IdPeriod+1)==true)
								{
									arrJury[ListOfProfJoining[j]].IdPeriod=arrJury[ListOfProfJoining[k]].IdPeriod+1;
									arrIsChoosePeriod[ListOfProfJoining[j]]=true;
									break;
								}
							}
						}
						//if cannot choose next or previous, go to the next jury
						if (arrIsChoosePeriod[ListOfProfJoining[j]]==false)
						{
							
						}
					}
				}
			}
	return true;
}

bool clsSolution::FindPeriodForJuries_Step2(vector<int> ListOfProfJoining)
{
	for (int j=0;j<ListOfProfJoining.size();j++)
			{
				if (arrIsChoosePeriod[ListOfProfJoining[j]]==false)
				{
					int check=0;//check=1 if proffesor has already joined at least 1 jury
								
					for (int k=0;k<ListOfProfJoining.size();k++)
					{
						if (arrIsChoosePeriod[ListOfProfJoining[k]]==true)
						{
							check=1;
							break;
						}
					}
					//if professor hasn't joined any jury yet, choose the soonest one
					if (check==0)
					{
						int iPerid=FindSoonestPeriod(ListOfProfJoining[j]);
						if(iPerid == -1)
						{
							this->numUnFesibleJury++;//increase number of unfesible jury
							this->vcrUnFesibleJury.push_back(ListOfProfJoining[j]);
							cout<<endl<<"All periods of jury "<<ListOfProfJoining[j]+1<<" is unvailable!";
							//return false;
						}
						else
							arrJury[ListOfProfJoining[j]].IdPeriod=iPerid;	
						if(this->numUnFesibleJury > 0) //if there is unfesible jury in the solution
							return false;
						arrIsChoosePeriod[ListOfProfJoining[j]]=true;
					}
					//if professor has joined some juries, try to choose the next one or the previous one
					if (check==1)
					{
						for (int k=0;k<ListOfProfJoining.size();k++)
						{
							if (arrIsChoosePeriod[ListOfProfJoining[k]]==true)
							{
								//try the previous one
								if ((arrJury[ListOfProfJoining[j]].arrAvailPeriod[arrJury[ListOfProfJoining[k]].IdPeriod-1]==true)&&
									((arrJury[ListOfProfJoining[k]].IdPeriod/NUMSLOT)==((arrJury[ListOfProfJoining[k]].IdPeriod-1)/NUMSLOT))&&
									CheckIfPeriodIsOK(ListOfProfJoining[j],arrJury[ListOfProfJoining[k]].IdPeriod-1)==true)
								{
									arrJury[ListOfProfJoining[j]].IdPeriod=arrJury[ListOfProfJoining[k]].IdPeriod-1;
									arrIsChoosePeriod[ListOfProfJoining[j]]=true;
									break;
								}
								//try the next one
								if ((arrJury[ListOfProfJoining[j]].arrAvailPeriod[arrJury[ListOfProfJoining[k]].IdPeriod+1]==true)&&
									((arrJury[ListOfProfJoining[k]].IdPeriod/NUMSLOT)==((arrJury[ListOfProfJoining[k]].IdPeriod+1)/NUMSLOT))&&
									CheckIfPeriodIsOK(ListOfProfJoining[j],arrJury[ListOfProfJoining[k]].IdPeriod+1)==true)
								{
									arrJury[ListOfProfJoining[j]].IdPeriod=arrJury[ListOfProfJoining[k]].IdPeriod+1;
									arrIsChoosePeriod[ListOfProfJoining[j]]=true;
									break;
								}
							}
						}
						//if cannot choose next or previous, choose the soonest one
			
						if (arrIsChoosePeriod[ListOfProfJoining[j]]==false)
						{
							int iPerid =FindSoonestPeriod(ListOfProfJoining[j]);
							if(iPerid == -1)
							{
								this->numUnFesibleJury++;//increase number of unfesible jury
								this->vcrUnFesibleJury.push_back(ListOfProfJoining[j]);
								cout<<endl<<"All periods of jury "<<ListOfProfJoining[j]+1<<" is unvailable!";
								//return false;
							}
							else
								arrJury[ListOfProfJoining[j]].IdPeriod=iPerid;	
							if(this->numUnFesibleJury > 0) //if there is unfesible jury in the solution
								return false;
							arrIsChoosePeriod[ListOfProfJoining[j]]=true;
						}
					}
				}
			}
}
//Step 2 - task 2 (a) find the minimize period of juries those period is not chosen, return ID of jury
int clsSolution::FindMinPeriod()
{
	int minPerid=MAXJURY;
	int minIDJury=-1;
	for(int i=0;i<iJury;i++)
		if(arrIsChoosePeriod[i] == false && arrJury[i].IdPeriod < minPerid)
		{
				minPerid=arrJury[i].IdPeriod;
				minIDJury=i;
		}
	return minIDJury;
}
//Step 2 - task 2 (b) Solve the case which there are more than 1 jury at the same period
void clsSolution::SolveCoincideJuries(int arrTempIDJury[MAXJURY], int n)
{
	//Sorting the list, based on the next available period of jury
	//Find the next period of juries in the arrTempIDJry list
	int arrNextPerid[MAXJURY];
	for(int i=0;i<n;i++)
	{
		int IDJury=arrTempIDJury[i];
		int k=arrJury[IDJury].IdPeriod + 1;
		int j;
		for(j=k;j<MAXPERIOD;j++)
			if(arrJury[IDJury].arrAvailPeriod[j] == true)
				break;
		arrNextPerid[i]=j;
	}
	//Decrease Sorting
	for(int i=0;i<n;i++)
		for(int j=i+1;j<n;j++)
			if(arrNextPerid[i] < arrNextPerid[j])
			{
				std::swap(arrNextPerid[i], arrNextPerid[j]);
				std::swap(arrTempIDJury[i], arrTempIDJury[j]);
			}
	//Leave the juries have the same lecturers
	bool arrIsRemove[MAXJURY];
	for(int i=0;i<n;i++)
		arrIsRemove[i]=false;
	for(int i=0;i<n;i++)
	{
		if(arrIsRemove[i] == false)
			for(int j=i+1;j<n;j++)
			{
				int IDJury_i=arrTempIDJury[i];
				int IDJury_j=arrTempIDJury[j];
				if(arrIsRemove[j] == false && arrJury[IDJury_i].IsHavingSameLecturers(arrJury[IDJury_j]))
				{
					arrJury[IDJury_j].IdPeriod = arrNextPerid[j];
					arrIsRemove[j]=true;
				}
			}
	}
	//Final choose the period for juries
	for(int i=0;i<n;i++)
		if(arrIsRemove[i] == false)
			arrIsChoosePeriod[arrTempIDJury[i]]=true;
}

//additional function: Find max of 3 integer
int Max3(int a, int b, int c)
{
	int max=a;
	if(b>max)
		max=b;
	if(c>max)
		max=c;
	return max;
}
//Task 3: Choose room for juries
void clsSolution::ChooseRoomForJuries()
{
	this->iIDRoom=0;
	//calculate list of juries at each period
	std::vector<int> arrNumJuryAtPeriod[MAXPERIOD];
	for(int i=0;i<iJury;i++)
		arrNumJuryAtPeriod[arrJury[i].IdPeriod].push_back(arrJury[i].IdJury);//arrJury[i].IdJury = i

	for(int s=0;s<MAXPERIOD/3;s++) //MAXPERIOD is divisible by 3
	{
		int maxNumJury=Max3(arrNumJuryAtPeriod[s*3].size(), arrNumJuryAtPeriod[s*3+1].size(), arrNumJuryAtPeriod[s*3+2].size());
		for(int j=0;j<maxNumJury;j++)//choose room
		{
			if(arrNumJuryAtPeriod[s*3].size() > 0)
			{
				int IDJury=arrNumJuryAtPeriod[s*3][arrNumJuryAtPeriod[s*3].size()-1];//get the last element
				arrJury[IDJury].IdRoom=iIDRoom;
				arrNumJuryAtPeriod[s*3].pop_back();//remove the last element
			}
			if(arrNumJuryAtPeriod[s*3+1].size() > 0)
			{
				int IDJury=arrNumJuryAtPeriod[s*3+1][arrNumJuryAtPeriod[s*3+1].size()-1];//get the last element
				arrJury[IDJury].IdRoom=iIDRoom;
				arrNumJuryAtPeriod[s*3+1].pop_back();//remove the last element
			}
			if(arrNumJuryAtPeriod[s*3+2].size() > 0)
			{
				int IDJury=arrNumJuryAtPeriod[s*3+2][arrNumJuryAtPeriod[s*3+2].size()-1];//get the last element
				arrJury[IDJury].IdRoom=iIDRoom;
				arrNumJuryAtPeriod[s*3+2].pop_back();//remove the last element
			}
			iIDRoom++;//Increase the number of rooms
		}
		if(s*3 +2 >= this->Y)
			break;
	}
	//Room for juries
	//cout<<endl<<"Set room for juries: "<<endl;
	//for(int i=0;i<iJury;i++)
	//{
	//	cout<<arrJury[i].IdRoom+1<<"  ";
	//}

}

//Update some variables after do scheduling
void clsSolution::UpdateFinalState()
{
	//Update unavailability period of lecturers
	for(int i=0;i<iIntLect + iExtLect;i++)
	{
		for(int j=0;j<iJury;j++)
			if(arrJury[j].IsLecturerJoinJury(i) == true)
				arrLecturer[i].arrAvailTime[arrJury[j].IdPeriod]=2;
		//calculate the number of sessions
		int count=0;
		for(int s=0;s<MAXPERIOD/3;s++)
		{
			bool flag=false;
			for(int p=s*3;p<s*3+3;p++)
				if(arrLecturer[i].arrAvailTime[p] ==2)
				{
					flag=true;
					break;
				}
			if(flag==true)
				count++;
		}
		arrLecturer[i].iSessionJoined=count;
	}
}

//Calculate Objective function, return 0 if the solution is feasible
int clsSolution::CalculateObjectiveValue()
{
	//Calculate the number of room used
	int maxIDRoom=-1;
	for(int i=0;i<iJury;i++)
		if(arrJury[i].IdRoom > maxIDRoom)
			maxIDRoom=arrJury[i].IdRoom;
	numRoom = maxIDRoom+1;

	//Calculate the total number of lecturers' sessions
	numSession=0;
	for(int i=0;i<iIntLect + iExtLect;i++)
		numSession=numSession + arrLecturer[i].iSessionJoined;
	//Calculate the total number of external lecturers' sessions
	exSession=0;
	for(int i=0;i<iIntLect + iExtLect;i++)
	{
		if (arrLecturer[i].IsExternalLect==true)
		exSession=exSession + arrLecturer[i].iSessionJoined;
	}
	//Calculate makespan
	int iMakeSpan=-1;
	for(int i=0;i<iJury;i++)
		if(arrJury[i].IdPeriod > iMakeSpan)
			iMakeSpan = arrJury[i].IdPeriod;
	makeSpan=iMakeSpan+1;


	//calculate missing timeslot for each session
	int iMiss=-1;
	
	for (int i=0;i<numRoom;i++)
	{
		int slotUsed=0;
		for (int j=0;j<iJury;i++)
		{
			if (arrJury[j].IdRoom==i)
				slotUsed++;
		}
		if (slotUsed!=NUMSLOT)
			iMiss=iMiss+(NUMSLOT-slotUsed);
		
	}
	missingTimeslot=iMiss+1;
	//calculate missing timeslot for each prof
	int iMissTime=-1;
	for (int i=0;i<iIntLect+iExtLect;i++)
	{
		vector<int> ListOfProfJoining= FindJuriesProfIsIn(i);
		int count= ListOfProfJoining.size();
		if (count!=0)
		{
		vector<int> ListOfTime;
		for (int j=0;j<count;j++)
		{
			ListOfTime.push_back(arrJury[ListOfProfJoining[j]].IdPeriod);
		}
		for (int j=0;j<count-1;j++)
			for (int k=j+1;k<count;k++)
			{
				if (ListOfTime[j]<ListOfTime[k])
				{
					int t= ListOfTime[j];
					ListOfTime[j]=ListOfTime[k];
					ListOfTime[k]=t;
				}
			}
		int iSessCount=1;
		int iConsiderSess=ListOfTime[0]/NUMSLOT;
		for (int j=0;j<count;j++)
		{
			if (ListOfTime[j]/NUMSLOT!=iConsiderSess)
			{
				iSessCount++;
				iConsiderSess=ListOfTime[j]/NUMSLOT;
			}
		}
		iMissTime=iMissTime+(iSessCount*NUMSLOT)-count;
		}
	}
	

	return 0;
}


//================Using genetic ideas============================
//Start to schedule, using mutation
bool clsSolution::DoScheduling_Genetic()
{
	//Fisrt the first subsolution: choose lecturers, allways be true if DATA is correct;
	if(ChooseLecturerForJuries() == false)
			return false; //DATA is not correct
	//Create AT MOST other 9 solutions, totally we have 10 solutions
	std::vector<clsSolution> arrSolution;
	clsSolution* mySolution=new clsSolution;
	this->CopySolution(*mySolution);
	arrSolution.push_back(*mySolution);
	
	srand (time(0));
	for(int i=0;i<9;i++)
	{
		clsSolution* tempSolution=new clsSolution;
		CopySolution(*tempSolution);
		//for(int i=0;i<iJury;i++)
		//	tempSolution->arrIsChoosePeriod[i]=false;
		int iJuryRandom=rand() % iJury;
		int iFlagMember=rand() % 2;
		//find lecturer to replace other member 0 or 1
		bool myflag;
		if(tempSolution->arrJury[iJuryRandom].arrOtherMembers[iFlagMember].IsExternalLect == true)
			myflag =tempSolution->FindLecturerToReplace(iJuryRandom,iFlagMember, true);
		else
			myflag=tempSolution->FindLecturerToReplace(iJuryRandom,iFlagMember, false);
		if(myflag == true)
			arrSolution.push_back(*tempSolution);
	}
	int minNumRoom=iJury + 1;
	clsSolution tempThisSolution;
	//cout<<endl<<"SOLUTION SIZE: "<<arrSolution.size()<<endl;
	for(int i=0;i<arrSolution.size();i++)
	{
		if(arrSolution[i].ChoosePeriodForJuries() == true)
		{
			arrSolution[i].ChooseRoomForJuries();
			arrSolution[i].UpdateFinalState();
			arrSolution[i].CalculateObjectiveValue();
			if(arrSolution[i].numRoom < minNumRoom)
			{
				minNumRoom = arrSolution[i].numRoom;
				tempThisSolution = arrSolution[i];
			}
		}
	}
	if(minNumRoom == iJury+1)
		return false;
	*this=tempThisSolution;
	return true;
}
//Find another lecturer to replace other member 0 (flag=0), or other member 1 (flag=1)
bool clsSolution::FindLecturerToReplace(int IDJury, int flags, bool IsExternal)
{
	int indexLect;
	if(IsExternal == true)
		indexLect = iIntLect;
	else
		indexLect = 0;
	for(int i=iIntLect+iExtLect; i>=indexLect;i--)
		if(arrJury[IDJury].IsLecturerJoinJury(i) == false && 
				(arrLecturer[i].arrSpecialize[IDJury] == 0 || arrLecturer[i].arrSpecialize[IDJury] == 3) &&
				arrNumProjofLect[i] < this->arrThreNumProjOfLect[i]) //there are available period
		{
			arrJury[IDJury].arrOtherMembers[flags]=arrLecturer[i];
			//Reset state of lecture......
			//do not need
			return true;
		}
	return false;
}
//Find other chair
bool clsSolution::FindChairToReplace(int IDJury,int &myCount)
{
	//Must choose internal lecturer
	srand (time(0));
	//int seed = static_cast<int>(time(0));
	//srand(seed);
	//srand ( (unsigned) time(NULL) );

	//int idLect=(myCount + rand()) % (this->iIntLect + this->iExtLect); //Random choosing to start at lecturer idLect
	//myCount++;
	int idLect;
	int minIndex,maxIndex;
	if(arrJury[IDJury].arrOtherMembers[1].IsExternalLect == true&&arrJury[IDJury].iNumExtLect==1)
	{
		minIndex=iIntLect;
		maxIndex=iIntLect+ iExtLect;//Must choose external lecturer
		idLect=(myCount+rand()) % this->iExtLect; //Random choosing to start at lecturer idLect
	}
	else
	{
		minIndex=0;
		maxIndex=iIntLect+iExtLect;//Choose in all
		idLect=(myCount+rand()) % (this->iIntLect+this->iExtLect); //Random choosing to start at lecturer idLect
	}
	idLect=minIndex+ idLect;
	//cout<<"RANDOM: "<<idLect<<",";
	for(int i=idLect;i<maxIndex;i++)
		if(arrJury[IDJury].IsLecturerJoinJury(i) == false &&
			(arrLecturer[i].arrSpecialize[IDJury] == 0 || arrLecturer[i].arrSpecialize[IDJury] == 3) &&
			this->arrThreNumProjOfLect[i] > 0) //there are available period
		{
			arrNumProjofLect[this->arrJury[IDJury].arrOtherMembers[0].IdLect]--;
			this->arrThreNumProjOfLect[this->arrJury[IDJury].arrOtherMembers[0].IdLect]++;
			arrJury[IDJury].arrOtherMembers[0]=arrLecturer[i];
			arrNumProjofLect[i]++;
			this->arrThreNumProjOfLect[i]--;
			return true;
		}
	//Can not find any candidates, continue finding
	for(int i=minIndex;i<idLect;i++)
		if(arrJury[IDJury].IsLecturerJoinJury(i) == false &&
			(arrLecturer[i].arrSpecialize[IDJury] == 0 || arrLecturer[i].arrSpecialize[IDJury] == 3) &&
			this->arrThreNumProjOfLect[i] > 0) //there are available period
		{
			arrNumProjofLect[this->arrJury[IDJury].arrOtherMembers[0].IdLect]--;
			this->arrThreNumProjOfLect[this->arrJury[IDJury].arrOtherMembers[0].IdLect]++;
			this->arrJury[IDJury].arrOtherMembers[0]=arrLecturer[i];
			arrNumProjofLect[i]++;
			this->arrThreNumProjOfLect[i]--;
			return true;
		}
		myCount++;
	return false;
}
//Find other secretary
bool clsSolution::FindSecretaryToReplace(int IDJury,int& myCount)
{
	//must choose internal lecturer
	srand (time(0));
//	int minIndex, maxIndex;
	int idLect=(myCount + rand()) % (this->iIntLect); //Random choosing to start at lecturer idLect
	myCount++;
	
	//Find candidate
	for(int i=idLect;i<iIntLect;i++)
		if(arrJury[IDJury].IsLecturerJoinJury(i) == false &&
			(arrLecturer[i].arrSpecialize[IDJury] == 0 || arrLecturer[i].arrSpecialize[IDJury] == 3) &&
			arrNumProjofLect[i] < this->arrThreNumProjOfLect[i]) //there are available period
		{
			arrNumProjofLect[this->arrJury[IDJury].arrOtherMembers[1].IdLect]--;
			arrThreNumProjOfLect[this->arrJury[IDJury].arrOtherMembers[1].IdLect]++;
			arrJury[IDJury].arrOtherMembers[1]=arrLecturer[i];
			arrNumProjofLect[i]++;
			arrThreNumProjOfLect[i]--;
			return true;
		}
	//Can not find any candidates, continue finding
	for(int i=0;i<idLect;i++)
		if(arrJury[IDJury].IsLecturerJoinJury(i) == false &&
			(arrLecturer[i].arrSpecialize[IDJury] == 0 || arrLecturer[i].arrSpecialize[IDJury] == 3) &&
			arrNumProjofLect[i] < this->arrThreNumProjOfLect[i]) //there are available period
		{
			arrNumProjofLect[this->arrJury[IDJury].arrOtherMembers[1].IdLect]--;
			arrThreNumProjOfLect[this->arrJury[IDJury].arrOtherMembers[1].IdLect]++;
			this->arrJury[IDJury].arrOtherMembers[1]=arrLecturer[i];
			arrNumProjofLect[i]++;
			arrThreNumProjOfLect[i]--;
			return true;
		}
	myCount++;
	return false;
}
//Find other reviewer 0 or 1
bool clsSolution::FindReviewerToReplace(int IDJury, int iRevi, int& myCount)
{
	srand (time(0));
	int minIndex, maxIndex;
	int idLect;
	if(arrJury[IDJury].arrReviewers[iRevi].IsExternalLect == true&&arrJury[IDJury].iNumExtLect==1)
	{
		minIndex=iIntLect;
		maxIndex=iIntLect+ iExtLect;//Must choose external lecturer
		idLect=(myCount+rand()) % this->iExtLect; //Random choosing to start at lecturer idLect
	}
	else
	{
		minIndex=0;
		maxIndex=iIntLect+iExtLect;//Choose in all
		idLect=(myCount+rand()) % (this->iIntLect+this->iExtLect); //Random choosing to start at lecturer idLect
	}
	idLect=minIndex+ idLect;

	//Find candidate
	for(int i=idLect;i<maxIndex;i++)
		if(arrJury[IDJury].IsLecturerJoinJury(i) == false &&
			(arrLecturer[i].arrSpecialize[IDJury] == 3) &&
			arrNumProjofLect[i] < this->arrThreNumProjOfLect[i]) //there are available period
		{
			arrNumProjofLect[this->arrJury[IDJury].arrReviewers[iRevi].IdLect]--;
			arrThreNumProjOfLect[this->arrJury[IDJury].arrReviewers[iRevi].IdLect]++;
			arrJury[IDJury].arrReviewers[iRevi]=arrLecturer[i];
			arrNumProjofLect[i]++;
			arrThreNumProjOfLect[i]--;
			return true;
		}
	//Can not find any candidates, continue finding
	for(int i=minIndex;i<idLect;i++)
		if(arrJury[IDJury].IsLecturerJoinJury(i) == false &&
			(arrLecturer[i].arrSpecialize[IDJury] == 3) &&
			arrNumProjofLect[i] < this->arrThreNumProjOfLect[i]) //there are available period
		{
			arrNumProjofLect[this->arrJury[IDJury].arrReviewers[iRevi].IdLect]--;
			arrThreNumProjOfLect[this->arrJury[IDJury].arrReviewers[iRevi].IdLect]++;
			this->arrJury[IDJury].arrReviewers[iRevi]=arrLecturer[i];
			arrNumProjofLect[i]++;
			arrThreNumProjOfLect[i]--;
			return true;
		}
	myCount++;
	return false;
}
void clsSolution::MakeChildSolution(clsSolution* parentSol2, clsSolution& childSolution,int crossPart, int crossQuantity)
{
	childSolution.iJury=iJury;
	childSolution.iIntLect=iIntLect;
	childSolution.iExtLect=iExtLect;
	childSolution.Y=Y;
	childSolution.SetLectures(arrLecturer);
	int control=0;
	for (int j=0;j<crossPart;j++)
	{
		for (int i=0;i<crossQuantity;i++)
		{
			if (control==0)
			{
			childSolution.arrJury[(j*crossQuantity)+i]=arrJury[(j*crossQuantity)+i];
			childSolution.arrIsChoosePeriod[(j*crossQuantity)+i]=arrIsChoosePeriod[(j*crossQuantity)+i];
			}
			else
			{
				childSolution.arrJury[(j*crossQuantity)+i]=parentSol2->arrJury[(j*crossQuantity)+i];
			childSolution.arrIsChoosePeriod[(j*crossQuantity)+i]=parentSol2->arrIsChoosePeriod[(j*crossQuantity)+i];
			
			}
		}
		if (control==0)
			control=1;
		else
			control=0;
	}
	if (crossPart*crossQuantity<iJury)
	{
		for (int i=(crossPart*crossQuantity);i<iJury;i++)
		{
			if (control==0)
			{
			childSolution.arrJury[i]=arrJury[i];
			childSolution.arrIsChoosePeriod[i]=arrIsChoosePeriod[i];
			control=1;
			}
			else
			{
				childSolution.arrJury[i]=parentSol2->arrJury[i];
			childSolution.arrIsChoosePeriod[i]=parentSol2->arrIsChoosePeriod[i];
			control=0;
			}
		}
	}
	
	childSolution.iThreNumOfProj=max(iThreNumOfProj,parentSol2->iThreNumOfProj);
	for (int i=0;i<iIntLect+iExtLect;i++)
	{
		childSolution.arrNumProjofLect[i]=FindJuriesProfIsIn(i,childSolution).size();
		if (i<crossQuantity)
			childSolution.arrThreNumProjOfLect[i]=arrThreNumProjOfLect[i];
		else
			childSolution.arrThreNumProjOfLect[i]=parentSol2->arrThreNumProjOfLect[i];
	}
	childSolution.ResetData();
}