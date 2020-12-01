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
        public DbSet<Book> Book { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    options.UseFreeSql(Startup.Fsql);
        //}

        protected override void OnModelCreating(ICodeFirst codefirst)
        {
            codefirst.Entity<Book>(b =>
            {
                b.HasOne(a => a.User).HasForeignKey(a => a.UserId).WithMany(a => a.Books);

                b.HasIndex(a => a.UserId).HasName("Book_Idx_UserId_001");

            });

            //codefirst.SyncStructure<User>();
        }
    }
}
