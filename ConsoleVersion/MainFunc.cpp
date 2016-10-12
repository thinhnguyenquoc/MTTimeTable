
#include "stdafx.h"
#include "MainFunc.h"

//Functions

//Analyse a substring, used in ReadFile() function
void clsMainFunc::AnalyseSubstring(string sStr, int iLect)
{
	unsigned dash_pos=sStr.find("-");
	if(dash_pos == std::string::npos) //these is not dash
	{
		int iPer=stoi(sStr) -1;		
		arrLecturer[iLect].arrAvailTime[iPer]=1;
	}
	else //These is a dash
	{
		int iPer1, iPer2;
		iPer1=stoi(sStr.substr(0, dash_pos)) -1;

		if(sStr[sStr.size()-1] != '-') 
			iPer2=stoi(sStr.substr(dash_pos + 1, sStr.size() - dash_pos -1)) -1;
		else //case: example "8-"
			iPer2=MAXPERIOD-1;
		for(int i=iPer1;i<=iPer2;i++)
			arrLecturer[iLect].arrAvailTime[i]=1;

	}

}
//Read input file
void clsMainFunc::ReadFile()
{
	try
	{
		std::ifstream myFile(sInputFileName);
		if(!myFile) 
			cout<<"Cannot open input file ";
		std::string myBuf;
		getline(myFile, myBuf); //get line containing the number of internal LECTURERS
		int mpos=myBuf.find("/");
		iIntLect=stoi(myBuf.substr(0,mpos));
		getline(myFile, myBuf); //get line containing the number of external LECTURERS
		mpos=myBuf.find("/");
		iExtLect=stoi(myBuf.substr(0,mpos));
		getline(myFile, myBuf); //get line containing the number of JURIES
		mpos=myBuf.find("/");
		iJury=stoi(myBuf.substr(0,mpos));
		getline(myFile, myBuf); //get line containing the start and end PERIOD
		mpos=myBuf.find(" ");
		X=stoi(myBuf.substr(0,mpos));
		int next_mpos=myBuf.find("/", mpos+1);
		Y=stoi(myBuf.substr(mpos+1,next_mpos - mpos -1));
		//MAXPERIOD=Y;

		//cout<<endl<<X<<" --->  "<<Y;
		//cout<<iIntLect<<endl<<iExtLect<<endl<<iJury<<endl;

		getline(myFile, myBuf); //skip this line
		getline(myFile, myBuf); //skip this line

		//get the input matrix
		for(int i=0;i<iIntLect + iExtLect;i++)
		{
			getline(myFile, myBuf); //get a line of matrix
			unsigned pos=myBuf.find("\t");		
			for(int j=0;j<iJury;j++)
			{
				arrLecturer[i].arrSpecialize[j]=stoi(myBuf.substr(pos + 1 + j*2,1));
				arrLecturer[i].IdLect=i;
				if(i<iIntLect)
					arrLecturer[i].IsExternalLect=false;
				else
					arrLecturer[i].IsExternalLect=true;
			}
		}

		//PrintMatrix();
	
		//get unavailability information
		getline(myFile, myBuf); //skip this line
		//Init the availability matrix
		for(int i=0;i<iIntLect + iExtLect;i++)
			for(int j=0;j<MAXPERIOD;j++)
				arrLecturer[i].arrAvailTime[j]=0; //availability
		cout<<endl<<"Unavailability time of lecturers: "<<endl;
		for(int i=0;i<iIntLect + iExtLect;i++)
		{
			getline(myFile, myBuf); //get a line
			unsigned cur_pos=myBuf.find(" ");
			if(cur_pos != myBuf.size() - 1 && cur_pos != std::string::npos) //there are unavailability times)
			{
				unsigned next_pos=myBuf.find(" ", cur_pos + 1);
				string myStr;
				while(next_pos != std::string::npos)
				{
					myStr=myBuf.substr(cur_pos+1, next_pos - cur_pos -1);
					AnalyseSubstring(myStr,i);//Get and store information to matrix arrAvail[][]

					cout<<myStr<<" ";
					cur_pos=next_pos;
					next_pos=myBuf.find(" ", cur_pos + 1);
				}
				//get the last substring
				myStr=myBuf.substr(cur_pos+1, myBuf.size() - cur_pos -1);
				AnalyseSubstring(myStr,i); //Get and store information to matrix arrAvail[][]
				cout<<myStr<<" ";		
			}
			cout<<endl;
		}
		//Calculate number of availability time of lecturers
		for(int i=0;i<iIntLect + iExtLect;i++)
		{
			arrLecturer[i].Y=Y;
			arrLecturer[i].InitData();
		}
	}
	catch(std::string errStr)
	{
		cout<<endl<<"Read file error! wrong file format?"<<endl;
	}
//	PrintMatrix_1();
}
//Write output file
void clsMainFunc::WriteFile()
{
	ofstream myFile(sOutputFileName);
	if(!myFile)
		cout<<"Cannot open output file "; 
	myFile<<iIntLect<<"// number of internal lecturers: IT_1, ..., IT_m1"<<endl;
	myFile<<iExtLect<<"// number of external lecturers: IT_m1+1, ..., IT_m"<<endl;
	myFile<<iJury<<"// number of jury: J1, ..., Jn"<<endl;
	myFile<<"// matrix "<<endl;
	//output matrix
	for(int i=0;i<iJury;i++)
		myFile<<"\t"<<"J"<<i+1;
		//myFile<<"\t"<<"J"<<std::to_string((long long)(i+1));
	myFile<<endl;
	for(int i=0;i<iIntLect + iExtLect;i++)
	{
		myFile<<"IT"<<std::to_string((long long)(i+1));
		for(int j=0;j<iJury;j++)
			myFile<<"\t"<<arrAOutput[i][j];
		myFile<<endl;
	}
	myFile<<"Time";
	for(int i=0;i<iJury;i++)
		myFile<<"\t"<<arrTimeJury[i];

	myFile<<endl<<"Room";
	for(int i=0;i<iJury;i++)
		myFile<<"\t"<<arrRoomJury[i];

}
//Print input data
void clsMainFunc::PrintInputData()
{
	ofstream myLogFile;
	cout<<endl<<"Numbers of internal, external lecturers and jury:"<<endl;
	myLogFile<<endl<<"Numbers of internal, external lecturers and jury:"<<endl;
	
	cout<<iIntLect<<endl<<iExtLect<<endl<<iJury<<endl;
	myLogFile<<iIntLect<<endl<<iExtLect<<endl<<iJury<<endl;

	cout<<endl<<"Start and end time: X="<<X<<", Y="<<Y<<endl;	
	myLogFile<<endl<<"Start and end time: X="<<X<<", Y="<<Y<<endl;	
	//Input matrix
	cout<<endl<<"Input matrix: "<<endl;
	myLogFile<<endl<<"Input matrix: "<<endl;

	for(int i=0;i<iIntLect + iExtLect;i++)
	{
		for(int j=0;j<iJury;j++)
		{
			cout<<arrLecturer[i].arrSpecialize[j]<<"   ";
			myLogFile<<arrLecturer[i].arrSpecialize[j]<<"   ";
		}
		cout<<endl;
		myLogFile<<endl;
	}

	//Unavailability matrix
	/*cout<<endl<<"Unavailability matrix"<<endl;
	for(int i=0;i<iIntLect + iExtLect;i++)
	{
		for(int j=0;j<MAXPERIOD;j++)
			cout<<arrLecturer[i].arrAvailTime[j]<<"   ";			
		cout<<endl;
	}*/

	//arrLecturers
	cout<<endl<<"Interal or exteral lecturers: "<<endl;
	myLogFile<<endl<<"Interal or exteral lecturers: "<<endl;

	for(int i=0;i<iIntLect + iExtLect;i++)
	{
		cout<<arrLecturer[i].IsExternalLect<<"  ";
		myLogFile<<arrLecturer[i].IsExternalLect<<"  ";
	}

}

