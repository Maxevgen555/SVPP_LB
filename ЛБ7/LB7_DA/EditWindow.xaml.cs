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

namespace LB7_DA
{
    public partial class EditWindow : Window
    {
        public EditWindow(Patient patient, bool isEditMode = false)
        {
            InitializeComponent();
            grid.DataContext = patient;
            if (isEditMode)
            {
                Title = "Редактировать пациента";
            }
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxLastName.Text) || string.IsNullOrEmpty(textBoxFirstName.Text))
            {
                MessageBox.Show("Фамилия и имя обязательны для заполнения!");
                return;
            }
            this.DialogResult = true;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}