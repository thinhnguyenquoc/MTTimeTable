#pragma once
//#include "Utilities.h"
#include "Lecturer.h"
#include <iostream>
using namespace std;

class clsJury
{
public:
	int IdJury;//is the index of Jury array
	clsLecturer advisor[2];
	clsLecturer arrReviewers[2];
	clsLecturer arrOtherMembers[3];
	int iNumExtLect; //number of external lecturers
	int IdRoom;//Id of room for jury
	int IdPeriod; //Id of period for jury
	bool arrAvailPeriod[MAXPERIOD];//=true if the period at when 5 lecturers are free
public:
	void InitData();
	void ResetData();//Reset information after this jury is used
	bool IsLecturerJoinJury(int IDLecturer);//check whether the lecturer IDLecturer have joined this jury or not
	bool IsHavingSameLecturers(clsJury jury_x);//Check whether two juries having the same lecturer	
	int GetRoleOfLecturerInJury(int IDLecturer);//Get role of lecturer in jury
												//0: not join, 1: advisor, 2: co advisor, 3: Reviewer,
												//4, 5, 6: Other member

};