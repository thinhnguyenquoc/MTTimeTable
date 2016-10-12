
#include"Solution.h"
#include<vector>
using namespace std;
class clsGeneticAlg
{
public:
	int iPopSize;//Size of population
	int iNumMuta;//Number of mutation for each solution
	int iNumCros;//Number of crossover in population
	int iK;//Number of iterations
	std::vector<clsSolution*> arrPopOfSol;
	std::vector<clsSolution*> arrRealSol;

	int iIntLect; //Number of internal LECTURERS
	int iExtLect; //Number of external LECTURERS
	int iJury; //Number of JURIES
	int Y;//Maximum number of PERIODS
	clsLecturer arrLecturer[MAXLECT];//list of lecturers
	int iThreNumOfProj;//maximum number of jury that each lecturer could join.

	int myCount;
public:
	void InitData(int i_PopSize, int i_NumMuta, int i_NumCros, int i_K, int i_IntLect, int i_ExtLect, int i_Jury, int Y, clsLecturer arr_Lecturer[MAXLECT], int i_ThreNumOfProj);//Initialize data
	bool DoGAScheduling();//Start to schedule
	bool GeneratingInitalGeneration();//Generating inital generation
	bool Mutation(clsSolution* my_Sol, vector<clsSolution*>& arr_Sol);//Mutation
	bool Mutation_Controled(clsSolution* my_Sol, vector<clsSolution*>& arr_Sol);//Controlled Mutation
	bool LocalSearch(clsSolution* &child_Sol);//Local Search to improve one child solution
	bool Crossover(vector<clsSolution*>& arr_Sol);//Crossover
	void SelectingBestSolutions();//Sorting and choose best solutions
	int CompareTwoSolutions(clsSolution* mySol1, clsSolution* mySol2);//Compare two solutions, return 0 if mySol1 > mySol2




};