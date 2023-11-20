using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace JustToDo
{
    /// <summary>
    /// Interaction logic for showList.xaml
    /// </summary>
    public partial class showList : Window, INotifyPropertyChanged
    {

        // Implementing the INotifyPropertyChanged interface for data binding
        public event PropertyChangedEventHandler PropertyChanged;

        // A collection of Quest objects that will be displayed in the UI
        private ObservableCollection<Quest> _quests;
        public ObservableCollection<Quest> Quests
        {
            get { return _quests; }
            set
            {
                _quests = value;
                OnPropertyChanged(nameof(Quests));
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public showList()
        {
            InitializeComponent();
            InitializeQuestList();
            DataContext = this;
        }

        public void InitializeQuestList()
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\frees\\Documents\\JustToDo.mdf;Integrated Security=True;Connect Timeout=30";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT Id, Name, Description, Priority, Date FROM QuestList";

                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn))

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            Quests = new ObservableCollection<Quest>();

                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["Id"]);
                                string name = reader["Name"].ToString();
                                string description = reader["Description"].ToString();
                                int priority = Convert.ToInt32(reader["Priority"]);
                                DateTime date = Convert.ToDateTime(reader["Date"]);

                                Quest quest = new Quest
                                {
                                    Id = id,
                                    Name = name,
                                    Description = description,
                                    Priority = priority,
                                    Date = date
                                };

                                Quests.Add(quest);
                            }
                        }


                    }
                    catch (SqlException sqlError)
                    {
                        MessageBox.Show("Błąd bazy danych: " + sqlError.Message, "BŁĄD SQL", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show("Błąd z połączeniem bazy danych: " + ex.Message, "BŁĄD SQL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        
        public void InitializeQuestList(DateTime selectedDate)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\frees\\Documents\\JustToDo.mdf;Integrated Security=True;Connect Timeout=30";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT Id, Name, Description, Priority, Date FROM QuestList WHERE Date = @selectedDate";

                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("selectedDate", selectedDate);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                Quests = new ObservableCollection<Quest>();

                                while (reader.Read())
                                {
                                    int id = Convert.ToInt32(reader["Id"]);
                                    string name = reader["Name"].ToString();
                                    string description = reader["Description"].ToString();
                                    int priority = Convert.ToInt32(reader["Priority"]);
                                    DateTime date = Convert.ToDateTime(reader["Date"]);

                                    Quest quest = new Quest
                                    {
                                        Id = id,
                                        Name = name,
                                        Description = description,
                                        Priority = priority,
                                        Date = date
                                    };

                                    Quests.Add(quest);
                                }
                            }
                        }
                    }
                    catch (SqlException sqlError)
                    {
                        MessageBox.Show("Błąd bazy danych: " + sqlError.Message, "BŁĄD SQL", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Błąd z połączeniem bazy danych: " + ex.Message, "BŁĄD SQL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddQuest_Click(object sender, EventArgs e)
        {
            addQuestW newQuestW = new addQuestW(this);
            newQuestW.ShowDialog();
        }

        private void btnEditQuest_Click(object sender, EventArgs e)
        {
            selectQuestW selectQ = new selectQuestW(this);
            selectQ.ShowDialog();
        }

        private void btnDeleteQuest_Click(object sender, EventArgs e)
        {
            deleteQuestW deleteQuest = new deleteQuestW(this);
            deleteQuest.ShowDialog();
        }

        //double left
        private void QuestLists_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Quest selectedQuest = (Quest)QuestLists.SelectedItem;

            int id = selectedQuest.Id;
            string name = selectedQuest.Name;
            string description = selectedQuest.Description;
            int priority = selectedQuest.Priority;
            DateTime date = selectedQuest.Date;

            if (selectedQuest != null) 
            {
                editQuestW editQuest = new editQuestW(id, name, description, priority, date, this);
                editQuest.ShowDialog();
                InitializeQuestList();
            }
        }

        //from right click
        private void RightClick_EditQuest(object sender, RoutedEventArgs e)
        {
            Quest selectedQuest = (Quest)QuestLists.SelectedItem;

            int id = selectedQuest.Id;
            string name = selectedQuest.Name;
            string description = selectedQuest.Description;
            int priority = selectedQuest.Priority;
            DateTime date = selectedQuest.Date;

            if (selectedQuest != null)
            {
                editQuestW editQuest = new editQuestW(id, name, description, priority, date, this);
                editQuest.ShowDialog();
                InitializeQuestList();
            }
        }

        //from right click
        private void RightClick_AddQuest(object sender, RoutedEventArgs e)
        {
            addQuestW addQuest = new addQuestW(this);
            addQuest.ShowDialog();
            InitializeQuestList();
        }

        //from right click
        private void RightClick_DeleteQuest(object sender, RoutedEventArgs e)
        {
            Quest selectedQuest = (Quest)QuestLists.SelectedItem;

            string name = selectedQuest.Name;

            MessageBoxResult YesOrNo = MessageBox.Show("Czy na pewno chcesz usunąć zadanie: " + '"' + name + '"', "Usunąć zadanie?", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if(YesOrNo == MessageBoxResult.Yes )
            {
                deleteQuestW deleteQuest = new deleteQuestW(this, name);
                deleteQuest.Close();
            }
            else
            {
                return;
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = DatePicker.SelectedDate ?? DateTime.MinValue;
            if (selectedDate != DateTime.MinValue) 
            {
                InitializeQuestList(selectedDate);
            }
            else
            {
                InitializeQuestList();
            }
        }

        private void ClearDateMenuItem_Click(object sender, RoutedEventArgs e)
        {
            DatePicker.SelectedDate = null; 
        }
    }
}
