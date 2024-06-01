using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispacthBalanceCalled
{
    public class OrchestratorInput
    {
        public string ServiceCode { get; set; }
        public int CeveCode { get; set; }
        public DateOnly SaleDate { get; set; }
    }
}
