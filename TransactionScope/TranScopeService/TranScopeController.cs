using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R_Common;
using Microsoft.AspNetCore.Mvc;
using TranScopeCommon;
using TranScopeBack;

namespace TranScopeService
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TranScopeController : ControllerBase, ITranScope
    {
        [HttpPost]
        public TranScopeDemoResultDTO ProcessWithoutTransaction(int poProcessRecord)
        {
            R_Exception loException = new R_Exception();
            TranScopeDemoResultDTO loReturn = null;
            TranScopeCls loCls = null;
            try
            {
                loCls = new TranScopeCls();
                loReturn = new TranScopeDemoResultDTO();
                loReturn.data = loCls.ProcessWithoutTransactionDB(poProcessRecord);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();

            return loReturn;
        }
        [HttpPost]
        public TranScopeDemoResultDTO ProcessAllWithTransaction(int poProcessRecord)
        {
            R_Exception loException = new R_Exception();
            TranScopeDemoResultDTO loReturn = null;
            TranScopeCls loCls = null;
            try
            {
                loCls = new TranScopeCls();
                loReturn = new TranScopeDemoResultDTO();
                loReturn.data = loCls.ProcessAllWithTransactionDB(poProcessRecord);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();

            return loReturn;

        }
        [HttpPost]
        public TranScopeDemoResultDTO ProcessPerRecordTransaction(int poProcessRecord)
        {
            R_Exception loException = new R_Exception();
            TranScopeDemoResultDTO loRtn = null;
            TranScopeCls loCls = null;
            try
            {
                loRtn = new TranScopeDemoResultDTO();

                loCls = new TranScopeCls();
                loRtn.data = loCls.ProcessEachTransactionDB(poProcessRecord);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        //[HttpPost]
        //public TranScopeDemoResultDTO TranScopeDemo(TranScopDemoParameterDTO poTranScopeDemoParameter)
        //{
        //    R_Exception loException = new R_Exception();
        //    TranScopeDemoResultDTO loReturn = null;
        //    try
        //    {
        //        loReturn = new TranScopeDemoResultDTO()
        //        {
        //            data = new TranScopeDemoDataDTO()
        //        };
        //        loReturn.data.IsSuccess = false;

        //        TranScopeCls loCls = new TranScopeCls();
        //        loCls.TranScopeDemo(poTranScopeDemoParameter);
        //        loReturn.data.IsSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        loException.Add(ex);
        //    }
        //EndBlock:
        //    loException.ThrowExceptionIfErrors();

        //    return loReturn;
        //}
    }
}
