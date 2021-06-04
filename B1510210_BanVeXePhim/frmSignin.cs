using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace B1510210_QuanLyRapPhim
{
    public partial class frmSignin : Form
    {
        string strConnectionString = @"Data Source=DESKTOP-46B87U9\SQLEXPRESS;Initial Catalog = CinemaTicket; Integrated Security = True";
        // Đối tượng kết nối
        SqlConnection conn = null;
        // Đối tượng đưa dữ liệu vào DataTable dtTABLENAME
        SqlDataAdapter daTABLENAME = null;
        // Đối tượng hiển thị dữ liệu lên Form
        DataTable dtTABLENAME = null;

        public frmSignin()
        {
            InitializeComponent();
        }

        public DataTable extractData(string query)
        {
            // Khởi động kết nối
            conn = new SqlConnection(strConnectionString);
            // Vận chuyển dữ liệu lên DataTable dtTABLENAME
            daTABLENAME = new SqlDataAdapter(query, conn);
            dtTABLENAME = new DataTable();
            daTABLENAME.Fill(dtTABLENAME);
            return dtTABLENAME;
        }

        public string extractValueFromRow0Column(string column_name, DataTable datatable)
        {
            DataRow[] rows = datatable.Select();

            return rows[0][column_name].ToString();
        }

        public static string ToSha256(string text) =>
            string.IsNullOrEmpty(text) ? string.Empty : BitConverter.ToString(new System.Security.Cryptography.SHA256Managed().ComputeHash(System.Text.Encoding.UTF8.GetBytes(text))).Replace("-", string.Empty);

        private void signin()
        {
            if (txtUsername.Text == "" || txtPassword.Text == "") return;
            conn = new SqlConnection(strConnectionString);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            string query = "SELECT COUNT(*) FROM CINEMA_USER where username = '" + txtUsername.Text + "'";
            SqlCommand comm = new SqlCommand(query, conn);
            Int32 count = (Int32)comm.ExecuteScalar();

            if (count > 0)
            {
                query = "SELECT * FROM CINEMA_USER where username = '" + txtUsername.Text + "'";
                DataTable data = extractData(query);
                string value_id = extractValueFromRow0Column("userid", data);
                string value_pass = extractValueFromRow0Column("password", data);
                string value_type = extractValueFromRow0Column("typeuser_alias", data);

                if (ToSha256(txtPassword.Text).ToUpper() == value_pass.ToUpper())
                {
                    if (value_type == "ad") //admin
                    {
                        Form frmAdmin = new frmAdmin(value_id);
                        this.Hide();
                        frmAdmin.ShowDialog();
                        this.Close();
                    }
                    else //value_type == "em" employee
                    {
                        Form frmEmployee = new frmEmployee(value_id);
                        this.Hide();
                        frmEmployee.ShowDialog();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Mật khẩu không đúng", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Tài khoản không tồn tại", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSignin_Click(object sender, EventArgs e)
        {
            signin();    
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Xác nhận hủy đăng nhập?", "Hủy đăng nhập",
            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
            {
                // Giải phóng tài nguyên
                dtTABLENAME = null;
                // Hủy kết nối
                conn = null;
                this.Close();
            }
        }

        private void frmSignin_Load(object sender, EventArgs e)
        {
            lblHiddenUser.Visible = false;
            lblHiddenPass.Visible = false;
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
                lblHiddenPass.Visible = true;
            else
                lblHiddenPass.Visible = false;
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
                lblHiddenUser.Visible = true;
            else
                lblHiddenUser.Visible = false;
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                signin();
            }
        }
    }
}
