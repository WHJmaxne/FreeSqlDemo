using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreesqlDemo.API.DbModels
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> User { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    options.UseFreeSql(Startup.Fsql);
        //}

        protected override void OnModelCreating(ICodeFirst codefirst)
        {
            codefirst.Entity<User>(eb =>
            {
                eb.ToTable("tb_user");
            });

            codefirst.SyncStructure<User>();
        }
    }
}
