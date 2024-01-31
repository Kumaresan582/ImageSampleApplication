using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class ApiResponse
    {
        public int ResponseCode { get; set; }
        public int Result { get; set; }
        public string Results { get; set; }
        public string Errormessage { get; set; }
    }
}
