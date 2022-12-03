using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DB_test
{
    interface IStoredProcedure
    {
        void storedInsert();
        void storedDelete();
    }
}
