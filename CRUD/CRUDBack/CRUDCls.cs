using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data;
using CRUDCommon;
using System.Data.Common;

namespace CRUDBack
{
    public class CRUDCls : R_BusinessObject<CustomerDTO>
    {
        public List<CustomerStreamDTO> CustomerListDB(CustomerListDBParameterDTO poParameter)
        {
            R_Exception loException = new R_Exception();
            List<CustomerStreamDTO> loReturn = null;
            string lcCmd;
            R_Db loDb;

            try
            {
                lcCmd = @"Select CCOMPANY_ID, CustomerID, CustomerName 
                        FROM TrainCustomer (nolock)
                        WHERE CCOMPANY_ID = {0}";
                loDb = new R_Db();
                loReturn = loDb.SqlExecObjectQuery<CustomerStreamDTO>(lcCmd, poParameter.CCOMPANY_ID);
            }
            catch (Exception ex)
            {

                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();

            return loReturn;
        }

        protected override void R_Deleting(CustomerDTO poEntity)
        {
            R_Exception loEexception = new R_Exception();
            string lcCmd;
            R_Db loDb;
            DbCommand loCommand;
            DbConnection loConn = null;
            try
            {
                lcCmd = @"DELETE TrainCustomer
                        WHERE CCOMPANY_ID = @CCOMPANY_ID
                        AND CustomerID = @CustomerID";

                loDb = new R_Db();
                loConn = loDb.GetConnection();
                loCommand = loDb.GetCommand();
                loCommand.CommandText = lcCmd;

                loDb.R_AddCommandParameter(loCommand, "CCOMPANY_ID", DbType.String, 10, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "CustomerID", DbType.String, 10, poEntity.CustomerID);

                loDb.SqlExecNonQuery(loConn, loCommand, true);
            }
            catch (Exception ex)
            {

                loEexception.Add(ex);
            }
        EndBlock:
            loEexception.ThrowExceptionIfErrors();
        }

        protected override CustomerDTO R_Display(CustomerDTO poEntity)
        {
            R_Exception loEexception = new R_Exception();
            CustomerDTO loReturn = null;
            string lcCmd;
            R_Db loDb;
            try
            {
                lcCmd = @"Select CCOMPANY_ID, CustomerID, CustomerName, ContactName
                        FROM TrainCustomer (nolock)
                        WHERE CCOMPANY_ID = {0}
                        AND CustomerID = {1}";

                loDb = new R_Db();
                loReturn = loDb.SqlExecObjectQuery<CustomerDTO>(lcCmd, poEntity.CCOMPANY_ID, poEntity.CustomerID).FirstOrDefault();

            }
            catch (Exception ex)
            {

                loEexception.Add(ex);
            }
        EndBlock:
            loEexception.ThrowExceptionIfErrors();

            return loReturn;
        }

        protected override void R_Saving(CustomerDTO poNewEntity, eCRUDMode poCRUDMode)
        {
            R_Exception loEexception = new R_Exception();
            string lcCmd = null;
            R_Db loDb;
            DbCommand loCommand;
            DbConnection loConn = null;
            CustomerDTO loTempEntity;
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection();
                lcCmd = @"Select CCOMPANY_ID
                        FROM TrainCustomer (updlock)
                        WHERE CCOMPANY_ID = {0}
                        AND CustomerID = {1}";
                loTempEntity = loDb.SqlExecObjectQuery<CustomerDTO>(lcCmd, loConn, false, poNewEntity.CCOMPANY_ID, poNewEntity.CustomerID).FirstOrDefault();

                switch (poCRUDMode)
                {
                    case eCRUDMode.AddMode:
                        if (loTempEntity!=null)
                        {
                            loEexception.Add("001", R_Utility.R_GetMessage("CRUDBackResources", "001"));
                            goto EndBlock;
                        }
                        lcCmd = @"INSERT INTO TrainCustomer (CCOMPANY_ID, CustomerID, CustomerName, ContactName)
                                Values (@CCOMPANY_ID, @CustomerID, @CustomerName, @ContactName)";
                        break;

                    case eCRUDMode.EditMode:
                        if (loTempEntity == null)
                        {
                            loEexception.Add("002", R_Utility.R_GetMessage("CRUDBackResources", "002"));
                            goto EndBlock;
                        }
                        lcCmd = @"UPDATE TrainCustomer
                                SET CustomerName = @CustomerName, ContactName = @ContactName
                                WHERE CCOMPANY_ID = @CCOMPANY_ID AND CustomerID = @CustomerID";
                        break;

                    default:

                        break;

                }

                loCommand = loDb.GetCommand();
                loCommand.CommandText = lcCmd;

                loDb.R_AddCommandParameter(loCommand, "CCOMPANY_ID", DbType.String, 10, poNewEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "CustomerID", DbType.String, 10, poNewEntity.CustomerID);
                loDb.R_AddCommandParameter(loCommand, "CustomerName", DbType.String, 50, poNewEntity.CustomerName);
                loDb.R_AddCommandParameter(loCommand, "ContactName", DbType.String, 10, poNewEntity.ContactName);

                loDb.SqlExecNonQuery(loConn, loCommand, true);
            }
            catch (Exception ex)
            {
                loEexception.Add(ex);
            }
            finally
            {
                if (loConn != null)
                {
                    if (loConn.State != ConnectionState.Closed)
                    {
                        loConn.Close();
                    }
                    loConn.Dispose();
                }
            }
        EndBlock:
            loEexception.ThrowExceptionIfErrors();
        }
    }
}
