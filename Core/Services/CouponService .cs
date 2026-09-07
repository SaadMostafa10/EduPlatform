using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.Common;
using Domain.Models.Coupons;
using Domain.Models.Lessons;
using Services.Abstractions;
using Services.Helpers;
using Services.Specifications.CouponSpecs;
using Shared;
using Shared.Dtos.CouponDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CouponService : ICouponService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CouponService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<CouponDto>> GetAllAsync(CouponFilterDto filter)
        {
            var spec = new CouponWithDetailsSpecification(filter);
            var countSpec = new CouponCountSpecification(filter);

            var coupons = await _unitOfWork.Repository<Coupon>().GetAllWithSpecAsync(spec);
            var totalCount = await _unitOfWork.Repository<Coupon>().GetCountWithSpecAsync(countSpec);

            var data = _mapper.Map<IEnumerable<CouponDto>>(coupons);

            return new PaginationResponse<CouponDto>(filter.PageIndex, filter.PageSize, totalCount, data);
        }
        public async Task<CouponDto> GetByIdAsync(int id)
        {
            var spec = new CouponWithDetailsSpecification(id);
            var coupon = await _unitOfWork.Repository<Coupon>().GetWithSpecAsync(spec)
                ?? throw new NotFoundException(nameof(Coupon), id);

            return _mapper.Map<CouponDto>(coupon);
        }

        public async Task<CouponStatsDto> GetStatsAsync()
        {
            var projection = await _unitOfWork.CouponRepository.GetStatsAsync();

            return new CouponStatsDto
            {
                TotalCount = projection.TotalCount,
                ActiveCount = projection.ActiveCount,
                InactiveCount = projection.InactiveCount,
                ExpiredCount = projection.ExpiredCount,
                TotalUsages = projection.TotalUsages,
                RemainingUsages = projection.RemainingUsages,
                UsagePercentage = (projection.TotalUsages + projection.RemainingUsages) == 0
                    ? 0
                    : Math.Round((double)projection.TotalUsages / (projection.TotalUsages + projection.RemainingUsages) * 100, 2)
            };
        }

        public async Task<CouponDto> CreateAsync(CreateCouponDto dto, string createdByUserId)
        {
            await ValidateGradeAndLessonAsync(dto.GradeId, dto.LessonId);

            string code;

            if (dto.AutoGenerateCode)
            {
                code = await GenerateUniqueCodeAsync(dto.CodePrefix ?? "CPN");
            }
            else
            {
                var normalizedCode = dto.ManualCode!.Trim().ToUpperInvariant();

                if (await _unitOfWork.CouponRepository.CodeExistsAsync(normalizedCode))
                    throw new BadRequestException("Coupon code already exists.");

                code = normalizedCode;
            }

            var coupon = new Coupon
            {
                Code = code,
                GradeId = dto.GradeId,
                LessonId = dto.LessonId,
                MaxUsage = dto.MaxUsage,
                ExpiryDate = dto.ExpiryDate,
                IsActive = dto.IsActive,
                CreatedByUserId = createdByUserId
            };

            await _unitOfWork.Repository<Coupon>().AddAsync(coupon);
            await _unitOfWork.SaveChangesAsync();

            var spec = new CouponWithDetailsSpecification(coupon.Id);
            var couponWithDetails = await _unitOfWork.Repository<Coupon>().GetWithSpecAsync(spec);

            return _mapper.Map<CouponDto>(couponWithDetails);
        }
        public async Task<IReadOnlyList<CouponDto>> BulkGenerateAsync(BulkGenerateCouponDto dto, string createdByUserId)
        {
            await ValidateGradeAndLessonAsync(dto.GradeId, dto.LessonId);

            var normalizedPrefix = dto.CodePrefix.Trim().ToUpperInvariant();
            var coupons = new List<Coupon>(dto.Count);

            for (int i = 0; i < dto.Count; i++)
            {
                var code = await GenerateUniqueCodeAsync(normalizedPrefix, coupons);

                coupons.Add(new Coupon
                {
                    Code = code,
                    GradeId = dto.GradeId,
                    LessonId = dto.LessonId,
                    MaxUsage = dto.MaxUsagePerCoupon,
                    ExpiryDate = dto.ExpiryDate,
                    IsActive = dto.ActivateImmediately,
                    CreatedByUserId = createdByUserId
                });
            }

            foreach (var coupon in coupons)
            {
                await _unitOfWork.Repository<Coupon>().AddAsync(coupon);
            }

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<IReadOnlyList<CouponDto>>(coupons);
        }

        public async Task<CouponDto> UpdateAsync(int id, UpdateCouponDto dto)
        {
            var coupon = await _unitOfWork.Repository<Coupon>().GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Coupon), id);

            if (dto.MaxUsage < coupon.UsedCount)
            {
                throw new BadRequestException(
                    $"MaxUsage cannot be less than the current UsedCount ({coupon.UsedCount}).");
            }

            coupon.MaxUsage = dto.MaxUsage;
            coupon.ExpiryDate = dto.ExpiryDate;
            coupon.IsActive = dto.IsActive;

            await _unitOfWork.SaveChangesAsync();

            var spec = new CouponWithDetailsSpecification(id);
            var updatedCoupon = await _unitOfWork.Repository<Coupon>().GetWithSpecAsync(spec);

            return _mapper.Map<CouponDto>(updatedCoupon);
        }

        public async Task UpdateStatusAsync(int id, bool isActive)
        {
            var coupon = await _unitOfWork.Repository<Coupon>().GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Coupon), id);

            coupon.IsActive = isActive;

            _unitOfWork.Repository<Coupon>().Update(coupon);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var coupon = await _unitOfWork.Repository<Coupon>().GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(Coupon), id);

            if (coupon.UsedCount > 0)
            {
                throw new BadRequestException("Cannot delete a coupon that has already been used. You can deactivate it instead.");
            }
            coupon.IsDeleted = true;
            coupon.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Coupon>().Update(coupon);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<string> GenerateUniqueCodeAsync(string prefix, List<Coupon>? existingInBatch = null)
        {
            string code;
            bool exists;

            do
            {
                code = $"{prefix}{CouponGenerator.GenerateCode()}";
                exists = await _unitOfWork.CouponRepository.CodeExistsAsync(code)
                         || (existingInBatch != null && existingInBatch.Any(c => c.Code == code));
            }
            while (exists);

            return code;
        }

        private async Task ValidateGradeAndLessonAsync(int gradeId, int lessonId)
        {
            var grade = await _unitOfWork.Repository<Grade>().GetByIdAsync(gradeId)
                ?? throw new BadRequestException($"Grade with Id {gradeId} does not exist.");

            var lesson = await _unitOfWork.Repository<Lesson>().GetByIdAsync(lessonId)
                ?? throw new BadRequestException($"Lesson with Id {lessonId} does not exist.");

            if (lesson.GradeId != gradeId)
                throw new BadRequestException("The selected lesson does not belong to the specified grade.");
        }

    }
}
