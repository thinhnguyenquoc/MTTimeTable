#include "Jury.h"
#include "Utilities.h"
#include <iostream>
#include<sstream>
#include<fstream>
#include<istream>
#include<string>
#include<vector>
#include<algorithm>
#include<utility>
#include <time.h>
//#include "windows.h"
using namespace std;
class clsSolution
{
public:
	int iIntLect; //Number of internal LECTURERS
	int iExtLect; //Number of external LECTURERS
	int iJury; //Number of JURIES
	int Y;//Maximum number of PERIODS
	clsLecturer arrLecturer[MAXLECT];//list of lecturers
	clsJury arrJury[MAXJURY];//list of juries
	int iThreNumOfProj;//maximum number of jury that each lecturer could join.
	int arrNumProjofLect[MAXLECT];//number of jury that each lecturer joined.
	int arrThreNumProjOfLect[MAXLECT];//The maximum number of jury that each lecturer can be joined

	int numUnFesibleJury;//Number of unfesible jury
	vector<int>vcrUnFesibleJury;//list of unfesible jury in solution
	//additional variables
	bool arrIsChoosePeriod[MAXJURY];//array of flags to show whether period of jury is chosen or not.
	int iIDRoom; //this template variable is used to set Room for jury
	int numRoom; //number of room used
	int numSession; //number of sesion of all lecturer
	int exSession; //number of sessions of external lecturer
	int makeSpan; //the max of finished time of solution
	int missingTimeslot; //the timeslot that interrupt the schedule
	int middleMissingTimeslot; //the middle missing timeslot
	int arrAvailTemp[MAXPERIOD];
	//ofstream myLogFile;//file pointer for logging
public:
	void InitData();
	void InitData(int i_Jury, int i_IntLect, int i_ExtLect, int Y, clsLecturer arr_Lecturer[MAXLECT], int i_ThreNumOfProj);
	void PrintSolution();//Print current solution
	void SetLectures(clsLecturer arrLects[MAXLECT]); //Set list of lecturers
	void ResetData();//Reset data from executed solution
	
	bool ChooseLecturerForJuries();//Task 1: Choose lecturers for juries
	bool ChooseAdvisor();//Step 1: Choose advisor for juries
	bool ChooseReviewers(); //Step 3: Choose reviewers for juries
	int ChooseReviewerForOneJury_1(int IDJury, int thresholdNumOfJury, int& firstReviewer);//Step 2a: Choose first reviewer for ONE jury
	int ChooseReviewerForOneJury_2(int IDJury, int thresholdNumOfJury, int& secondReviewer, int iCase);//Step 2b: Choose second reviewer for ONE jury, bCase=0: Must choose internal lecturer, bCase=2: Must choose external lecturer, bCase=1: do not need more condition
	//bool ChooseOthermembers();//Step 3: Choose other members for juries
	bool ChooseChairsForNotCompletedJuries();//choose chair for 3-member juries
	int ChooseChairForOneNotCompletedJury(int IDJury, int thresholdNumOfJury, int& chairID, int iCase,int iSpec);
	bool ChooseSecretariesForNotCompletedJuries();//choose secretaries for 3-member juries
	int ChooseSecretaryForOneNotCompletedJury(int IDJury, int thresholdNumOfJury, int& secretID, int iCase);
	bool ChooseChairs();//Step 2: Choose chair for juries
	int ChooseChairForOneJury(int IDJury, int thresholdNumOfJury, int& chairID, int iCase,int iSpec);//Step 2a: Choose chair for one jury, iCase:number of external,iSpec: 0 or 3 _ specilize in jury
	bool ChooseSecretaries();//Step 4: Choose secretary for juries
	int ChooseSecretaryForOneJury(int IDJury, int thresholdNumOfJury, int& secretID, int iCase);//Step 4a: Choose secretary for ONE jury, iCase =1, or iCase = 2 (number of external lecturers)
	//int ChooseOtherMemberForOneJury_1b(int IDJury, int thresholdNumOfJury, int& firstLect);//Step 3b: Choose 1 lecturer for ONE jury
	//int ChooseOtherMemberForOneJury_1(int IDJury, int thresholdNumOfJury, int& firstLect);//Step 3a: Choose 1 lecturer for ONE jury
	//int ChooseOtherMemberForOneJury_2(int IDJury, int thresholdNumOfJury, int& secondLect);//Step 3b: Choose last lecturer for ONE jury
	bool Choose3MembersForJuries();//Task 1_new: choose 3 members for juries
	bool ChoosePeriodForNotCompletedJuries();//Task 2_new: choose period for 3-member juries
	bool ChoosePeriodForJuries();//Task 2: Choose period for juries
	void FindAvailabilityPeriodForJuries();//Step 1 - task 2: Find availability period for juries
	void FindAvailabilityPeriodForNotCompletedJuries();//Step 1 - task 2: find availability period for 3-member juries
	vector<int> FindJuriesProfIsIn(int ProfID); //find a list of juries that professor has to join
	vector<int> FindJuriesProfIsIn(int ProfID,clsSolution Sol); //find a list of juries that professor has to join
	int FindSoonestPeriodForNotCompletedJury(int JuryID);//Find the soonest period that a 3-member jury can be hold
	int FindSoonestPeriod(int JuryID);//Find the soonest period that a jury can be hold
	bool CheckIfProfIsStillAvailable(int JuryID, int iPeriod, int LectID);//check if LectID is still free at iPeriod
	bool CheckIfPeriodIsOKForNotCompletedJury(int JuryID, int iPeriod);//check if JuryID can be hold at iPeriod, note that iPeriod is in the list of JuryID's availability period
	
