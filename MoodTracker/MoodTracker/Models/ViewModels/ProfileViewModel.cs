using MoodTracker.Models.Enums;

namespace MoodTracker.Models.ViewModels;

public class ProfileViewModel
{
    public string? UserName { get; set; } = string.Empty;
    public string? FullName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? TrustedPersonsName { get; set; } = string.Empty;
    public string? TrustedPersonsEmail { get; set; } = string.Empty;
    public string? TrustedPersonsNumber { get; set; } = string.Empty;
    public GenderOptions?  Gender { get; set; }
    public DateTime? Birthday { get; set; }
}