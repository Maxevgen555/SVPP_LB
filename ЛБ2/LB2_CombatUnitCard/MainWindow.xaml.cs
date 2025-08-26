using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LB2_CombatUnitCard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CombatUnit currentUnit = new CombatUnit();

        public MainWindow()
        {
            InitializeComponent();

            // Установка текущей даты
            dpCreationDate.SelectedDate = DateTime.Now;

            // Обработчик изменения уровня
            sliderLevel.ValueChanged += (s, e) =>
            {
                txtLevel.Text = $"Уровень: {sliderLevel.Value}";
            };
        }

        // Обработчик клика по изображению
        private void UnitImage_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (dialog.ShowDialog() == false) return;
            currentUnit.ImagePath = dialog.FileName;
            unitImage.Source = new BitmapImage(new Uri(currentUnit.ImagePath, UriKind.RelativeOrAbsolute));
        }

        // Сохранить данные из формы в переменную
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            currentUnit.Name = txtName.Text;
            currentUnit.Type = cmbType.Text;
            currentUnit.Level = (int)sliderLevel.Value;
            currentUnit.Health = int.Parse(txtHealth.Text);
            currentUnit.Attack = int.Parse(txtAttack.Text);
            currentUnit.IsElite = chkElite.IsChecked ?? false;
            currentUnit.CreationDate = dpCreationDate.SelectedDate ?? DateTime.Now;

            if (radFaction1.IsChecked == true) currentUnit.Faction = "Альянс";
            else if (radFaction2.IsChecked == true) currentUnit.Faction = "Орда";
            else if (radFaction3.IsChecked == true) currentUnit.Faction = "Нейтралы";
            currentUnit.Abilities.Clear();
            foreach (ListBoxItem item in lstAbilities.SelectedItems)
            {
                currentUnit.Abilities.Add(item.Content.ToString());
            }
            MessageBox.Show("Данные сохранены!\n\n" + currentUnit.ToString());
        }

        // Загрузить данные из переменной в форму
        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {

            // Создаем случайные данные 
            Random random = new Random();
            string[] names = { "Элитный воин", "Магический стрелок", "Тяжелый пехотинец", "Летучий разведчик" };
            string[] types = { "Пехота", "Кавалерия", "Артиллерия", "Маги" };
            string[] factions = { "Альянс", "Орда", "Нейтралы" };
            string[] allAbilities = { "Ближний бой", "Дальний бой", "Защита", "Лечение", "Магия", "Скорость" };

            currentUnit.Name = names[random.Next(names.Length)];
            currentUnit.Type = types[random.Next(types.Length)];
            currentUnit.Level = random.Next(1, 101);
            currentUnit.Health = random.Next(50, 501);
            currentUnit.Attack = random.Next(20, 201);
            currentUnit.Faction = factions[random.Next(factions.Length)];
            currentUnit.IsElite = random.Next(2) == 0;
            currentUnit.CreationDate = DateTime.Now.AddDays(-random.Next(1000));
            currentUnit.ImagePath = $"Images/{random.Next(1, 8)}.jpg";
            currentUnit.Abilities.Clear();
            for (int i = 0; i < allAbilities.Length; i++)
            {
                if (random.Next(2) == 0) 
                {
                    currentUnit.Abilities.Add(allAbilities[i]);
                }
            }
            // Заполняем элементы управления данными из объекта
            txtName.Text = currentUnit.Name;
            cmbType.Text = currentUnit.Type;
            sliderLevel.Value = currentUnit.Level;
            txtHealth.Text = currentUnit.Health.ToString();
            txtAttack.Text = currentUnit.Attack.ToString();
            chkElite.IsChecked = currentUnit.IsElite;
            dpCreationDate.SelectedDate = currentUnit.CreationDate;

            // Устанавливаем фракцию
            radFaction1.IsChecked = currentUnit.Faction == "Альянс";
            radFaction2.IsChecked = currentUnit.Faction == "Орда";
            radFaction3.IsChecked = currentUnit.Faction == "Нейтралы";
            // Устанавливаем способности в ListBox
            lstAbilities.SelectedItems.Clear();
            foreach (ListBoxItem item in lstAbilities.Items)
            {
                if (currentUnit.Abilities.Contains(item.Content.ToString()))
                {
                    item.IsSelected = true;
                }
            }
            unitImage.Source = new BitmapImage(new Uri(currentUnit.ImagePath, UriKind.RelativeOrAbsolute));

            MessageBox.Show("Данные загружены\n\n" + currentUnit.ToString());
        }

        // Очистить форму
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            // Очистка полей
            txtName.Text = "";
            cmbType.SelectedIndex = -1;
            sliderLevel.Value = 1;
            txtHealth.Text = " ";
            txtAttack.Text = " ";
            chkElite.IsChecked = false;
            dpCreationDate.SelectedDate = null;

            // Сброс радиокнопок
            radFaction1.IsChecked = false;
            radFaction2.IsChecked = false;
            radFaction3.IsChecked = false;
            // Сброс ListBox
            lstAbilities.SelectedItems.Clear();
            // Сброс изображения
            unitImage.Source = new BitmapImage(new Uri("Images/foto0.jpg", UriKind.Relative));

            // Очистка переменной
            currentUnit = new CombatUnit();

            MessageBox.Show("Форма и переменная очищены!");
        }
    }
}