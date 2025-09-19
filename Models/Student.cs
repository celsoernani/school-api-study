using System;

namespace SchoolApi.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public int PeopleInHousehold { get; set; }
        public string? Notes { get; set; }

        public int InstitutionId { get; set; }
        public Institution? Institution { get; set; }
    }
}


