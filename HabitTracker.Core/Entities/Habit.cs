using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HabitTracker.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Core.Entities
{
    public class Habit
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Habit name is required")]
        [StringLength(100, ErrorMessage = "Habit name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500,ErrorMessage ="Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required]
        public HabitFrequency Frequency { get; set; } = HabitFrequency.Daily;
        [Required]
        public string UserId { get; set; } = string.Empty;

        public int? CategoryId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastCompletedAt { get; set; }

        public int CurrentStreak { get; set; } = 0;

        public int LongestStreak { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        // Navigation property - habit belongs to one user
        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }

        // Navigation property - habit belongs to one category (optional)
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        // Navigation property - one habit has many logs
        public ICollection<HabitLog> HabitLogs { get; set; } = new List<HabitLog>();
    }
}
   