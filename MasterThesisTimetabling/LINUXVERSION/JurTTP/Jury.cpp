#include "Jury.h"
void clsJury::InitData()
{
	for(int i=0;i<MAXPERIOD;i++)
		arrAvailPeriod[i]=true;
	iNumExtLect=0;
}
//Reset information after this jury is used
void clsJury::ResetData()
{
	for(int i=0;i<MAXPERIOD;i++)
		arrAvailPeriod[i]=true;
}
//check whether the lecturer IDLecturer have joined this jury or not
bool clsJury::IsLecturerJoinJury(int IDLecturer)
{
	if(advisor.IdLect == IDLecturer || 
			arrReviewers[0].IdLect == IDLecturer || 
			arrReviewers[1].IdLect == IDLecturer ||
			arrOtherMembers[0].IdLect == IDLecturer ||
			arrOtherMembers[1].IdLect == IDLecturer)
		return true;
	return false;		
}
//check whether the lecturer IDLecturer have joined this jury or not
int clsJury::GetRoleOfLecturerInJury(int IDLecturer)
{
	if(advisor.IdLect == IDLecturer)
		return 1;
	if(arrReviewers[0].IdLect == IDLecturer || arrReviewers[1].IdLect == IDLecturer)
		return 3;
	if(arrOtherMembers[0].IdLect == IDLecturer)
		return 4;
	if(arrOtherMembers[1].IdLect == IDLecturer)
		return 5;
	return 0;
}

//Check whether two juries having the same lecturer	
bool clsJury::IsHavingSameLecturers(clsJury jury_x)
{
	if(this->IsLecturerJoinJury(jury_x.advisor.IdLect) ||
			this->IsLecturerJoinJury(jury_x.arrReviewers[0].IdLect) ||
			this->IsLecturerJoinJury(jury_x.arrReviewers[1].IdLect) ||
			this->IsLecturerJoinJury(jury_x.arrOtherMembers[0].IdLect) ||
			this->IsLecturerJoinJury(jury_x.arrOtherMembers[1].IdLect))
		return true;
	return false;
}
