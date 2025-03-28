using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace caro27_033.GUI
{
    public partial class frmScore : Form
    {
        public frmScore()
        {
            InitializeComponent();
        }
        static string connString = @"Data Source=LaptopOfNino;Initial Catalog=Score;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        SqlConnection conn = new SqlConnection(connString);

        private void CustomizeDataGridView()
        {
            // Tự động co giãn
            dtgScoreBoard.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtgScoreBoard.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Font chữ
            dtgScoreBoard.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dtgScoreBoard.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            // Header màu xanh, chữ trắng, căn giữa
            dtgScoreBoard.EnableHeadersVisualStyles = false;
            dtgScoreBoard.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dtgScoreBoard.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgScoreBoard.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Màu nền xen kẽ
            dtgScoreBoard.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            // Căn giữa dữ liệu
            dtgScoreBoard.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            if (!(dtgScoreBoard.DataSource == null && dtgScoreBoard.Columns["PlayerName"] == null))
            {
                //dtgScoreBoard.Columns["PlayerName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
                

            // Viền & Border
            dtgScoreBoard.BorderStyle = BorderStyle.Fixed3D;
            dtgScoreBoard.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // Không cho chỉnh sửa
            dtgScoreBoard.ReadOnly = true;

            // Chặn chọn nhiều dòng
            dtgScoreBoard.MultiSelect = false;

            // Ẩn cột ID
            //dtgScoreBoard.Columns["ID"].Visible = false;

            // Hiệu ứng hover
            dtgScoreBoard.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
            dtgScoreBoard.DefaultCellStyle.SelectionForeColor = Color.White;
        }


        private void frmScore_Load(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT *FROM Score", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                dtgScoreBoard.DataSource = dt;

                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            CustomizeDataGridView();

        }
    }
}