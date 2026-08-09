using Shared;
using Shared.Dtos.LessonDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface ILessonService
    {
        Task<PaginationResponse<LessonResponseDto>> GetAllLessonsAsync(LessonSpecParams specParams);
        Task<LessonResponseDto?> GetLessonByIdAsync(int id);
        Task<LessonResponseDto> CreateLessonAsync(CreateLessonRequestDto request);
        Task<LessonResponseDto?> UpdateLessonAsync(int id, UpdateLessonRequestDto request);
        Task<bool> DeleteLessonAsync(int id);
    }
}
