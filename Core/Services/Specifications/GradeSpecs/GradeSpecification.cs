using Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.GradeSpecs
{
    public class GradeSpecification : BaseSpecifications<Grade>
    {
        public GradeSpecification() : base(g => true)
        {
            AddOrderBy(g => g.Order);
        }
    }
}
