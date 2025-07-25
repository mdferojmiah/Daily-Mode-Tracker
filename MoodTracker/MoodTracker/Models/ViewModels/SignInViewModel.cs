using System.ComponentModel.DataAnnotations;

namespace MoodTracker.Models.ViewModels;

public class SignInViewModel
{
    [Required(ErrorMessage = "Please enter your email address.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Please enter your password.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}