using Domain.Models.Lessons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.LessonSpecs
{
    public class LessonsByGradeSpecification : BaseSpecifications<Lesson>
    {
        public LessonsByGradeSpecification(int gradeId)
            : base(l => l.GradeId == gradeId && l.Status == LessonStatus.Published)
        {
        }
    }
}
