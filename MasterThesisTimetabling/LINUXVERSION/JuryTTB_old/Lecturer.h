#include "Utilities.h"
#include <iostream>
using namespace std;

class clsLecturer
{
public:
	int IdLect;//Id is index of the lecturers array
	bool IsExternalLect; //is external lecturer or not
	int arrSpecialize[MAXJURY];//role of lecturer with Project: 0, 1, 2, 3.
						//0: could be member of jury
						//1: be advisor, must be member of jury
						//2: be co-avisor, but not be member of jury
						//3: could be reviewer of jury
	int arrAvailTime[MAXPERIOD];//availability time of lecturer,=0: free; =1: unavailability, =2: unavailability because have joined a jury
	int numAvailTime;//Number of availability time of lecturer
	int Y;
	
	//Specific variable, depend on a given solution
	int iSessionJoined;
	
public:
	void InitData();
	void ResetData();//Reset information afer used this lecturer to solutions.
};