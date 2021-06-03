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
using System.Globalization;

namespace B1510210_QuanLyRapPhim
{
    public partial class frmAdmin : Form
    {
        string strConnectionString = @"Data Source=DESKTOP-46B87U9\SQLEXPRESS;Initial Catalog = CinemaTicket; Integrated Security = True";
        // Đối tượng kết nối
        SqlConnection conn = null;
        // Đối tượng đưa dữ liệu vào DataTable dtTABLENAME
        SqlDataAdapter daTABLENAME = null;
        // Đối tượng hiển thị dữ liệu lên Form
        DataTable dtTABLENAME = null;

        private int userid;
        private int cur_id = -1;

        private string cur_moviename;
        private string cur_cinema_date;
        private string cur_cinemaid;
        private string cur_time_start;
        public frmAdmin(string userid)
        {
            this.userid = Int32.Parse(userid);
            InitializeComponent();
        }
        public void executeQuery(string query)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            SqlCommand sqlCmd = new SqlCommand(query, conn);
            sqlCmd.ExecuteNonQuery();
        }
        public DataTable extractData(string query)
        {
            // Vận chuyển dữ liệu lên DataTable dtTABLENAME
            daTABLENAME = new SqlDataAdapter(query, conn);
            dtTABLENAME = new DataTable();
            dtTABLENAME.Clear();
            daTABLENAME.Fill(dtTABLENAME);
            return dtTABLENAME;
        }

        public string extractValueFromRow0Column(string column_name, DataTable datatable)
        {
            DataRow[] rows = datatable.Select();

            return rows[0][column_name].ToString();
        }

        private void frmAdmin_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(strConnectionString);
            string query = "select * from CINEMA_USER ur " +
                            "left join CINEMA_USER_TYPE ut " +
                            "on ur.typeuser_alias = ut.typeuser_alias " +
                            "where ur.userid = " + userid.ToString();
            DataTable data = extractData(query);
            string value_type = extractValueFromRow0Column("typeuser_name", data);
            string value_username = extractValueFromRow0Column("username", data);
            tstUserType.Text = value_type;
            tstUsername.Text = value_username;

