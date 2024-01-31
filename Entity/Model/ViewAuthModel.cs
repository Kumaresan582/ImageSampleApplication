using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class ViewAuthModel
    {
        public string USER_NAME { get; set; }
        public int PASSWORD { get; set; }
    }
}
