#pragma once
#ifdef MYDLL_EXPORTS
#define MYDLL_API __declspec(dllexport)
#else
#define MYDLL_API __declspec(dllimport)
#endif
#include "GeneticAlg.h"


class clsMainFunc
{
public:
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
	clsSolution* finalSolution;// Final solution to export
public:
	void AnalyseSubstring(string sStr, int iLect);//Analyse a substring, used in ReadFile() function
	void ReadFile();//Read input file
	void WriteFile();//Write output file
	void PrintInputData();//Print input data
	void LocalSearch();//Local search algorithm, print two solutions for two objectives
	void HeuristicApproach();//Heuristic approach
	void GeneticAlgorithm();//Genetic algorithm
	int run_many(int argc, char* argv[]);//run solution
	void run0();
	void run(char* inputFileFullPath);
	void runF(char* inputFileFullPath);
};