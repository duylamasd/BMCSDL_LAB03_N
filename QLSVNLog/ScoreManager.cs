using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSVNLog
{
    public partial class ScoreManager : Form
    {
        string username { get; set; }
        string password { get; set; }
        SqlConnection connection { get; set; }
        string pubkey { get; set; }
        public ScoreManager(string username, string password, SqlConnection connection)
        {
            InitializeComponent();

            this.username = username;
            this.password = password;
            this.connection = connection;

            SqlCommand getPubkeyCommand = new SqlCommand("select PUBKEY from NHANVIEN where TENDN = @username", connection);
            getPubkeyCommand.Parameters.Add("@username", SqlDbType.NVarChar);
            getPubkeyCommand.Parameters["@username"].Value = username;

            using (SqlDataReader reader = getPubkeyCommand.ExecuteReader())
                while (reader.Read())
                    this.pubkey = reader[0].ToString();

            SqlCommand getScoreTable = new SqlCommand("exec SP_SEL_PUBLIC_BANGDIEM @pubkey, @password", connection);
            getScoreTable.Parameters.Add("@pubkey", SqlDbType.VarChar);
            getScoreTable.Parameters.Add("@password", SqlDbType.NVarChar);
            getScoreTable.Parameters["@pubkey"].Value = pubkey;
            getScoreTable.Parameters["@password"].Value = password;

            DataTable scoreTable = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter(getScoreTable))
                adapter.Fill(scoreTable);
            scoreView.DataSource = scoreTable;
        }

        private void apply_Click(object sender, EventArgs e)
        {
            if (studentIDBox.Text == "")
            {
                MessageBox.Show("Please insert MASV!");
                return;
            }

            if (subjectIDBox.Text == "")
            {
                MessageBox.Show("Please insert MAHP!");
                return;
            }

            if (scoreBox.Text == "")
            {
                MessageBox.Show("Please insert score!");
                return;
            }

            SqlCommand insertCommand = new SqlCommand("exec SP_INS_PUBLIC_BANGDIEM @pubkey, @studentID, @subjectID, @score", connection);
            insertCommand.Parameters.Add("@pubkey", SqlDbType.VarChar);
            insertCommand.Parameters.Add("@studentID", SqlDbType.VarChar);
            insertCommand.Parameters.Add("@subjectID", SqlDbType.VarChar);
            insertCommand.Parameters.Add("@score", SqlDbType.Float);
            insertCommand.Parameters["@pubkey"].Value = pubkey;
            insertCommand.Parameters["@studentID"].Value = studentIDBox.Text;
            insertCommand.Parameters["@subjectID"].Value = subjectIDBox.Text;
            insertCommand.Parameters["@score"].Value = Convert.ToSingle(scoreBox.Text);
            insertCommand.ExecuteNonQuery();

            SqlCommand reloadCommand = new SqlCommand("exec SP_SEL_PUBLIC_BANGDIEM @pubkey, @password", connection);
            reloadCommand.Parameters.Add("@pubkey", SqlDbType.VarChar);
            reloadCommand.Parameters.Add("@password", SqlDbType.NVarChar);
            reloadCommand.Parameters["@pubkey"].Value = pubkey;
            reloadCommand.Parameters["@password"].Value = password;

            DataTable scoreTable = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter(reloadCommand))
                adapter.Fill(scoreTable);
            scoreView.DataSource = scoreTable;
        }
    }
}
