using System.ComponentModel.DataAnnotations;
using MoodTracker.Models.Entities;
using MoodTracker.Models.Enums;

namespace MoodTracker.Models.ViewModels;

public class MoodEntryViewModel
{
    [Required(ErrorMessage = "Please enter a mood tag.")]
    public MoodTagOptions Tag { get; set; }
    [Required(ErrorMessage = "Please enter a mood description.")]
    [StringLength(200, MinimumLength = 10, ErrorMessage = "Mood Description must be between 10 and 200 characters long.")]
    public string Description { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.Now;
}