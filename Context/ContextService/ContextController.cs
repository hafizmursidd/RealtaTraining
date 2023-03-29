using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R_Common;
using R_CommonFrontBackAPI;
using Microsoft.AspNetCore.Mvc;
using ContextCommon;
using ContextBack;
using R_BackEnd;
using System.ComponentModel;

namespace ContextService
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ContextController : ControllerBase, IContextProgram
    {
        [HttpPost]
        public IAsyncEnumerable<SalesStreamDTO> GetSalesList()
        {
            R_Exception loException = new R_Exception();
            ProgramContextDTO loProgramContextDTO;
            GetSalesListContextDTO loContextParameter;
            GetSalesListDbParameterDTO loBackparameter = null;
            try
            {
                //dapatkan Context
                loBackparameter = new GetSalesListDbParameterDTO();
                //public context
                loProgramContextDTO = R_Utility.R_GetContext<ProgramContextDTO>(ContextConstant.PROGRAM_CONTEXT);
                //Stream Context
                loContextParameter = R_Utility.R_GetStreamingContext<GetSalesListContextDTO>(ContextConstant.SALES_STREAM_CONTEXT);

                //get internal context using global var company ID
                loBackparameter.CompanyId = R_BackGlobalVar.COMPANY_ID;
                loBackparameter.DepartmentId = loProgramContextDTO.DepartmentId;
                loBackparameter.SalesCount = loContextParameter.SalesCount;
            }
            catch (Exception ex)
            {

                loException.Add(ex);
            }

        EndBlock:
            loException.ThrowExceptionIfErrors();

            return GetStreamSales(loBackparameter);
        }

        private async IAsyncEnumerable<SalesStreamDTO> GetStreamSales(GetSalesListDbParameterDTO poParameter)
        {
            ContextCls loCls = null;
            List<SalesStreamDTO> loSalesList;

            loCls = new ContextCls();
            loSalesList = loCls.GetSalesListDb(poParameter);
            foreach (SalesStreamDTO item in loSalesList)
            {
                yield return item;
                await Task.Delay(3000);
            }
        }


        [HttpPost]
        public IAsyncEnumerable<OrderStreamDTO> GetOrderList()
        {
            R_Exception loException = new R_Exception();
            ProgramContextDTO loProgramContextDTO;
            GetOrderListContextDTO loContextParameter;
            GetOrderListDbParameterDTO loBackparameter = null;

            ContextCls loContextCls;
            IAsyncEnumerable<SalesStreamDTO> loRtn = null;
            List<OrderStreamDTO> loTempRtn;

            //string lcCompanyId;
            //string lcDepartmentId;
            //string lcSalesId;
            //int lnOrderCount;

            try
            {
                //dapatkan Context
                loBackparameter = new GetOrderListDbParameterDTO();
                //public context
                loProgramContextDTO = R_Utility.R_GetContext<ProgramContextDTO>(ContextConstant.PROGRAM_CONTEXT);
                //Stream Context
                loContextParameter = R_Utility.R_GetStreamingContext<GetOrderListContextDTO>(ContextConstant.ORDER_STREAM_CONTEXT);

                //get internal context using global var company ID
                loBackparameter.CompanyId = R_BackGlobalVar.COMPANY_ID;
                loBackparameter.DepartmentId = loProgramContextDTO.DepartmentId;
                loBackparameter.OrderCount = loContextParameter.OrderCount;
                loBackparameter.SalesId = loContextParameter.SalesId;
            }
            catch (Exception ex)
            {

                loException.Add(ex);
            }

        EndBlock:
            loException.ThrowExceptionIfErrors();

            return GetStreamOrder(loBackparameter);
        }

        private async IAsyncEnumerable<OrderStreamDTO> GetStreamOrder(GetOrderListDbParameterDTO poParameter)
        {
            ContextCls loCls = null;
            List<OrderStreamDTO> loSalesList;

            loCls = new ContextCls();
            loSalesList = loCls.GetOrderListDb(poParameter);
            foreach (OrderStreamDTO item in loSalesList)
            {
                yield return item;
            }
        }
    }
}
