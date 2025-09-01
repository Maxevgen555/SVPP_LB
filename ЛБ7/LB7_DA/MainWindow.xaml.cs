using System.Data;
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

namespace LB7_DA
{
    public partial class MainWindow : Window
    {
        DataTable patientTable = new DataTable();

        public MainWindow()
        {
            InitializeComponent();
            Fill();
        }

        private void Fill()
        {
            patientTable = Patient.ViewAll();
            patientGrid.ItemsSource = patientTable.DefaultView;
        }

        private void ButtonView_Click(object sender, RoutedEventArgs e)
        {
            Fill();
        }

        private void ButtonFind_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = new Patient();
            SearchWindow searchWindow = new SearchWindow(patient);
            if (searchWindow.ShowDialog() == true)
            {
                MessageBox.Show(patient.Find(), "Результаты поиска");
            }
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            Patient.Update();
            Fill();
            MessageBox.Show("Данные обновлены!");
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = new Patient();
            EditWindow editWindow = new EditWindow(patient, false);
            if (editWindow.ShowDialog() == true)
            {
                // Добавление через DataTable
                DataRow newRow = patientTable.NewRow();
                newRow["LastName"] = patient.LastName;
                newRow["FirstName"] = patient.FirstName;
                newRow["Diagnosis"] = patient.Diagnosis;
                newRow["AdmissionDate"] = patient.AdmissionDate;
                newRow["DoctorInCharge"] = patient.DoctorInCharge;
                patientTable.Rows.Add(newRow);

                Patient.Update();
                Fill();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (patientGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)patientGrid.SelectedItem;
                int id = (int)selectedRow["Id"];

                var result = MessageBox.Show($"Удалить пациента {selectedRow["LastName"]} {selectedRow["FirstName"]}?",
                    "Подтверждение удаления", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    selectedRow.Delete();
                    Patient.Update();
                    Fill();
                }
            }
            else
            {
                MessageBox.Show("Выберите пациента для удаления!");
            }
        }
    }
}