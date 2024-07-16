using sqlClientPractic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ex1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.DataSource = getAllusers();
        }

        public DataTable getAllusers()
        {
            string q = @"select * from users;";

            return DBContext.MakeQuery(q);
        }
    }
}
