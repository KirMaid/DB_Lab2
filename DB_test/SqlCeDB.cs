using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DB_test
{
    public class SqlCeDB : SQL<SqlConnection>
    {
        string databaseName = "People";
        private SqlConnection cnn;
        public SqlCeDB(string databaseName)
        {
            this.databaseName = databaseName;
            cnn = setConnection();
        }

        //string connectionString = string.Format(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog={0};Integrated Security=True", databaseName);
        public override void ExecCommand(string sql)
        {
            SqlConnection cnn = setConnection();
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.ExecuteNonQuery();
            closeConnection();
        }

        public override DataTable getAll()
        {
            SqlConnection cnn = setConnection();
            SqlDataAdapter da = new SqlDataAdapter("select * from People", cnn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            closeConnection();
            return ds.Tables[0];
        }

        public override List<Person> getPeople()
        {
            List<Person> lst = new List<Person>();
            try
            {
                DataTable t = getAll();
                foreach (DataRow dr in t.Rows)
                {
                    lst.Add(new Person(Int32.Parse(dr[0].ToString()),
                        dr[1].ToString(), dr[2].ToString(), dr[3].ToString()));
                }
            }
            catch {}
            return lst;
        }

        public override bool delPerson(int id)
        {
            SqlConnection cnn = setConnection();
            try
            {
                ExecCommand("delete from People where id = " + id.ToString());
                closeConnection();
                return true;  
            }
            catch
            {
                closeConnection();
                return false;
            }       
        }


        public override bool newPerson(Person p)
        {
            SqlConnection cnn = setConnection();
            try
            {
                string sqlExpression = "sp_InsertUser";
                SqlCommand command = new SqlCommand(sqlExpression, cnn);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter firstNameParam = new SqlParameter
                {
                    ParameterName = "@firstname",
                    Value = p.Firstname
                };

                SqlParameter middleNameParam = new SqlParameter
                {
                    ParameterName = "@middlename",
                    Value = p.Middlename
                };

                SqlParameter lastNameParam = new SqlParameter
                {
                    ParameterName = "@lastname",
                    Value = p.Lastname
                };
                command.Parameters.Add(firstNameParam);
                command.Parameters.Add(middleNameParam);
                command.Parameters.Add(lastNameParam);

                command.ExecuteNonQuery();

                /*ExecCommand(
                    "insert into People (Lastname, Firstname, Middlename) values('"
                    + p.Lastname + "','" + p.Firstname + "','" + p.Middlename + "')");*/
                closeConnection();
                return true;
            }
            catch(Exception e)
            {
                //transaction.Rollback();
                //Console.WriteLine(e);
                closeConnection();
                return false;
            }

        }

        public override bool addNewPersons(List<Person> persons)
        {
            SqlConnection cnn = setConnection();
            SqlTransaction transaction = cnn.BeginTransaction();
            try
            {
               
                string sqlExpression = "sp_InsertUser";
                foreach (var p in persons)
                {
                    SqlCommand command = new SqlCommand(sqlExpression, cnn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transaction;

                    SqlParameter firstNameParam = new SqlParameter
                    {
                        ParameterName = "@firstname",
                        Value = p.Firstname
                    };

                    SqlParameter middleNameParam = new SqlParameter
                    {
                        ParameterName = "middlename",
                        Value = p.Middlename
                    };

                    SqlParameter lastNameParam = new SqlParameter
                    {
                        ParameterName = "@lastname",
                        Value = p.Lastname
                    };

                  
                    command.Parameters.Add(firstNameParam);
                    command.Parameters.Add(middleNameParam);
                    command.Parameters.Add(lastNameParam);
                    command.ExecuteNonQuery();

                }
                transaction.Commit();
                closeConnection();
                return true;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                Console.WriteLine(e);
                closeConnection();
                return false;
            }
        }

        /*public override void storedInsert()
        {
            string insertStr = @"CREATE PROCEDURE [dbo].[sp_InsertUser]
                                @Lastname nvarchar(50),
                                @Middlename nvarchar(50),
                                @Firstname nvarchar(50)
                            AS
                                INSERT INTO People (Lastname, Firstname, Middlename)
                                VALUES (@Lastname, @Firstname, @Middlename)
                            ";
            SqlCommand insertProc = new SqlCommand(insertStr, cnn);
            SqlCommand insertProcDel = new SqlCommand("DROP PROCEDURE sp_InsertUser",cnn);
            insertProcDel.ExecuteNonQuery();
            insertProc.ExecuteNonQuery();
        }

        public override void storedDelete()
        {
            string deleteStr = @"CREATE PROCEDURE [dbo].[sp_DeleteUser]
                                @id int
                            AS
                                INSERT INTO People (Lastname, Firstname, Middlename)
                                VALUES (@Lastname, @Firstname, @Middlename)
                            GO";
            SqlCommand deleteProc = new SqlCommand(deleteStr, cnn);
            deleteProc.ExecuteNonQuery();
        }*/

        public override SqlConnection setConnection()
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
            return cnn;
        }

        public override void closeConnection()
        {
            cnn.Close();
        }
    }
}
