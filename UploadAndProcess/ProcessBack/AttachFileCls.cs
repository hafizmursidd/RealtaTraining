using ProcessCommon;
using R_BackEnd;
using R_Common;
using System.Data;
using System.Data.Common;

namespace ProcessBack
{
    public class AttachFileCls : R_IAttachFile
    {
        public void R_AttachFile(R_AttachFilePar poAttachFile)
        {
            R_Exception loException = new R_Exception();
            R_Db loDb;
            string lcEmpId = "";
            string lcCmd;
            DbCommand loCommand;

            try
            {
                if (poAttachFile.UserParameters.Count > 0)
                {

                    var loVar = poAttachFile.UserParameters.Where((x) => x.Key.Equals(ProcessConstant.EMPLOYEE_ID)).FirstOrDefault().Value;
                    if (loVar == null)
                    {
                        loException.Add("001", "Employee ID is not found");
                        goto EndBlock;
                    }

                    lcEmpId = ((System.Text.Json.JsonElement)loVar).GetString();
                    if (string.IsNullOrEmpty(lcEmpId))
                    {
                        loException.Add("001", "Employee ID is not found");
                        goto EndBlock;
                    }
                }
                 //lcCmd = $"Insert into TestEmployeeAttachment(CoId,EmpId,FileName,oData,FileExtension) values('{poAttachFile.Key.COMPANY_ID}','{lcEmpId}','{poAttachFile.File.FileDescription}'" +
                 //   $",dbo.RFN_CombineByte('{poAttachFile.Key.COMPANY_ID}','{poAttachFile.Key.USER_ID}','{poAttachFile.Key.KEY_GUID}'),'{poAttachFile.File.FileExtension}' )";

                lcCmd = $"INSERT INTO TestEmployeeAttachment (CoId, EmpId, FileName, oData, FileExtension) " +
                         $"VALUES ('{poAttachFile.Key.COMPANY_ID}', '{lcEmpId}', '{poAttachFile.File.FileDescription}', " +
                         $"dbo.RFN_CombineByte('{poAttachFile.Key.COMPANY_ID}', '{poAttachFile.Key.USER_ID}','{poAttachFile.Key.KEY_GUID}'), " +//Odata
                         $"'{poAttachFile.File.FileExtension}')"; //fileExtension


                loDb = new R_Db();
                loCommand = loDb.GetCommand();
                loCommand.CommandText = lcCmd;
                loCommand.CommandType = CommandType.Text;
                loDb.SqlExecNonQuery(loDb.GetConnection(), loCommand, true);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();

        }
    }

    public class ProcessFileCls
    {

    }
}
