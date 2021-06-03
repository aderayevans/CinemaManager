using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace B1510210_QuanLyRapPhim
{
    public partial class index : Form
    {
        public index()
        {
            InitializeComponent();
        }

        private void index_Load(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
        }

        private void Menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form frmSignin = new frmSignin();
            frmSignin.ShowDialog();

            //for test admin form
            //Form frmAdmin = new frmAdmin("1");
            //frmAdmin.ShowDialog();

            //for test employee form
            //Form frmEmployee = new frmEmployee("2");
            //frmEmployee.ShowDialog();
        }
    }
}
