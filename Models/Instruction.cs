using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Appraisal.Models
{
    public class Instruction
    {
        public EA_Users EA_User { get; set; }
        public EA_Instruction EA_Ins { get; set; }
        public EA_Instruction EA_Ins2 { get; set; }
        public EA_Leave EA_Leav { get; set; }
    }
}