using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Appraisal.Models
{
    public class Section_1
    {
        public string Emp_ID { get; set; }
        public string Man_ID { get; set; }
        public string Factor { get; set; }
	    public int Emp_Eval { get; set; }
	    public int Man_Eval { get; set; }
	    public decimal Agreed_Score { get; set; }
	    public string Support { get; set; }
	    public string Actions { get; set; }
	    public DateTime Years { get; set; } 
    }
}