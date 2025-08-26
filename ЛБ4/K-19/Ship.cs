using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K19GameLibrary
{
    /// <summary>
    /// Класс корабля - наследуется от GameObject
    /// </summary>
    public class Ship : GameObject
    {
        /// <summary>
        /// Скорость движения корабля (пикселей за тик)
        /// </summary>
        public double Speed { get; set; } = 3;

        /// <summary>
        /// Флаг видимости корабля (false после попадания)
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Реализация движения корабля - простое перемещение вправо
        /// </summary>
        public override void Move()
        {
            X += Speed; // Увеличиваем координату X на скорость
        }
    }
}