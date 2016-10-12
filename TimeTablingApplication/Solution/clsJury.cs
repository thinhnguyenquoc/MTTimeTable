using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeTablingApplication
{
   public class clsJury
    
{

	public int IdJury;//is the index of Jury array
    public clsLecturer advisor;
    public List<clsLecturer> arrReviewers;
    public List<clsLecturer> arrOtherMembers;
    public int iNumExtLect; //number of external lecturers
    public int IdRoom;//Id of room for jury
    public int IdPeriod; //Id of period for jury
    public List<bool> arrAvailPeriod;//=true if the period at when 5 lecturers are free
    public JuryDetails details;
    public clsJury()
    {
        this.arrReviewers = new List<clsLecturer>();
        this.arrOtherMembers = new List<clsLecturer>();
        this.arrAvailPeriod = new List<bool>();
    }
    public int IsIn(int MaGV)
    {
        if (this.advisor.IdLect == MaGV)
            return 1;
        if (this.arrReviewers[0].IdLect == MaGV ||
            this.arrReviewers[1].IdLect == MaGV)
            return 3;
        if (this.arrOtherMembers[0].IdLect == MaGV)
            return 4;
        if (this.arrOtherMembers[1].IdLect == MaGV)
            return 5;
        return -1;
    }
}
}
