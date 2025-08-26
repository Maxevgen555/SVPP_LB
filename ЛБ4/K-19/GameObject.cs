using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace K19GameLibrary
{
    /// <summary>
    /// Абстрактный базовый класс для всех игровых объектов
    /// Реализует INotifyPropertyChanged для поддержки привязки данных
    /// </summary>
    public abstract class GameObject : INotifyPropertyChanged
    {
        // Приватные поля для координат
        private double x;
        private double y;

        // Событие для уведомления об изменении свойств
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Координата X объекта
        /// При изменении вызывает уведомление для привязки данных
        /// </summary>
        public double X
        {
            get => x;
            set
            {
                x = value;
                OnPropertyChanged(nameof(X)); // Уведомляем об изменении
            }
        }

        /// <summary>
        /// Координата Y объекта
        /// При изменении вызывает уведомление для привязки данных
        /// </summary>
        public double Y
        {
            get => y;
            set
            {
                y = value;
                OnPropertyChanged(nameof(Y)); // Уведомляем об изменении
            }
        }

        /// <summary>
        /// Абстрактный метод движения - должен быть реализован в потомках
        /// </summary>
        public abstract void Move();

        /// <summary>
        /// Метод для вызова события PropertyChanged
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}