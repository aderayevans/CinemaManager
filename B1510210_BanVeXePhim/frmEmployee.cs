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
    public partial class frmEmployee : Form
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

        private Queue<Button> button_queue = new Queue<Button>();
        private Stack<Button> button_stack = new Stack<Button>();

        Button[] button_array;

        public frmEmployee(string userid)
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

        private void frmEmployee_Load(object sender, EventArgs e)
        {
            string query = "select * from CINEMA_USER ur " +
                            "left join CINEMA_USER_TYPE ut " +
                            "on ur.typeuser_alias = ut.typeuser_alias " +
                            "where ur.userid = " + userid.ToString();
            DataTable data = extractData(query);
            string value_type = extractValueFromRow0Column("typeuser_name", data);
            string value_username = extractValueFromRow0Column("username", data);
            tstUserType.Text = value_type;
            tstUsername.Text = value_username;

            grbSeat.Visible = false;
            btnTime.Visible = false;
            btnBack.Visible = false;
            btnSeat.Visible = false;
            btnPrint.Visible = false;
        }

        private void disMovie()
        {
            reload();
            string query = "select * from CINEMA_MOVIE";
            DataTable data = extractData(query);

            DataColumn[] columns =
            {
                data.Columns[0],    //movieid
                data.Columns[1],     //moviename
                data.Columns[2],     //duration
                data.Columns[3]     //price
            };
            this.dgvData.Columns.Add("movieid", "ID");
            this.dgvData.Columns.Add("moviename", "Tên Phim");
            this.dgvData.Columns.Add("duration", "Thời lượng");
            this.dgvData.Columns.Add("price", "Giá vé");

            foreach (DataRow row in data.Rows)
            {
                this.dgvData.Rows.Add(row.ItemArray[0].ToString(),
                                        row.ItemArray[1].ToString(),
                                        row.ItemArray[2].ToString(),
                                        row.ItemArray[3].ToString()
                                       );
            }
        }
        private void tsiDisMovie_Click(object sender, EventArgs e)
        {
            btnTime.Visible = true;
            btnBack.Visible = false;
            btnSeat.Visible = false;
            disMovie();
        }

        private void disTime()
        {
            reload();
            string query = "select * from CINEMA_TIME ct left join CINEMA_MOVIE cm on ct.MOVIEID = cm.MOVIEID";
            DataTable data = extractData(query);

            DataColumn[] columns =
            {
                data.Columns[0],    //movieid
                data.Columns[5],     //moviename
                data.Columns[1],     //cinemaid
                data.Columns[3],     //cinema_date
                data.Columns[2],     //time_start
                data.Columns[6],     //duration
                data.Columns[7]     //price
            };
            this.dgvData.Columns.Add("movieid", "MovieID");
            this.dgvData.Columns.Add("moviename", "Tên phim");
            this.dgvData.Columns.Add("cinemaid", "Rạp");
            this.dgvData.Columns.Add("cinema_date", "Ngày chiếu");
            this.dgvData.Columns.Add("time_start", "Giờ bắt đầu");
            this.dgvData.Columns.Add("duration", "Thời lượng");
            this.dgvData.Columns.Add("price", "Giá vé");

            foreach (DataRow row in data.Rows)
            {

                DateTime date = DateTime.Parse(row.ItemArray[3].ToString());

                this.dgvData.Rows.Add(row.ItemArray[0].ToString(),
                                        row.ItemArray[5].ToString(),
                                        row.ItemArray[1].ToString(),
                                        date.ToString("dd/MM/yyyy"),
                                        row.ItemArray[2].ToString(),
                                        row.ItemArray[6].ToString(),
                                        row.ItemArray[7].ToString()
                                       );
            }
        }

        private void tsiDisTime_Click(object sender, EventArgs e)
        {
            btnTime.Visible = false;
            btnBack.Visible = false;
            btnSeat.Visible = true;
            disTime();
        }
        private void disSeat()
        {
            resetMemory();
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            button_array = grbSeat.Controls.OfType<Button>().ToArray();
            foreach (Button btn in button_array)
            {
                if (btn == btnScreen || btn == btnReset || btn == btnConfirm) continue;
                string content = btn.Text;

                char row_chr = content[0];
                int row_index = takeRowInt(row_chr);
                int col_index = int.Parse(content[1].ToString()) - 1;

                var movieid = cur_id.ToString();
                var cinemaid = this.txtCinemaID.Text;
                var time_start = this.txtTime.Text;
                var cinema_date = this.txtDate.Text;

                SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM ticket " +
                                                "where cin_cinemaid = '" + cinemaid + "' and " +
                                                "row_index = '" + row_index + "' and " +
                                                "col_index = '" + col_index + "' and " +
                                                "movieid = '" + movieid + "' and " +
                                                "cinemaid = '" + cinemaid + "' and " +
                                                "time_start = '" + time_start + "' and " +
                                                "cinema_date = CONVERT(DATETIME,'" + cinema_date + "', 105)"
                                                , conn);
                Int32 count = (Int32)comm.ExecuteScalar();

                if (count > 0)
                {
                    btn.Enabled = false;
                    btn.BackColor = System.Drawing.Color.Silver;
                }
                else
                {
                    btn.Enabled = true;
                    btn.BackColor = System.Drawing.Color.Lime;
                }
            }
        }
        private void btnSeat_Click(object sender, EventArgs e)
        {
            grbSeat.Visible = true;

            int cur_row = this.dgvData.CurrentCell.RowIndex;

            var movieid = this.dgvData.Rows[cur_row].Cells[0].Value.ToString();
            cur_id = int.Parse(movieid);
            var cinemaid = this.dgvData.Rows[cur_row].Cells[2].Value.ToString();
            var time_start = this.dgvData.Rows[cur_row].Cells[4].Value.ToString();
            var cinema_date = this.dgvData.Rows[cur_row].Cells[3].Value.ToString();

            string query = "select * from CINEMA_TIME ct left join CINEMA_MOVIE cm on ct.MOVIEID = cm.MOVIEID " +
                            "left join CINEMA_SEAT cs on ct.CINEMAID = cs.CINEMAID " +
                            "where ct.movieid = '" + movieid + "' and " +
                            "ct.cinemaid = '" + cinemaid + "' and " +
                            "time_start = '" + time_start + "' and " +
                            "cinema_date = CONVERT(DATETIME,'" + cinema_date + "', 105)";
            DataTable data = extractData(query);

            foreach (DataRow row in data.Rows)
            {
                DateTime date = DateTime.Parse(row.ItemArray[3].ToString());

                this.txtMovieName.Text = row.ItemArray[5].ToString();
                this.txtCinemaID.Text = row.ItemArray[1].ToString();
                this.txtDate.Text = date.ToString("dd/MM/yyyy");
                this.txtTime.Text = row.ItemArray[2].ToString();
                this.txtPrice.Text = row.ItemArray[7].ToString();
            }
            disSeat();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.txtMovieName.Clear();
            btnBack.Visible = false;
            btnSeat.Visible = false;
            btnTime.Visible = true;
            disMovie();
            resetMemory();
            grbSeat.Visible = false;
        }

        private void reload()
        {
            this.dgvData.DataSource = null;
            this.dgvData.Rows.Clear();
            this.dgvData.Columns.Clear();
            btnPrint.Visible = false;
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

        private void btnTime_Click(object sender, EventArgs e)
        {
            btnTime.Visible = false;
            btnSeat.Visible = true;
            btnBack.Visible = true;


            int cur_row = this.dgvData.CurrentCell.RowIndex;
            this.cur_id = int.Parse(this.dgvData.Rows[cur_row].Cells[0].Value.ToString());
            reload();

            string query = "select * from CINEMA_TIME ct left join CINEMA_MOVIE cm on ct.MOVIEID = cm.MOVIEID " +
                            "where ct.movieid = '" + cur_id + "'";
            DataTable data = extractData(query);

            DataColumn[] columns =
            {
                data.Columns[0],    //movieid
                data.Columns[5],     //moviename
                data.Columns[1],     //cinemaid
                data.Columns[3],     //cinema_date
                data.Columns[2],     //time_start
                data.Columns[6],     //duration
                data.Columns[7]     //price
            };
            this.dgvData.Columns.Add("movieid", "MovieID");
            this.dgvData.Columns.Add("moviename", "Tên phim");
            this.dgvData.Columns.Add("cinemaid", "Rạp");
            this.dgvData.Columns.Add("cinema_date", "Ngày chiếu");
            this.dgvData.Columns.Add("time_start", "Giờ bắt đầu");
            this.dgvData.Columns.Add("duration", "Thời lượng");
            this.dgvData.Columns.Add("price", "Giá vé");

            foreach (DataRow row in data.Rows)
            {

                DateTime date = DateTime.Parse(row.ItemArray[3].ToString());

                this.dgvData.Rows.Add(row.ItemArray[0].ToString(),
                                        row.ItemArray[5].ToString(),
                                        row.ItemArray[1].ToString(),
                                        date.ToString("dd/MM/yyyy"),
                                        row.ItemArray[2].ToString(),
                                        row.ItemArray[6].ToString(),
                                        row.ItemArray[7].ToString()
                                       );
            }
        }
        private int takeRowInt(char rowchr)
        {
            int row = 0;
            switch (rowchr)
            {
                case 'a':
                    row = 0;
                    break;
                case 'b':
                    row = 1;
                    break;
                case 'c':
                    row = 2;
                    break;
                case 'd':
                    row = 3;
                    break;
                case 'e':
                    row = 4;
                    break;
                case 'f':
                    row = 5;
                    break;
                case 'g':
                    row = 6;
                    break;
                case 'h':
                    row = 7;
                    break;
                case 'i':
                    row = 8;
                    break;
            }
            return row;
        }
        private void btnSelectSeat_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            btn.BackColor = System.Drawing.Color.Yellow;

            button_queue.Enqueue(btn);
            button_stack.Push(btn);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            int num = button_queue.Count;
            if (num == 0) return;
            lblCount.Text = num.ToString();
            DialogResult result = MessageBox.Show("Xác nhận xuất " + num + " vé", "Tạo vé", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {


                txtSeat.Text = button_queue.First().Text;
                MessageBox.Show("Thêm vào hàng đợi thành công", "Tạo vé", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            btnPrint.Visible = true;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Xác nhận xuất vé", "Tạo vé", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                string content = button_queue.First().Text;

                char row_chr = content[0];
                int row_index = takeRowInt(row_chr);
                int col_index = int.Parse(content[1].ToString()) - 1;
                var movieid = cur_id.ToString();
                var cinemaid = this.txtCinemaID.Text;
                var time_start = this.txtTime.Text;
                var cinema_date = this.txtDate.Text;



                string query = "insert into TICKET (cin_cinemaid, row_index, col_index, movieid, cinemaid, time_start, cinema_date) " +
                                "values ('" +
                                cinemaid + "','" +
                                row_index + "','" +
                                col_index + "','" +
                                movieid + "','" + 
                                cinemaid + "','" + 
                                time_start + 
                                "',CONVERT(DATETIME,'" + cinema_date + "', 105))";
                executeQuery(query);

                button_queue.First().BackColor = System.Drawing.Color.Silver;
                button_queue.First().Enabled = false;
                button_queue.Dequeue();
                lblCount.Text = button_queue.Count.ToString();
                if (button_queue.Count > 0)
                {
                    txtSeat.Text = button_queue.First().Text;
                }
                else
                {
                    btnPrint.Visible = false;
                }
                MessageBox.Show("Tạo vé thành công", "Tạo vé", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void resetMemory()
        {
            while (button_stack.Count != 0)
            {
                Button btn = button_stack.Pop();
                btn.Enabled = true;
                btn.BackColor = System.Drawing.Color.Lime;
            }
            button_queue.Clear();
            lblCount.Text = "";
            btnPrint.Visible = false;
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            resetMemory();
        }
    }
}
