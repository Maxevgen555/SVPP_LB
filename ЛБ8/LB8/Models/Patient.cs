using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LB8.Models
{
    public class Patient
    {
      //  [Key]
        public int PatientId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Diagnosis { get; set; }
        public string AdmissionDate { get; set; }
        public string DoctorInCharge { get; set; }
    }
}