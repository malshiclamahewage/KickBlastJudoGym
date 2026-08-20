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
            // Set up high-contrast crisp labels over background
            label1.Parent = pictureBox1;
            label1.BackColor = Color.Transparent;
            label1.ForeColor = Color.White;

            label2.Parent = pictureBox1;
            label2.BackColor = Color.Transparent;
            label2.ForeColor = Color.White;

            // Load background image
            LoadBackgroundImage();

            // Default credentials
            if (string.IsNullOrEmpty(textBox1.Text)) textBox1.Text = "admin";
            if (string.IsNullOrEmpty(textBox2.Text)) textBox2.Text = "1234";
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
