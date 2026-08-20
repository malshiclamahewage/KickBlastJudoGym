using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KickblastJudoGym
{
    public class AthleteRecord
    {
        public int AthleteID { get; set; }
        public string AthleteName { get; set; }
        public string TrainingPlan { get; set; }
        public double CurrentWeight { get; set; }
        public double CompetitionWeight { get; set; }
        public int NumCompetitions { get; set; }
        public int PrivateCoachingHours { get; set; }
        public decimal TotalCost { get; set; }
        public string BreakdownText { get; set; }
    }

    public static class DatabaseHelper
    {
        private static readonly string DataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kickblast_athletes.json");
        private static List<AthleteRecord> _athletes = new List<AthleteRecord>();
        private static int _nextId = 1;

        static DatabaseHelper()
        {
            LoadData();
        }

        private static void LoadData()
        {
            try
            {
                if (File.Exists(DataFilePath))
                {
                    string json = File.ReadAllText(DataFilePath);
                    var list = SimpleJsonParser.DeserializeList(json);
                    if (list != null && list.Count > 0)
                    {
                        _athletes = list;
                        _nextId = _athletes.Max(a => a.AthleteID) + 1;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading data: " + ex.Message);
            }

            // Default seed data
            _athletes = new List<AthleteRecord>
            {
                new AthleteRecord
                {
                    AthleteID = 1,
                    AthleteName = "Malshi Lamahewage",
                    TrainingPlan = "Intermediate",
                    CurrentWeight = 72.5,
                    CompetitionWeight = 73.0,
                    NumCompetitions = 2,
                    PrivateCoachingHours = 3,
                    TotalCost = 188.00m,
                    BreakdownText = "Itemized Costs for Malshi Lamahewage:\n- Training Plan (Intermediate): £30.00\n- Competitions (2 @ £22.00): £44.00\n- Private Coaching (3 hrs/wk x 4 wks @ £9.50/hr): £114.00\nTotal Monthly Fee: £188.00\nWeight Category: Under Weight by 0.50 kg"
                }
            };
            _nextId = 2;
            SaveData();
        }

        private static void SaveData()
        {
            try
            {
                string json = SimpleJsonParser.SerializeList(_athletes);
                File.WriteAllText(DataFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save database file: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool SaveAthlete(AthleteRecord record, out string message)
        {
            var existing = _athletes.FirstOrDefault(a => (record.AthleteID > 0 && a.AthleteID == record.AthleteID) ||
                                                         a.AthleteName.Equals(record.AthleteName, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.AthleteName = record.AthleteName;
                existing.TrainingPlan = record.TrainingPlan;
                existing.CurrentWeight = record.CurrentWeight;
                existing.CompetitionWeight = record.CompetitionWeight;
                existing.NumCompetitions = record.NumCompetitions;
                existing.PrivateCoachingHours = record.PrivateCoachingHours;
                existing.TotalCost = record.TotalCost;
                existing.BreakdownText = record.BreakdownText;
                SaveData();
                message = $"Athlete '{record.AthleteName}' updated successfully!";
                return true;
            }
            else
            {
                record.AthleteID = _nextId++;
                _athletes.Add(record);
                SaveData();
                message = $"Athlete '{record.AthleteName}' saved successfully with ID: {record.AthleteID}!";
                return true;
            }
        }

        public static AthleteRecord SearchAthlete(string nameOrId)
        {
            if (string.IsNullOrWhiteSpace(nameOrId)) return null;

            if (int.TryParse(nameOrId.Trim(), out int id))
            {
                var match = _athletes.FirstOrDefault(a => a.AthleteID == id);
                if (match != null) return match;
            }

            return _athletes.FirstOrDefault(a => a.AthleteName.Equals(nameOrId.Trim(), StringComparison.OrdinalIgnoreCase) ||
                                                 a.AthleteName.IndexOf(nameOrId.Trim(), StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public static bool UpdateAthlete(AthleteRecord record, out string message)
        {
            AthleteRecord existing = null;
            if (record.AthleteID > 0)
            {
                existing = _athletes.FirstOrDefault(a => a.AthleteID == record.AthleteID);
            }
            if (existing == null && !string.IsNullOrWhiteSpace(record.AthleteName))
            {
                existing = _athletes.FirstOrDefault(a => a.AthleteName.Equals(record.AthleteName, StringComparison.OrdinalIgnoreCase));
            }

            if (existing != null)
            {
                existing.AthleteName = record.AthleteName;
                existing.TrainingPlan = record.TrainingPlan;
                existing.CurrentWeight = record.CurrentWeight;
                existing.CompetitionWeight = record.CompetitionWeight;
                existing.NumCompetitions = record.NumCompetitions;
                existing.PrivateCoachingHours = record.PrivateCoachingHours;
                existing.TotalCost = record.TotalCost;
                existing.BreakdownText = record.BreakdownText;
                SaveData();
                message = $"Athlete '{record.AthleteName}' updated successfully!";
                return true;
            }
            else
            {
                record.AthleteID = _nextId++;
                _athletes.Add(record);
                SaveData();
                message = $"Athlete '{record.AthleteName}' saved as new record (ID: {record.AthleteID})!";
                return true;
            }
        }

        public static bool DeleteAthlete(string nameOrId, out string message)
        {
            var existing = SearchAthlete(nameOrId);
            if (existing == null)
            {
                message = "Athlete record not found for deletion.";
                return false;
            }

            _athletes.Remove(existing);
            SaveData();
            message = $"Athlete '{existing.AthleteName}' deleted successfully!";
            return true;
        }

        public static List<AthleteRecord> GetAllAthletes()
        {
            return _athletes.ToList();
        }
    }

    public static class SimpleJsonParser
    {
        public static string SerializeList(List<AthleteRecord> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[");
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                sb.AppendLine("  {");
                sb.AppendLine($"    \"AthleteID\": {item.AthleteID},");
                sb.AppendLine($"    \"AthleteName\": \"{Escape(item.AthleteName)}\",");
                sb.AppendLine($"    \"TrainingPlan\": \"{Escape(item.TrainingPlan)}\",");
                sb.AppendLine($"    \"CurrentWeight\": {item.CurrentWeight},");
                sb.AppendLine($"    \"CompetitionWeight\": {item.CompetitionWeight},");
                sb.AppendLine($"    \"NumCompetitions\": {item.NumCompetitions},");
                sb.AppendLine($"    \"PrivateCoachingHours\": {item.PrivateCoachingHours},");
                sb.AppendLine($"    \"TotalCost\": {item.TotalCost},");
                sb.AppendLine($"    \"BreakdownText\": \"{Escape(item.BreakdownText)}\"");
                sb.Append("  }");
                if (i < list.Count - 1) sb.Append(",");
                sb.AppendLine();
            }
            sb.AppendLine("]");
            return sb.ToString();
        }

        public static List<AthleteRecord> DeserializeList(string json)
        {
            List<AthleteRecord> list = new List<AthleteRecord>();
            if (string.IsNullOrWhiteSpace(json)) return list;

            string[] blocks = json.Split(new[] { "}," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var block in blocks)
            {
                var rec = new AthleteRecord();
                rec.AthleteID = GetInt(block, "AthleteID");
                rec.AthleteName = GetString(block, "AthleteName");
                rec.TrainingPlan = GetString(block, "TrainingPlan");
                rec.CurrentWeight = GetDouble(block, "CurrentWeight");
                rec.CompetitionWeight = GetDouble(block, "CompetitionWeight");
                rec.NumCompetitions = GetInt(block, "NumCompetitions");
                rec.PrivateCoachingHours = GetInt(block, "PrivateCoachingHours");
                rec.TotalCost = GetDecimal(block, "TotalCost");
                rec.BreakdownText = GetString(block, "BreakdownText");
                if (rec.AthleteID > 0 || !string.IsNullOrEmpty(rec.AthleteName))
                {
                    list.Add(rec);
                }
            }
            return list;
        }

        private static string Escape(string str)
        {
            if (str == null) return "";
            return str.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "").Replace("\n", "\\n");
        }

        private static string Unescape(string str)
        {
            if (str == null) return "";
            return str.Replace("\\n", "\r\n").Replace("\\\"", "\"").Replace("\\\\", "\\");
        }

        private static string GetString(string block, string key)
        {
            string search = $"\"{key}\": \"";
            int start = block.IndexOf(search);
            if (start < 0) return "";
            start += search.Length;
            int end = block.IndexOf("\"", start);
            if (end < 0) return "";
            return Unescape(block.Substring(start, end - start));
        }

        private static int GetInt(string block, string key)
        {
            string search = $"\"{key}\": ";
            int start = block.IndexOf(search);
            if (start < 0) return 0;
            start += search.Length;
            int end = block.IndexOf(",", start);
            if (end < 0) end = block.IndexOf("\n", start);
            if (end < 0) return 0;
            string val = block.Substring(start, end - start).Trim();
            int.TryParse(val, out int result);
            return result;
        }

        private static double GetDouble(string block, string key)
        {
            string search = $"\"{key}\": ";
            int start = block.IndexOf(search);
            if (start < 0) return 0;
            start += search.Length;
            int end = block.IndexOf(",", start);
            if (end < 0) end = block.IndexOf("\n", start);
            if (end < 0) return 0;
            string val = block.Substring(start, end - start).Trim();
            double.TryParse(val, out double result);
            return result;
        }

        private static decimal GetDecimal(string block, string key)
        {
            string search = $"\"{key}\": ";
            int start = block.IndexOf(search);
            if (start < 0) return 0m;
            start += search.Length;
            int end = block.IndexOf(",", start);
            if (end < 0) end = block.IndexOf("\n", start);
            if (end < 0) return 0m;
            string val = block.Substring(start, end - start).Trim();
            decimal.TryParse(val, out decimal result);
            return result;
        }
    }
}
