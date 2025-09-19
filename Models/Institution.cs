using System.Collections.Generic;

namespace SchoolApi.Models
{
    public class Institution
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Phone { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}


