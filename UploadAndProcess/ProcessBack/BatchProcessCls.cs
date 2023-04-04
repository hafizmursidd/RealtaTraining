using ProcessCommon;
using R_BackEnd;
using R_Common;
using System.Data;
using System.Data.Common;

namespace ProcessBack
{
    public class BatchProcessCls : R_IBatchProcess
    {
        public void R_BatchProcess(R_BatchProcessPar poBatchProcessPar)
        {
            R_Exception loException = new R_Exception();
            int lnLoop;
            bool llIsError;
            bool llIsErrorStt;
            R_Db loDb;
            DbCommand loCommand;
            string lcCmd;

            try
            {
                var loVar = poBatchProcessPar.UserParameters.Where((x) => x.Key.Equals(ProcessConstant.LOOP)).FirstOrDefault().Value;
                if (loVar ==null)
                {
                    loException.Add("001", "Loop Parameter not found");
                    goto EndBlock;
                }
                lnLoop = ((System.Text.Json.JsonElement)loVar).GetInt16();

                loVar = poBatchProcessPar.UserParameters.Where((x) => x.Key.Equals(ProcessConstant.IS_ERROR)).FirstOrDefault().Value;
                if (loVar == null)
                {
                    loException.Add("001", "IS Error not found");
                    goto EndBlock;
                }
                llIsError = ((System.Text.Json.JsonElement)loVar).GetBoolean();

                loVar = poBatchProcessPar.UserParameters.Where((x) => x.Key.Equals(ProcessConstant.IS_ERROR_STATEMENT)).FirstOrDefault().Value;
                if (loVar == null)
                {
                    loException.Add("001", "IS Error Statement Parameter not found");
                    goto EndBlock;
                }
                llIsErrorStt = ((System.Text.Json.JsonElement)loVar).GetBoolean();

                if (llIsErrorStt == true)
                {
                    loException.Add("002", "Error Statement");
                    goto EndBlock;
                }

                loDb = new R_Db();
                loCommand = loDb.GetCommand();
                loCommand.CommandText = "SampleProcessBatch";
                loCommand.CommandType = CommandType.StoredProcedure;


                loDb.R_AddCommandParameter(loCommand, "@CoId", DbType.String, 50, poBatchProcessPar.Key.COMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "@UserId", DbType.String, 50, poBatchProcessPar.Key.USER_ID);
                loDb.R_AddCommandParameter(loCommand, "@KeyGUID", DbType.String, 50, poBatchProcessPar.Key.KEY_GUID);
                loDb.R_AddCommandParameter(loCommand, "@Loop", DbType.Int16, 0, lnLoop);
                loDb.R_AddCommandParameter(loCommand, "@IsError", DbType.Boolean, 0, llIsError);

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
}
