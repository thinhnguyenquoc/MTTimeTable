#include "Lecturer.h"
//Init data, after reading file
void clsLecturer::InitData()
{
	int count=0;
	for(int j=0;j<=this->Y;j++) //Y = MAXPERIOD
		if(this->arrAvailTime[j] == 0)
			count++;
	this->numAvailTime=count;
	this->iSessionJoined=0;
}
//Reset information afer used this lecturer to solutions.
void clsLecturer::ResetData()
{
	for(int j=0;j<=this->Y;j++) //Y = MAXPERIOD
		if(this->arrAvailTime[j] == 2)
			this->arrAvailTime[j]=0;
	this->iSessionJoined=0;
}

