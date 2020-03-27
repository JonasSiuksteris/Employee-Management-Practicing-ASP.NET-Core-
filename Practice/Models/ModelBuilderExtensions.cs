using Microsoft.EntityFrameworkCore;

namespace Practice.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Mary",
                    Department = Dept.IT,
                    Email = "mary@email.com"
                },
                new Employee
                {
                    Id = 2,
                    Name = "John",
                    Department = Dept.HR,
                    Email = "John@email.com"
                }
            );
        }
    }
}
