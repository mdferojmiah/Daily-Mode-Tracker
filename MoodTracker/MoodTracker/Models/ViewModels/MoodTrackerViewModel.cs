using MoodTracker.Models.Entities;
using MoodTracker.Models.Enums;

namespace MoodTracker.Models.ViewModels;

public class MoodTrackerViewModel
{
    public int? EntriesThisWeek { get; set; }
    public int? ConsistencyIn7Days { get; set; }
    public MoodTagOptions? DominantMood { get; set; } 
    public List<MoodEntry> AllMoodEntries { get; set; } = new List<MoodEntry>();
}