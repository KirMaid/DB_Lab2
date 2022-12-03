using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DB_test
{
    public class Person
    {
        public int ID { get; set; }
        public String Lastname { get; set; }
        public String Firstname { get; set; }
        public String Middlename { get; set; }

        public Person (int id, string l, string f, string m)
	    {
            ID = id;
            Lastname = l;
            Firstname = f;
            Middlename = m;
	    }

    }
}
