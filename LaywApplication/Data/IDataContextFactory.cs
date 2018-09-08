using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaywApplication.Data
{
    public interface IDataContextFactory<T> where T : DataConnection
    {
        T Create();
    }
}
