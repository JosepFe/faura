using System.ComponentModel.DataAnnotations.Schema;

namespace Faura.WebAPI.Domain.Entities;

[Table("employee")]
public class Employee
{
    public Employee(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    [Column("id")]
    public long Id { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; }

    [Column("last_name")]
    public string LastName { get; set; }

    [Column("email")]
    public string Email { get; set; }
}
