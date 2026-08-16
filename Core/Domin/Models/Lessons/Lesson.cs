using Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Lessons
{
    public class Lesson : AuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string YouTubeUrl { get; set; } = string.Empty;
        public int DurationInMinutes { get; set; }
        public decimal Price { get; set; }
        public bool IsFree { get; set; }
        public LessonStatus Status { get; set; } = LessonStatus.Published;

        // FK
        public int GradeId { get; set; }
        public Grade Grade { get; set; } = null!;

        public int? MaxViews { get; set; }
    }
}
