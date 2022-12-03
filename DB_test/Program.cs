using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DB_test
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //SqlCeDB sql = new SqlCeDB();
            //TestDS testSql = new TestDS();
            //Application.Run(new Form1(sql));
            Application.Run(new Form1());

            /* if (args.Length > 0 && args[0] == "test")
                 Application.Run(new Form1(testSql));
             else
                 Application.Run(new Form1(sql));*/
        }
    }
}
