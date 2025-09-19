using Microsoft.EntityFrameworkCore;
using SchoolApi.Models;

namespace SchoolApi.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(SchoolDbContext dbContext)
        {
            await dbContext.Database.MigrateAsync();

            if (!await dbContext.Institutions.AnyAsync())
            {
                var school = new Institution
                {
                    Name = "Escola Municipal Central",
                    Address = "Rua das Flores, 123",
                    City = "São Paulo",
                    State = "SP",
                    PostalCode = "01000-000",
                    Phone = "+55 11 1234-5678"
                };

                var students = new List<Student>
                {
                    new Student
                    {
                        FirstName = "Ana",
                        LastName = "Silva",
                        DateOfBirth = new DateTime(2010, 5, 12),
                        PeopleInHousehold = 4,
                        Notes = "Alergia a amendoim"
                    },
                    new Student
                    {
                        FirstName = "Bruno",
                        LastName = "Souza",
                        DateOfBirth = new DateTime(2009, 11, 2),
                        PeopleInHousehold = 3,
                        Notes = null
                    }
                };

                school.Students = students;
                dbContext.Institutions.Add(school);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}


