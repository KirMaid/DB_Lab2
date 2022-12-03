using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Data;
using System.Data.SQLite;

namespace DB_test
{
    public partial class Form1 : Form
    {
        //IDataSource ids;
        //IStoredProcedure isp;
        SQL<SqlConnection> ids;
        List<Person> persons = new List<Person>() {
        new Person(1,"Пупкина","Василиса","Ивановна"),
        //new Person(1,"Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc,","Василий","Иванович"),
        new Person(1,"Пупкин","Сергей","Иванович")
        };

        public Form1()
        {
            InitializeComponent();
            ids = new SqlCeDB("People");
            ids.storedInsert();
            dataGridView1.DataSource = ids.getPeople();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ids.newPerson(new Person(1, lastNameBox.Text, firstNameBox.Text, middleNameBox.Text));
            dataGridView1.DataSource = ids.getPeople();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow != null)
            {
                string id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                ids.delPerson(Int32.Parse(id));
                dataGridView1.DataSource = ids.getPeople();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ids.addNewPersons(persons);
            dataGridView1.DataSource = ids.getPeople();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            SQL<SQLiteConnection> ids = new SqlLiteDB("People");
            ids.storedInsert();
            dataGridView1.DataSource = ids.getPeople();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            SQL<SqlConnection>  ids = new SqlCeDB("People");
            ids.storedInsert();
            dataGridView1.DataSource = ids.getPeople();
        }
    }
}
