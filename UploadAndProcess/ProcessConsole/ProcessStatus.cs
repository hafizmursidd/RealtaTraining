using R_APICommonDTO;
using R_CommonFrontBackAPI;
using R_ProcessAndUploadFront;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProcessConsole
{
    public class ProcessStatus : R_IProcessProgressStatus
    {
        public string CompanyId { get; set; }
        public string UserId { get; set; }

        public Task ProcessComplete(string pcKeyGuid, eProcessResultMode poProcessResultMode)
        {
            if (poProcessResultMode == eProcessResultMode.Success)
            {
                Console.WriteLine($"Program Success with GUID {pcKeyGuid}");
            }
            else
            {
                Console.WriteLine($"Program Success with FAIL GUID {pcKeyGuid}");
                GetError(pcKeyGuid);
            }
            return Task.CompletedTask;
        }

        public Task ProcessError(string pcKeyGuid, R_APIException ex)
        {
            Console.WriteLine($"Program Failed with GUID {pcKeyGuid}");
            foreach (R_Error item in ex.ErrorList)
            {
                Console.WriteLine($"Error with {item.ErrDescp}");
            }
            return Task.CompletedTask;
        }

        public Task ReportProgress(int pnProgress, string pcStatus)
        {
            Console.WriteLine($"Step {pnProgress} with status {pcStatus}");
            return Task.CompletedTask;
        }

        private async Task GetError(string pcKeyGuid)
        {
            R_APIException loException;
            R_ProcessAndUploadClient loCls;
            List<R_ErrorStatusReturn> loErrorReturn;
            try
            {
                loCls = new R_ProcessAndUploadClient(plSendWithContext: false, plSendWithToken: false);
                loErrorReturn =await loCls.R_GetErrorProcess(new R_UploadAndProcessKey() { COMPANY_ID = this.CompanyId, USER_ID = this.UserId, KEY_GUID = pcKeyGuid });
                foreach (var item in loErrorReturn)
                {
                    Console.WriteLine($"Error Seq: {item.SeqNo}, Error Msg: {item.ErrorMessage} !!!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); ;
            }
        }
    }
}
