using Domain.Models.Lessons;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Services.Specifications.LessonSpecs
{
    public class LessonWithGradeSpecification : BaseSpecifications<Lesson>
    {
        public LessonWithGradeSpecification(LessonSpecParams specParams)
            : base(l =>
                (!specParams.GradeId.HasValue || l.GradeId == specParams.GradeId.Value) &&
                (!specParams.Status.HasValue || (int)l.Status == specParams.Status.Value) &&
                (string.IsNullOrEmpty(specParams.Search) || l.Title.ToLower().Contains(specParams.Search)))
        {
            AddInclude(l => l.Grade);
            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort.ToLower())
                {
                    case "priceasc":
                        AddOrderBy(l => l.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(l => l.Price);
                        break;
                    case "duration":
                        AddOrderBy(l => l.DurationInMinutes);
                        break;
                    default:
                        AddOrderByDescending(l => l.CreatedAt);
                        break;
                }
            }
            else
            {
                AddOrderByDescending(l => l.CreatedAt);
            }
        }
        public LessonWithGradeSpecification(int id)
            : base(l => l.Id == id)
        {
            AddInclude(l => l.Grade);
        }
    }
}
