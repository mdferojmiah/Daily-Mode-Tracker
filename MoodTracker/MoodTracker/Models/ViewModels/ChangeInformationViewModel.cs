using System.ComponentModel.DataAnnotations;
using MoodTracker.Models.Enums;

namespace MoodTracker.Models.ViewModels;

public class ChangeInformationViewModel
{
    [Display(Name = "Full Name")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 20 characters")]
    public string? FullName { get; set; }
    
    [Display(Name = "Trusted Persons Email")]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    [StringLength(25, MinimumLength = 3, ErrorMessage = "Email must be between 3 and 25 characters")]
    public string? TrustedPersonsEmail { get; set; }
    
    [Display(Name = "Trusted Persons Phone Number")]
    [Phone]
    [DataType(DataType.PhoneNumber)]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone must be 11 characters")]
    public string? TrustedPersonsNumber { get; set; }
    
    public GenderOptions?  Gender { get; set; }
    
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? Birthday { get; set; }
    
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }
    
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
    public string? ConfirmNewPassword { get; set; }
}