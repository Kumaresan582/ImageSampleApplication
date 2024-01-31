using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DatabaseConn
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options) 
        {

        }
        public DbSet<Customer> customer { get; set; }
        public DbSet<AuthUserModel> USER_TABLE { get; set; }
        public DbSet<ImageIploadModel> ImageUploads { get; set; }
    }
}
