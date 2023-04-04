using ProcessCommon;
using R_APIClient;
using R_APICommonDTO;
using R_AuthenticationEnumAndInterface;
using R_CommonFrontBackAPI;
using R_ProcessAndUploadFront;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.IO;
using ProcessBack;
using ProcessConsole;

internal class Program
{

    private static HttpClient loHttpClient;
    private static R_HTTPClient loClient;
    private static void Main(string[] args)
    {
        loHttpClient = new HttpClient();
        loHttpClient.BaseAddress = new Uri("http://localhost:5084/");
        R_HTTPClient.R_CreateInstanceWithName("DEFAULT", loHttpClient);
        loClient = R_HTTPClient.R_GetInstanceWithName("DEFAULT");

        //Task.Run(() => ServiceAtttachFile());
       // Task.Run(() => ServiceProcess());
        Task.Run(() => ServiceSaveBatchWithBulkCopy(false));

        Console.ReadKey();
    }

    static async Task ServiceAtttachFile()
    {
        R_APIException loException = new R_APIException();
        List<R_KeyValue> loUserParameters;
        R_UploadPar loUploadPar;
        R_ProcessAndUploadClient loCls;
        R_IProcessProgressStatus loProgress;

        try
        {
            //mempersiapkan user par
            loUserParameters = new List<R_KeyValue>();
            loUserParameters.Add(new R_KeyValue()
            {
                Key = ProcessConstant.EMPLOYEE_ID,
                Value = "Employee01"
            });


            //mempersiapkan upload file
            loUploadPar = new R_UploadPar();
            loUploadPar.UserParameters = loUserParameters;

            loUploadPar.USER_ID = "User002";
            loUploadPar.COMPANY_ID = "CO0012";
            loUploadPar.ClassName = "ProcessBack.AttachFileCls";

            loUploadPar.FilePath = $@"D:\Materi\Training Realta\Day 5 Upload and Process\Data\Test.pdf";
            loUploadPar.File = new R_File();
            loUploadPar.File.FileId = Path.GetFileNameWithoutExtension(loUploadPar.FilePath);
            loUploadPar.File.FileDescription = $"This is desc of {loUploadPar.File.FileId} file";
            loUploadPar.File.FileExtension = Path.GetExtension(loUploadPar.FilePath);

            //proses status
            loProgress = new ProcessStatus();

            //proses kelas
            loCls = new R_ProcessAndUploadClient(poProcessProgressStatus: loProgress, plSendWithContext: false, plSendWithToken: false);
            await loCls.R_AttachFile<object>(loUploadPar);
        }
        catch (Exception ex)
        {
            loException.add(ex);
        }
    EndBlock:
        loException.ThrowExceptionIfErrors();
    }

    static async Task ServiceProcess()
    {
        R_APIException loException = new R_APIException();
        List<R_KeyValue> loUserParameters;
        R_BatchParameter loBatchPar;
        R_ProcessAndUploadClient loCls;
        R_IProcessProgressStatus loProgress;
        string lcGuid;

        try
        {
            //prepare for user par
            loUserParameters = new List<R_KeyValue>();
            loUserParameters.Add(new R_KeyValue() { Key = ProcessConstant.LOOP, Value = 10 });
            loUserParameters.Add(new R_KeyValue() { Key = ProcessConstant.IS_ERROR, Value = true });
            loUserParameters.Add(new R_KeyValue() { Key = ProcessConstant.IS_ERROR_STATEMENT, Value = false });


            //mempersiapkan upload file
            loBatchPar = new R_BatchParameter();
            loBatchPar.UserParameters = loUserParameters;

            loBatchPar.USER_ID = "User001";
            loBatchPar.COMPANY_ID = "CO001";
            loBatchPar.ClassName = "ProcessBack.BatchProcessCls";

            //proses status
            loProgress = new ProcessStatus();
            //((ProcessStatus)loProgress).CompanyId

            //proses kelas
            loCls = new R_ProcessAndUploadClient(poProcessProgressStatus: loProgress, plSendWithContext: false, plSendWithToken: false);
            lcGuid = await loCls.R_BatchProcess<object>(loBatchPar, 10);

            Console.WriteLine($"Process with return GUID {lcGuid}");
            // GEt
        }
        catch (Exception ex)
        {
            loException.add(ex);
        }
    EndBlock:
        loException.ThrowExceptionIfErrors();
    }
    private static async Task ServiceSaveBatchWithBulkCopy(bool plGenerateErrorData)
    {
        R_APIException loException = new R_APIException();
        R_BatchParameter loBatchPar;
        List<EmployeeDTO> loBigObject;
        R_ProcessAndUploadClient loCls;

        R_IProcessProgressStatus loProgressStatus;


        string lcRtn;

        try
        {
            // Kirim Data ke Big Object
            loBigObject = GenerateEmployeeData("01", 100, plGenerateErrorData);

            loProgressStatus = new ProcessStatus();

            // Instantiate ProcessClient
            loCls = new R_ProcessAndUploadClient(poProcessProgressStatus: loProgressStatus, plSendWithContext: false, plSendWithToken: false);

            // preapare Batch Parameter
            loBatchPar = new R_BatchParameter();

            loBatchPar.COMPANY_ID = "01";
            loBatchPar.USER_ID = "GY";
            loBatchPar.ClassName = "ProcessBack.SaveBatchWithBulkCopyCls";
            loBatchPar.BigObject = loBigObject;

            //Initial For Error Report
            ((ProcessStatus)loProgressStatus).CompanyId = loBatchPar.COMPANY_ID;
            ((ProcessStatus)loProgressStatus).UserId = loBatchPar.USER_ID;


            lcRtn = await loCls.R_BatchProcess<List<EmployeeDTO>>(loBatchPar, 100);
        }
        catch (Exception ex)
        {
            loException.add(ex);
        }
    EndBlock:
        loException.ThrowExceptionIfErrors();

    }

    private static List<EmployeeDTO> GenerateEmployeeData(string pcCoId, int pnTotalEmployee, bool plGenerateErrorData)
    {
        List<EmployeeDTO> loRtn = new List<EmployeeDTO>();
        string lcSex;

        for (var lnCount = 1; lnCount <= pnTotalEmployee; lnCount++)
        {
            if (plGenerateErrorData && lnCount == 3)
                lcSex = "D";
            else
                if ((lnCount % 2) == 0)
            {
                lcSex = "M";
            }
            else
            {
                lcSex = "F";
            }

            loRtn.Add(new EmployeeDTO()
            {
                CompanyId = pcCoId.Trim(),
                EmployeeId = string.Format("Emp{0}", lnCount.ToString("00000")),
                FirstName = string.Format("Employee {0}", lnCount.ToString("00000")),
                LastName = string.Format("Last Name {0}", lnCount.ToString("00000")),
                SeqNo = lnCount,
                SexId = lcSex,
                TotalChildren = (lnCount % 3)
            });
        }
        return loRtn;
    }
}