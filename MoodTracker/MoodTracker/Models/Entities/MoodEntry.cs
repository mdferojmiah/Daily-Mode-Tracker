using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;
using MoodTracker.Models.Enums;

namespace MoodTracker.Models.Entities;

public class MoodEntry
{
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Please enter a mood tag.")]
    public MoodTagOptions Tag { get; set; }
    [Required(ErrorMessage = "Please enter a mood description.")]
    [StringLength(200, MinimumLength = 10, ErrorMessage = "Mood Description must be between 10 and 200 characters long.")]
    public string Description { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    //navigation propertry
    [Required]
    public Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
}