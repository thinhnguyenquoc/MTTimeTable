// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include "MainFunc.h"
BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}
extern "C"
{
	__declspec(dllexport) clsMainFunc* clsMainFunc_constructor()
	{
		return new clsMainFunc();
	}
	__declspec(dllexport) void clsMainFunc_free(clsMainFunc* m)
	{
		delete m;
	}
	__declspec(dllexport) void myDllmain()
	{
		clsMainFunc* m = new clsMainFunc();
		m->run0();
	}
	__declspec(dllexport) void myDllGenerateSolution(char* inputFileFullPath)
	{
		clsMainFunc* m = new clsMainFunc();
		m->run(inputFileFullPath);
	}
	__declspec(dllexport) void myDllGenerateSolutionF(char* inputFileFullPath)
	{
		clsMainFunc* m = new clsMainFunc();
		m->runF(inputFileFullPath);
	}
	__declspec(dllexport) clsSolution* getSolution()
	{
		
		clsMainFunc* ma= new clsMainFunc();
		ma->run0();
		return ma->finalSolution;
	}
	__declspec(dllexport) bool CheckIfPeriodIsOK(clsSolution* solution,int juryID, int Period)
	{
		return solution->CheckIfPeriodIsOK(juryID,Period);
	}
	__declspec(dllexport) vector<int> List()
	{
		vector<int> list;
		list.push_back(1);
		return list;
	}
}

