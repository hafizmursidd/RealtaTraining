using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextBack
{
    public class GetSalesListDbParameterDTO
    {
        public string CompanyId { get; set; }
        public string DepartmentId { get; set; }
        public int SalesCount { get; set; }

    }
}