#include "stdafx.h"
#include"GeneticAlg.h"
#include<algorithm>
#include<utility>
#include<vector>
#include <time.h>
//#include "windows.h"

//Initialize data


	void clsGeneticAlg::InitData(int i_PopSize, int i_NumMuta, int i_NumCros, int i_K, int i_IntLect, int i_ExtLect, int i_Jury, int Y, clsLecturer arr_Lecturer[MAXLECT], int i_ThreNumOfProj)
	{
		this->iPopSize=i_PopSize;
		this->iNumMuta=i_NumMuta;
		this->iNumCros=i_NumCros;
		this->iK=i_K;

		this->iIntLect=i_IntLect;
		this->iExtLect=i_ExtLect;
		this->iJury=i_Jury;
		this->Y=Y;
		this->iThreNumOfProj=i_ThreNumOfProj;
		for(int i=0;i<iIntLect + iExtLect;i++)
			this->arrLecturer[i]=arr_Lecturer[i];
		this->myCount=0;
	}
	//Start to schedule
	bool clsGeneticAlg::DoGAScheduling()
	{
		//Generate inital generation
		
		if(GeneratingInitalGeneration()== false)
			return false;

		//Begin offspring circle

		for(int iIterative=0;iIterative<this->iK;iIterative++) //terminate condition
		{
			//Choose all as parents
			//Crossover n times to generate n offsprings
			for(int i=0;i<10;i++)
			{
				Crossover(arrPopOfSol);

			}
			//Caculate value
			int sizePop=this->arrPopOfSol.size();
			for(int i=0;i<sizePop;i++)
			{
				if(this->arrPopOfSol[i]->ChoosePeriodForJuries() == true)//feasible solution
				{
					this->arrPopOfSol[i]->ChooseRoomForJuries();
					this->arrPopOfSol[i]->UpdateFinalState();
					this->arrPopOfSol[i]->CalculateObjectiveValue();
				}
			}
			//Mutation all of them (both parents and offsprings)


			for(int p=0;p<sizePop;p++)
			{
				clsSolution* copySolution=new clsSolution;
				this->arrPopOfSol[p]->CopySolution(*copySolution);
				//Reset some states for executed and feasible solutions
				if(copySolution->numUnFesibleJury ==0)
				{
					copySolution->ResetData();
					this->Mutation(copySolution, this->arrPopOfSol);
				}
				else
				{
					this->Mutation_Controled(copySolution, this->arrPopOfSol);
				}
				delete copySolution;
			}
			//Remove bad solutions to return to parents' population size
			//Calculate results for feasible solutions		
			for(int i=sizePop;i<this->arrPopOfSol.size();i++)
			{
				if(this->arrPopOfSol[i]->ChoosePeriodForJuries() == true)//feasible solution
				{
					this->arrPopOfSol[i]->ChooseRoomForJuries();
					this->arrPopOfSol[i]->UpdateFinalState();
					this->arrPopOfSol[i]->CalculateObjectiveValue();
				}
			}
			this->SelectingBestSolutions();
			//Try to improve solutions using Local Search
			for (int i=0; i<arrPopOfSol.size();i++)
			{
				LocalSearch(arrPopOfSol[i]);
			}
		}
		this->SelectingBestSolutions();
		if(this->arrPopOfSol[0]->numUnFesibleJury !=0 || this->arrPopOfSol[0]->numRoom == 0)
		{
			int numIter=0;
			while((this->arrPopOfSol[0]->numUnFesibleJury !=0 || this->arrPopOfSol[0]->numRoom == 0) || numIter<1000)
			{
				//continue mutation
				numIter++;
				int sizePop=this->arrPopOfSol.size();
				for(int p=0;p<sizePop;p++)
				{
					clsSolution* copySolution=new clsSolution;
					this->arrPopOfSol[p]->CopySolution(*copySolution);
					//Reset some states for executed and feasible solutions
					this->Mutation_Controled(copySolution, this->arrPopOfSol);
					delete copySolution;
				}

				//Calculate results for feasible solutions		
				for(int i=sizePop;i<this->arrPopOfSol.size();i++)
				{
					if(this->arrPopOfSol[i]->ChoosePeriodForJuries() == true)//feasible solution
					{
						this->arrPopOfSol[i]->ChooseRoomForJuries();
						this->arrPopOfSol[i]->UpdateFinalState();
						this->arrPopOfSol[i]->CalculateObjectiveValue();
					}
				}
				this->SelectingBestSolutions();

			}

		}

		if(this->arrPopOfSol[0]->numUnFesibleJury ==0 && this->arrPopOfSol[0]->numRoom != 0)
		{

			return true;
		}
		return false;
	}
	//Local Search to improve one child solution
	bool clsGeneticAlg::LocalSearch(clsSolution* &child_Sol)
	{
		vector<clsSolution*> arr_child_Sol;
		clsSolution* tempSol= new clsSolution();
		child_Sol->CopySolution(*tempSol);
		if (tempSol->numUnFesibleJury==0) //feasible solution
		{
			tempSol->ResetData();
			Mutation(tempSol, arr_child_Sol);
		}
		else
			Mutation_Controled(tempSol, arr_child_Sol);
		//Caculate the objective value
		for(int i=0;i<arr_child_Sol.size();i++)
		{
			if(arr_child_Sol[i]->ChoosePeriodForJuries() == true)//feasible solution
			{
				arr_child_Sol[i]->ChooseRoomForJuries();
				arr_child_Sol[i]->UpdateFinalState();
				arr_child_Sol[i]->CalculateObjectiveValue();
			}
		}
		//Sorting the solutions in offsprings
		for(int i=0;i<arr_child_Sol.size()-1;i++)
			for(int j=i+1;j<arr_child_Sol.size();j++)
				if(CompareTwoSolutions(arr_child_Sol[i], arr_child_Sol[j]) == 1)
				{
					clsSolution*tempSortSol=arr_child_Sol[i];
					arr_child_Sol[i]=arr_child_Sol[j];
					arr_child_Sol[j]=tempSortSol;
				}
				//Compare the original and the best in offspring
				if (CompareTwoSolutions(child_Sol,arr_child_Sol[0])==1)
					arr_child_Sol[0]->CopySolution(*child_Sol);
				while(arr_child_Sol.size() > 0)
				{
					delete arr_child_Sol[arr_child_Sol.size()-1];
					arr_child_Sol.pop_back();
				}
				delete tempSol;
				return true;
	}
	//Generating inital generation
	bool clsGeneticAlg::GeneratingInitalGeneration()
	{
		int mythreshold;
		int myCount=0;
		for(mythreshold=1; mythreshold <= iJury;mythreshold++)
		{
			clsSolution*tempSolution=new clsSolution;
			tempSolution->InitData(iJury, iIntLect, iExtLect, Y, arrLecturer, mythreshold);
			tempSolution->InitData();//Initialize data

			if(tempSolution->ChooseLecturerForJuries() == true)
			{		
				this->arrPopOfSol.push_back(tempSolution);//The first solution (not be guarateed that it is feasible)
			}

		}
		cout<<endl<<"Number of solution generated: "<<this->arrPopOfSol.size();

		if(this->arrPopOfSol.size()==0) return false;
		return true;
	}
	//Mutation
	bool clsGeneticAlg::Mutation(clsSolution* my_Sol, vector<clsSolution*>& arr_Sol)
	{
		srand (time(0));
		for(int i=0;i<this->iNumMuta;i++)
		{
			clsSolution* tempSolution=new clsSolution;
			my_Sol->CopySolution(*tempSolution);

			//int iJuryRandom=rand() % iJury;
			//int iFlagMember=rand() % 4;
			int iJuryRandom=(myCount + rand()) % iJury;
			int iFlagMember=(myCount + rand()) % 4;
			myCount++;
			cout<<"RANDOM: ("<<iJuryRandom<<", "<<iFlagMember<<")";
			//find lecturer to replace
			switch (iFlagMember)
			{
			case 0://Chair
				if(tempSolution->FindChairToReplace(iJuryRandom, myCount) == true)
					arr_Sol.push_back(tempSolution);
				break;
			case 1://Secretary
				if(tempSolution->FindSecretaryToReplace(iJuryRandom, myCount) == true)
					arr_Sol.push_back(tempSolution);
				break;
			case 2://Reviewer 1
				if(tempSolution->FindReviewerToReplace(iJuryRandom, 0, myCount) == true)
					arr_Sol.push_back(tempSolution);
				break;
			case 3://Reviewer 2
				if(tempSolution->FindReviewerToReplace(iJuryRandom, 1, myCount) == true)
					arr_Sol.push_back(tempSolution);
				break;
			}
		}
		return true;
	}
	//Controlled Mutation
	bool clsGeneticAlg::Mutation_Controled(clsSolution* my_Sol, vector<clsSolution*>& arr_Sol)
	{
		//srand (time(0));
		for(int i=0;i<my_Sol->vcrUnFesibleJury.size();i++)
		{
			clsSolution* tempSolution=new clsSolution;
			my_Sol->CopySolution(*tempSolution);

			//int iJuryRandom=rand() % iJury;
			//int iFlagMember=rand() % 4;
			int iJuryRandom=my_Sol->vcrUnFesibleJury[i];
			myCount++;
			for(int iFlagMember =0;iFlagMember<4;iFlagMember++)
			{
				//find lecturer to replace
				switch (iFlagMember)
				{
				case 0://Chair
					if(tempSolution->FindChairToReplace(iJuryRandom, myCount) == true)
						arr_Sol.push_back(tempSolution);
					break;
				case 1://Secretary
					if(tempSolution->FindSecretaryToReplace(iJuryRandom, myCount) == true)
						arr_Sol.push_back(tempSolution);
					break;
				case 2://Reviewer 1
					if(tempSolution->FindReviewerToReplace(iJuryRandom, 0, myCount) == true)
						arr_Sol.push_back(tempSolution);
					break;
				case 3://Reviewer 2
					if(tempSolution->FindReviewerToReplace(iJuryRandom, 1, myCount) == true)
						arr_Sol.push_back(tempSolution);
					break;
				}
			}
		}
		return true;
	}

	//Crossover
	bool clsGeneticAlg::Crossover(vector<clsSolution*>& arr_Sol)
	{
		srand(time(0));
		int iPar1,iPar2;
		iPar1=(myCount+rand()) % arr_Sol.size(); //select solution iPar1 as parent1
		iPar2=(iPar1+rand()) % arr_Sol.size();// select solution iPar2 as parent2
		int crossPart = (myCount+rand()+1)%iJury; //number of parts that a solution will be divided in
		if (crossPart<2)
			crossPart=2;
		int crossQuantity= iJury/crossPart; //quantity of juries of each part
		clsSolution* tempSolution= new clsSolution();
		arr_Sol[iPar1]->MakeChildSolution(arr_Sol[iPar2], *tempSolution, crossPart, crossQuantity);
		arr_Sol.push_back(tempSolution);
		myCount++;
		return true;
	}
	//Sorting and choose best solutions
	void clsGeneticAlg::SelectingBestSolutions()
	{
		//Sorting the solutions in population
		for(int i=0;i<this->arrPopOfSol.size()-1;i++)
			for(int j=i+1;j<this->arrPopOfSol.size();j++)
				if(this->CoefficienceCompareTwoSolutions(this->arrPopOfSol[i], this->arrPopOfSol[j],1,2,3,4) == 1)
				{
					clsSolution*tempSol=this->arrPopOfSol[i];
					this->arrPopOfSol[i]=this->arrPopOfSol[j];
					this->arrPopOfSol[j]=tempSol;
				}

				//Remove the last solutions
				while(this->arrPopOfSol.size() > this->iPopSize)
				{
					delete this->arrPopOfSol[this->arrPopOfSol.size()-1];
					this->arrPopOfSol.pop_back();
				}
	}
	int clsGeneticAlg::CoefficienceCompareTwoSolutions(clsSolution* mySol1, clsSolution* mySol2, int coF1, int coF2, int coF3, int coF4)
	{
		//compare the number of infeasible solutions
		if(mySol1->numUnFesibleJury < mySol2->numUnFesibleJury)
			return 0; //mySol1 is better than mySol2
		if(mySol1->numUnFesibleJury > mySol2->numUnFesibleJury)
			return 1; //mySol2 is better than mySol1
		if(mySol1->numUnFesibleJury == 0)//or mySol2->numUnFesibleJury == 0
		{
			if(mySol1->numRoom !=0 && mySol2->numRoom !=0)//feasible solution
			{
				int mySol1_f1=coF1*mySol1->numRoom;
				int mySol1_f2= coF2*mySol1->missingTimeslot;
				int mySol1_f3= coF3*mySol1->exSession;
				int mySol1_f4= coF4*mySol1->numSession;
				int mySol1_F= mySol1_f1+mySol1_f2+mySol1_f3+mySol1_f4;
				int mySol2_f1=coF1*mySol2->numRoom;
				int mySol2_f2= coF2*mySol2->missingTimeslot;
				int mySol2_f3= coF3*mySol2->exSession;
				int mySol2_f4= coF4*mySol2->numSession;
				int mySol2_F= mySol2_f1+mySol2_f2+mySol2_f3+mySol2_f4;
				if (mySol1_F<mySol2_F)//sol1 is better
					return 0;
				return 1;
			}
			if(mySol2->missingTimeslot ==0)
				return 1;
			return 0;
		}
		return 0;
	}
	//Compare two solutions, return 0 if mySol1 > mySol2
	int clsGeneticAlg::CompareTwoSolutions(clsSolution* mySol1, clsSolution* mySol2)
	{
		if(mySol1->numUnFesibleJury < mySol2->numUnFesibleJury)
			return 0; //mySol1 is better than mySol2
		if(mySol1->numUnFesibleJury > mySol2->numUnFesibleJury)
			return 1; //mySol2 is better than mySol1

		if(mySol1->numUnFesibleJury == 0)//or mySol2->numUnFesibleJury == 0
		{
			if(mySol1->numRoom !=0 && mySol2->numRoom !=0)//feasible solution
			{
				if(mySol1->missingTimeslot < mySol2->missingTimeslot)
					return 0;
				return 1;
			}
			if(mySol2->missingTimeslot ==0)
				return 1;
			return 0;
		}
		return 0;
	}
	
