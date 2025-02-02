using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        public String CompensationId { get; set; }
        public float Salary { get; set; }
        public DateOnly EffectiveDate { get; set; }

        public String EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}
