using LinqToDB.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Data
{
    public class AdminDataContextFactory : IDataContextFactory<AdminDataContext>
    {
        private readonly IDataProvider dataProvider;

        private readonly string connectionString;

        public AdminDataContextFactory(IDataProvider dataProvider, string connectionString)
        {
            this.dataProvider = dataProvider;
            this.connectionString = connectionString;
        }

        public AdminDataContext Create() => new AdminDataContext(dataProvider, connectionString);
    }
}
