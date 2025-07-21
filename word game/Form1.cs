using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace word_game
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            tableLayoutPanel1.Visible = false;
            tableLayoutPanel2.Visible = false;
        }
        int num, Attempts, Position = 0, Match = 0;
        string[] orgWord;
        string[] word;


        void disable(int number)
        {
            num = number;
            btn4.Visible = false;
            btn5.Visible = false;
            btn6.Visible = false;
            btn7.Visible = false;
            btn8.Visible = false;
            textBox1.MaxLength = num;
            textBox1.Visible = true;
            button1.Visible = true;
            Attempts = 0;
            tableLayoutPanel1.Visible = true;
            tableLayoutPanel1.ColumnCount = num;
            tableLayoutPanel2.Visible = true;
            tableLayoutPanel2.ColumnCount = num;
            for (int i = 0; i < num; i++)
            {
                float percent = 100 / num;
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, percent));
                tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, percent));
                int temp = i + 1;
                string temp1 = "Collumn" + temp.ToString();
                Label lblName = new Label() { Text = temp.ToString(), Anchor = AnchorStyles.Left, Name = temp1 };
                tableLayoutPanel1.Controls.Add(lblName, i, 0);
                Label lblName2 = new Label() { Text = temp.ToString(), Anchor = AnchorStyles.Left, Name = temp.ToString() };
                tableLayoutPanel2.Controls.Add(lblName2, i, 0);
            }


        }


        private void btn4_Click(object sender, EventArgs e)
        {
            disable(4);

        }

        private void btn5_Click(object sender, EventArgs e)
        {
            disable(5);
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            disable(6);
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            disable(7);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string input = textBox1.Text.ToLower();
            orgWord = input.Select(c => c.ToString()).ToArray();
            button1.Visible = false;
            button2.Visible = true;
            label2.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = true;
            textBox2.MaxLength = num;
            for (int i = 0; i < num; i++)
            {
                try
                {
                    int temp = i + 1;
                    string labName = "Collumn" + temp.ToString();
                    Label lbl = (Label)tableLayoutPanel1.Controls[labName];
                    if (lbl != null)
                    {
                        lbl.Text = orgWord[i];
                        lbl.Visible = false;
                    }

                }
                catch
                {

                }
            }


        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn8_Click_1(object sender, EventArgs e)
        {
            disable(8);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Attempts += 1;
            Position = 0;
            Match = 0;
            string input = textBox2.Text.ToLower();
            word = input.Select(c => c.ToString()).ToArray();

            for (int i = 0; i < num; i++)
            {
                if (word[i] == orgWord[i])
                {
                    Position += 1;
                }
                for (int j = 0; j < num; j++)
                {
                    if (orgWord[j] == word[i])
                    {
                        Match += 1;
                        break;
                    }
                }
            }

            if (Position == num && Match == num)
            {
                MessageBox.Show("You Won! The word is: " + input);
                // Save the result to the database upon winning
                SaveGameResult(string.Join("", orgWord), Attempts);
            }

            // ... (rest of your existing code)
            if (Attempts == 3 || Attempts == 5 || Attempts == 7)
            {
                // ...
            }
            label4.Text = Attempts.ToString();
            label6.Text = Position.ToString();
            label8.Text = Match.ToString();
        }

        private void SaveGameResult(string word, int attempts)
        {
            // --- Database Connection and Data Insertion ---
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\Marble\\source\\repos\\WordGame\\word game\\Database1.mdf\";Integrated Security=True";
            // Assumes you have a table named GameResults with columns Word and Attempts
            string query = "INSERT INTO GameResults (Word, Attempts) VALUES (@Word, @Attempts)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Use parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@Word", word);
                        command.Parameters.AddWithValue("@Attempts", attempts);

                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Game result saved successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving game result: " + ex.Message);
            }
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            disable(8);
        }
    }
}
