using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LB2_CombatUnitCard
{
    /// <summary>
    /// Класс, представляющий боевого юнита в компьютерной игре
    /// </summary>
    public class CombatUnit
    {
        public CombatUnit()
        {
        }

        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public int Level { get; set; } = 1;
        public int Health { get; set; } = 100;
        public int Attack { get; set; } = 20;
        public string Faction { get; set; } = "";
        public bool IsElite { get; set; } = false;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string ImagePath { get; set; } = "";
        public List<string> Abilities { get; set; } = new List<string>();

        public override string ToString()
        {
            string abilitiesStr = Abilities.Count > 0 ? string.Join(", ", Abilities) : "Нет способностей";
            return $"Имя: {Name}\nТип: {Type}\nУровень: {Level}\n" +
                   $"Здоровье: {Health}\nАтака: {Attack}\nФракция: {Faction}\n" +
                   $"Элитный: {(IsElite ? "Да" : "Нет")}\n" +
                   $"Дата создания: {CreationDate:dd.MM.yyyy}" +
                   $"Способности: {abilitiesStr}"; ;
        }
    }
}
