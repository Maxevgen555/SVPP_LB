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

namespace LB1_Calc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double x;
        string oper; // string, потому что есть кнопки с более чем одним символом
        bool flagEnter = false; // флаг запрещает удаление последнего символа, добавление новых

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonNumber_Click(object sender, RoutedEventArgs e)
        {
            if(textBox.Text == "0" && !flagEnter)
                textBox.Text = (sender as Button)?.Content.ToString();
            else
                if(textBox.Text.Length < 18 && !flagEnter)
                    textBox.Text += (sender as Button)?.Content;
        }

        private void ButtonPoint_Click(object sender, RoutedEventArgs e)
        {
            if(!textBox.Text.Contains(","))
                textBox.Text+=",";
        }

        private void ButtonOper_Click(object sender, RoutedEventArgs e)
        {
            x = Convert.ToDouble(textBox.Text);
            oper = (sender as Button).Content.ToString();
            flagEnter = false;
            textBox.Text = "0";
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            double y = Convert.ToDouble(textBox.Text);
            double result = 0;
            flagEnter = true;

            switch (oper)
            {
                case "+": result = x + y; break;
                case "-": result = x - y; break;
                case "*": result = x * y; break;
                case "/": result = x / y; break;
                case "-/+": result = 0 - x; break;
                case "1/x": result = 1 / x; break;
                case "x*x": result = x * x; break;
                case "%": result = x / 100; break;
            }

            // если запись числа имеет более 18 символов, обрежем менее значимые порядки у мантиссы для приведения к 18 символам
            if (result.ToString().Length > 18)
            {
                string resultString = result.ToString();
                int i = resultString.Length - 18;
                string[] resultMultiString = resultString.Split("E"); 
                resultMultiString[0] = resultMultiString[0].Remove(resultMultiString[0].Length - i, i);
                textBox.Text = string.Join("E", resultMultiString); 
            }
            else 
                textBox.Text = result.ToString();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if(!flagEnter)
            {
                if (textBox.Text.Length < 2)
                    textBox.Text = "0";
                else
                    textBox.Text = textBox.Text.ToString().Substring(0, textBox.Text.Length - 1);
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            textBox.Text = "0";
            x = 0;
            oper = "0";
            flagEnter = false;
        }
    }
}