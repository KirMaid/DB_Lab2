using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DB_test
{
    public interface IDataSource
    {
        List<Person> getPeople();
        bool delPerson(int id);
        bool newPerson(Person p);
        void ExecCommand(string sql);
        bool addNewPersons(List<Person> persons);
        DataTable getAll(); 
    }
}
