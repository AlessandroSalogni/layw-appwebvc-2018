using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Data
{
    public class AdminDataContext : DataConnection
    {
        public AdminDataContext(IDataProvider dataProvider, string connectionString)
            : base(dataProvider, connectionString)
        { }

        public ITable<Admin> Admin => GetTable<Admin>();
    }
}
