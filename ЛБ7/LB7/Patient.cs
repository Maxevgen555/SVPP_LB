using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace LB7
{
    internal class Patient
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Diagnosis { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public string DoctorInCharge { get; set; }

        private static SqlConnection GetConnection()
        {
            try
            {
                var connectionStringSettings = ConfigurationManager.ConnectionStrings["DefaultConnection"];
                if (connectionStringSettings == null)
                {
                    throw new Exception("Строка подключения 'DefaultConnection' не найдена в файле конфигурации.");
                }

                return new SqlConnection(connectionStringSettings.ConnectionString);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при создании подключения: " + ex.Message);
            }
        }

        // Универсальный метод поиска по любому параметру
        public static List<Patient> Find(string searchTerm = null, string searchBy = "LastName")
        {
            var foundPatients = new List<Patient>();

            using (var connection = GetConnection())
            {
                connection.Open();

                string commandString;
                SqlCommand findCommand;

                if (string.IsNullOrEmpty(searchTerm))
                {
                    // Если поисковый термин пустой, возвращаем всех пациентов
                    commandString = "SELECT * FROM Patients";
                    findCommand = new SqlCommand(commandString, connection);
                }
                else
                {
                    // Поиск по конкретному полю
                    commandString = $"SELECT * FROM Patients WHERE {GetSearchField(searchBy)} LIKE @searchTerm";
                    findCommand = new SqlCommand(commandString, connection);
                    findCommand.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                }

                var reader = findCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var lastName = reader.GetString(1);
                        var firstName = reader.GetString(2);

                        string diagnosis = null;
                        if (!reader.IsDBNull(3))
                            diagnosis = reader.GetString(3);

                        DateTime? admissionDate = null;
                        if (!reader.IsDBNull(4))
                            admissionDate = reader.GetDateTime(4);

                        string doctorInCharge = null;
                        if (!reader.IsDBNull(5))
                            doctorInCharge = reader.GetString(5);

                        foundPatients.Add(new Patient()
                        {
                            Id = id,
                            LastName = lastName,
                            FirstName = firstName,
                            Diagnosis = diagnosis,
                            AdmissionDate = admissionDate,
                            DoctorInCharge = doctorInCharge
                        });
                    }
                }
            }

            return foundPatients;
        }

        // Вспомогательный метод для определения поля поиска
        private static string GetSearchField(string searchBy)
        {
            switch (searchBy.ToLower())
            {
                case "lastname": return "LastName";
                case "firstname": return "FirstName";
                case "diagnosis": return "Diagnosis";
                case "doctor": return "DoctorInCharge";
                case "id": return "Id";
                default: return "LastName";
            }
        }

        public static IEnumerable<Patient> getAllPatients()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var SQLstr = "SELECT * FROM Patients";
                SqlCommand getAllCommand = new SqlCommand(SQLstr, connection);
                var reader = getAllCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var lastName = reader.GetString(1);
                        var firstName = reader.GetString(2);

                        string diagnosis = null;
                        if (!reader.IsDBNull(3))
                            diagnosis = reader.GetString(3);

                        DateTime? admissionDate = null;
                        if (!reader.IsDBNull(4))
                            admissionDate = reader.GetDateTime(4);

                        string doctorInCharge = null;
                        if (!reader.IsDBNull(5))
                            doctorInCharge = reader.GetString(5);

                        yield return new Patient()
                        {
                            Id = id,
                            LastName = lastName,
                            FirstName = firstName,
                            Diagnosis = diagnosis,
                            AdmissionDate = admissionDate,
                            DoctorInCharge = doctorInCharge
                        };
                    }
                }
            }
        }

        public void Insert()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var SQLstr = @"INSERT INTO Patients (LastName, FirstName, Diagnosis, AdmissionDate, DoctorInCharge) 
                             VALUES (@lastName, @firstName, @diagnosis, @admissionDate, @doctorInCharge)";

                SqlCommand insertCommand = new SqlCommand(SQLstr, connection);
                insertCommand.Parameters.AddWithValue("@lastName", LastName);
                insertCommand.Parameters.AddWithValue("@firstName", FirstName);
                insertCommand.Parameters.AddWithValue("@diagnosis", (object)Diagnosis ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@admissionDate", (object)AdmissionDate ?? DBNull.Value);
                insertCommand.Parameters.AddWithValue("@doctorInCharge", (object)DoctorInCharge ?? DBNull.Value);

                insertCommand.ExecuteNonQuery();
            }
        }

        public static void Delete(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var commandString = "DELETE FROM Patients WHERE Id=@id";
                SqlCommand deleteCommand = new SqlCommand(commandString, connection);
                deleteCommand.Parameters.AddWithValue("@id", id);
                deleteCommand.ExecuteNonQuery();
            }
        }

        public void Update()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var commandString = @"UPDATE Patients SET LastName=@lastName, FirstName=@firstName, 
                                    Diagnosis=@diagnosis, AdmissionDate=@admissionDate, DoctorInCharge=@doctorInCharge 
                                    WHERE Id=@id";

                SqlCommand updateCommand = new SqlCommand(commandString, connection);
                updateCommand.Parameters.AddWithValue("@lastName", LastName);
                updateCommand.Parameters.AddWithValue("@firstName", FirstName);
                updateCommand.Parameters.AddWithValue("@diagnosis", (object)Diagnosis ?? DBNull.Value);
                updateCommand.Parameters.AddWithValue("@admissionDate", (object)AdmissionDate ?? DBNull.Value);
                updateCommand.Parameters.AddWithValue("@doctorInCharge", (object)DoctorInCharge ?? DBNull.Value);
                updateCommand.Parameters.AddWithValue("@id", Id);

                updateCommand.ExecuteNonQuery();
            }
        }

        public override string ToString()
        {
            string dateStr = AdmissionDate.HasValue ? AdmissionDate.Value.ToShortDateString() : "не указана";
            return $"№ {Id}: {LastName} {FirstName}, Диагноз: {Diagnosis ?? "не указан"}, Дата: {dateStr}, Врач: {DoctorInCharge ?? "не назначен"}";
        }
    }
}