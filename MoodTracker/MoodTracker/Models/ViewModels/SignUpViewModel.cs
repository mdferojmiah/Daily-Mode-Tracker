using System.ComponentModel.DataAnnotations;

namespace MoodTracker.Models.ViewModels;

public class SignUpViewModel
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
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    [StringLength(25, MinimumLength = 3, ErrorMessage = "Email must be between 3 and 25 characters")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }
}