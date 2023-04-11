using GSM00200Common;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GSM00200Model
{
    public class GSM00200Model : R_BusinessObjectServiceClientBase<GSM00200DTO>, IGSM00200
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/GSM00200";

        public GSM00200Model(string pcHttpClientName,
            string pcRequestServiceEndPoint,
            bool plSendWithContext = true,
            bool plSendWithToken = true) :
            base(pcHttpClientName, pcRequestServiceEndPoint, plSendWithContext, plSendWithToken)
        {
        }

        public async IAsyncEnumerable<GSM00200DTO> GetTableHDList()
        {
            var loException = new R_Exception();
            IAsyncEnumerable<GSM00200DTO> loRtn = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loRtn = await R_HTTPClientWrapper.R_APIRequestObject<GSM00200DTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM00200.GetTableHDList),
                    _SendWithContext, _SendWithToken
                    );
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            //EndBlock:
            loException.ThrowExceptionIfErrors();
          return  loRtn;
        }

        public Task<GSM00200DTO> GetTableList()
        {
            throw new NotImplementedException();
        }
}

        /*
        public async IAsyncEnumerable<GSM00200DTOnon> GetTableHDListAsync()
        {
            var loException = new R_Exception();
            GSM00200DTOnon<GSM00200DTOnon> loReturn = null;

            try
            {

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
        }

        */
    }
}