//Local search algorithm, print two solutions for two objectives
void clsMainFunc::LocalSearch()
{
	ofstream myLogFile(sLogFileName);
	clsSolution mySolution_room, mySolution_makespan;
	int mythreshold;
	int minNumRoom=iJury + 1;	
	int minMakeSpan=MAXPERIOD + 1;
	for(mythreshold=1; mythreshold <= iJury;mythreshold++)
	{
		clsSolution*tempSolution=new clsSolution;
		tempSolution->iJury=iJury;
		tempSolution->iIntLect=iIntLect;
		tempSolution->iExtLect=iExtLect;
		tempSolution->Y=Y;
		tempSolution->SetLectures(arrLecturer);
		tempSolution->InitData();
		tempSolution->iThreNumOfProj=mythreshold;
		if(tempSolution->DoScheduling() == true && tempSolution->makeSpan <= Y)//Y=MAXPERIOD
		{			
			cout<<endl<<"Number of rooms: "<<tempSolution->numRoom;
			myLogFile<<endl<<"Number of rooms: "<<tempSolution->numRoom;
			cout<<endl<<"Makespan: "<<tempSolution->makeSpan;
			myLogFile<<endl<<"Makespan: "<<tempSolution->makeSpan;
			if(tempSolution->numRoom < minNumRoom)
			{
				minNumRoom=tempSolution->numRoom;
				mySolution_room = *tempSolution;
			}			
			if(tempSolution->makeSpan < minMakeSpan)
			{
				minMakeSpan=tempSolution->makeSpan;
				mySolution_makespan = *tempSolution;
			}
		
		}
	}
	if(minNumRoom == iJury+1)
	{
		cout<<endl<<"STOP SCHEDULING!";
		myLogFile<<endl<<"STOP SCHEDULING!";
	}
	else
	{		
		cout<<endl<<endl<<"===============================================================";
		myLogFile<<endl<<endl<<"===============================================================";
		cout<<endl<<"The feasible solution with minimum number of rooms: "<<endl;
		myLogFile<<endl<<"The feasible solution with minimum number of rooms: "<<endl;
		//print solution 1 for number of room objective
		cout<<endl<<"Number of rooms: "<<mySolution_room.numRoom;
		myLogFile<<endl<<"Number of rooms: "<<mySolution_room.numRoom;
		cout<<endl<<"Number of sessions: "<<mySolution_room.numSession;
		myLogFile<<endl<<"Number of sessions: "<<mySolution_room.numSession;
		cout<<endl<<"Makespan: "<<mySolution_room.makeSpan;
		myLogFile<<endl<<"Makespan: "<<mySolution_room.makeSpan;
//		mySolution_room.WriteFileSolutionResult("D:/data_test_2/output_room.txt");
		mySolution_room.PrintSolution();

		//print solution 2 for makespan objective
		cout<<endl<<endl<<"===============================================================";
		myLogFile<<endl<<endl<<"===============================================================";
		cout<<endl<<"The feasible solution with minimum finished time (makespan): "<<endl;
		myLogFile<<endl<<"The feasible solution with minimum finished time (makespan): "<<endl;
		cout<<endl<<"Number of rooms: "<<mySolution_makespan.numRoom;
		myLogFile<<endl<<"Number of rooms: "<<mySolution_makespan.numRoom;
		cout<<endl<<"Number of sessions: "<<mySolution_room.numSession;
		myLogFile<<endl<<"Number of sessions: "<<mySolution_room.numSession;
		cout<<endl<<"Makespan: "<<mySolution_makespan.makeSpan;
		myLogFile<<endl<<"Makespan: "<<mySolution_makespan.makeSpan;
//		mySolution_makespan.WriteFileSolutionResult("D:/data_test_2/output_makespan.txt");
		mySolution_makespan.PrintSolution();

		cout<<endl<<"SCHEDULING FINISHED!";
	}
}
//Heuristic approach
void clsMainFunc::HeuristicApproach()
{
	ofstream myLogFile(sLogFileName);
	clsSolution mySolution;
	mySolution.iJury=iJury;
	mySolution.iIntLect=iIntLect;
	mySolution.iExtLect=iExtLect;
	mySolution.Y=Y;
	mySolution.SetLectures(arrLecturer);
	mySolution.InitData();
	mySolution.iThreNumOfProj=4;	
	bool flag=mySolution.DoScheduling();
	if(flag == true)
	{
		cout<<endl<<endl<<"===============================================================";
		myLogFile<<endl<<endl<<"===============================================================";
		cout<<endl<<"The feasible solution: "<<endl;
		myLogFile<<endl<<"The feasible solution: "<<endl;
		cout<<endl<<"Number of rooms: "<<mySolution.numRoom;
		myLogFile<<endl<<"Number of rooms: "<<mySolution.numRoom;
		cout<<endl<<"Number of sessions: "<<mySolution.numSession;
		myLogFile<<endl<<"Number of sessions: "<<mySolution.numSession;
		cout<<endl<<"Makespan: "<<mySolution.makeSpan;
		myLogFile<<endl<<"Makespan: "<<mySolution.makeSpan;
		cout<<endl<<"Missing timeslot: "<<mySolution.missingTimeslot;
		mySolution.WriteFileSolutionResult(sOutputFileName, sLectFileName);
		mySolution.PrintSolution();
		cout<<endl<<"SCHEDULING FINISHED!";
		myLogFile<<endl<<"SCHEDULING FINISHED!";
	}
	else
	{
		cout<<endl<<"STOP SCHEDULING!";
		myLogFile<<endl<<"STOP SCHEDULING!";
	}

}
//Genetic algorithm
void clsMainFunc::GeneticAlgorithm()
{
	ofstream myLogFile(sLogFileName);
	clsGeneticAlg*myGASolution;
	myGASolution=new clsGeneticAlg();
	int threNumOfPro=iJury; //not be used
	myGASolution->InitData(iPopSize, iNumMutation, 10, iK, iIntLect, iExtLect, iJury, Y, arrLecturer, threNumOfPro);
	
	if(myGASolution->DoGAScheduling()== true)
	{
		clsSolution mySolution;
		mySolution=*myGASolution->arrPopOfSol[0];
		finalSolution = new clsSolution();
		mySolution.CopySolution(*finalSolution);
		cout<<endl<<"The feasible solution with minimum number of rooms: "<<endl;
		myLogFile<<endl<<"The feasible solution with minimum number of rooms: "<<endl;
		//print solution 1 for number of room objective
		cout<<endl<<"Number of rooms: "<<mySolution.numRoom;
		myLogFile<<endl<<"Number of rooms: "<<mySolution.numRoom;
		cout<<endl<<"Number of sessions: "<<mySolution.numSession;
		myLogFile<<endl<<"Number of sessions: "<<mySolution.numSession;
		cout<<endl<<"Makespan: "<<mySolution.makeSpan;
		myLogFile<<endl<<"Makespan: "<<mySolution.makeSpan;
		cout<<endl<<"Missing timeslot: "<<mySolution.missingTimeslot;
		myLogFile<<endl<<"Missing timeslot: "<<mySolution.missingTimeslot;
		myLogFile.close();
		
		mySolution.WriteFileSolutionResult(sOutputFileName, sLectFileName);
		mySolution.PrintSolution();		
		//return true;
		cout<<endl<<"SCHEDULING FINISHED!";
	}
	else
	{
	myLogFile.close();
		
		//return false;
		cout<<endl<<"STOP SCHEDULING!";
	}
		
		

}
int clsMainFunc::run_many(int argc, char* argv[])
{
	int iApp=2;
	clock_t t;
	t=clock();
	//folder="D:/data/";
	filename="6L5J_1.txt";
//	sInputFileName=folder + filename + ".txt";	//15L10J_3.txt
//	sOutputFileName=folder + "output/" + filename + "_out.txt";
//	sLogFileName=folder + "output/" + filename + "_log.txt";

	iK=10;
	iNumMutation=20;
	iNumCross=10;
	iPopSize=10;

	for(int i=1;i<argc;i++)
	{
		if(strcmp(argv[i], "-infile") == 0)
		{
			i++;
			filename.assign(argv[i]);
		}
		if(strcmp(argv[i], "-method") == 0)
		{
			i++;
			iApp=atoi(argv[i]);
		}
		else if(strcmp(argv[i], "-numGer") == 0)
		{
			i++;
			iK=atoi(argv[i]);
		}
		else if(strcmp(argv[i], "-numMut") == 0)
		{
			i++;
			iNumMutation=atoi(argv[i]);
		}
		else if(strcmp(argv[i], "-numCro") == 0)
		{
			i++;
			iNumCross=atoi(argv[i]);
		}
		else if(strcmp(argv[i], "-popSize") == 0)
		{
			i++;
			iPopSize=atoi(argv[i]);			
		}		
	}
	

	string ffinput="./input/" + filename;
	char*sffinput=new char[ffinput.size()];
	strcpy(sffinput, ffinput.c_str());
	string ffoutput="./output/" + filename;

	cout<<endl<<"Input file: "<<ffinput<<endl<<"Number of generations: "<<iK;
	cout<<endl<<"Number of mutations: "<<iNumMutation<<endl<<"Number of crossover: "<<iNumCross;
	cout<<endl<<"Population size: "<<iPopSize;

	sInputFileName=ffinput;
	sOutputFileName=ffoutput + ".out";
	sLogFileName=ffoutput + ".log";
	sLectFileName=ffoutput + ".lec";

	ReadFile();
	PrintInputData();
	//CheckInputData();	

	switch(iApp)
	{
	case 0:
		HeuristicApproach();
		break;
	case 1:
		LocalSearch();
		break;
	case 2:
		GeneticAlgorithm();
		break;
	}
	//HeuristicApproach();
	//LocalSearch();
	//PseudoGeneticApproach();
	//GeneticAlgorithm();
	
	t=clock() - t;
	cout<<endl<<"Processing time: "<<(float)t/CLOCKS_PER_SEC<<" seconds";
//	getch();
	//output
	//set temporary data
	/*for(int i=0;i<iIntLect + iExtLect;i++)
		for(int j=0;j<iJury;j++)
			arrAOutput[i][j]=arrLecturer[i].arrSpecialize[j];
	for(int j=0;j<iJury;j++)
	{
		arrRoomJury[j]=1;
		arrTimeJury[j]=2;
	}*/

	//WriteFile();
	return 0;
}
void clsMainFunc::run0()
{
	int iApp=0;
	clock_t t;
	t=clock();
	string inputFileFullPath="D:\\MasterCourse\\luan van\\dang lam\\Solution_2_input.txt";
	folder=inputFileFullPath.substr(0,inputFileFullPath.find_last_of("\\"));
	filename=inputFileFullPath.substr(inputFileFullPath.find_last_of("\\")+1);
	sInputFileName=folder +"\\"+ filename;	
	sOutputFileName=folder + "\\output\\" + filename + ".out";
	sLogFileName=folder + "\\output\\" + filename + ".log";
	sLectFileName= folder +"\\ouput\\"+filename+".lec";


	//folder="D:/data/";
	//filename="input.txt";
//	sInputFileName=folder + filename + ".txt";	//15L10J_3.txt
//	sOutputFileName=folder + "output/" + filename + "_out.txt";
//	sLogFileName=folder + "output/" + filename + "_log.txt";

	iK=10;
	iNumMutation=20;
	iNumCross=10;
	iPopSize=10;

	string ffinput="D:/" + filename;
	char*sffinput=new char[ffinput.size()];
	strcpy(sffinput, ffinput.c_str());
	string ffoutput="D:/output/" + filename;

	//cout<<endl<<"Input file: "<<ffinput<<endl<<"Number of generations: "<<iK;
	cout<<endl<<"Number of mutations: "<<iNumMutation<<endl<<"Number of crossover: "<<iNumCross;
	cout<<endl<<"Population size: "<<iPopSize;

/*	sInputFileName=ffinput;
	sOutputFileName=ffoutput + ".out";
	sLogFileName=ffoutput + ".log";
	sLectFileName=ffoutput + ".lec";
*/
	ReadFile();
	PrintInputData();
	//CheckInputData();	

	switch(iApp)
	{
	case 0:
		HeuristicApproach();
		break;
	case 1:
		LocalSearch();
		break;
	case 2:
		GeneticAlgorithm();
		break;
	}
	//HeuristicApproach();
	//LocalSearch();
	//PseudoGeneticApproach();
	//GeneticAlgorithm();
	
	t=clock() - t;
	cout<<endl<<"Processing time: "<<(float)t/CLOCKS_PER_SEC<<" seconds";
//	getch();
	//output
	//set temporary data
	/*for(int i=0;i<iIntLect + iExtLect;i++)
		for(int j=0;j<iJury;j++)
			arrAOutput[i][j]=arrLecturer[i].arrSpecialize[j];
	for(int j=0;j<iJury;j++)
	{
		arrRoomJury[j]=1;
		arrTimeJury[j]=2;
	}*/

	//WriteFile();
	
}
void clsMainFunc::CreateLPSolve()
{
	ofstream myLogFile(sLogFileName);
	
	myLogFile<<"min: F1 + 2 * F2 + 3 * F3 + 4 * F4;"<<endl;
	//initialize variables
	//variable A_ist
	for (int i=0;i<iIntLect+iExtLect;i++)
		for (int s=0;s<Y/3;s++)
			for (int t=0; t<3; t++)
			{
				if (arrLecturer[i].arrAvailTime[s*3+t]==0)//available
					myLogFile<<"a"<<i<<s<<t<<" = 1;"<<endl;
				else
					myLogFile<<"a"<<i<<s<<t<<" = 0;"<<endl;
			}
	//variable p_ij
	for (int i=0;i<iIntLect+iExtLect;i++)
		for (int j=0;j<iJury;j++)
		{
			if (arrLecturer[i].arrSpecialize[j]==1||arrLecturer[i].arrSpecialize[j]==2)//supervision
				myLogFile<<"p"<<i<<j<<" = 1;"<<endl;
			else
				myLogFile<<"p"<<i<<j<<" = 0;"<<endl;
		}
	//variable e_ij
	for (int i=0;i<iIntLect+iExtLect;i++)
		for (int j=0; j<iJury; j++)
		{
			if (arrLecturer[i].arrSpecialize[j]==3)
				myLogFile<<"e"<<i<<j<<" = 1;"<<endl;
			else
				myLogFile<<"e"<<i<<j<<" = 0;"<<endl;
		}

	//constraint H1
	for (int j=0;j<iJury;j++)
	{
		for (int i=0;i<iIntLect+iExtLect;i++)
			for (int s=0;s<Y/3;s++)
				for (int t=0; t<3; t++)
					for (int r=0;r<iJury;r++)
					{
						if (i==iIntLect+iExtLect-1&&s==Y/3-1&&t==2&&r==iJury-1)
							myLogFile<<"x"<<i<<j<<0<<s<<t<<r;
						else
							myLogFile<<"x"<<i<<j<<0<<s<<t<<r<<" + ";
					}
		myLogFile<<" = 2;"<<endl;
	}
	for (int j=0;j<iJury;j++)
	{
		for (int i=0;i<iIntLect+iExtLect;i++)
			for (int s=0;s<Y/3;s++)
				for (int t=0; t<3; t++)
					for (int r=0;r<iJury;r++)
					{
						if (i==iIntLect+iExtLect-1&&s==Y/3-1&&t==2&&r==iJury-1)//last one
							myLogFile<<"x"<<i<<j<<1<<s<<t<<r;
						else
							myLogFile<<"x"<<i<<j<<1<<s<<t<<r<<" + ";
					}
		myLogFile<<" = 3;"<<endl;
	}
	//constraint H2
	for (int i=0;i<iIntLect+iExtLect;i++)
		for (int s=0;s<Y/3;s++)
			for (int t=0; t<3; t++)
			{
				
				for (int j=0;j<iJury;j++)
					for (int v=0;v<2;v++)
						for (int r=0;r<iJury;r++)
						{
							if (j==iJury-1&&v==1&&r==iJury-1)//last one
								myLogFile<<"x"<<i<<j<<v<<s<<t<<r;
							else
								myLogFile<<"x"<<i<<j<<v<<s<<t<<r<<" + ";
						}
				myLogFile<<" - ";
				for (int j=0;j<iJury;j++)
					for (int v=0;v<2;v++)
						for (int r=0;r<iJury;r++)
						{
							if (j==iJury-1&&v==1&&r==iJury-1)//last one
							{
								if (arrLecturer[i].arrAvailTime[s*3+t]==0)//available
									myLogFile<<"1 x"<<i<<j<<v<<s<<t<<r<<" = 0;"<<endl;
								else
									myLogFile<<"0 x"<<i<<j<<v<<s<<t<<r<<" = 0;"<<endl;
							}
							else
							{
								if (arrLecturer[i].arrAvailTime[s*3+t]==0)//available
									myLogFile<<"1 x"<<i<<j<<v<<s<<t<<r<<" - ";
								else
									myLogFile<<"0 x"<<i<<j<<v<<s<<t<<r<<" - ";
							}
						}
				
			}
	//constraint H3
	for (int i=0;i<iIntLect+iExtLect;i++)
		for (int s=0;s<Y/3;s++)
			for (int t=0; t<3; t++)
			{
				
				for (int j=0;j<iJury;j++)
					for (int v=0;v<2;v++)
						for (int r=0;r<iJury;r++)
						{
							if (j==iJury-1&&v==1&&r==iJury-1)//last one
								myLogFile<<"x"<<i<<j<<v<<s<<t<<r;
							else
								myLogFile<<"x"<<i<<j<<v<<s<<t<<r<<" + ";
						}
				myLogFile<<" <= 1;"<<endl;
			}
	//constraint H4
	for (int i=0;i<iIntLect+iExtLect;i++)
		for (int j=0;j<iJury;j++)
		{
			for (int v=0;v<2;v++)
				for (int s=0;s<Y/3;s++)
					for (int t=0; t<3; t++)
						for (int r=0;r<iJury;r++)
						{
							if (v==1&&s==Y/3-1&&t==2&&r==iJury-1)//last one
								myLogFile<<"x"<<i<<j<<v<<s<<t<<r;
							else
								myLogFile<<"x"<<i<<j<<v<<s<<t<<r<<" + ";
						}
			myLogFile<<" <= 1;"<<endl;
		}
	//constraint H5
	//variable h51 and h52
	for (int j=0;j<iJury;j++)
		for (int s=0;s<Y/3;s++)
			for (int t=0;t<3;t++)
				for (int r=0;r<iJury;r++)
				{
					myLogFile<<"5 h51_"<<j<<s<<t<<r<<" >= ";
					for (int i=0;i<iIntLect+iExtLect;i++)
						for (int v=0;v<2;v++)
						{
							if (i==iIntLect+iExtLect-1&&v==1) //last one
								myLogFile<<"x"<<i<<j<<v<<s<<t<<";"<<endl;
							else
								myLogFile<<"x"<<i<<j<<v<<s<<t<<" + ";
						}
						myLogFile<<"5 h51_"<<j<<s<<t<<r<<" < ";
					for (int i=0;i<iIntLect+iExtLect;i++)
						for (int v=0;v<2;v++)
						{
							if (i==iIntLect+iExtLect-1&&v==1) //last one
								myLogFile<<"x"<<i<<j<<v<<s<<t<<" + 1;"<<endl;
							else
								myLogFile<<"x"<<i<<j<<v<<s<<t<<" + ";
						}
					myLogFile<<"5 h52_"<<j<<s<<t<<r<<" >= 5 - ";
					for (int i=0;i<iIntLect+iExtLect;i++)
						for (int v=0;v<2;v++)
						{
							if (i==iIntLect+iExtLect-1&&v==1) //last one
								myLogFile<<"x"<<i<<j<<v<<s<<t<<";"<<endl;
							else
								myLogFile<<"x"<<i<<j<<v<<s<<t<<" - ";
						}
					myLogFile<<"5 h52_"<<j<<s<<t<<r<<" < 5 - ";
					for (int i=0;i<iIntLect+iExtLect;i++)
						for (int v=0;v<2;v++)
						{
							if (i==iIntLect+iExtLect-1&&v==1) //last one
								myLogFile<<"x"<<i<<j<<v<<s<<t<<" + 1;"<<endl;
							else
								myLogFile<<"x"<<i<<j<<v<<s<<t<<" - ";
						}
	
				}
	for (int j=0;j<iJury;j++)
		for (int s=0;s<Y/3;s++)
			for (int t=0;t<3;t++)
				for (int r=0;r<iJury;r++)
				{
					myLogFile<<"bin h51_"<<j<<s<<t<<r<<";"<<endl;
					myLogFile<<"bin h52_"<<j<<s<<t<<r<<";"<<endl;
				}
	for (int j=0;j<iJury;j++)
		for (int s=0;s<Y/3;s++)
			for (int t=0;t<3;t++)
				for (int r=0;r<iJury;r++)
				{
					myLogFile<<"h51_"<<j<<s<<t<<r<<" + h52_"<<j<<s<<t<<r<<" = 1;"<<endl;
				}
	//constraint H6
	for (int j=0;j<iJury;j++)
		for (int s=0;s<Y/3;s++)
			for (int t=0; t<3; t++)
				for (int r=0;r<iJury;r++)
				{
					for (int i=0;i<iIntLect+iExtLect;i++)
						for (int v=0;v<2;v++)
						{
							if (i==iIntLect+iExtLect-1&&v==1)
								myLogFile<<"x"<<i<<j<<v<<s<<t<<r;
							else
								myLogFile<<"x"<<i<<j<<v<<s<<t<<r<<" + ";
						}
					myLogFile<<" <= 5;"<<endl;
				}
	//constraint H7
	for (int i=iIntLect;i<iIntLect+iExtLect;i++)
		for (int j=0;j<iJury;j++)
		{
			for (int v=0;v<2;v++)
				for (int s=0;s<Y/3;s++)
					for (int t=0; t<3; t++)
						for (int r=0;r<iJury;r++)
						{
							if (v==1&&s==Y/3-1&&t==2&&r==iJury-1)//last one
								myLogFile<<"x"<<i<<j<<v<<s<<t<<r;
							else
								myLogFile<<"x"<<i<<j<<v<<s<<t<<r<<" + ";
						}
			myLogFile<<" >= 2;"<<endl;
		}
	//constraint H8
	for (int i=0;i<iIntLect+iExtLect;i++)
		for (int j=0;j<iJury;j++)
		{
		//	*1 if supervision, else *0
			for (int v=0;v<2;v++)
				for (int s=0;s<Y/3;s++)
					for (int t=0; t<3; t++)
						for (int r=0;r<iJury;r++)
						{
							if (v==1&&s==Y/3-1&&t==2&&r==iJury-1)//last one
							{
								if (arrLecturer[i].arrSpecialize[j]==1||arrLecturer[i].arrSpecialize[j]==2)//supervision
									myLogFile<<"1 x"<<i<<j<<v<<s<<t<<r;
								else
									myLogFile<<"0 x"<<i<<j<<v<<s<<t<<r;
							}
							else
							{
								if (arrLecturer[i].arrSpecialize[j]==1||arrLecturer[i].arrSpecialize[j]==2)//supervision
									myLogFile<<"1 x"<<i<<j<<v<<s<<t<<r<<" + ";
								else
									myLogFile<<"0 x"<<i<<j<<v<<s<<t<<r<<" + ";
							}
						}
			myLogFile<<" = 0;"<<endl;
		}
	//constraint H9
	//constraint H10
	for (int i=0;i<iIntLect+iExtLect;i++)
		for (int j=0;j<iJury;j++)
		{
			//myLogFile<<"( 1 - e"<<i<<j<<" ) * ( ";
			for (int s=0;s<Y/3;s++)
				for (int t=0; t<3; t++)
					for (int r=0;r<iJury;r++)
						{
							if (s==Y/3-1&&t==2&&r==iJury-1)//last one
								myLogFile<<"x"<<i<<j<<0<<s<<t<<r;
							else
								myLogFile<<"x"<<i<<j<<0<<s<<t<<r<<" + ";
						}
			myLogFile<<" - ";
			for (int s=0;s<Y/3;s++)
				for (int t=0; t<3; t++)
					for (int r=0;r<iJury;r++)
						{
							if (s==Y/3-1&&t==2&&r==iJury-1)//last one
								myLogFile<<"e"<<i<<j<<" x"<<i<<j<<0<<s<<t<<r;
							else
								myLogFile<<"e"<<i<<j<<" x"<<i<<j<<0<<s<<t<<r<<" + ";
						}
			myLogFile<<" = 0;"<<endl;
		}
	//Objective function
	//room
	myLogFile<<"5 F1 = ";
	for (int r=0;r<iJury;r++)
	{
		for (int i=0;i<iIntLect+iExtLect;i++)
			for (int j=0;j<iJury;j++)
				for (int v=0;v<2;v++)
					for (int s=0;s<Y/3;s++)
						for (int t=0; t<3; t++)
						{
							if (r==iJury-1&&i==iIntLect+iExtLect-1&&j==iJury-1&&v==1&&s==Y/3-1&&t==2)//last one
								myLogFile<<"x"<<i<<j<<v<<s<<t<<r<<";"<<endl;
							else
								myLogFile<<"x"<<i<<j<<v<<s<<t<<r<<" + ";
						}
	}
	//all session
	//s_i
	for (int i=0;i<iIntLect+iExtLect;i++)
	{
		
		for (int s=0;s<Y/3;s++)
		{
			myLogFile<<"3 temp"<<i<<s<<" >= ";
			for (int j=0;j<iJury;j++)
					for (int v=0;v<2;v++)
						for (int t=0; t<3; t++)
							for (int r=0;r<iJury;r++)			
						{
							if (j==iJury-1&&v==1&&t==2&&r==iJury-1)//last one
								myLogFile<<"x"<<i<<j<<v<<s<<t<<r<<";"<<endl;
							else
								myLogFile<<"x"<<i<<j<<v<<s<<t<<r<<" + ";
						}
			myLogFile<<"3 temp"<<i<<s<<" < ";
			for (int j=0;j<iJury;j++)
					for (int v=0;v<2;v++)
						for (int t=0; t<3; t++)
							for (int r=0;r<iJury;r++)			
						{
							if (j==iJury-1&&v==1&&t==2&&r==iJury-1)//last one
								myLogFile<<"x"<<i<<j<<v<<s<<t<<r<<" + 1;"<<endl;
							else
								myLogFile<<"x"<<i<<j<<v<<s<<t<<r<<" + ";
						}
		}
	}
	myLogFile<<"F2 = ";
	for (int i=0;i<iIntLect+iExtLect;i++)
	{
		
		for (int s=0;s<Y/3;s++)
			
			{
				if (i==iIntLect+iExtLect-1&&s==Y/3-1)
					myLogFile<<"temp"<<i<<s<<";"<<endl;
				else
				myLogFile<<"temp"<<i<<s<<" + ";
			}
		
	}
	//external session
	myLogFile<<"F3 = ";
	for (int i=iIntLect;i<iIntLect+iExtLect;i++)
	{
		
		for (int s=0;s<Y/3;s++)
			
			{
				if (i==iIntLect+iExtLect-1&&s==Y/3-1)
					myLogFile<<"temp"<<i<<s<<";"<<endl;
				else
				myLogFile<<"temp"<<i<<s<<" + ";
			}
		
	}
	//missing timeslot
	myLogFile<<"F4 = 3 * F1 - "<<iJury<<" ;"<<endl;
		//variable x_ijvstr
	for (int i=0;i<iIntLect+iExtLect;i++)
		for (int j=0;j<iJury;j++)
			for (int v=0;v<2;v++)
				for (int s=0;s<Y/3;s++)
					for (int t=0; t<3; t++)
						for (int r=0;r<iJury;r++)
						{
							
							myLogFile<<"bin x"<<i<<j<<v<<s<<t<<r<<";"<<endl;
						}
	//variable temp_is
	for (int i=0;i<iIntLect+iExtLect;i++)
				for (int s=0;s<Y/3;s++)
						{
							
							myLogFile<<"bin temp"<<i<<s<<";"<<endl;
						}
	
	myLogFile.close();
}

