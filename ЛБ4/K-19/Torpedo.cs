using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K19GameLibrary
{
    /// <summary>
    /// Класс торпеды - наследуется от GameObject
    /// </summary>
    public class Torpedo : GameObject
    {
        /// <summary>
        /// Текущая скорость торпеды (уменьшается со временем)
        /// </summary>
        public double Speed { get; set; } = 5;

        /// <summary>
        /// Флаг запуска торпеды (true когда торпеда в движении)
        /// </summary>
        public bool IsLaunched { get; set; } = false;

        /// <summary>
        /// Реализация движения торпеды - параболическая траектория
        /// </summary>
        public override void Move()
        {
            if (IsLaunched)
            {
                X += 2;           // Постоянное движение вправо
                Y -= Speed;        // Движение вверх с текущей скоростью
                Speed *= 0.98;     // Уменьшение скорости (имитация сопротивления воды)
            }
        }
    }
}