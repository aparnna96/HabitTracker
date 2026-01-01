using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTracker.Core.Entities
{
    public class HabitLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HabitId { get; set; }

        [Required]
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "date")]
        public DateOnly CompletionDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }

        // Navigation property - log belongs to one habit
        [ForeignKey(nameof(HabitId))]
        public Habit? Habit { get; set; }
    }
}