void clsMainFunc::run(char* inputFileFullPath)
{
	int iApp=2;
	clock_t t;
	t=clock();
	string inputFile(inputFileFullPath);
	//inputFileFullPath="D:\\MasterCourse\\luan van\\dang lam\\Solution_2_input.txt";
	folder=inputFile.substr(0,inputFile.find_last_of("\\"));
	filename=inputFile.substr(inputFile.find_last_of("\\")+1);
	sInputFileName=folder +"\\"+ filename;	
	sOutputFileName=folder + "\\output\\" + filename + ".out";
	sLogFileName=folder + "\\output\\" + filename + ".log";
	sLectFileName= folder +"\\output\\"+filename+".lec";

	iK=15;
	iNumMutation=30;
	iNumCross=10;
	iPopSize=10;

	string ffinput="D:/" + filename;
	char*sffinput=new char[ffinput.size()];
	strcpy(sffinput, ffinput.c_str());
	string ffoutput="D:/output/" + filename;

	//cout<<endl<<"Input file: "<<ffinput<<endl<<"Number of generations: "<<iK;
	//cout<<endl<<"Number of mutations: "<<iNumMutation<<endl<<"Number of crossover: "<<iNumCross;
	//cout<<endl<<"Population size: "<<iPopSize;

	//sInputFileName=ffinput;
	//sOutputFileName=ffoutput + ".out";
	//sLogFileName=ffoutput + ".log";
	//sLectFileName=ffoutput + ".lec";

	ReadFile();
	CreateLPSolve();
	/*
	PrintInputData();

	
	switch(iApp)
	{
	case 0:
		HeuristicApproach();
		break;
	case 1:
		LocalSearch();
		break;
	case 2:
		GeneticAlgorithm();
		break;
	}
	t=clock() - t;
	cout<<endl<<"Processing time: "<<(float)t/CLOCKS_PER_SEC<<" seconds";
	*/

}
void clsMainFunc::runF(char* inputFileFullPath)
{
	int iApp=0;
	clock_t t;
	t=clock();
	string inputFile(inputFileFullPath);
	//inputFileFullPath="D:\\MasterCourse\\luan van\\dang lam\\Solution_2_input.txt";
	folder=inputFile.substr(0,inputFile.find_last_of("\\"));
	filename=inputFile.substr(inputFile.find_last_of("\\")+1);
	sInputFileName=folder +"\\"+ filename;	
	sOutputFileName=folder + "\\output\\" + filename + ".out";
	sLogFileName=folder + "\\output\\" + filename + ".log";
	sLectFileName= folder +"\\output\\"+filename+".lec";

	iK=15;
	iNumMutation=30;
	iNumCross=10;
	iPopSize=10;

	string ffinput="D:/" + filename;
	char*sffinput=new char[ffinput.size()];
	strcpy(sffinput, ffinput.c_str());
	string ffoutput="D:/output/" + filename;

	//cout<<endl<<"Input file: "<<ffinput<<endl<<"Number of generations: "<<iK;
	cout<<endl<<"Number of mutations: "<<iNumMutation<<endl<<"Number of crossover: "<<iNumCross;
	cout<<endl<<"Population size: "<<iPopSize;

	//sInputFileName=ffinput;
	//sOutputFileName=ffoutput + ".out";
	//sLogFileName=ffoutput + ".log";
	//sLectFileName=ffoutput + ".lec";

	ReadFile();
	PrintInputData();

	
	switch(iApp)
	{
	case 0:
		HeuristicApproach();
		break;
	case 1:
		LocalSearch();
		break;
	case 2:
		GeneticAlgorithm();
		break;
	}
	t=clock() - t;
	cout<<endl<<"Processing time: "<<(float)t/CLOCKS_PER_SEC<<" seconds";
	
}