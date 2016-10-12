using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Windows.Forms;

namespace TimeTablingApplication
{
    public class clsSolution
    
{    
    public int iIntLect; //Number of internal LECTURERS
    public int iExtLect; //Number of external LECTURERS
    public int iJury; //Number of JURIES
    public int Y;//Maximum number of PERIODS
    public List<clsLecturer> arrLecturer;//list of lecturers
    public List<clsJury> arrJury;//list of juries
    public int iThreNumOfProj;//maximum number of jury that each lecturer could join.
    public List<int> arrNumProjofLect;//number of jury that each lecturer joined.
    public List<int> arrThreNumProjOfLect;//The maximum number of jury that each lecturer can be joined

    public int numUnFesibleJury;//Number of unfesible jury
    public List<int> vcrUnFesibleJury;//list of unfesible jury in solution
	//additional variables
    public List<bool> arrIsChoosePeriod;//array of flags to show whether period of jury is chosen or not.
    public int iIDRoom; //this template variable is used to set Room for jury
    public int numRoom; //number of room used
    public int numSession; //number of sesion of all lecturer
    public int makeSpan; //the max of finished time of solution
    public int missingTimeslot; //the timeslot that interrupt the schedule
	//ofstream myLogFile;//file pointer for logging

    public clsSolution()
    {
        this.arrLecturer = new List<clsLecturer>();
        this.arrJury = new List<clsJury>();
        this.arrNumProjofLect = new List<int>();
        this.arrThreNumProjOfLect = new List<int>();
        this.arrIsChoosePeriod = new List<bool>();
        this.vcrUnFesibleJury = new List<int>();
    }
    public void ReadSolutionFromDatabase(string MaGP, DataConnection dataConnection)
    {
        //read iIntLect
        DataTable dtTemp = dataConnection.OpenDataSet("SELECT * FROM GIANGVIEN WHERE MaGP='" + MaGP + "'").Tables[0];
        DataTable dtCopy = dtTemp.Clone();
        dtCopy.Columns["MaGV"].DataType = typeof(int);
        foreach (DataRow r in dtTemp.Rows)
            dtCopy.ImportRow(r);
        DataView dv = dtCopy.DefaultView;
        dv.Sort = "MaGV asc";
        DataTable dtLect = dv.ToTable();

        dtTemp.Clear();
        dtCopy.Clear();
        dtTemp=dataConnection.OpenDataSet("SELECT * FROM HOIDONG WHERE MaGP='" + MaGP + "'").Tables[0];
        dtCopy = dtTemp.Clone();
        dtCopy.Columns["MaHD"].DataType = typeof(int);
        foreach (DataRow r in dtTemp.Rows)
            dtCopy.ImportRow(r);
        dv= dtCopy.DefaultView;
        dv.Sort="MaHD asc";
        DataTable dtJury = dv.ToTable();

        dtTemp.Clear();
        dtCopy.Clear();
        dtTemp=dataConnection.OpenDataSet("SELECT * FROM DETAI WHERE MaGP='" + MaGP + "'").Tables[0];
        dtCopy = dtTemp.Clone();
        dtCopy.Columns["MaDT"].DataType = typeof(int);
        foreach (DataRow r in dtTemp.Rows)
            dtCopy.ImportRow(r);
        dv = dtCopy.DefaultView;
        dv.Sort="MaDT asc";
        DataTable dtThesis = dv.ToTable();

        dtTemp.Clear();
        dtCopy.Clear();
        dtTemp=dataConnection.OpenDataSet("SELECT * FROM THOIGIAN WHERE MaGP='" + MaGP + "'").Tables[0];
        dtCopy = dtTemp.Clone();
        dtCopy.Columns["MaTG"].DataType = typeof(int);
        foreach (DataRow r in dtTemp.Rows)
            dtCopy.ImportRow(r);
        dv = dtCopy.DefaultView;
        dv.Sort="MaTG asc";
        DataTable dtTime = dv.ToTable();
        int iInternalLect = 0;
        int iExternalLect = 0;
        foreach (DataRow row in dtLect.Rows)
        {
            if ((bool)row["GVNgoai"] == false)
                iInternalLect++;
            else
                iExternalLect++;
        }
        //read iIntLect
        this.iIntLect = iInternalLect;
        //read iExtLect
        this.iExtLect = iExternalLect;
        //read iJury
        this.iJury = dtJury.Rows.Count;
        //read Y
        this.Y = dtTime.Rows.Count;
        for (int i = 0; i < iIntLect + iExtLect; i++) //each lecturer
        {
            //read IdLect
            clsLecturer lecturer = new clsLecturer();
            lecturer.IdLect = int.Parse(dtLect.Rows[i]["MaGV"].ToString())-1;
            //read IsExternalLect
            if (bool.Parse(dtLect.Rows[i]["GVNgoai"].ToString()) == false)
                lecturer.IsExternalLect = false;
            else
                lecturer.IsExternalLect = true;
            //read arrSpecialize
            string s = dtLect.Rows[i]["ChuyenMon"].ToString();
            string[] split = s.Split(',');
            for (int k = 0; k < iJury; k++)
            {
                lecturer.arrSpecialize.Add(int.Parse(split[k]));
            }
            //read arrAvailTime
            s = dtLect.Rows[i]["TGTG"].ToString();
            split = s.Split(',');
            for (int k = 0; k < Y; k++)
            {
                lecturer.arrAvailTime.Add(int.Parse(split[k]));
            }
            //read numAvailTime
            lecturer.numAvailTime = int.Parse(dtLect.Rows[i]["TongTGTG"].ToString());
            //read Y
            lecturer.Y = this.Y;
            //read sSessionJoin
            lecturer.iSessionJoined = int.Parse(dtLect.Rows[i]["BTG"].ToString());
            //read details
            lecturer.details= new LecturerDetails(lecturer.IdLect,dtLect.Rows[i]["TenGV"].ToString());
            this.arrLecturer.Add(lecturer);

        }
        for (int i = 0; i < iJury; i++)
        {
            clsJury jury = new clsJury();
            jury.IdJury = int.Parse(dtJury.Rows[i]["MaHD"].ToString())-1;
            string s;
            //read advisor, reviewers, others
            jury.advisor = this.arrLecturer[int.Parse(dtJury.Rows[i]["GVHD"].ToString())-1];

            jury.arrReviewers.Add(this.arrLecturer[int.Parse(dtJury.Rows[i]["GVPB1"].ToString())-1]);
            jury.arrReviewers.Add(this.arrLecturer[int.Parse(dtJury.Rows[i]["GVPB2"].ToString())-1]);

            jury.arrOtherMembers.Add(this.arrLecturer[int.Parse(dtJury.Rows[i]["ChuTich"].ToString())-1]);
            jury.arrOtherMembers.Add(this.arrLecturer[int.Parse(dtJury.Rows[i]["ThuKy"].ToString())-1]);


            jury.iNumExtLect = int.Parse(dtJury.Rows[i]["GVNgoai"].ToString());

            jury.IdRoom = int.Parse(dtJury.Rows[i]["Phong"].ToString())-1;

            jury.IdPeriod = int.Parse(dtJury.Rows[i]["ThoiGian"].ToString())-1;
            s = dtJury.Rows[i]["TGTC"].ToString();
            string[] split = s.Split(',');
            //read available time for jury
            for (int k = 0; k < Y; k++)
            {
                jury.arrAvailPeriod.Add(bool.Parse(split[k]));

            }
            jury.details= new JuryDetails(jury.IdJury,dtThesis.Rows[i]["TenDT"].ToString(),dtThesis.Rows[i]["SVTH"].ToString());
            this.arrJury.Add(jury);
            arrIsChoosePeriod.Add(true);
        }


    }
    public void ReadSolutionFromFile(string sInputSolutionName)
    {
        StreamReader myReader = new StreamReader(sInputSolutionName);
        if (myReader == null)
        {
            MessageBox.Show("Cannot open file!");
            return;
        }
        string myLine;
        myLine = myReader.ReadLine();//read iIntLect
        this.iIntLect = int.Parse(myLine);
        myLine = myReader.ReadLine();//read iExtLect
        this.iExtLect = int.Parse(myLine);
        myLine = myReader.ReadLine();//read iJury
        this.iJury = int.Parse(myLine);
        myLine = myReader.ReadLine();//read Y
        this.Y = int.Parse(myLine);
        for (int i = 0; i < iIntLect + iExtLect; i++) //each lecturer
        {
            myLine=myReader.ReadLine();//read IdLect
            clsLecturer lecturer = new clsLecturer();
            lecturer.IdLect = int.Parse(myLine);
            myLine = myReader.ReadLine();//read IsExternalLect
            if (myLine == "0"||myLine=="False")
                lecturer.IsExternalLect = false;
            else
                lecturer.IsExternalLect = true;
            myLine = myReader.ReadLine();//read arrSpecialize
           
            string s;
            for (int k = 0; k < iJury; k++)
            {
                myLine = myLine.Substring(myLine.IndexOf("\t")+1);
                if (k == iJury - 1)
                    s = myLine;
                else
                    s = myLine.Substring(0, myLine.IndexOf("\t"));
                lecturer.arrSpecialize.Add(int.Parse(s));
                
            }
            myLine = myReader.ReadLine();//read arrAvailTime
            for (int k = 0; k < Y; k++)
            {
                myLine = myLine.Substring(myLine.IndexOf("\t") + 1);
                if (k == Y - 1)
                    s = myLine;
                else
                    s = myLine.Substring(0, myLine.IndexOf("\t"));
                lecturer.arrAvailTime.Add(int.Parse(s));
            }
            myLine = myReader.ReadLine();//read numAvailTime
            lecturer.numAvailTime = int.Parse(myLine);
            myLine = myReader.ReadLine();//read Y
            lecturer.Y = int.Parse(myLine);
            myLine = myReader.ReadLine();//read sSessionJoin
            lecturer.iSessionJoined = int.Parse(myLine);
            this.arrLecturer.Add(lecturer);

        }
        for (int i = 0; i < iJury; i++)
        {
            myLine = myReader.ReadLine();
            clsJury jury = new clsJury();
            jury.IdJury = int.Parse(myLine);
            string s;
            myLine = myReader.ReadLine();//read advisor, reviewers, others
            for (int k = 0; k < 5; k++)
            {
                myLine = myLine.Substring(myLine.IndexOf("\t") + 1);
                if (k == 4)
                    s = myLine;
                else
                    s = myLine.Substring(0, myLine.IndexOf("\t"));
                if (k == 0)
                    jury.advisor = this.arrLecturer[int.Parse(s)];
                if (k == 1||k==2)
                    jury.arrReviewers.Add(this.arrLecturer[int.Parse(s)]);
               
                if (k == 3||k==4)
                    jury.arrOtherMembers.Add(this.arrLecturer[int.Parse(s)]);
                
            }
            myLine = myReader.ReadLine();
            jury.iNumExtLect = int.Parse(myLine);
            myLine = myReader.ReadLine();
            jury.IdRoom = int.Parse(myLine);
            myLine = myReader.ReadLine();
            jury.IdPeriod = int.Parse(myLine);
            myLine = myReader.ReadLine();//read available time for jury
            for (int k = 0; k < Y; k++)
            {
                myLine = myLine.Substring(myLine.IndexOf("\t") + 1);
                if (k == Y-1)
                    s = myLine;
                else
                    s = myLine.Substring(0, myLine.IndexOf("\t"));
                if (s == "0"||s=="False")
                    jury.arrAvailPeriod.Add(false);
                else
                    jury.arrAvailPeriod.Add(true);
            }
            this.arrJury.Add(jury);
        }
        myLine = myReader.ReadLine();
        this.iThreNumOfProj = int.Parse(myLine);
        myLine = myReader.ReadLine();
        string t;
        for (int k = 0; k < iIntLect+iExtLect; k++)
        {
            myLine = myLine.Substring(myLine.IndexOf("\t") + 1);
            if (k == (iIntLect + iExtLect) - 1)
                t = myLine;
            else
                t = myLine.Substring(0, myLine.IndexOf("\t"));
            this.arrNumProjofLect.Add(int.Parse(t));
        }
        myLine = myReader.ReadLine();
        for (int k = 0; k < iIntLect + iExtLect; k++)
        {
            myLine = myLine.Substring(myLine.IndexOf("\t") + 1);
            if (k == (iIntLect + iExtLect) - 1)
                t = myLine;
            else
                t = myLine.Substring(0, myLine.IndexOf("\t"));
            this.arrThreNumProjOfLect.Add(int.Parse(t));
        }
        myLine = myReader.ReadLine();
        this.numUnFesibleJury = int.Parse(myLine);
        myLine = myReader.ReadLine();
        for (int k = 0; k < numUnFesibleJury; k++)
        {
            myLine = myLine.Substring(myLine.IndexOf("\t") + 1);
            if (k == numUnFesibleJury - 1)
                t = myLine;
            else
                t = myLine.Substring(0, myLine.IndexOf("\t"));
            this.vcrUnFesibleJury.Add(int.Parse(t));
        }
        myLine = myReader.ReadLine();
        for (int k = 0; k < iJury; k++)
        {
            myLine = myLine.Substring(myLine.IndexOf("\t") + 1);
            if (k == iJury - 1)
                t = myLine;
            else
                t = myLine.Substring(0, myLine.IndexOf("\t"));
            if (t == "0"||t=="False")
                this.arrIsChoosePeriod.Add(false);
            else
                this.arrIsChoosePeriod.Add(true);
        }
        myLine = myReader.ReadLine();
        this.iIDRoom = int.Parse(myLine);
        myLine = myReader.ReadLine();
        this.numRoom = int.Parse(myLine);
        myLine = myReader.ReadLine();
        this.numSession = int.Parse(myLine);
        myLine = myReader.ReadLine();
        this.makeSpan = int.Parse(myLine);
        myLine = myReader.ReadLine();
        this.missingTimeslot = int.Parse(myLine);
        myReader.Close();
       // ReadJuryDetails(sInputSolutionName + ".jury");
        //ReadLecturerDetails(sInputSolutionName + ".lec");
    }
    public void ReadJuryDetailsFromFile(string path)
    {
        StreamReader myReader = new StreamReader(path,Encoding.Unicode);
        string myLine;
        int ID;
        string Name, StudentName;
        for (int i = 0; i < iJury; i++)
        {
             myLine= myReader.ReadLine();
             ID = int.Parse(myLine.Substring(0, myLine.IndexOf("\t")));
             Name = myLine.Substring(myLine.IndexOf("\t") + 1, myLine.LastIndexOf("\t")-myLine.IndexOf("\t")-1);
             StudentName = myLine.Substring(myLine.LastIndexOf("\t") + 1, myLine.Length - myLine.LastIndexOf("\t")-1);
            this.arrJury[i].details = new JuryDetails(ID, Name, StudentName);
        }
        myReader.Close();
    }
    public void ReadLecturerDetailsFromFile(string path)
    {
        StreamReader myReader = new StreamReader(path, Encoding.Unicode);
        string myLine;
        int ID;
        string Name;
        for (int i = 0; i < iIntLect+iExtLect; i++)
        {
            myLine = myReader.ReadLine();
            ID = int.Parse(myLine.Substring(0, myLine.IndexOf("\t")));
            Name = myLine.Substring(myLine.IndexOf("\t") + 1, myLine.Length - myLine.IndexOf("\t") - 1);
            this.arrLecturer[i].details = new LecturerDetails(ID, Name);
        }
        myReader.Close();
    }
    public bool CheckIfPeriodIsOK(int JuryID, int iPeriod)
    {
        List<int> JuryHasSamePeriod = new List<int>();
        for (int i = 0; i < iJury; i++)
        {
            if (arrIsChoosePeriod[i] == true && arrJury[i].IdPeriod == iPeriod)
                JuryHasSamePeriod.Add(i);
        }
        List<int> LectInJury = new List<int>();
        List<int> LectInJuryNeedTested = new List<int>();
        LectInJuryNeedTested.Add(arrJury[JuryID].advisor.IdLect);
        LectInJuryNeedTested.Add(arrJury[JuryID].arrReviewers[0].IdLect);
        LectInJuryNeedTested.Add(arrJury[JuryID].arrReviewers[1].IdLect);
        LectInJuryNeedTested.Add(arrJury[JuryID].arrOtherMembers[0].IdLect);
        LectInJuryNeedTested.Add(arrJury[JuryID].arrOtherMembers[1].IdLect);
        for (int i = 0; i < JuryHasSamePeriod.Count; i++)
        {
            LectInJury.Add(arrJury[JuryHasSamePeriod[i]].advisor.IdLect);
            LectInJury.Add(arrJury[JuryHasSamePeriod[i]].arrReviewers[0].IdLect);
            LectInJury.Add(arrJury[JuryHasSamePeriod[i]].arrReviewers[1].IdLect);
            LectInJury.Add(arrJury[JuryHasSamePeriod[i]].arrOtherMembers[0].IdLect);
            LectInJury.Add(arrJury[JuryHasSamePeriod[i]].arrOtherMembers[1].IdLect);
            for (int j = 0; j < 5; j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    if (LectInJury[j] == LectInJuryNeedTested[k])
                        return false;
                }
            }
            LectInJury.Clear();
        }
        //all members are available

