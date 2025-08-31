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

namespace LB6
{
    public partial class ParametersWindow : Window
    {
        public double A { get; private set; }
        public double B { get; private set; }
        public int N { get; private set; }

        public ParametersWindow(double currentA, double currentB, int currentN)
        {
            InitializeComponent();
            TextBoxA.Text = currentA.ToString();
            TextBoxB.Text = currentB.ToString();
            TextBoxN.Text = currentN.ToString();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidateInput()
        {
            try
            {
                A = double.Parse(TextBoxA.Text);
                B = double.Parse(TextBoxB.Text);
                N = int.Parse(TextBoxN.Text);

                if (A >= B)
                {
                    MessageBox.Show("Верхний предел должен быть больше нижнего предела.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                if (N <= 0)
                {
                    MessageBox.Show("Число разбиений должно быть положительным.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                return true;
            }
            catch (FormatException)
            {
                MessageBox.Show("Пожалуйста, введите корректные числовые значения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (OverflowException)
            {
                MessageBox.Show("Введенные значения слишком большие.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}