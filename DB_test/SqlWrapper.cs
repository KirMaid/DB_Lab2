using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_test
{
    public class SqlWrapper : IStoredProcedure
    {
        SqlConnection cnn;
        string databaseName = "People";
        public SqlWrapper(SqlConnection cnn, string databaseName)
        {
            this.cnn = cnn;
            this.databaseName = databaseName;
        }

        public SqlWrapper()
        {
        }

        public virtual void storedDelete()
        {
            string connectionString = string.Format(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog={0};Integrated Security=True", databaseName);
            SqlConnection cnn = new SqlConnection(connectionString);
            try
            {
                cnn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            string deleteStr = @"CREATE PROCEDURE [dbo].[sp_DeleteUser]
                                @id int
                            AS
                                INSERT INTO People (Lastname, Firstname, Middlename)
                                VALUES (@Lastname, @Firstname, @Middlename)
                            GO";
            SqlCommand deleteProc = new SqlCommand(deleteStr, cnn);
            deleteProc.ExecuteNonQuery();
        }

        public virtual void storedInsert()
        {
            string connectionString = string.Format(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog={0};Integrated Security=True", databaseName);
            SqlConnection cnn = new SqlConnection(connectionString);
            try
            {
                cnn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            string insertStr = @"CREATE PROCEDURE [dbo].[sp_InsertUser]
                                @Lastname nvarchar(50),
                                @Middlename nvarchar(50),
                                @Firstname nvarchar(50)
                            AS
                                INSERT INTO People (Lastname, Firstname, Middlename)
                                VALUES (@Lastname, @Firstname, @Middlename)
                            ";
            SqlCommand insertProc = new SqlCommand(insertStr, cnn);
            SqlCommand insertProcDel = new SqlCommand("DROP PROCEDURE sp_InsertUser", cnn);
            insertProcDel.ExecuteNonQuery();
            insertProc.ExecuteNonQuery();
        }
    }
}
