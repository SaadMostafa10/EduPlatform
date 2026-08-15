using Domain.Models.Lessons;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.LessonSpecs
{
    public class LessonCountSpecification : BaseSpecifications<Lesson>
    {
        public LessonCountSpecification(LessonSpecParams specParams)
            : base(l =>
                (!specParams.GradeId.HasValue || l.GradeId == specParams.GradeId.Value) &&
                (!specParams.Status.HasValue || (int)l.Status == specParams.Status.Value) &&
                (string.IsNullOrEmpty(specParams.Search) || l.Title.ToLower().Contains(specParams.Search)))
        {
        }
    }
}
