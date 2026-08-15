using Shared.Dtos.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IGradeService
    {
        Task<IReadOnlyList<GradeDto>> GetAllWithSpecAsync();
    }
}