	bool CheckIfPeriodIsOK(int JuryID, int iPeriod);//check if JuryID can be hold at iPeriod, note that iPeriod is in the list of JuryID's availability period
	bool FindPeriodForNotCompletedJuries();//Step 2 - task 2: find suitable period for 3-member juries
	bool FindPeriodForJuries();//Step 2 - task 2: Find suitable period for juries
	bool FindPeriodForJuries1();
	bool FindPeriodForJuries2();//graph idea
	bool FindPeriodForNotCompletedJuries_Step1(vector<int> ListOfProfJoining);
	bool FindPeriodForNotCompletedJuries_Step2(vector<int> ListOfProfJoining);
	bool FindPeriodForJuries_Step1(vector<int> ListOfProfJoining);
	bool FindPeriodForJuries_Step2(vector<int> ListOfProfJoining);
	int FindMinPeriod();//Step 2 - task 2 (a) find the minimize period of juries those period is not chosen, return ID of jury
	void SolveCoincideJuries(int arrTempIDJury[MAXJURY], int n);//Step 2 - task 2 (b) Solve the case which there are more than 1 jury at the same period
	
	void ChooseRoomForJuries();//Task 3: Choose room for juries
	bool DoScheduling_3Members();//Start to schedule 3 members first, then the rest of them
	bool DoScheduling();//Start to schedule
	void UpdateFinalState();//Update some variables after do scheduling
	void WriteFileSolutionResult(std::string sOutputFileName, string sLectFileName);//Write the solution result to file
	void WriteProfSchedule(std::string sLectFileName);
	int CalculateObjectiveValue();//Calculate the objective value of solution

	bool CheckConstrainstsAfterTask1();//Check constrainsts after task 1 has finished
	//void ChooseLecturerWithoutConstrainsts();//Choose lecture for juries without using constrainsts
	bool DoScheduling_Genetic();//Start to schedule, using mutation
	bool FindPeriodToReplace_MA(int JuryID);//find period to move jury to, used for MA
	bool FindPeriodToReplace(int JuryID);//find period to move jury to (for case 3 member is already selected, but cannot choose the rest member)
	bool FindLecturerToReplace(int IDJury, int flags, bool IsExternal);//Find another lecturer to replace other member 0 (flag=0), or other member 1 (flag=1)

	bool FindChairToReplace(int IDJury,int& myCount);
	bool FindSecretaryToReplace(int IDJury,int& myCount);
	bool FindReviewerToReplace(int IDJury, int iRevi,int& myCount);//iRevi=0 or 1
	void MakeChildSolution (clsSolution* parentSol2, clsSolution& childSolution, int crossPart,int crossQuantity);
	void CopySolution(clsSolution& newSolution)
	{
		newSolution.iJury=iJury;
		newSolution.iIntLect=iIntLect;
		newSolution.iExtLect=iExtLect;
		newSolution.Y=Y;
		newSolution.SetLectures(arrLecturer);				
		for(int i=0;i<iJury;i++)
		{
			newSolution.arrJury[i]=arrJury[i];
			newSolution.arrIsChoosePeriod[i]=arrIsChoosePeriod[i];
		}
		newSolution.iThreNumOfProj=iThreNumOfProj;
		for(int i=0;i<iIntLect + iExtLect;i++)
		{
			newSolution.arrNumProjofLect[i]=arrNumProjofLect[i];
			newSolution.arrThreNumProjOfLect[i]=arrThreNumProjOfLect[i];
		}
		newSolution.iIDRoom=iIDRoom;
		newSolution.numRoom=numRoom;
		newSolution.numSession=numSession;
		newSolution.exSession=exSession;
		newSolution.makeSpan=makeSpan;
		newSolution.missingTimeslot=missingTimeslot;
		newSolution.middleMissingTimeslot=middleMissingTimeslot;
		newSolution.numUnFesibleJury=numUnFesibleJury;
		for(int i=0;i<numUnFesibleJury;i++)
			newSolution.vcrUnFesibleJury.push_back(vcrUnFesibleJury[i]);
	}
};
