using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace K19GameLibrary
{
    // Пользовательский элемент управления для игры "Стрельба из подводной лодки"
    // Реализует вид из перископа со взрывом при попадании
    public partial class ShipTorpedoGame : UserControl, INotifyPropertyChanged
    {
        // Таймер для обновления игрового состояния
        private DispatcherTimer gameTimer;

        // Генератор случайных чисел для позиционирования корабля
        private Random random = new Random();

        // Флаг состояния игры (запущена/на паузе)
        private bool isGameRunning = false;

        // Координаты подводной лодки (центр перископа)
        private double submarineX = 200;
        private double submarineY = 300;

        // Скорость торпеды (уменьшается со временем)
        private double torpedoSpeed = 5;

        // Флаг, указывающий запущена ли торпеда
        private bool torpedoLaunched = false;

        // Координаты торпеды
        private double torpedoX, torpedoY;

        // Координаты корабля
        private double shipX = -100, shipY = 50;

        // Скорость движения корабля
        private double shipSpeed = 3;

        // Флаг видимости корабля (исчезает при попадании)
        private bool shipVisible = true;

        // Координаты взрыва (в реальных координатах)
        private double explosionX, explosionY;

        // Флаг видимости взрыва
        private bool explosionVisible = false;

        // Константы для отображения в перископе
        private const double PERISCOPE_SCALE = 0.3;          // Масштаб объектов в перископе
        private const double PERISCOPE_CENTER_X = 100;       // Центр перископа по X
        private const double PERISCOPE_CENTER_Y = 100;       // Центр перископа по Y

        // Событие для уведомления об изменении свойств (для привязки данных)
        public event PropertyChangedEventHandler PropertyChanged;

        // Событие для отправки сообщений в главное окно
        public event Action<string> GameMessage;

        // Видимость корабля (свойство для привязки данных)
        public bool ShipVisible
        {
            get => shipVisible;
            set
            {
                shipVisible = value;
                // Уведомляем об изменении всех связанных свойств
                OnPropertyChanged(nameof(ShipVisible));
                OnPropertyChanged(nameof(ShipPeriscopeX));
                OnPropertyChanged(nameof(ShipPeriscopeY));
            }
        }

        // Состояние торпеды (свойство для привязки данных)
        public bool TorpedoLaunched
        {
            get => torpedoLaunched;
            set
            {
                torpedoLaunched = value;
                // Уведомляем об изменении всех связанных свойств
                OnPropertyChanged(nameof(TorpedoLaunched));
                OnPropertyChanged(nameof(TorpedoPeriscopeX));
                OnPropertyChanged(nameof(TorpedoPeriscopeY));
            }
        }

        // Видимость взрыва (свойство для привязки данных)
        public bool ExplosionVisible
        {
            get => explosionVisible;
            set
            {
                explosionVisible = value;
                OnPropertyChanged(nameof(ExplosionVisible));
            }
        }

        // Конструктор элемента управления
        public ShipTorpedoGame()
        {
            InitializeComponent();
            InitializeGame();
            DataContext = this; // Устанавливаем контекст данных для привязки
        }

        // Инициализация игровых компонентов
        private void InitializeGame()
        {
            // Создаем и настраиваем игровой таймер
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(50); // 20 FPS
            gameTimer.Tick += GameTimer_Tick; // Подписываемся на событие тика

            // Инициализация начальных позиций
            torpedoX = submarineX;
            torpedoY = submarineY;
        }

        // Обработчик тика таймера - основной игровой цикл
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (!isGameRunning) return; // Если игра на паузе - выходим

            MoveShip();      // Двигаем корабль
            MoveTorpedo();   // Двигаем торпеду
            CheckCollision(); // Проверяем столкновения

            // Обновляем отображение в перископе
            OnPropertyChanged(nameof(ShipPeriscopeX));
            OnPropertyChanged(nameof(ShipPeriscopeY));
            OnPropertyChanged(nameof(TorpedoPeriscopeX));
            OnPropertyChanged(nameof(TorpedoPeriscopeY));
            OnPropertyChanged(nameof(ExplosionPeriscopeX));
            OnPropertyChanged(nameof(ExplosionPeriscopeY));
        }

        // Движение корабля по игровому полю
        private void MoveShip()
        {
            if (!ShipVisible) return; // Если корабль уничтожен - не двигаем

            shipX += shipSpeed; // Увеличиваем координату X на скорость

            // Если корабль ушел за правую границу - возвращаем его в начало
            if (shipX > ActualWidth + 100)
            {
                shipX = -100;
                shipY = random.Next(50, 150); // Случайная высота
                ShipVisible = true; // Появляется новый корабль

                // Уведомляем об изменении позиции корабля
                OnPropertyChanged(nameof(ShipPeriscopeX));
                OnPropertyChanged(nameof(ShipPeriscopeY));
            }

            // Уведомляем об изменении позиции корабля
            OnPropertyChanged(nameof(ShipPeriscopeX));
            OnPropertyChanged(nameof(ShipPeriscopeY));
        }

        // Движение торпеды по прямой траектории (без смещения вправо)
        private void MoveTorpedo()
        {
            if (!TorpedoLaunched) return; // Если торпеда не запущена - выходим

            // Торпеда движется прямо вперед (без смещения вправо)
            torpedoY -= torpedoSpeed; // Движение вверх с текущей скоростью
            torpedoSpeed *= 0.98; // Уменьшение скорости (сопротивление воды)

            // Если торпеда вышла за границы экрана - сбрасываем
            if (torpedoY < 0 || torpedoY > ActualHeight)
            {
                ResetTorpedo();
                GameMessage?.Invoke("Торпеда не достигла цели!");
            }

            // Уведомляем об изменении позиции торпеды
            OnPropertyChanged(nameof(TorpedoPeriscopeX));
            OnPropertyChanged(nameof(TorpedoPeriscopeY));
        }

        // Проверка столкновения торпеды с кораблем
        private void CheckCollision()
        {
            if (!TorpedoLaunched || !ShipVisible) return; // Проверяем только если оба объекта активны

            // Вычисляем расстояние между торпедой и кораблем по теореме Пифагора
            double distance = Math.Sqrt(Math.Pow(torpedoX - shipX, 2) + Math.Pow(torpedoY - shipY, 2));

            // Если расстояние меньше 30 пикселей - считаем что попадание
            if (distance < 30)
            {
                Explode(); // Создаем взрыв
                ShipVisible = false; // Скрываем корабль
                ResetTorpedo(); // Сбрасываем торпеду
                GameMessage?.Invoke("Попадание! Корабль уничтожен!"); // Отправляем сообщение
            }
        }

        // Создание анимации взрыва в месте попадания
        private void Explode()
        {
            // Устанавливаем координаты взрыва в месте корабля
            explosionX = shipX;
            explosionY = shipY;
            ExplosionVisible = true;

            // Создаем анимацию взрыва - появляется и исчезает
            DoubleAnimation explosionAnimation = new DoubleAnimation
            {
                From = 1.0,    // Начальная прозрачность - полностью видим
                To = 0.0,      // Конечная прозрачность - полностью прозрачен
                Duration = TimeSpan.FromSeconds(1.5), // Длительность анимации
                AutoReverse = false // Не возвращаться обратно
            };

            // Запускаем анимацию прозрачности
            explosion.BeginAnimation(Ellipse.OpacityProperty, explosionAnimation);

            // Создаем анимацию увеличения размера взрыва
            DoubleAnimation sizeAnimation = new DoubleAnimation
            {
                From = 20,     // Начальный размер
                To = 40,       // Конечный размер (увеличивается)
                Duration = TimeSpan.FromSeconds(0.5) // Быстрое увеличение
            };

            // Запускаем анимацию размера
            explosion.BeginAnimation(Ellipse.WidthProperty, sizeAnimation);
            explosion.BeginAnimation(Ellipse.HeightProperty, sizeAnimation);

            // Через 2 секунды скрываем взрыв полностью
            DispatcherTimer explosionTimer = new DispatcherTimer();
            explosionTimer.Interval = TimeSpan.FromSeconds(2);
            explosionTimer.Tick += (s, e) =>
            {
                ExplosionVisible = false;
                // Восстанавливаем исходный размер взрыва
                explosion.Width = 20;
                explosion.Height = 20;
                explosionTimer.Stop();
            };
            explosionTimer.Start();
        }

        // Обработчик клика по перископу для определения попадания по объектам
        private void PeriscopeView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Получаем координаты клика относительно перископа
            Point clickPosition = e.GetPosition(periscopeView);

            // Проверяем попадание по торпеде (в радиусе 10 пикселей)
            double torpedoDistance = Math.Sqrt(
                Math.Pow(clickPosition.X - TorpedoPeriscopeX, 2) +
                Math.Pow(clickPosition.Y - TorpedoPeriscopeY, 2));

            if (TorpedoLaunched && torpedoDistance < 10)
            {
                GameMessage?.Invoke(TorpedoInfo); // Показываем информацию о торпеде
                return;
            }

            // Проверяем попадание по кораблю (в радиусе 15 пикселей)
            double shipDistance = Math.Sqrt(
                Math.Pow(clickPosition.X - ShipPeriscopeX, 2) +
                Math.Pow(clickPosition.Y - ShipPeriscopeY, 2));

            if (ShipVisible && shipDistance < 15)
            {
                GameMessage?.Invoke(ShipInfo); // Показываем информацию о корабле
                return;
            }

            // Если клик не попал ни в один объект
            GameMessage?.Invoke("Цельтесь в корабль или торпеду для информации");
        }

        // Запуск игры
        public void StartGame()
        {
            isGameRunning = true;
            gameTimer.Start(); // Запускаем игровой таймер
            GameMessage?.Invoke("Игра запущена! Цельтесь и стреляйте!");
        }

        // Пауза игры
        public void PauseGame()
        {
            isGameRunning = false;
            gameTimer.Stop(); // Останавливаем игровой таймер
            GameMessage?.Invoke("Игра на паузе");
        }

        // Сброс игры в начальное состояние
        public void ResetGame()
        {
            // Останавливаем игру на время сброса
            bool wasRunning = isGameRunning;
            isGameRunning = false;
            gameTimer.Stop();

            // Сбрасываем торпеду
            ResetTorpedo();

            // Восстанавливаем состояние корабля
            ShipVisible = true;
            shipX = -100;
            shipY = 50;
            shipSpeed = 3;

            // Восстанавливаем состояние торпеды
            torpedoSpeed = 5;

            // Скрываем взрыв
            ExplosionVisible = false;

            // Сбрасываем анимацию взрыва
            explosion.BeginAnimation(Ellipse.OpacityProperty, null);
            explosion.Opacity = 0;
            explosion.Width = 20;
            explosion.Height = 20;

            // Уведомляем об изменении ВСЕХ свойств
            OnPropertyChanged(nameof(ShipVisible));
            OnPropertyChanged(nameof(ShipPeriscopeX));
            OnPropertyChanged(nameof(ShipPeriscopeY));
            OnPropertyChanged(nameof(TorpedoPeriscopeX));
            OnPropertyChanged(nameof(TorpedoPeriscopeY));
            OnPropertyChanged(nameof(ExplosionPeriscopeX));
            OnPropertyChanged(nameof(ExplosionPeriscopeY));
            OnPropertyChanged(nameof(ExplosionVisible));

            // Возвращаем состояние игры
            if (wasRunning)
            {
                isGameRunning = true;
                gameTimer.Start();
            }

            GameMessage?.Invoke("Игра сброшена. Готов к запуску!");
        }

        // Сброс состояния торпеды
        private void ResetTorpedo()
        {
            TorpedoLaunched = false;
            torpedoX = submarineX; // Возвращаем к подлодке
            torpedoY = submarineY;
            torpedoSpeed = 5; // Восстанавливаем начальную скорость

            // Уведомляем об изменении позиции торпеды в перископе
            OnPropertyChanged(nameof(TorpedoPeriscopeX));
            OnPropertyChanged(nameof(TorpedoPeriscopeY));
        }

        // Движение перископа влево
        public void MovePeriscopeLeft()
        {
            // Двигаем влево, но не выходим за левую границу
            if (submarineX > 50) submarineX -= 10;

            // Уведомляем об изменении позиции перископа
            OnPropertyChanged(nameof(PeriscopePosition));
        }

        // Движение перископа вправо
        public void MovePeriscopeRight()
        {
            // Двигаем вправо, но не выходим за правую границу
            if (submarineX < ActualWidth - 50) submarineX += 10;

            // Уведомляем об изменении позиции перископа
            OnPropertyChanged(nameof(PeriscopePosition));
        }

        // Запуск торпеды
        public void LaunchTorpedo()
        {
            // Запускаем торпеду только если она еще не запущена и игра идет
            if (!TorpedoLaunched && isGameRunning)
            {
                TorpedoLaunched = true;
                torpedoX = submarineX; // Стартовая позиция = позиция перископа
                torpedoY = submarineY;

                // Уведомляем об изменении позиции торпеды в перископе
                OnPropertyChanged(nameof(TorpedoPeriscopeX));
                OnPropertyChanged(nameof(TorpedoPeriscopeY));

                GameMessage?.Invoke("Торпеда выпущена!");
            }
        }

        // Свойства для привязки данных к XAML

        /// Позиция перископа для привязки данных
        public Point PeriscopePosition => new Point(submarineX, submarineY);

        // Позиция корабля в перископе (относительно центра)
        public double ShipPeriscopeX => PERISCOPE_CENTER_X + (shipX - submarineX) * PERISCOPE_SCALE;

        // Позиция корабля в перископе (относительно центра)
        public double ShipPeriscopeY => PERISCOPE_CENTER_Y + (shipY - submarineY) * PERISCOPE_SCALE;

        // Позиция торпеды в перископе (относительно центра)
        public double TorpedoPeriscopeX => PERISCOPE_CENTER_X + (torpedoX - submarineX) * PERISCOPE_SCALE;

        // Позиция торпеды в перископе (относительно центра)
        public double TorpedoPeriscopeY => PERISCOPE_CENTER_Y + (torpedoY - submarineY) * PERISCOPE_SCALE;

        // Позиция взрыва в перископе (относительно центра)
        public double ExplosionPeriscopeX => PERISCOPE_CENTER_X + (explosionX - submarineX) * PERISCOPE_SCALE;

        // Позиция взрыва в перископе (относительно центра)
        public double ExplosionPeriscopeY => PERISCOPE_CENTER_Y + (explosionY - submarineY) * PERISCOPE_SCALE;

        // Информация о корабле для отображения (координаты)
        public string ShipInfo => $"Корабль: X={shipX:F1}, Y={shipY:F1}, Скорость={shipSpeed:F1}";

        // Информация о торпеде для отображения (скорость)
        public string TorpedoInfo => $"Торпеда: Скорость={torpedoSpeed:F1}";

        // Метод для уведомления об изменении свойств (реализация INotifyPropertyChanged)
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}