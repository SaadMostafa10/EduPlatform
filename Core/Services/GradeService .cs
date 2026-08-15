using AutoMapper;
using Domain.Contracts;
using Domain.Models.Identity;
using Services.Abstractions;
using Services.Specifications.GradeSpecs;
using Shared.Dtos.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class GradeService : IGradeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GradeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<GradeDto>> GetAllWithSpecAsync()
        {
            var spec = new GradeSpecification();
            var grades = await _unitOfWork.Repository<Grade>().GetAllWithSpecAsync(spec);
            return _mapper.Map<IReadOnlyList<GradeDto>>(grades);
        }
    }
}
