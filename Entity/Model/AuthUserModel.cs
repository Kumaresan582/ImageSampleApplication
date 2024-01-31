using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class AuthUserModel
    {
        [Key]
        [Column("USER_ID")]
        public int UserId { get; set; }
        public string USER_NAME { get; set; }
        public int PASSWORD { get; set; }
        [Column("CREATED_DATE")]
        public DateTime CreatedDate { get; set; }
        [Column("LASTMODIFIED_DATE")]
        public DateTime LastModifiedDate { get; set; }
        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }
        [Column("ROLL")]
        public string Roll { get; set; }
    }
}
 
