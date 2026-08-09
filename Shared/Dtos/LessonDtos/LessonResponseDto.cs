using Domain.Models.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.LessonDtos
{
    public class LessonResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string YouTubeUrl { get; set; } = string.Empty;
        public int DurationInMinutes { get; set; }
        public decimal Price { get; set; }
        public bool IsFree { get; set; }
        public LessonStatus Status { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
