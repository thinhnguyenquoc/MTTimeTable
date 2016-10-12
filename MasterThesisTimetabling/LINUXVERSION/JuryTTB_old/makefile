JuryTTB: MasterThesisTimetabling.o GeneticAlg.o 	Jury.o Lecturer.o Solution.o Utilities.o
	g++ -std=c++0x -o Jury MasterThesisTimetabling.o GeneticAlg.o Jury.o Lecturer.o Solution.o Utilities.o
MasterThesisTimetabling.o: MasterThesisTimetabling.cpp
	g++ -std=c++0x -c MasterThesisTimetabling.cpp
GeneticAlg.o: GeneticAlg.cpp
	g++ -std=c++0x -c GeneticAlg.cpp
Jury.o: Jury.cpp
	g++ -std=c++0x -c Jury.cpp
Lecturer.o: Lecturer.cpp
	g++ -std=c++0x -c Lecturer.cpp
Solution.o: Solution.cpp
	g++ -std=c++0x -c Solution.cpp
Utilities.o: Utilities.cpp
	g++ -std=c++0x -c Utilities.cpp
clean:
	-rm *.o JuryTTB
