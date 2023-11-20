using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace JustToDo
{
    /// <summary>
    /// Interaction logic for selectQuestW.xaml
    /// </summary>
    public partial class selectQuestW : Window
    {
        public selectQuestW()
        {
            InitializeComponent();
        }

        showList actuallyList = null;

        public selectQuestW(showList list)
        {
            InitializeComponent();
            actuallyList = list;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Tutaj wpisz nazwe zadania!")
            {
                textBox.Text = "";
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Tutaj wpisz nazwe zadania!";
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            string nameQuest = nQuest.Text;

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\frees\\Documents\\JustToDo.mdf;Integrated Security=True;Connect Timeout=30";

            using (SqlConnection conn = new SqlConnection(connectionString)) 
            {
                conn.Open();

                string query = "SELECT ID, Name, Description, Priority, Date FROM QuestList WHERE Name = @Name;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", nameQuest);

                    using (SqlDataReader reader = cmd.ExecuteReader()) 
                    {
                        if(reader.Read())
                        {
                            int id = Convert.ToInt32(reader["ID"]);
                            string name = reader["Name"].ToString();
                            string description = reader["Description"].ToString();
                            int priority = Convert.ToInt32(reader["Priority"]);
                            DateTime date = Convert.ToDateTime(reader["Date"]);

                            editQuestW editQuest = new editQuestW(id, name, description, priority, date, actuallyList);
                            this.Hide();
                            editQuest.ShowDialog();
                            this.Close();
                        }
                    }
                }
            }
        }
    }
}
