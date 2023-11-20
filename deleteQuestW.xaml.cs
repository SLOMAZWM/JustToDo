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
    /// Interaction logic for deleteQuestW.xaml
    /// </summary>
    public partial class deleteQuestW : Window
    {

        private showList actuallyList = null;

        public deleteQuestW()
        {
            InitializeComponent();
        }

        public deleteQuestW(showList list) // konstruktor z menu kontekstowego
        {
            InitializeComponent();
            actuallyList = list;
        }

        public deleteQuestW(showList list, string n) // konstruktor prawy przycisk z showlist
        {
            InitializeComponent();
            actuallyList = list;
            nQuest.Text = n;
            btnSelect_Click(this, new RoutedEventArgs());
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

        private bool isValid() //sprawdzanie poprawności wprowadzonego pola
        {
            if(string.IsNullOrEmpty(nQuest.Text)) //pole jest puste (?)
            {
                MessageBox.Show("Wprowadź nazwę zadania!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {

            if(isValid() == true)
            {
                string name = nQuest.Text;

                string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\frees\\Documents\\JustToDo.mdf;Integrated Security=True;Connect Timeout=30";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string query = "DELETE FROM QuestList WHERE Name = @name";

                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("name", name);

                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Pomyślnie usunięto zadanie!", "Sukces!", MessageBoxButton.OK, MessageBoxImage.Information);
                                    actuallyList.InitializeQuestList(); // refresh
                                }
                                else
                                {
                                    MessageBox.Show("Niepoprawna nazwa zadania!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                        }
                        catch(SqlException sqlEx)
                        {
                            MessageBox.Show("Błąd wykonania SQL" + sqlEx.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        finally 
                        { 
                            conn.Close(); 
                        }
                    }
                }
                catch(SqlException sqlEx)
                {
                    MessageBox.Show("Błąd połączenia SQL" + sqlEx.Message, "Błąd SQL", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
        }
    }
}
