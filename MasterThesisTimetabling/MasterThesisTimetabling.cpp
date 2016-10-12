
#include "Utilities.h"
#include<iostream>
#include<sstream>
#include<fstream>
#include<istream>
#include<string>
#include<string.h>
#include<time.h>
//#include<conio.h>
using namespace std;
//Input variables
int iIntLect; //Number of internal LECTURERS
int iExtLect; //Number of external LECTURERS
int iJury; //Number of JURIES
int X, Y; //start and end PERIOD
std::string sInputFileName, sOutputFileName, sLogFileName, sLectFileName;
string filename;
string folder;

int iK, iNumMutation, iNumCross, iPopSize;
//int arrAInput[MAXLECT][MAXJURY]; //Input maxtrix LECTURERS with JURIES and ROLES
//bool arrAvail[MAXLECT][MAXPERIOD]; //Availability or unavailability of LECTURERS at a given PERIOD
									   //=true if lecturer is free.

clsLecturer arrLecturer[MAXLECT];//list of lecturers

//Output variables
int arrAOutput[MAXLECT][MAXJURY]; //Output maxtrix LECTURERS with JURIES and ROLES
int arrTimeJury[MAXJURY]; //Time for JURIES
int arrRoomJury[MAXJURY]; //Room for JURIES
clock_t t;
//==================================================================//
//Functions

//Analyse a substring, used in ReadFile() function
void AnalyseSubstring(string sStr, int iLect)
{
	unsigned dash_pos=sStr.find("-");
	if(dash_pos == std::string::npos) //these is not dash
	{
		stringstream ss(sStr);
		int iPer;
		ss>>iPer;
		iPer--;
		arrLecturer[iLect].arrAvailTime[iPer]=1;
	}
	else //These is a dash
	{
		int iPer1, iPer2;
		stringstream ss1(sStr.substr(0, dash_pos));
		ss1>>iPer1;
		iPer1=iPer1 -1;

		if(sStr[sStr.size()-1] != '-') 
		{
			stringstream ss2(sStr.substr(dash_pos + 1, sStr.size() - dash_pos -1));
			ss2>>iPer2;
			iPer2--;
		}
		else //case: example "8-"
		{
			iPer2=MAXPERIOD-1;
		}
		for(int i=iPer1;i<=iPer2;i++)
			arrLecturer[iLect].arrAvailTime[i]=1;

	}

}
//Read input file
void ReadFile()
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
		for (int i=0;i<21;i++)
			cout<<endl<<"\t"<<arrLecturer[10].arrAvailTime[i];
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
void WriteFile()
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
void PrintInputData()
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
//check input data
void CheckInputData()
{
}
//Check constraints
int CheckConstraints(clsSolution mySolution)
{

	return 0;
}
//Pseudo-genetic algorithm, minimize number of rooms
void PseudoGeneticApproach()
{
	ofstream myLogFile(sLogFileName);
	clsSolution mySolution_room;
	int mythreshold=5;
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
		if(tempSolution->DoScheduling_Genetic() == true && tempSolution->makeSpan <= Y)//Y=MAXPERIOD
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
		//mySolution_room.WriteFileSolutionResult("D:/output_room.txt");
		mySolution_room.PrintSolution();

		cout<<endl<<"SCHEDULING FINISHED!";
		myLogFile<<endl<<"SCHEDULING FINISHED!";
	}

}
//Local search algorithm, print two solutions for two objectives
void LocalSearch()
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
void HeuristicApproach()
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
		for (int i=0;i<mySolution.iJury;i++)
			cout<<"\t"<<mySolution.arrJury[i].IdPeriod;
	}
	else
	{
		cout<<endl<<"STOP SCHEDULING!";
		myLogFile<<endl<<"STOP SCHEDULING!";
	}

}
//Genetic algorithm
bool GeneticAlgorithm()
{
	ofstream myLogFile(sLogFileName);
	clsGeneticAlg*myGASolution;
	myGASolution=new clsGeneticAlg();
	int threNumOfPro=iJury; //not be used
	myGASolution->InitData(iPopSize, iNumMutation, 10, iK, iIntLect, iExtLect, iJury, Y, arrLecturer, threNumOfPro);
	if(myGASolution->DoGAScheduling() == true)
	{
		clsSolution mySolution;
		mySolution=*myGASolution->arrPopOfSol[0];
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
		cout<<endl<<"Middle missing timeslot: "<<mySolution.middleMissingTimeslot;
		myLogFile<<endl<<"Middle missing timeslot: "<<mySolution.middleMissingTimeslot;
		t=clock() - t;
		cout<<endl<<"Processing time: "<<(float)t/CLOCKS_PER_SEC<<" seconds";
		myLogFile<<endl<<"Processing time: "<<(float)t/CLOCKS_PER_SEC<<" seconds";
		myLogFile.close();
		mySolution.WriteFileSolutionResult(sOutputFileName, sLectFileName);
		mySolution.PrintSolution();

		cout<<endl<<"SCHEDULING FINISHED!";
		return true;
	}
	else
	{
			cout<<endl<<"STOP SCHEDULING!";
			return false;
	}
		


}
void Run(string inputFileFullPath,int iIK, int iPOP, int iCross, int iMut, int iCount)
{
	int iApp=0;
	
	t=clock();
	
	
	iK=iIK;
	iPopSize=iPOP;
	iNumCross=iCross;
	iNumMutation=iMut;
	stringstream ss;
	ss<<(iK);
	ss<<(iPopSize);
	ss<<(iNumCross);
	ss<<(iNumMutation);
	ss<<(iCount);
	string sInformation=ss.str();
	folder=inputFileFullPath.substr(0,inputFileFullPath.find_last_of("\\"));
	folder=folder.substr(0,folder.find_last_of("\\"));
	filename=inputFileFullPath.substr(inputFileFullPath.find_last_of("\\")+1);
	

	sInputFileName=folder +"\\input\\"+ filename;
	sOutputFileName=folder + "\\output\\" + filename +".out";
	sLogFileName=folder + "\\output\\" + filename+".log";
	sLectFileName= folder +"\\output\\"+filename+ ".lec";

	cout<<endl<<"Input file: "<<filename<<endl<<"Number of generations: "<<iK;
	cout<<endl<<"Number of mutations: "<<iNumMutation<<endl<<"Number of crossover: "<<iNumCross;
	cout<<endl<<"Population size: "<<iPopSize;

	ReadFile();
	PrintInputData();

	bool flag=false;
	switch(iApp)
	{
	case 0:
		HeuristicApproach();
		break;
	case 1:
		LocalSearch();
		break;
	case 2:
		flag=GeneticAlgorithm();
		break;
	}

}
int main(int argc, char* argv[])
{
	//vector<string> arrFileName;
	Run("D:\\input\\Solution_11_input.txt",30,30,30,50,0);
}