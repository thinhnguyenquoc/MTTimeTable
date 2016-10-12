using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
namespace TimeTablingApplication
{
    public class clsLecturer
    {

        public int IdLect;//Id is index of the lecturers array
        public bool IsExternalLect; //is external lecturer or not
        public List<int> arrSpecialize;//role of lecturer with Project: 0, 1, 2, 3.
        //0: could be member of jury
        //1: be advisor, must be member of jury
        //2: be co-avisor, but not be member of jury
        //3: could be reviewer of jury
        public List<int> arrAvailTime;//availability time of lecturer,=0: free; =1: unavailability, =2: unavailability because have joined a jury
        public int numAvailTime;//Number of availability time of lecturer
        public int Y;

        //Specific variable, depend on a given solution
        public int iSessionJoined;

        public LecturerDetails details;
        public clsLecturer()
        {
            this.arrSpecialize = new List<int>();
            this.arrAvailTime = new List<int>();
        }
       
        
    }
}
