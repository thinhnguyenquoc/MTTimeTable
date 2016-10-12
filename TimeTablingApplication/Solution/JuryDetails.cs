using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeTablingApplication
{
    public class JuryDetails
    {
        public int ID;
        public string Name;
        public string StudentName;
        public JuryDetails(int ID, string Name, string StudentName)
        {
            this.ID = ID;
            this.Name = Name;
            this.StudentName = StudentName;
            
        }

    }
}
