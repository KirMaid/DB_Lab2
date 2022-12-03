using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DB_test
{
    abstract public class SQL<T>:SqlWrapper ,IDataSource,IDataConnection<T>
    {
        T cnn;
        protected string databaseName;
        public abstract bool addNewPersons(List<Person> persons);
        public abstract void closeConnection();
        public abstract bool delPerson(int id);
        public abstract void ExecCommand(string sql);
        public abstract DataTable getAll();
        public abstract List<Person> getPeople();
        public abstract bool newPerson(Person p);
        public abstract T setConnection();
    }
}
