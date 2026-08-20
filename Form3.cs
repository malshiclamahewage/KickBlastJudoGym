using System;
using System.Windows.Forms;

namespace KickblastJudoGym
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            if (richTextBox1 != null && string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                richTextBox1.Text = "Welcome to KickBlast Judo Gym Management System!\n\n" +
                    "Features & Information:\n" +
                    "1. Athlete Registration & Fee Calculation System\n" +
                    "2. Itemized Cost Breakdown (Training Plans, Private Tuition, Competition Fees)\n" +
                    "3. Athlete Weight Category Tracking & Weight Comparison\n" +
                    "4. Data Persistence & Management (Save, Update, Delete, Search)\n\n" +
                    "Use the navigation buttons to manage athletes or log out of the system.";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Log Out Button
            DialogResult dr = MessageBox.Show("Are you sure you want to log out?", "Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Form1 loginForm = new Form1();
                loginForm.Show();
                this.Close();
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Event handler stub
        }

        private void buttonRegistration_Click(object sender, EventArgs e)
        {
            Form2 regForm = new Form2();
            regForm.Show();
            this.Hide();
        }
    }
}