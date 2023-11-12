using System.ComponentModel.DataAnnotations;

namespace ContactApp.Models;

public class Contact
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid OwnerId { get; set; }
    
    [Required]
    public string ContactName { get; set; }
    
    [Phone]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }

    public DateTime CreatedAt { get; set; }
}