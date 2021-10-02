using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Appraisal.Models
{
    public class All_Model
    {
        public EA_Users EA_User { get; set; }
        public List<EA_Section_1> EA_Sec1 { get; set; }
        public List<EA_Section_2> EA_Sec2 { get; set; }
        public List<EA_Section_3> EA_Sec3 { get; set; }
        public List<EA_Section_4> EA_Sec4 { get; set; }
        public List<EA_Section_5> EA_Sec5 { get; set; }
        public List<EA_Section_6> EA_Sec6 { get; set; }

        public List<EA_Section_3> EA_Sec3_ly { get; set; }
        public List<EA_Section_4> EA_Sec4_ly { get; set; }
    }
}