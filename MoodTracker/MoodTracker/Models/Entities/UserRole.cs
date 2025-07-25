using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MoodTracker.Models.Entities;

public class UserRole: IdentityRole<Guid>
{
    [StringLength(50)]
    public string? Description { get; set; }
}