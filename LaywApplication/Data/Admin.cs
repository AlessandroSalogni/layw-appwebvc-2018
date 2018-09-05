using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace LaywApplication.Data
{ 
    public class Admin
    {
        [PrimaryKey]
        [NotNull]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
