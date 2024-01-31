using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class ImageIploadModel
    {
        [Key]
        public int Id { get; set; }

        public string Imagecode { get; set; }

        [Column("UploadImage", TypeName = "image")]
        public byte[] Images { get; set; }
    }
}
