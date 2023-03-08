using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class Emp
    {
        public int EmpId { get; set; }
        public string Fname { get; set; } = null!;
        public string Lname { get; set; } = null!;
        public string JobTitle { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int? ManagedBy { get; set; }
    }
}
