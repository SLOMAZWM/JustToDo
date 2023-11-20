using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for editQuestW.xaml
    /// </summary>
    public partial class editQuestW : Window
    {
        showList actuallyList = null;
        int QuestId = 0;

        public editQuestW() //domyslny konstruktor
        {
            InitializeComponent();
        }

        private bool isValid() //Sprawdzanie poprawności pól
        {
            //czy pola są puste?

            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtDescription.Text) || Priority.SelectedItem == null || DatePicker == null)
            {
                MessageBox.Show("Nie wypełniono wszystkich pól!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        public editQuestW(int I, string N, string D, int P, DateTime dt, showList list) //konstruktor z selectQuestW lub showList
        {
            InitializeComponent();
            actuallyList = list;

            lblNameQuest.Content = '"' + N + '"' + " - zmieniane zadanie";

            QuestId = I;
            txtName.Text = N;
            txtDescription.Text = D;
            DatePicker.SelectedDate = dt;
            switch (P)
            {
                case 1:
                    {
                        Priority.SelectedIndex = 0;
                        break;
                    }
                case 2:
                    {
                        Priority.SelectedIndex = 1;
                        break;
                    }
                case 3:
                    {
                        Priority.SelectedIndex = 2;
                        break;
                    }
                default:
                    {
                        string wtf = "Jak to się kurwa wykona - to zaczne wierzyć w cuda";
                        break;
                    }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            actuallyList.InitializeQuestList();
            this.Close();
        }

        private void EditQuest_Click(object sender, RoutedEventArgs e)
        {

            bool Valid = isValid();

            if (Valid == true)
            {
                string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\frees\\Documents\\JustToDo.mdf;Integrated Security=True;Connect Timeout=30";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {

                        conn.Open();

                        string query = "UPDATE QuestList SET Name = @Name, Description = @Description, Priority = @Priority, Date = @Date WHERE Id = @QuestId";

                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                                cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                                cmd.Parameters.AddWithValue("@Priority", Priority.SelectedIndex + 1);
                                cmd.Parameters.AddWithValue("@QuestId", QuestId);
                                cmd.Parameters.AddWithValue("@Date", DatePicker.SelectedDate);

                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Zaktualizowano Questa!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Nie udało się zaktualizować Questa.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("Błąd w wykonaniu polecenia SQL!" + ex.Message, "Błąd SQL", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Błąd połączenia z bazą!" + ex.Message, "Błąd SQL", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    actuallyList.InitializeQuestList();
                }
            }
        }
    }
}