            this.reload(0);
        }
        private void disAdminAndEmployee()
        {
            string query = "select * from CINEMA_USER ur " +
                            "left join CINEMA_USER_TYPE ut " +
                            "on ur.typeuser_alias = ut.typeuser_alias";
            DataTable data = extractData(query);

            DataColumn[] columns =
            {
                data.Columns[0],    //userid
                data.Columns[4],     //user_fullname
                data.Columns[1],     //username
                data.Columns[6],     //typeuser_name
            };
            this.dgvAdmin.Columns.Add("userid", "ID");
            this.dgvAdmin.Columns.Add("user_fname", "Họ Tên");
            this.dgvAdmin.Columns.Add("username", "Tên tài khoản");
            this.dgvAdmin.Columns.Add("typeuser_name", "Chức vụ");

            foreach (DataRow row in data.Rows)
            {
                this.dgvAdmin.Rows.Add(row.ItemArray[0].ToString(),
                                        row.ItemArray[4].ToString(),
                                        row.ItemArray[1].ToString(),
                                        row.ItemArray[6].ToString()
                                       );
            }
        }
        private void disAdmin()
        {
            string query = "select * from CINEMA_USER ur " +
                            "left join CINEMA_USER_TYPE ut " +
                            "on ur.typeuser_alias = ut.typeuser_alias " +
                            "where ur.typeuser_alias = 'ad';";
            DataTable data = extractData(query);

            DataColumn[] columns =
            {
                data.Columns[0],    //userid
                data.Columns[4],     //user_fullname
                data.Columns[1],     //username
                data.Columns[6],     //typeuser_name
            };
            this.dgvAdmin.Columns.Add("userid", "ID");
            this.dgvAdmin.Columns.Add("user_fname", "Họ Tên");
            this.dgvAdmin.Columns.Add("username", "Tên tài khoản");
            this.dgvAdmin.Columns.Add("typeuser_name", "Chức vụ");

            foreach (DataRow row in data.Rows)
            {
                this.dgvAdmin.Rows.Add(row.ItemArray[0].ToString(),
                                        row.ItemArray[4].ToString(),
                                        row.ItemArray[1].ToString(),
                                        row.ItemArray[6].ToString()
                                       );
            }
        }

        private void disEmployee()
        {
            string query = "select * from CINEMA_USER ur " +
                            "left join CINEMA_USER_TYPE ut " +
                            "on ur.typeuser_alias = ut.typeuser_alias " +
                            "where ur.typeuser_alias = 'em';";
            DataTable data = extractData(query);

            DataColumn[] columns =
            {
                data.Columns[0],    //userid
                data.Columns[4],     //user_fullname
                data.Columns[1],     //username
                data.Columns[6],     //typeuser_name
            };
            this.dgvAdmin.Columns.Add("userid", "ID");
            this.dgvAdmin.Columns.Add("user_fname", "Họ Tên");
            this.dgvAdmin.Columns.Add("username", "Tên tài khoản");
            this.dgvAdmin.Columns.Add("typeuser_name", "Chức vụ");

            foreach (DataRow row in data.Rows)
            {
                this.dgvAdmin.Rows.Add(row.ItemArray[0].ToString(),
                                        row.ItemArray[4].ToString(),
                                        row.ItemArray[1].ToString(),
                                        row.ItemArray[6].ToString()
                                       );
            }
        }

        private void disMovie()
        {
            string query = "select * from CINEMA_MOVIE";
            DataTable data = extractData(query);

            DataColumn[] columns =
            {
                data.Columns[0],    //movieid
                data.Columns[1],     //moviename
                data.Columns[2],     //duration
                data.Columns[3]     //price
            };
            this.dgvAdmin.Columns.Add("movieid", "ID");
            this.dgvAdmin.Columns.Add("moviename", "Tên Phim");
            this.dgvAdmin.Columns.Add("duration", "Thời lượng");
            this.dgvAdmin.Columns.Add("price", "Giá vé");

            foreach (DataRow row in data.Rows)
            {
                this.dgvAdmin.Rows.Add(row.ItemArray[0].ToString(),
                                        row.ItemArray[1].ToString(),
                                        row.ItemArray[2].ToString(),
                                        row.ItemArray[3].ToString()
                                       );
            }
        }

        private void disCinema()
        {
            string query = "select * from CINEMA";
            DataTable data = extractData(query);

            DataColumn[] columns =
            {
                data.Columns[0],    //cinemaid
                data.Columns[1],     //cinemaid_rownum
                data.Columns[2]     //cinemaid_colnum
            };
            this.dgvAdmin.Columns.Add("cinemaid", "ID");
            this.dgvAdmin.Columns.Add("cinemaid_rownum", "Số hàng");
            this.dgvAdmin.Columns.Add("cinemaid_colnum", "Số ghế một hàng");

            foreach (DataRow row in data.Rows)
            {
                this.dgvAdmin.Rows.Add(row.ItemArray[0].ToString(),
                                        row.ItemArray[1].ToString(),
                                        row.ItemArray[2].ToString()
                                       );
            }
        }
        private void disTime()
        {
            /*Update cinemaid combobox*/
            string query = "select * from CINEMA";
            DataTable data = extractData(query);
            List<Object> _items = new List<Object>();
            foreach (DataRow row in data.Rows)
            {
                _items.Add(row.ItemArray[0].ToString());
            }
            cbbCinema.DataSource = _items;
            /*Update moviename combobox*/
            query = "select * from CINEMA_MOVIE";
            data = extractData(query);

            cbbMovieName.DisplayMember = "Text";
            cbbMovieName.ValueMember = "Value";
            List<Object> items = new List<Object>();
            foreach (DataRow row in data.Rows)
            {
                items.Add(new { Text = row.ItemArray[1].ToString(), Value = row.ItemArray[0].ToString() });
            }
            cbbMovieName.DataSource = items;


            query = "select * from CINEMA_TIME ct left join CINEMA_MOVIE cm on ct.MOVIEID = cm.MOVIEID";
            data = extractData(query);

            DataColumn[] columns =
            {
                data.Columns[0],    //movieid
                data.Columns[5],     //moviename
                data.Columns[1],     //cinemaid
                data.Columns[3],     //cinema_date
                data.Columns[2],     //time_start
                data.Columns[6]     //duration
            };
            this.dgvAdmin.Columns.Add("movieid", "MovieID");
            this.dgvAdmin.Columns.Add("moviename", "Tên phim");
            this.dgvAdmin.Columns.Add("cinemaid", "Rạp");
            this.dgvAdmin.Columns.Add("cinema_date", "Ngày chiếu");
            this.dgvAdmin.Columns.Add("time_start", "Giờ bắt đầu");
            this.dgvAdmin.Columns.Add("duration", "Thời lượng");

            foreach (DataRow row in data.Rows)
            {

                DateTime date = DateTime.Parse(row.ItemArray[3].ToString());

                this.dgvAdmin.Rows.Add(row.ItemArray[0].ToString(),
                                        row.ItemArray[5].ToString(),
                                        row.ItemArray[1].ToString(),
                                        date.ToString("dd/MM/yyyy"),
                                        row.ItemArray[2].ToString(),
                                        row.ItemArray[6].ToString()
                                       );
            }
        }
        private void reload(int default_num=1)
        {
            if (default_num == 0) this.grbDis.Text = "Danh sách";

            this.dgvAdmin.DataSource = null;
            this.dgvAdmin.Rows.Clear();
            this.dgvAdmin.Columns.Clear();
            switch (this.grbDis.Text)
            {
                case "Danh sách":
                    this.grbAddUser.Visible = false;
                    this.grbAddMovie.Visible = false;
                    this.grbAddTime.Visible = false;
                    this.grbAddCinema.Visible = false;
                    btnThem.Enabled = true;
                    btnSua.Enabled = true;
                    btnXoa.Enabled = true;
                    cur_id = -1;
                    break;
                case "Danh sách nhân viên":
                    disEmployee();
                    break;
                case "Danh sách quản trị và nhân viên":
                    disAdminAndEmployee();
                    break;
                case "Danh sách quản trị viên":
                    disAdmin();
                    break;
                case "Danh sách rạp":
                    disCinema();
                    break;
                case "Danh sách phim":
                    disMovie();
                    break;
                case "Lịch chiếu phim":
                    disTime();
                    break;
            }
        }

        private void tsiSignout_Click(object sender, EventArgs e)
        {
            DialogResult traloi;
            traloi = MessageBox.Show("Xác nhận đăng xuất?", "Đăng xuất",
            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (traloi == DialogResult.OK)
            {
                // Giải phóng tài nguyên
                dtTABLENAME.Dispose();
                dtTABLENAME = null;
                // Hủy kết nối
                conn = null;
                this.Close();
            }
        }
        private void tsiAddAdmin_Click(object sender, EventArgs e)
        {
            string query = "select * from CINEMA_USER ur " +
                            "left join CINEMA_USER_TYPE ut " +
                            "on ur.typeuser_alias = ut.typeuser_alias " +
                            "where ur.typeuser_alias = 'ad';";
            DataTable data = extractData(query);

            this.dgvAdmin.DataSource = null;
            this.dgvAdmin.Rows.Clear();
            this.dgvAdmin.Columns.Clear();

            DataColumn[] columns =
            {
                data.Columns[0],    //userid
                data.Columns[4],     //user_fullname
                data.Columns[1],     //username
                data.Columns[6],     //typeuser_name
            };
            this.dgvAdmin.Columns.Add("userid", "ID");
            this.dgvAdmin.Columns.Add("user_fname", "Họ Tên");
            this.dgvAdmin.Columns.Add("username", "Tên tài khoản");
            this.dgvAdmin.Columns.Add("typeuser_name", "Chức vụ");

            foreach (DataRow row in data.Rows)
            {
                this.dgvAdmin.Rows.Add(row.ItemArray[0].ToString(),
                                        row.ItemArray[4].ToString(),
                                        row.ItemArray[1].ToString(),
                                        row.ItemArray[6].ToString()
                                       );
            }
            //this.dgvAdmin.Enabled = false;
            this.grbAddUser.Visible = true; 
            this.grbDis.Text = "Danh sách quản trị viên";
        }
        private void tsiAddEmployee_Click(object sender, EventArgs e)
        {
            string query = "select * from CINEMA_USER ur " +
                            "left join CINEMA_USER_TYPE ut " +
                            "on ur.typeuser_alias = ut.typeuser_alias " +
                            "where ur.typeuser_alias = 'em';";
            DataTable data = extractData(query);

            this.dgvAdmin.DataSource = null;
            this.dgvAdmin.Rows.Clear();
            this.dgvAdmin.Columns.Clear();

            DataColumn[] columns =
            {
                data.Columns[0],    //userid
                data.Columns[4],     //user_fullname
                data.Columns[1],     //username
                data.Columns[6],     //typeuser_name
            };
            this.dgvAdmin.Columns.Add("userid", "ID");
            this.dgvAdmin.Columns.Add("user_fname", "Họ Tên");
            this.dgvAdmin.Columns.Add("username", "Tên tài khoản");
            this.dgvAdmin.Columns.Add("typeuser_name", "Chức vụ");

            foreach (DataRow row in data.Rows)
            {
                this.dgvAdmin.Rows.Add(row.ItemArray[0].ToString(),
                                        row.ItemArray[4].ToString(),
                                        row.ItemArray[1].ToString(),
                                        row.ItemArray[6].ToString()
                                       );
            }
            //this.dgvAdmin.Enabled = false;
            this.grbAddUser.Visible = true;
            this.grbDis.Visible = true;
            this.grbDis.Text = "Danh sách nhân viên";
        }
        private void tsiDisAdmin_Click(object sender, EventArgs e)
        {
            this.reload(0);
            this.grbDis.Text = "Danh sách quản trị viên";
            this.reload();
        }
        private void tsiDisEmployee_Click(object sender, EventArgs e)
        {
            this.reload(0);
            this.grbDis.Text = "Danh sách nhân viên";
            this.reload();
        }
        private void tsiDisTime_Click(object sender, EventArgs e)
        {
            this.reload(0);
            this.grbDis.Text = "Lịch chiếu phim";
            this.reload();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            reload();
        }

        private void tsiDisMovie_Click(object sender, EventArgs e)
        {
            this.reload(0);
            this.grbDis.Text = "Danh sách phim";
            this.reload();
        }

        private void tsiDisCinema_Click(object sender, EventArgs e)
        {
            this.reload(0);
            this.grbDis.Text = "Danh sách rạp";
            this.reload();
        }

        /// <summary>
        /// 
        /// </summary>
        private void btnThem_Click(object sender, EventArgs e)
        {
            switch (this.grbDis.Text)
            {
                case "Danh sách":
                    break;
                case "Danh sách nhân viên":
                    this.grbAddUser.Visible = true;
                    break;
                case "Danh sách quản trị và nhân viên":
                    this.grbAddUser.Visible = true;
                    break;
                case "Danh sách quản trị viên":
                    this.grbAddUser.Visible = true;
                    break;
                case "Danh sách rạp":
                    this.grbAddCinema.Visible = true;
                    break; 
                case "Danh sách phim":
                    this.grbAddMovie.Visible = true;
                    break;
                case "Lịch chiếu phim":
                    this.grbAddTime.Visible = true;
                    break;
            }
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }
        private void editUser()
        {
            int cur_row = this.dgvAdmin.CurrentCell.RowIndex;

            this.cur_id = int.Parse(this.dgvAdmin.Rows[cur_row].Cells[0].Value.ToString());

            var fullname = this.dgvAdmin.Rows[cur_row].Cells[1].Value.ToString();
            var username = this.dgvAdmin.Rows[cur_row].Cells[2].Value.ToString();
            var password = "";
            var typeuser = this.dgvAdmin.Rows[cur_row].Cells[3].Value.ToString();
            if (typeuser == "administrator") typeuser = "Quản trị";
            else typeuser = "Nhân viên";

            this.txtFname.Text = fullname;
            this.txtUsername.Text = username;
            this.txtPassword.Text = password;
            this.cbbType.SelectedIndex = this.cbbType.Items.IndexOf(typeuser);
        }
        private void editCinema()
        {
            int cur_row = this.dgvAdmin.CurrentCell.RowIndex;

            this.cur_id = int.Parse(this.dgvAdmin.Rows[cur_row].Cells[0].Value.ToString());

            var cinema_rownum = this.dgvAdmin.Rows[cur_row].Cells[1].Value.ToString();
            var cinema_colum = this.dgvAdmin.Rows[cur_row].Cells[2].Value.ToString();

            this.txtRownum.Text = cinema_rownum;
            this.txtColumnnum.Text = cinema_colum;
        }
        private void editMovie()
        {
            int cur_row = this.dgvAdmin.CurrentCell.RowIndex;

            this.cur_id = int.Parse(this.dgvAdmin.Rows[cur_row].Cells[0].Value.ToString());

            var moviename = this.dgvAdmin.Rows[cur_row].Cells[1].Value.ToString();
            var duration = this.dgvAdmin.Rows[cur_row].Cells[2].Value.ToString();
            var price = this.dgvAdmin.Rows[cur_row].Cells[3].Value.ToString();


            this.txtMovieName.Text = moviename;
            this.txtDuration.Text = duration;
            this.txtPrice.Text = price;
        }
        private void editTime()
        {
            int cur_row = this.dgvAdmin.CurrentCell.RowIndex;

            this.cur_id = int.Parse(this.dgvAdmin.Rows[cur_row].Cells[0].Value.ToString());


            var moviename = this.dgvAdmin.Rows[cur_row].Cells[1].Value.ToString();
            var cinemaid = this.dgvAdmin.Rows[cur_row].Cells[2].Value.ToString();
            var time_start = this.dgvAdmin.Rows[cur_row].Cells[4].Value.ToString();
            var cinema_date = this.dgvAdmin.Rows[cur_row].Cells[3].Value.ToString();
            tstUsername.Text = time_start;

            this.cbbMovieName.Text = moviename;
            this.txtDate.Text = cinema_date;
            this.cbbCinema.Text = cinemaid;
            this.cbbTime.Text = time_start; 
            
            this.cur_moviename = this.dgvAdmin.Rows[cur_row].Cells[1].Value.ToString();
            this.cur_cinema_date = this.dgvAdmin.Rows[cur_row].Cells[3].Value.ToString();
            this.cur_cinemaid = this.dgvAdmin.Rows[cur_row].Cells[2].Value.ToString();
            this.cur_time_start = this.dgvAdmin.Rows[cur_row].Cells[4].Value.ToString();
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (this.dgvAdmin.CurrentCell == null) return;
            switch (this.grbDis.Text)
            {
                case "Danh sách":
                    break;
                case "Danh sách nhân viên":
                    this.grbAddUser.Visible = true;
                    editUser();
                    break;
                case "Danh sách quản trị và nhân viên":
                    this.grbAddUser.Visible = true;
                    editUser();
                    break;
                case "Danh sách quản trị viên":
                    this.grbAddUser.Visible = true;
                    editUser();
                    break;
                case "Danh sách rạp":
                    this.grbAddCinema.Visible = true;
                    editCinema();
                    break;
                case "Danh sách phim":
                    this.grbAddMovie.Visible = true;
                    editMovie();
                    break;
                case "Lịch chiếu phim":
                    this.grbAddTime.Visible = true;
                    editTime();
                    break;
            }
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }
        public static string ToSha256(string text) =>
            string.IsNullOrEmpty(text) ? string.Empty : BitConverter.ToString(new System.Security.Cryptography.SHA256Managed().ComputeHash(System.Text.Encoding.UTF8.GetBytes(text))).Replace("-", string.Empty);

        private void btnSave_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text;
            string typeuser_alias;
            if (cbbType.Text == "Quản trị") typeuser_alias = "ad";
            else typeuser_alias = "em";

            var password = ToSha256(txtPassword.Text).ToUpper();
            string user_fullname = txtFname.Text;

            if (this.cur_id != -1) //Editing
            {

                string query = "UPDATE CINEMA_USER SET username = '" + username + "', " +
                                "typeuser_alias = '" + typeuser_alias + "', " +
                                "password = '" + password + "', " +
                                "user_fullname = '" + user_fullname + "' " +
                                "WHERE userid = '" + this.cur_id + "';";
                executeQuery(query);

                MessageBox.Show("Cập nhật thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string query = "insert into CINEMA_USER (username, typeuser_alias, password, user_fullname) " +
                                "values ('" + username + "','" + typeuser_alias + "', '" + password + "','" + user_fullname + "')";
                executeQuery(query);
                MessageBox.Show("Thêm thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.reload();
        }

        private void btnSaveCinema_Click(object sender, EventArgs e)
        {
            var cinema_rownum = txtRownum.Text;
            var cinema_colnum = txtColumnnum.Text;

            if (this.cur_id != -1) //Editing
            {

                string query = "UPDATE CINEMA SET cinema_rownum = " + cinema_rownum + ", " +
                                "cinema_colnum = '" + cinema_colnum + "' " +
                                "WHERE cinemaid = '" + this.cur_id + "';";
                executeQuery(query);

                for (var i = 0; i < int.Parse(cinema_rownum); i++)
                {
                    for (var j = 0; j < int.Parse(cinema_colnum); j++)
                    {
                        query = "insert into CINEMA_SEAT (cinemaid, row_index, col_index) " +
                                "values ('" + cur_id + "','" + i + "','" + j + "')";
                        executeQuery(query);
                    }
                }

                MessageBox.Show("Cập nhật thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string query = "insert into CINEMA (cinema_rownum, cinema_colnum) " +
                                "values ('" + cinema_rownum + "','" + cinema_colnum + "')";
                executeQuery(query);

                for (var i = 0; i < int.Parse(cinema_rownum); i++)
                {
                    for (var j = 0; j < int.Parse(cinema_colnum); j++)
                    {
                        query = "insert into CINEMA_SEAT (cinemaid, row_index, col_index) " +
                                "values ('" + cur_id + "','" + i + "','" + j + "')";
                        executeQuery(query);
                    }
                }

                MessageBox.Show("Thêm thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.reload();
        }

        private void btnSaveMovie_Click(object sender, EventArgs e)
        {
            var moviename = txtMovieName.Text;
            var duration = txtDuration.Text;
            var price = txtPrice.Text;

            if (this.cur_id != -1) //Editing
            {

                string query = "UPDATE CINEMA_MOVIE SET moviename = '" + moviename + "', " +
                                "duration = '" + duration + "', " +
                                "price = '" + price + "' " +
                                "WHERE movieid = '" + this.cur_id + "';";
                executeQuery(query);

                MessageBox.Show("Cập nhật thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string query = "insert into CINEMA_MOVIE (moviename, duration, price) " +
                                "values ('" + moviename + "','" + duration + "','" + price + "')";
                executeQuery(query);
                MessageBox.Show("Thêm thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.reload();
        }

        private void btnSaveTime_Click(object sender, EventArgs e)
        {
            var movieid = cbbMovieName.SelectedValue.ToString();
            var cinema_date = txtDate.Text;
            var cinemaid = cbbCinema.Text;
            string time_start = cbbTime.Text;

            if (this.cur_id != -1) //Editing
            {

                string query = "UPDATE CINEMA_TIME SET movieid = '" + movieid + "', " +
                                "cinemaid = '" + cinemaid + "', " +
                                "time_start = '" + time_start + "', " +
                                "cinema_date = '" + cinema_date + "' " +
                                "WHERE movieid = '" + cur_id + "' and " +
                                "cinemaid = '" + cur_cinemaid + "' and " +
                                "time_start =  '" + cur_time_start + "' and " +
                                "cinema_date = CONVERT(DATETIME,'" + cur_cinema_date + "', 105); ";
                executeQuery(query);
                MessageBox.Show("Cập nhật thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string query = "insert into CINEMA_TIME (movieid, cinemaid, time_start, cinema_date) " +
                                "values ('" + movieid + "','" + cinemaid + "','" + time_start + "',CONVERT(DATETIME,'" + cinema_date + "', 105))";
                executeQuery(query);
                MessageBox.Show("Thêm thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.reload();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.grbAddUser.Visible = false;
            this.grbAddMovie.Visible = false;
            this.grbAddTime.Visible = false;
            this.grbAddCinema.Visible = false;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            cur_id = -1;
        }

        private void btnHuyCinema_Click(object sender, EventArgs e)
        {
            this.grbAddUser.Visible = false;
            this.grbAddMovie.Visible = false;
            this.grbAddTime.Visible = false;
            this.grbAddCinema.Visible = false;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            cur_id = -1;
        }

        private void btnHuyTime_Click(object sender, EventArgs e)
        {
            this.grbAddUser.Visible = false;
            this.grbAddMovie.Visible = false;
            this.grbAddTime.Visible = false;
            this.grbAddCinema.Visible = false;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            cur_id = -1;
        }

        private void btnHuyMovie_Click(object sender, EventArgs e)
        {
            this.grbAddUser.Visible = false;
            this.grbAddMovie.Visible = false;
            this.grbAddTime.Visible = false;
            this.grbAddCinema.Visible = false;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            cur_id = -1;
        }
        private void deleteUser(int cur_row)
        {
            var userid = int.Parse(this.dgvAdmin.Rows[cur_row].Cells[0].Value.ToString());
            var user_fullname = this.dgvAdmin.Rows[cur_row].Cells[4].Value.ToString();
            DialogResult result = MessageBox.Show("Xác nhận xóa user tên " + user_fullname, "Xóa user", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string query = "delete from cinema_user where userid = '" + userid + "'";
                executeQuery(query);

                MessageBox.Show("Xóa thành công user tên " + user_fullname);
                reload();
            }
        }
        private void deleteCinema(int cur_row)
        {
            var cinemaid = int.Parse(this.dgvAdmin.Rows[cur_row].Cells[0].Value.ToString());

            DialogResult result = MessageBox.Show("Xác nhận rạp có id = " + cinemaid, "Xóa rạp", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string query = "delete from cinema_seat where cinemaid = '" + cinemaid + "'";
                executeQuery(query);
                query = "delete from cinema where cinemaid = '" + cinemaid + "'";
                executeQuery(query);

                MessageBox.Show("Xóa thành công rạp id = " + cinemaid);
                reload();
            }
        }
        private void deleteMovie(int cur_row)
        {
            var movieid = int.Parse(this.dgvAdmin.Rows[cur_row].Cells[0].Value.ToString());
            var moviename = this.dgvAdmin.Rows[cur_row].Cells[1].Value.ToString();
            DialogResult result = MessageBox.Show("Xác nhận xóa phim tên " + moviename, "Xóa movie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string query = "delete from cinema_movie where movieid = '" + movieid + "'";
                executeQuery(query);

                MessageBox.Show("Xóa thành công phim tên " + moviename);
                reload();
            }
        }
        private void deleteTime(int cur_row)
        {
            var movieid = this.dgvAdmin.Rows[cur_row].Cells[0].Value.ToString();
            var cinemaid = this.dgvAdmin.Rows[cur_row].Cells[2].Value.ToString();
            var time_start = this.dgvAdmin.Rows[cur_row].Cells[3].Value.ToString();
            var cinema_date = this.dgvAdmin.Rows[cur_row].Cells[4].Value.ToString();
            DialogResult result = MessageBox.Show("Xác nhận xóa buổi chiếu", "Xóa buổi chiếu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string query = "delete from cinema_time where " +
                               "movieid = '" + movieid + "' and " +
                               "cinemaid = '" + cinemaid + "' and " +
                               "time_start = '" + time_start + "' and " +
                               "cinema_date = CONVERT(DATETIME,'" + cinema_date + "', 105)" +
                               "";
                executeQuery(query);

                MessageBox.Show("Xóa thành công");
                reload();
            }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            int cur_row = this.dgvAdmin.CurrentCell.RowIndex;

            if (this.dgvAdmin.CurrentCell == null) return;
            switch (this.grbDis.Text)
            {
                case "Danh sách":
                    break;
                case "Danh sách nhân viên":
                    deleteUser(cur_row);
                    break;
                case "Danh sách quản trị và nhân viên":
                    deleteUser(cur_row);
                    break;
                case "Danh sách quản trị viên":
                    deleteUser(cur_row);
                    break;
                case "Danh sách rạp":
                    deleteCinema(cur_row);
                    break;
                case "Danh sách phim":
                    deleteMovie(cur_row);
                    break;
                case "Lịch chiếu phim":
                    deleteTime(cur_row);
                    break;
            }
        }
    }
}
