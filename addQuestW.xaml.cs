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
    /// Interaction logic for addQuestW.xaml
    /// </summary>
    public partial class addQuestW : Window
    {

        showList actuallyList = null;

        public addQuestW()
        {
            InitializeComponent();
        }

        public addQuestW(showList list)
        {
            InitializeComponent();
            actuallyList = list;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            actuallyList.InitializeQuestList();
            this.Close();
        }

        private bool isValid() //Sprawdzanie poprawności pól
        {
            //czy pola są puste?

            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtDescription.Text) || Priority.SelectedItem == null || DatePicker.SelectedDate == null)
            {
                MessageBox.Show("Nie wypełniono wszystkich pól!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void AddQuest_Click(object sender, RoutedEventArgs e)
        {

            if(isValid() == true) 
            {
                string name = txtName.Text;
                string description = txtDescription.Text;
                int priority = Priority.SelectedIndex + 1;
                DateTime? date = DatePicker.SelectedDate;

                string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\frees\\Documents\\JustToDo.mdf;Integrated Security=True;Connect Timeout=30";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string query = "INSERT QuestList (Name, Description, Priority, Date) VALUES (@Name, @Description, @Priority, @Date)";

                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@Name", name);
                                cmd.Parameters.AddWithValue("@Description", description);
                                cmd.Parameters.AddWithValue("@Priority", priority);
                                cmd.Parameters.AddWithValue("@Date", date);

                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Poprawnie dodano zadanie!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Nie udało się dodać nowego zadania!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            MessageBox.Show("Błąd SQL " + sqlEx.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                }
                catch (SqlException sqlEx)
                {
                    MessageBox.Show("Błąd SQL " + sqlEx.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    actuallyList.InitializeQuestList();
                }
            }
            else
            {
                return;
            }

        }
    }
}
