using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LB7
{
    public partial class MainWindow : Window
    {
        Patient patient;
        ObservableCollection<Patient> collection = new ObservableCollection<Patient>();

        public MainWindow()
        {
            InitializeComponent();
            patient = new Patient();
            stackpanelPatient.DataContext = patient;
            listBox.DataContext = collection;
            FillData();
        }

        private void FillData()
        {
            collection.Clear();
            foreach (var p in Patient.getAllPatients())
            {
                collection.Add(p);
            }
        }

        private void buttonView_Click(object sender, RoutedEventArgs e)
        {
            FillData();
        }

        private void buttonInsert_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(patient.LastName) || string.IsNullOrEmpty(patient.FirstName))
            {
                MessageBox.Show("Фамилия и имя обязательны для заполнения!");
                return;
            }

            patient.Insert();
            FillData();

            patient = new Patient();
            stackpanelPatient.DataContext = patient;
        }

        private void buttonFind_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = textBoxSearch.Text;
            string searchBy = "LastName"; // По умолчанию

            // Определяем критерий поиска из ComboBox
            if (comboSearchBy.SelectedItem is System.Windows.Controls.ComboBoxItem selectedItem)
            {
                switch (selectedItem.Content.ToString())
                {
                    case "Фамилия": searchBy = "LastName"; break;
                    case "Имя": searchBy = "FirstName"; break;
                    case "Диагноз": searchBy = "Diagnosis"; break;
                    case "Врач": searchBy = "Doctor"; break;
                    case "ID": searchBy = "Id"; break;
                }
            }

            var foundPatients = Patient.Find(searchTerm, searchBy);

            if (foundPatients.Any())
            {
                collection.Clear();
                foreach (var foundPatient in foundPatients)
                {
                    collection.Add(foundPatient);
                }
                MessageBox.Show($"Найдено {foundPatients.Count} пациентов");
            }
            else
            {
                MessageBox.Show("Пациенты не найдены!");
            }
        }

        private void buttonChange_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex == -1)
            {
                MessageBox.Show("Нет выбранной записи!");
                return;
            }

            var selectedPatient = (Patient)listBox.SelectedItem;

            selectedPatient.LastName = string.IsNullOrEmpty(textBoxLastName.Text) ? selectedPatient.LastName : textBoxLastName.Text;
            selectedPatient.FirstName = string.IsNullOrEmpty(textBoxFirstName.Text) ? selectedPatient.FirstName : textBoxFirstName.Text;
            selectedPatient.Diagnosis = textBoxDiagnosis.Text;
            selectedPatient.DoctorInCharge = textBoxDoctor.Text;

            if (DateTime.TryParse(textBoxAdmissionDate.Text, out DateTime date))
                selectedPatient.AdmissionDate = date;
            else if (string.IsNullOrEmpty(textBoxAdmissionDate.Text))
                selectedPatient.AdmissionDate = null;

            selectedPatient.Update();
            FillData();

            MessageBox.Show("Данные обновлены!");
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex == -1)
            {
                MessageBox.Show("Нет выбранной записи!");
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить этого пациента?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var id = ((Patient)listBox.SelectedItem).Id;
                Patient.Delete(id);
                FillData();
                MessageBox.Show("Пациент удален!");
            }
        }

        private void listBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (listBox.SelectedItem is Patient selectedPatient)
            {
                patient = selectedPatient;
                stackpanelPatient.DataContext = patient;
            }
        }
    }
}