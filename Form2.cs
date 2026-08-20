using System;
using System.Text;
using System.Windows.Forms;

namespace KickblastJudoGym
{
    public partial class Form2 : Form
    {
        private int currentAthleteId = 0;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Placeholder hints / default seed values
            if (string.IsNullOrEmpty(textBox1.Text)) textBox1.Text = "Malshi Lamahewage";
            if (string.IsNullOrEmpty(textBox2.Text)) textBox2.Text = "Intermediate";
            if (string.IsNullOrEmpty(textBox3.Text)) textBox3.Text = "72.5";
            if (string.IsNullOrEmpty(textBox4.Text)) textBox4.Text = "73.0";
            if (string.IsNullOrEmpty(textBox5.Text)) textBox5.Text = "2";
            if (string.IsNullOrEmpty(textBox6.Text)) textBox6.Text = "3";
        }

        private bool ValidateAndCalculate(out AthleteRecord record, out string breakdown)
        {
            record = null;
            breakdown = "";

            string name = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter the Athlete Name.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return false;
            }

            string planInput = textBox2.Text.Trim();
            decimal planCost = 0m;
            string planNormalized = "";

            if (planInput.IndexOf("begin", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                planNormalized = "Beginner";
                planCost = 25.00m;
            }
            else if (planInput.IndexOf("inter", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                planNormalized = "Intermediate";
                planCost = 30.00m;
            }
            else if (planInput.IndexOf("elite", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                planNormalized = "Elite";
                planCost = 35.00m;
            }
            else
            {
                MessageBox.Show("Invalid Training Plan! Please enter Beginner, Intermediate, or Elite.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return false;
            }

            if (!double.TryParse(textBox3.Text.Trim(), out double currentWeight) || currentWeight <= 0)
            {
                MessageBox.Show("Please enter a valid positive number for Current Weight (kg).", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus();
                return false;
            }

            if (!double.TryParse(textBox4.Text.Trim(), out double compWeight) || compWeight <= 0)
            {
                MessageBox.Show("Please enter a valid positive number for Competition Weight Category (kg).", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox4.Focus();
                return false;
            }

            if (!int.TryParse(textBox5.Text.Trim(), out int competitions) || competitions < 0)
            {
                MessageBox.Show("Please enter a valid non-negative integer for Number of Competitions.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox5.Focus();
                return false;
            }

            // Rule: Beginners cannot enter competitions!
            if (competitions > 0 && planNormalized == "Beginner")
            {
                MessageBox.Show("Rule Violation: Beginners are not eligible to enter competitions! Competitions are restricted to Intermediate and Elite athletes only.", "Rule Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox5.Focus();
                return false;
            }

            if (!int.TryParse(textBox6.Text.Trim(), out int coachingHoursWeekly) || coachingHoursWeekly < 0)
            {
                MessageBox.Show("Please enter a valid non-negative integer for Private Coaching Hours.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox6.Focus();
                return false;
            }

            // Rule: Maximum 5 hours private coaching per week!
            if (coachingHoursWeekly > 5)
            {
                MessageBox.Show("Rule Violation: Athletes can receive a maximum of 5 hours of private coaching per week.", "Rule Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox6.Focus();
                return false;
            }

            int coachingHoursMonthly = coachingHoursWeekly * 4; // 4 weeks per month
            decimal coachingCost = coachingHoursMonthly * 9.50m;
            decimal competitionCost = competitions * 22.00m;
            decimal totalCost = planCost + competitionCost + coachingCost;

            // Weight Category Comparison
            string weightStatus = "";
            double weightDiff = currentWeight - compWeight;
            if (Math.Abs(weightDiff) < 0.01)
            {
                weightStatus = "On Weight (Matches competition category)";
            }
            else if (weightDiff > 0)
            {
                weightStatus = $"Over Weight by {weightDiff:F2} kg";
            }
            else
            {
                weightStatus = $"Under Weight by {Math.Abs(weightDiff):F2} kg";
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("==========================================");
            sb.AppendLine(" KICKBLAST JUDO GYM - MONTHLY COST REPORT");
            sb.AppendLine("==========================================");
            sb.AppendLine($"Athlete Name:        {name}");
            sb.AppendLine($"Training Plan:       {planNormalized} (£{planCost:F2}/mo)");
            sb.AppendLine($"Current Weight:      {currentWeight:F2} kg");
            sb.AppendLine($"Comp Weight Cat:     {compWeight:F2} kg");
            sb.AppendLine($"Weight Status:       {weightStatus}");
            sb.AppendLine("------------------------------------------");
            sb.AppendLine("ITEMIZED COST BREAKDOWN:");
            sb.AppendLine($"1. Training Plan Fee:           £{planCost,8:F2}");
            sb.AppendLine($"2. Competition Fee ({competitions} @ £22.00): £{competitionCost,8:F2}");
            sb.AppendLine($"3. Private Coaching ({coachingHoursWeekly}h/wk x4): £{coachingCost,8:F2}");
            sb.AppendLine("------------------------------------------");
            sb.AppendLine($"TOTAL MONTHLY COST:             £{totalCost,8:F2}");
            sb.AppendLine("==========================================");

            breakdown = sb.ToString();

            record = new AthleteRecord
            {
                AthleteID = currentAthleteId,
                AthleteName = name,
                TrainingPlan = planNormalized,
                CurrentWeight = currentWeight,
                CompetitionWeight = compWeight,
                NumCompetitions = competitions,
                PrivateCoachingHours = coachingHoursWeekly,
                TotalCost = totalCost,
                BreakdownText = breakdown
            };

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Register / Calculate
            if (ValidateAndCalculate(out AthleteRecord record, out string breakdown))
            {
                richTextBoxOutput.Text = breakdown;
                MessageBox.Show("Fee calculation completed successfully!", "Calculation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Save to Database
            if (ValidateAndCalculate(out AthleteRecord record, out string breakdown))
            {
                richTextBoxOutput.Text = breakdown;
                if (DatabaseHelper.SaveAthlete(record, out string msg))
                {
                    if (record.AthleteID > 0) currentAthleteId = record.AthleteID;
                    MessageBox.Show(msg, "Database Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Clear
            currentAthleteId = 0;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            richTextBoxOutput.Clear();
            textBox1.Focus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Update Action
            if (ValidateAndCalculate(out AthleteRecord record, out string breakdown))
            {
                record.AthleteID = currentAthleteId;
                richTextBoxOutput.Text = breakdown;
                if (DatabaseHelper.UpdateAthlete(record, out string msg))
                {
                    if (record.AthleteID > 0) currentAthleteId = record.AthleteID;
                    MessageBox.Show(msg, "Record Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Delete Action
            string name = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter the Athlete Name or ID to delete.", "Delete Action", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show($"Are you sure you want to delete athlete record '{name}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                if (DatabaseHelper.DeleteAthlete(name, out string msg))
                {
                    MessageBox.Show(msg, "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button3_Click(sender, e);
                }
                else
                {
                    MessageBox.Show(msg, "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Search Action
            string query = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(query))
            {
                MessageBox.Show("Please enter an Athlete Name or ID in the Athlete Name field to search.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var athlete = DatabaseHelper.SearchAthlete(query);
            if (athlete != null)
            {
                currentAthleteId = athlete.AthleteID;
                textBox1.Text = athlete.AthleteName;
                textBox2.Text = athlete.TrainingPlan;
                textBox3.Text = athlete.CurrentWeight.ToString();
                textBox4.Text = athlete.CompetitionWeight.ToString();
                textBox5.Text = athlete.NumCompetitions.ToString();
                textBox6.Text = athlete.PrivateCoachingHours.ToString();
                richTextBoxOutput.Text = athlete.BreakdownText;
                MessageBox.Show($"Athlete '{athlete.AthleteName}' found (ID: {athlete.AthleteID})! Details loaded.", "Search Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"No athlete record found matching '{query}'.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonDashboard_Click(object sender, EventArgs e)
        {
            Form3 dashboard = new Form3();
            dashboard.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
    }
}
