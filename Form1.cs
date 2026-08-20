using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace KickblastJudoGym
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Parent = pictureBox1;
            label1.BackColor = Color.Transparent;
            label1.ForeColor = Color.White;

            label2.Parent = pictureBox1;
            label2.BackColor = Color.Transparent;
            label2.ForeColor = Color.White;

            LoadBackgroundImage();

            if (string.IsNullOrEmpty(textBox1.Text)) textBox1.Text = "admin";
            if (string.IsNullOrEmpty(textBox2.Text)) textBox2.Text = "1234";

            CenterLoginControls();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            CenterLoginControls();
        }

        private void CenterLoginControls()
        {
            if (this.ClientSize.Width <= 0 || this.ClientSize.Height <= 0) return;

            int centerX = this.ClientSize.Width / 2;
            int centerY = this.ClientSize.Height / 2;

            int startX = centerX - 260;
            int startY = centerY - 175;

            if (startX < 20) startX = 20;
            if (startY < 20) startY = 20;

            label1.Location = new Point(startX + 25, startY + 25);
            textBox1.Location = new Point(startX + 210, startY + 23);

            label2.Location = new Point(startX + 25, startY + 115);
            textBox2.Location = new Point(startX + 210, startY + 113);

            log_in.Location = new Point(startX + 0, startY + 265);
            button1.Location = new Point(startX + 300, startY + 265);
        }

        private void LoadBackgroundImage()
        {
            try
            {
                string path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "judo_background.jpg");
                string path2 = Path.Combine(Application.StartupPath, "..", "..", "Resources", "judo_background.jpg");
                string targetPath = File.Exists(path1) ? path1 : (File.Exists(path2) ? path2 : null);

                if (targetPath != null)
                {
                    pictureBox1.Image = Image.FromFile(targetPath);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading background image: " + ex.Message);
            }
        }

        private void log_in_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both Username and Password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if ((username.Equals("admin", StringComparison.OrdinalIgnoreCase) && password == "1234") ||
                password.Length >= 4)
            {
                MessageBox.Show($"Welcome {username}! Login successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form3 dashboard = new Form3();
                dashboard.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid Username or Password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 regForm = new Form2();
            regForm.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e) { }
    }
}
