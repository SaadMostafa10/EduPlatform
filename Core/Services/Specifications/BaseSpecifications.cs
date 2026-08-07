using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T>
    {
        public Expression<Func<T, bool>> Criteria { get; }

        protected BaseSpecifications(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
    }
}
