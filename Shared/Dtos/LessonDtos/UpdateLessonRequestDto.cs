using Domain.Models.Lessons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.LessonDtos
{
    public class UpdateLessonRequestDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(150, ErrorMessage = "Title cannot exceed 150 characters.")]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "YouTube URL is required.")]
        [RegularExpression(@"^(https?:\/\/)?(www\.)?(youtube\.com\/(watch\?v=|embed\/|v\/|shorts\/)|youtu\.be\/)[\w\-]{11}((\?|&|\/)\S*)?$",
        ErrorMessage = "Invalid YouTube URL format.")]
        public string YouTubeUrl { get; set; } = string.Empty;

        [Range(1, 1000, ErrorMessage = "Duration must be at least 1 minute.")]
        public int DurationInMinutes { get; set; }

        [Range(0, 100000, ErrorMessage = "Price must be a non-negative number.")]
        public decimal Price { get; set; }

        public bool IsFree { get; set; }

        public LessonStatus Status { get; set; }

        [Required(ErrorMessage = "Grade is required.")]
        public int GradeId { get; set; }
        [Range(1, 1000, ErrorMessage = "Max views must be between 1 and 1000.")]
        public int? MaxViews { get; set; }
    }
}
