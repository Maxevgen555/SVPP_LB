using System.Globalization;
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

namespace LB1_Conv
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeComboBoxes();
        }
        
        // Словарь единиц конвертации и их коэффициентов пересчета в квадратные метры
        private readonly Dictionary<string, double> units = new Dictionary<string, double>
        {
            {"Квадратные метры ", 1},
            {"Гектары ", 10000},
            {"Квадратные дюймы", 0.000645},
            {"Квадратные километры", 1000000},
            {"Квадратные футы", 0.092903}
        };

        private void InitializeComboBoxes()
        {
            foreach (var unit in units.Keys)
            {
                comboBoxInput.Items.Add(unit);
                comboBoxResult.Items.Add(unit);
            }
            comboBoxInput.SelectedIndex = 0; // При запуске квадратные метры (базовая единица)
            comboBoxResult.SelectedIndex = 1; // При запуске гектары
        }

        private void ButtonNumber_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxInput.Text == "0")
                textBoxInput.Text = (sender as Button)?.Content.ToString();
            else
                if (textBoxInput.Text.Length < 18)
                textBoxInput.Text += (sender as Button)?.Content;
            UpdateConversion();
        }

        private void ButtonPoint_Click(object sender, RoutedEventArgs e)
        {
            if (!textBoxInput.Text.Contains(","))
                textBoxInput.Text += ",";
            UpdateConversion();
        }

        private void UpdateConversion()
        {
            if (comboBoxInput.SelectedItem == null || comboBoxResult.SelectedItem == null) return;

            double inputValue = Convert.ToDouble(textBoxInput.Text);
            string fromUnit = comboBoxInput.SelectedItem.ToString();
            string toUnit = comboBoxResult.SelectedItem.ToString();

            // Конвертация в м.кв.
            double baseValue = inputValue * units[fromUnit];

            // Конвертация в заданные единицы
            double resultValue = baseValue / units[toUnit];

            // Проверка результата на переполнение textBoxResult и вывод
            if (resultValue.ToString().Length < 19)
                textBoxResult.Text = resultValue.ToString();
            else textBoxResult.Text = "Переполнение!";
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            textBoxInput.Text = "0";
            UpdateConversion();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxInput.Text.Length < 2)
                textBoxInput.Text = "0";
            else
                textBoxInput.Text = textBoxInput.Text.ToString().Substring(0, textBoxInput.Text.Length - 1);
            UpdateConversion();
        }

        private void comboBoxInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateConversion();
        }

        private void comboBoxResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateConversion();
        }

    }
}