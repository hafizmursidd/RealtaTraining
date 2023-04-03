﻿using ProcessCommon;
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

        Task.Run(() => ServiceAtttachFile());
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
            loUserParameters= new List<R_KeyValue>();
            loUserParameters.Add(new R_KeyValue()
            {
                Key = ProcessConstant.EMPLOYEE_ID,
                Value = "Employee01"
            });


            //mempersiapkan upload file
            loUploadPar = new R_UploadPar();
            loUploadPar.UserParameters= loUserParameters;

            loUploadPar.USER_ID = "User002";
            loUploadPar.COMPANY_ID = "CO0012";
            loUploadPar.ClassName = "ProcessBack.AttachFileCls";

            loUploadPar.FilePath = $@"D:\Materi\Training Realta\Day 5 Upload and Process\Data\Test.pdf";
            loUploadPar.File = new R_File();
            loUploadPar.File.FileId =Path.GetFileNameWithoutExtension(loUploadPar.FilePath);
            loUploadPar.File.FileDescription = $"This is desc of {loUploadPar.File.FileId} file";
            loUploadPar.File.FileExtension = Path.GetExtension(loUploadPar.FilePath);

            //proses status
            loProgress = new ProcessStatus();

            //proses kelas
            loCls = new R_ProcessAndUploadClient(poProcessProgressStatus: loProgress, plSendWithContext:false,plSendWithToken : false);
            await loCls.R_AttachFile<object>(loUploadPar);
        }
        catch (Exception ex)
        {
            loException.add(ex);
        }
    EndBlock:
        loException.ThrowExceptionIfErrors();
    }
}