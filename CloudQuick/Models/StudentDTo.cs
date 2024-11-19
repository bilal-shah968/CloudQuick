using CloudQuick.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CloudQuick.Models
{
    public class StudentDTo
    {
        [ValidateNever]
        public int Id { get; set; }

        [Required]
       
        public string StudentName { get; set; }

        [EmailAddress]
      
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }
        // [DateCheckAttributes]
        // public DateTime? AdmissionDate { get; set; } = DateTime.Now;
        public DateTime DOB { get; set; }


    }
}
