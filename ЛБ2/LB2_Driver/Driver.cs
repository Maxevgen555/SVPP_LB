using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LB2_Driver
{
    public enum GENDER { male, female, other}
    public enum COLOREYES { brown, green, gray, blue}
    public class Driver
    {
        int? number;
        char? class1;
        string? name;
        string? adress;
        double? hgt;
        DateTime? dob;
        DateTime? iss;
        DateTime? exp;
        GENDER? gender;
        COLOREYES? coloreyes;
        bool? donor;
        string? uriImage;

        public Driver()
        {
        }

        public int? Number { get => number; set => number = value; }
        public char? Class1 { get => class1; set => class1 = value; }
        public string? Name { get => name; set => name = value; }
        public string? Adress { get => adress; set => adress = value; }
        public double? Hgt { get => hgt; set => hgt = value; }
        public DateTime? Dob { get => dob; set => dob = value; }
        public DateTime? Iss { get => iss; set => iss = value; }
        public DateTime? Exp { get => exp; set => exp = value; }
        public GENDER? Gender { get => gender; set => gender = value; }
        public COLOREYES? Coloreyes { get => coloreyes; set => coloreyes = value; }
        public bool? Donor { get => donor; set => donor = value; }
        public string? UriImage { get => uriImage; set => uriImage = value; }

        public override string? ToString()
        {
            return $"№{Number?.ToString()} {Class1?.ToString()}  {Name?.ToString()} " +
                $"{Adress?.ToString()} {Hgt?.ToString()}  {Dob?.ToString("dd.MM.yyyy")} " +
           $" от {Iss?.ToString("dd.MM.yyyy")} до {Exp?.ToString("dd.MM.yyyy")} пол {Gender} " +
           $"глаза {coloreyes?.ToString()} {(Donor == true ? "Донор" : "Не донор")} \n {UriImage?.ToString()}";
        }
    }
}