        return true;

    }
    public DataTable GetDataTableForJury(DataTable tblJury)
    {
        tblJury.Columns.Add("Jury", typeof(int));
        tblJury.Columns.Add("Advisor", typeof(int));
        tblJury.Columns.Add("Reviewer 1", typeof(int));
        tblJury.Columns.Add("Reviewer 2", typeof(int));
        tblJury.Columns.Add("Chair", typeof(int));
        tblJury.Columns.Add("Secretary", typeof(int));
        tblJury.Columns.Add("Time", typeof(int));
        tblJury.Columns.Add("Room", typeof(int));
        for (int i = 0; i < this.iJury; i++)
        {
            tblJury.Rows.Add(this.arrJury[i].IdJury + 1, this.arrJury[i].advisor.IdLect + 1,
                this.arrJury[i].arrReviewers[0].IdLect + 1, this.arrJury[i].arrReviewers[1].IdLect + 1,
                this.arrJury[i].arrOtherMembers[0].IdLect + 1, this.arrJury[i].arrOtherMembers[1].IdLect + 1,
                this.arrJury[i].IdPeriod, this.arrJury[i].IdRoom + 1);
        }
        return tblJury;
    }
    public DataTable GetDataTableForTime(DataTable tblTime)
    {
        tblTime.Columns.Add("Time", typeof(int));

        for (int i = 0; i < this.numRoom; i++)
        {
            tblTime.Columns.Add("Room " + (i + 1).ToString(), typeof(int));
        }
        for (int i = 0; i < this.makeSpan; i++)
        {
            List<int> listJuriesHaveSamePeriod = new List<int>();
            for (int k = 0; k < this.iJury; k++)
            {
                if (this.arrJury[k].IdPeriod == i)
                    listJuriesHaveSamePeriod.Add(k);
            }
            DataRow row = tblTime.NewRow();
            row["Time"] = i;
            for (int k = 0; k < listJuriesHaveSamePeriod.Count; k++)
            {
                row[this.arrJury[listJuriesHaveSamePeriod[k]].IdRoom + 1] = listJuriesHaveSamePeriod[k];
            }
            tblTime.Rows.Add(row);

            listJuriesHaveSamePeriod.Clear();
        }

        return tblTime;
    }
    public void WriteSolution(string sOutputFileName)
    {
        StreamWriter myFile = new StreamWriter(sOutputFileName);

        if (myFile == null)
            MessageBox.Show("cannot open output file");
        myFile.WriteLine(iIntLect);
        myFile.WriteLine(iExtLect);
        myFile.WriteLine(iJury);
        myFile.WriteLine(Y);
        for (int k = 0; k < iIntLect + iExtLect; k++)//each lecturer
        {
            myFile.WriteLine(arrLecturer[k].IdLect);//Id is index of the lecturers array
            myFile.WriteLine(arrLecturer[k].IsExternalLect);//is external lecturer or not
            for (int h = 0; h < iJury; h++)
                myFile.Write("\t" + arrLecturer[k].arrSpecialize[h]);//role for each jury
            myFile.WriteLine();
            for (int h = 0; h < Y; h++)
                myFile.Write("\t" + arrLecturer[k].arrAvailTime[h]);//availability time of lecturer,=0: free; =1: unavailability, =2: unavailability because have joined a jury
            myFile.WriteLine();
            myFile.WriteLine(arrLecturer[k].numAvailTime);//Number of availability time of lecturer
            myFile.WriteLine(arrLecturer[k].Y);
            myFile.WriteLine(arrLecturer[k].iSessionJoined);
        }
        for (int k = 0; k < iJury; k++)//each jury
        {
            myFile.WriteLine(arrJury[k].IdJury);//is the index of Jury array
            myFile.WriteLine("\t" + arrJury[k].advisor.IdLect + "\t" + arrJury[k].arrReviewers[0].IdLect + "\t" + arrJury[k].arrReviewers[1].IdLect + "\t" + arrJury[k].arrOtherMembers[0].IdLect + "\t" + arrJury[k].arrOtherMembers[1].IdLect);//list of members
            myFile.WriteLine(arrJury[k].iNumExtLect); //number of external lecturers
            myFile.WriteLine(arrJury[k].IdRoom);//Id of room for jury
            myFile.WriteLine(arrJury[k].IdPeriod); //Id of period for jury
            for (int h = 0; h < Y; h++)//available time
                myFile.Write("\t" + arrJury[k].arrAvailPeriod[h]);
            myFile.WriteLine();
        }
        myFile.WriteLine(iThreNumOfProj);//maximum number of jury that each lecturer could join.
        for (int h = 0; h < iIntLect + iExtLect; h++)
            myFile.Write("\t" + arrNumProjofLect[h]);//number of jury that each lecturer joined.
        myFile.WriteLine();
        for (int h = 0; h < iIntLect + iExtLect; h++)
            myFile.Write("\t" + arrThreNumProjOfLect[h]);//The maximum number of jury that each lecturer can be joined
        myFile.WriteLine();
        myFile.WriteLine(numUnFesibleJury);//Number of unfesible jury
        for (int h = 0; h < vcrUnFesibleJury.Count; h++)
            myFile.Write("\t" + vcrUnFesibleJury[h]);//list of unfesible jury in solution
        myFile.WriteLine();
        //additional variables
        for (int h = 0; h < iJury; h++)
            myFile.Write("\t" + arrIsChoosePeriod[h]);//array of flags to show whether period of jury is chosen or not.
        myFile.WriteLine();
        myFile.WriteLine(iIDRoom); //this template variable is used to set Room for jury
        myFile.WriteLine(numRoom);//number of room used
        myFile.WriteLine(numSession); //number of sesion of all lecturer
        myFile.WriteLine(makeSpan); //the max of finished time of solution
        myFile.WriteLine(missingTimeslot);
        myFile.Close();
    }
    public  bool CheckIfProfIsStillAvailable(int JuryID, int iPeriod, int LectID)
    {
	    if (arrLecturer[LectID].arrAvailTime[arrJury[JuryID].IdPeriod]==1)//lecturer is busy
		    return false;
	    List<int> ListOfProfJoining= new List<int>();
        for (int i=0;i<iJury;i++)
        {
            if (arrJury[i].IsIn(LectID)!=-1)
                ListOfProfJoining.Add(i);
        }
	    for (int i=0;i<ListOfProfJoining.Count;i++)
	    {
		    if (arrJury[ListOfProfJoining[i]].IdPeriod==iPeriod)//lecturer has already joined another jury at that time
			    return false;
	    }
	    return true;
    }
    //Find other chair
    public List<int> FindChairToReplace(int IDJury)
    {
	    List<int> arrLecturerCanReplace= new List<int>();
	    int idLect;
	    int minIndex,maxIndex;
	    if(arrJury[IDJury].arrOtherMembers[0].IsExternalLect == true&&arrJury[IDJury].iNumExtLect==1)
	    {
		    minIndex=iIntLect;
		    maxIndex=iIntLect+ iExtLect;//Must choose external lecturer		
	    }
	    else
	    {
		    minIndex=0;
		    maxIndex=iIntLect+iExtLect;//Choose in all
        }
		for(int i=minIndex;i<maxIndex;i++)
	    	if(arrJury[IDJury].IsIn(i) == -1 &&//lecturer i doesn't join this IDJury
		    	(arrLecturer[i].arrSpecialize[IDJury] == 0 
                || arrLecturer[i].arrSpecialize[IDJury] == 3)
                && CheckIfProfIsStillAvailable(IDJury,arrJury[IDJury].IdPeriod,i)==true)             
		    {
                arrLecturerCanReplace.Add(i);
		    }
	    return arrLecturerCanReplace;	
    }
//Find other secretary
    public List<int> FindSecretaryToReplace(int IDJury)
    {
	//must choose internal lecturer
	List<int> arrLecturerCanReplace= new List<int>();
	//Find candidate
	for(int i=0;i<iIntLect;i++)
		if(arrJury[IDJury].IsIn(i) == -1 &&
			(arrLecturer[i].arrSpecialize[IDJury] == 0 || arrLecturer[i].arrSpecialize[IDJury] == 3) 
            &&CheckIfProfIsStillAvailable(IDJury,arrJury[IDJury].IdPeriod,i)==true)
		{
			arrLecturerCanReplace.Add(i);
		}
		return arrLecturerCanReplace;
    }
//Find other reviewer 0 or 1
    public List<int> FindReviewerToReplace(int IDJury, int iRevi)
    {
        List<int> arrLecturerCanReplace = new List<int>();
        int minIndex, maxIndex;

        if (arrJury[IDJury].arrReviewers[iRevi].IsExternalLect == true && arrJury[IDJury].iNumExtLect == 1)
        {
            minIndex = iIntLect;
            maxIndex = iIntLect + iExtLect;//Must choose external lecturer
        }
        else
        {
            minIndex = 0;
            maxIndex = iIntLect + iExtLect;//Choose in all
        }
        //Find candidate
        for (int i = minIndex; i < maxIndex; i++)
            if (arrJury[IDJury].IsIn(i) == -1 &&
                (arrLecturer[i].arrSpecialize[IDJury] == 3) &&
                CheckIfProfIsStillAvailable(IDJury, arrJury[IDJury].IdPeriod, i) == true)
            {
                arrLecturerCanReplace.Add(i);
            }
        return arrLecturerCanReplace;
    }
}
}
