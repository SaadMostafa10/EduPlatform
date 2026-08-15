using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.Lessons;
using Services.Abstractions;
using Services.Specifications.LessonSpecs;
using Shared;
using Shared.Dtos.LessonDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LessonService : ILessonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LessonService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<LessonResponseDto>> GetAllLessonsAsync(LessonSpecParams specParams, bool isStudent, int? studentGradeId)
        {
            if (isStudent)
            {
                if (!studentGradeId.HasValue)
                    throw new BadRequestException("Student account has no assigned grade.");
                specParams.GradeId = studentGradeId.Value;
            }

            var spec = new LessonWithGradeSpecification(specParams);
            var countSpec = new LessonCountSpecification(specParams);

            var lessons = await _unitOfWork.Repository<Lesson>().GetAllWithSpecAsync(spec);
            var totalItems = await _unitOfWork.Repository<Lesson>().GetCountWithSpecAsync(countSpec);

            var data = _mapper.Map<IReadOnlyList<LessonResponseDto>>(lessons);

            return new PaginationResponse<LessonResponseDto>(specParams.PageIndex, specParams.PageSize, totalItems, data);
        }

        public async Task<LessonResponseDto?> GetLessonByIdAsync(int id, bool isStudent, int? studentGradeId)
        {
            var lessonDto = await GetLessonDtoByIdAsync(id);
            if (lessonDto is null) return null;

            if (isStudent)
            {
                if (!studentGradeId.HasValue)
                    throw new BadRequestException("Student account has no assigned grade.");

                if (lessonDto.GradeId != studentGradeId.Value)
                    return null;
            }
            return lessonDto;
        }
        private async Task<LessonResponseDto?> GetLessonDtoByIdAsync(int id)
        {
            var spec = new LessonWithGradeSpecification(id);
            var lesson = await _unitOfWork.Repository<Lesson>().GetWithSpecAsync(spec);
            return lesson is null ? null : _mapper.Map<LessonResponseDto>(lesson);
        }

        public async Task<LessonResponseDto> CreateLessonAsync(CreateLessonRequestDto request)
        {
            if (request.IsFree && request.Price != 0)
                throw new BadRequestException("Free lessons must have a price of 0.");

            if (!request.IsFree && request.Price <= 0)
                throw new BadRequestException("Paid lessons must have a price greater than 0.");

            var lesson = _mapper.Map<Lesson>(request);

            await _unitOfWork.Repository<Lesson>().AddAsync(lesson);
            await _unitOfWork.SaveChangesAsync();

            // Re-fetch to include Grade entity for correct DTO response
            return await GetLessonDtoByIdAsync(lesson.Id) ?? _mapper.Map<LessonResponseDto>(lesson);
        }

        public async Task<LessonResponseDto?> UpdateLessonAsync(int id, UpdateLessonRequestDto request)
        {
            if (request.IsFree && request.Price != 0)
                throw new BadRequestException("Free lessons must have a price of 0.");

            if (!request.IsFree && request.Price <= 0)
                throw new BadRequestException("Paid lessons must have a price greater than 0.");

            var lesson = await _unitOfWork.Repository<Lesson>().GetByIdAsync(id);
            if (lesson is null) return null;

            _mapper.Map(request, lesson);

            _unitOfWork.Repository<Lesson>().Update(lesson);
            await _unitOfWork.SaveChangesAsync();

            return await GetLessonDtoByIdAsync(id);
        }

        public async Task<bool> DeleteLessonAsync(int id)
        {
            var lesson = await _unitOfWork.Repository<Lesson>().GetByIdAsync(id);
            if (lesson is null) return false;

            // Soft Delete Implementation
            lesson.IsDeleted = true;
            lesson.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Lesson>().Update(lesson);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}
