using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LB7_DA
{
    public class Patient
    {
        static string connectionString;
        static SqlConnection connection;
        static SqlDataAdapter adapter;
        static DataTable patientTable = new DataTable();

        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Diagnosis { get; set; }
        public string AdmissionDate { get; set; }
        public string DoctorInCharge { get; set; }

        public static void NewConnection()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public static DataTable ViewAll()
        {
            NewConnection();
            string sql = "SELECT * FROM Patients";
            adapter = new SqlDataAdapter(sql, connection);
            patientTable.Clear();
            adapter.Fill(patientTable);
            connection.Close();
            return patientTable;
        }

        public static void Update()
        {
            NewConnection();
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
            adapter.Update(patientTable);
            connection.Close();
        }

        public string Find()
        {
            NewConnection();
            DataTable searchTable = new DataTable();
            string result = "";

            if (!string.IsNullOrEmpty(LastName))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Patients WHERE LastName LIKE @lastName", connection);
                command.Parameters.AddWithValue("@lastName", "%" + LastName + "%");
                adapter = new SqlDataAdapter(command);
                adapter.Fill(searchTable);
            }
            else if (!string.IsNullOrEmpty(FirstName))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Patients WHERE FirstName LIKE @firstName", connection);
                command.Parameters.AddWithValue("@firstName", "%" + FirstName + "%");
                adapter = new SqlDataAdapter(command);
                adapter.Fill(searchTable);
            }
            else if (!string.IsNullOrEmpty(Diagnosis))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Patients WHERE Diagnosis LIKE @diagnosis", connection);
                command.Parameters.AddWithValue("@diagnosis", "%" + Diagnosis + "%");
                adapter = new SqlDataAdapter(command);
                adapter.Fill(searchTable);
            }
            else if (!string.IsNullOrEmpty(DoctorInCharge))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Patients WHERE DoctorInCharge LIKE @doctor", connection);
                command.Parameters.AddWithValue("@doctor", "%" + DoctorInCharge + "%");
                adapter = new SqlDataAdapter(command);
                adapter.Fill(searchTable);
            }
            else if (!string.IsNullOrEmpty(AdmissionDate))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Patients WHERE AdmissionDate = @date", connection);
                command.Parameters.AddWithValue("@date", AdmissionDate);
                adapter = new SqlDataAdapter(command);
                adapter.Fill(searchTable);
            }

            foreach (DataRow row in searchTable.Rows)
            {
                var cells = row.ItemArray;
                foreach (object cell in cells)
                    result += $"\t{cell}";
                result += "\n";
            }

            connection.Close();
            return string.IsNullOrEmpty(result) ? "Пациенты не найдены" : result;
        }

        public override string ToString()
        {
            return $"{ID}: {LastName} {FirstName} - {Diagnosis ?? "Диагноз не указан"}";
        }
    }
}