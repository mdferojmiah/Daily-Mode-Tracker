using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using MoodTracker.Models.Enums;

namespace MoodTracker.Models.Entities;

public class User: IdentityUser<Guid>
{
    [Required(ErrorMessage = "Name is required")]
    [Display(Name = "Full Name")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 20 characters")]
    public string? FullName { get; set; }
    
    [Required(ErrorMessage = "Trusted Persons Email is required")]
    [Display(Name = "Trusted Persons Email")]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    [StringLength(25, MinimumLength = 3, ErrorMessage = "Email must be between 3 and 25 characters")]
    public string? TrustedPersonsEmail { get; set; }
    
    [Required(ErrorMessage = "Trusted Persons Phone Number is required")]
    [Display(Name = "Trusted Persons Phone Number")]
    [Phone]
    [DataType(DataType.PhoneNumber)]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone must be 11 characters")]
    
    public string? TrustedPersonsNumber { get; set; }
    [Display(Name = "Trusted Persons Name")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 20 characters")]
    public string? TrustedPersonsName { get; set; }
    
    public GenderOptions?  Gender { get; set; }
    public DateTime? Birthday { get; set; }
}