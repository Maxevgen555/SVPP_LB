using LB8.Models;
using System.ComponentModel;
using System.Data.Entity;
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

namespace LB8
{
    public partial class MainWindow : Window
    {
        EntityContext context;

        public MainWindow()
        {
            InitializeComponent();
            context = new EntityContext();
            LoadData();
        }

        private void LoadData()
        {
            context.Patients.Load();
            dGrid.ItemsSource = context.Patients.Local;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var patient = new Patient();
            EditWindow ew = new EditWindow(patient);

            if (ew.ShowDialog() == true)
            {
                context.Patients.Add(patient);
                context.SaveChanges();
                LoadData();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dGrid.SelectedItem is Patient patient)
            {
                EditWindow ew = new EditWindow(patient);

                if (ew.ShowDialog() == true)
                {
                    context.SaveChanges();
                    LoadData();
                }
                else
                {
                    // Откатываем изменения
                    context.Entry(patient).Reload();
                    LoadData();
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dGrid.SelectedItem is Patient patient)
            {
                string message = $"Вы уверены, что хотите удалить пациента:\n{patient.LastName} {patient.FirstName}?";

                MessageBoxResult result = MessageBox.Show(
                    message,
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.No);

                if (result == MessageBoxResult.Yes)
                {
                    context.Patients.Remove(patient);
                    context.SaveChanges();
                    LoadData();

                    MessageBox.Show("Пациент удален!", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите пациента для удаления!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void dGrid_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        protected override void OnClosed(EventArgs e)
        {
            context?.Dispose(); // Освобождаем контекст при закрытии окна
            base.OnClosed(e);
        }
    }
}