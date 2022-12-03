using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
//using System.Data.SQLite;
//using Microsoft.Data.Sqlite;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace DB_test
{
    public class SqlLiteDB : SQL<SQLiteConnection>
    {
        string databaseName = "PeopleLite";
        private SQLiteConnection cnn;
        public SqlLiteDB(string databaseName)
        {
            this.databaseName = databaseName;
            cnn = setConnection();
        }

        //string connectionString = string.Format(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog={0};Integrated Security=True", databaseName);
        public override void ExecCommand(string sql)
        {
            SQLiteConnection cnn = setConnection();
            SQLiteCommand cmd = new SQLiteCommand(sql, cnn);
            cmd.ExecuteNonQuery();
            closeConnection();
        }

        public override DataTable getAll()
        {
            SQLiteConnection cnn = setConnection();

            SQLiteCommand command = new SQLiteCommand();
            command.Connection = cnn;
            SQLiteDataAdapter da = new SQLiteDataAdapter("select * from People", cnn);
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
            catch { }
            return lst;
        }

        public override bool delPerson(int id)
        {
            SQLiteConnection cnn = setConnection();
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
            SQLiteConnection cnn = setConnection();
            try
            {
                string sqlExpression = "sp_InsertUser";
                SQLiteCommand command = new SQLiteCommand(sqlExpression, cnn);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = CommandType.StoredProcedure;
                SQLiteParameter firstNameParam = new SQLiteParameter
                {
                    ParameterName = "@firstname",
                    Value = p.Firstname
                };

                SQLiteParameter middleNameParam = new SQLiteParameter
                {
                    ParameterName = "@middlename",
                    Value = p.Middlename
                };

                SQLiteParameter lastNameParam = new SQLiteParameter
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
            catch (Exception e)
            {
                //transaction.Rollback();
                //Console.WriteLine(e);
                closeConnection();
                return false;
            }

        }

        public override bool addNewPersons(List<Person> persons)
        {
            SQLiteConnection cnn = setConnection();
            SQLiteTransaction transaction = cnn.BeginTransaction();
            try
            {

                string sqlExpression = "sp_InsertUser";
                foreach (var p in persons)
                {
                    SQLiteCommand command = new SQLiteCommand(sqlExpression, cnn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transaction;

                    SQLiteParameter firstNameParam = new SQLiteParameter
                    {
                        ParameterName = "@firstname",
                        Value = p.Firstname
                    };

                    SQLiteParameter middleNameParam = new SQLiteParameter
                    {
                        ParameterName = "middlename",
                        Value = p.Middlename
                    };

                    SQLiteParameter lastNameParam = new SQLiteParameter
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

        public override void storedInsert()
        {
            SQLiteCommand insert = new SQLiteCommand("INSERT INTO People (Lastname, Firstname, Middlename) VALUES ('ЛАЙТ','ЛАЙТ','ЛАЙТ')", cnn);
            String Lastname = "ЛАЙТ";
            String Middlename = "ЛАЙТ";
            String Firstname = "ЛАЙТ";
            insert.ExecuteNonQuery();
        }

        public override void storedDelete()
        {
            string deleteStr = @"CREATE PROCEDURE [dbo].[sp_DeleteUser]
                                @id int
                            AS
                                INSERT INTO People (Lastname, Firstname, Middlename)
                                VALUES (@Lastname, @Firstname, @Middlename)
                            GO";
            SQLiteCommand deleteProc = new SQLiteCommand(deleteStr, cnn);
            deleteProc.ExecuteNonQuery();
        }

        public override SQLiteConnection setConnection()
        {
            SQLiteConnection cnn = new SQLiteConnection("Data Source = PeopleLite.db");
            try
            {
                cnn.Open();
                SQLiteCommand cmd = new SQLiteCommand("create table if not exists People(id integer primary key, lastname text not null, firstname text not null, middlename text not null);", cnn);
                cmd.ExecuteNonQuery();
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
