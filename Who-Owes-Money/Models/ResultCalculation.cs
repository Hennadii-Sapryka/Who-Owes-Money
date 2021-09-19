using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Who_Owes_Money.Models
{
     public class ResultCalculation
    {
        public string UserUnderPaid { get; set; }
        public string UserOverPaid { get; set; }
        public int Money { get; set; }
        public int Average { get; set; }
    }
}
