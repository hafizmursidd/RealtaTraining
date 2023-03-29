using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using R_Common;
using R_CommonFrontBackAPI;

//tergantung project
using ExceptionCommon;
using ExceptionBack;


namespace ExceptionService
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExceptionController : ControllerBase, ICustomer
    {
        [HttpPost]
        public CustomerResultDTO GetCustomerById(GetCustomerByIdParameterDTO poParameter)
        {

            R_Exception loException = new R_Exception();
            CustomerResultDTO loRtn = null;
            ExceptionCls loCls = null;

            try
            {
                loRtn = new CustomerResultDTO();
                loCls = new ExceptionCls();
                loRtn.data = loCls.GetCustomerByIdDb(new GetCustomerByIdDbParameterDTO()
                {
                    CustomerId = poParameter.CustomerId
                });
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public async IAsyncEnumerable<CustomerStreamDTO> GetCustomersList(GetCustomersParameterDTO poParameter)
        {
            //deklrasi variabel
            List<CustomerStreamDTO> loRtnTemp;
            ExceptionCls loCls = null;
            GetCustomersDbParameterDTO loParameter;

            //Siapkan Back Cls
            loCls = new ExceptionCls();
            //Siapkan parameter ke back
            loParameter = new GetCustomersDbParameterDTO() { CustomerCount = poParameter.CustomerCount };

            loRtnTemp = loCls.GetCustomersDb(loParameter);
            foreach (CustomerStreamDTO loEntity in loRtnTemp)
            {
                yield return loEntity;
                await Task.Delay(50);
            }
        }
    }
}
