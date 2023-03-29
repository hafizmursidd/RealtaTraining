using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R_Common;
using ContextCommon;

namespace ContextBack
{
    public class ContextCls
    {
        public List<SalesStreamDTO> GetSalesListDb(GetSalesListDbParameterDTO poParameter)
        {
            R_Exception loException = new R_Exception();
            List<SalesStreamDTO> loRtn = null;

            try
            {
                loRtn = new List<SalesStreamDTO>();
                for (int lnCount = 0; lnCount < poParameter.SalesCount; lnCount++)
                {
                    loRtn.Add(new SalesStreamDTO()
                    {
                        CompanyId = poParameter.CompanyId,
                        DepartmentId = poParameter.DepartmentId,
                        SalesId = $"S-ID- {lnCount}",
                        SalesName = $"Sales {lnCount}"
                    });
                }
            }
            catch (Exception ex)
            {

                loException.Add(ex);
            }
            EndBlock:
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        public List<OrderStreamDTO> GetOrderListDb(GetOrderListDbParameterDTO poParameter) {
            R_Exception loException = new R_Exception();
            List<OrderStreamDTO> loRtn = null;

            try
            {
                loRtn = new List<OrderStreamDTO>();
                for (int lnCount = 0; lnCount < poParameter.OrderCount; lnCount++)
                {
                    loRtn.Add(new OrderStreamDTO()
                    {
                        CompanyId = poParameter.CompanyId,
                        DepartmentId = poParameter.DepartmentId,
                        SalesId = poParameter.SalesId,
                        OrderId = $"Order-{lnCount}",
                        OrderDate = DateTime.Now.ToString("yyyyMMdd")
                    });
                }
            }
            catch (Exception ex)
            {

                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
    }
}
