using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApi.Models.Entities
{
    public class DateOfBirthValidationAttribute : ValidationAttribute
    {
        public int MinimumAge { get; }

        public DateOfBirthValidationAttribute(int minimumAge)
        {
            MinimumAge = minimumAge;
        }

        public override bool IsValid(object value)
        {
            if (value is DateTime dateOfBirth)
            {
                DateTime today = DateTime.Today;
                int age = today.Year - dateOfBirth.Year;

                // Adjust age if the birthday hasn't occurred yet this year
                if (dateOfBirth > today.AddYears(-age))
                {
                    age--;
                }

                return dateOfBirth.Date <= today.Date && age >= MinimumAge;
            }
            return false;
        }
    }
}