using core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Specification
{
    public class BrandListSpecification : BaseSpecification<Product, string>
    {
        public BrandListSpecification()
        {
            AddSelect(x => x.Brand);
            ApplyDistinct();
        }
    }
}
