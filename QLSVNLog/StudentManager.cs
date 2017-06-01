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
    public partial class StudentManager : Form
    {
        string username { get; set; }
        string password { get; set; }
        string classID { get; set; }
        SqlConnection connection { get; set; }
        public StudentManager(string username, string password, string classID, SqlConnection connection)
        {
            InitializeComponent();

            this.username = username;
            this.password = password;
            this.classID = classID;
            this.connection = connection;

            SqlCommand command = new SqlCommand("select * from SINHVIEN where MALOP = @classID", connection);
            command.Parameters.Add("@classID", SqlDbType.VarChar);
            command.Parameters["@classID"].Value = classID;

            DataTable studentTable = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                adapter.Fill(studentTable);

            studentView.DataSource = studentTable;
        }
    }
}
