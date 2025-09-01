using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace LB8.Models
{
    public class DataBaseInitializer : DropCreateDatabaseIfModelChanges<EntityContext>
    {
        protected override void Seed(EntityContext context)
        {
            // Небольшая задержка для избежания конфликтов
            Thread.Sleep(100);
            context.Patients.AddRange(new Patient[]
            {
                new Patient { LastName = "Иванов", FirstName = "Алексей", Diagnosis = "Гипертоническая болезнь", AdmissionDate = "2024-01-15", DoctorInCharge = "Петрова О.В." },
                new Patient { LastName = "Смирнова", FirstName = "Мария", Diagnosis = "Сахарный диабет 2 типа", AdmissionDate = "2024-02-20", DoctorInCharge = "Козлов С.И." },
                new Patient { LastName = "Петров", FirstName = "Дмитрий", Diagnosis = "Острый бронхит", AdmissionDate = "2024-03-10", DoctorInCharge = "Сидорова Е.А." }
            });
        }
    }
}