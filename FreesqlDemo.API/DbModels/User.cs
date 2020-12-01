using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreesqlDemo.API.DbModels
{
    [Index("{TableName}_Idx_01", "Name", isUnique: false)]
    public class User
    {
        [Column(IsIdentity = true)]
        public int Id { get; set; }
        [Column(StringLength = 50)]
        public string Name { get; set; }
        public int Age { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();
    }

    public class Book
    {
        [Column(IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Auther { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
