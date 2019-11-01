using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project1;
using System.Windows.Forms;
using System.IO;

namespace Project1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string imageLocation = "";
        SqlCommand cmd;
        private void Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP; *.JPG;*.GIF| All files(*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                imageLocation = ofd.FileName.ToString();
                pictureBox1.ImageLocation = imageLocation;
            }
        }
        private async void Save_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] images = null;
                FileStream stream = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(stream);
                images = br.ReadBytes((int)stream.Length);
                using (SqlConnection connection = new SqlConnection(Connection.GetString()))
                {
                    await connection.OpenAsync();
                    string query = $"insert into tbl1(FileName, Data) values('{textBox2.Text}', @images)";
                    cmd = new SqlCommand(query, connection);
                    cmd.Parameters.Add(new SqlParameter("@images", images));
                    await cmd.ExecuteNonQueryAsync();
                    MessageBox.Show("Data saved Successfully...!", "System Notification!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
