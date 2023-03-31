using R_BackEnd;
using R_Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TranScopeCommon;

namespace TranScopeBack
{
    public class TranScopeCls
    {
        public TranScopeDemoDataDTO ProcessWithoutTransactionDB(int poProcessRecordCount)
        {
            R_Exception loException = new R_Exception();
            TranScopeDemoDataDTO loReturn = new TranScopeDemoDataDTO();
            R_Db loDb = new R_Db();
            List<CustomerDbDTO> Customer;

            try
            {
                Customer = GetAllCustomer(poProcessRecordCount);
                //RemoveAllRecord(Customer);
                //AddAllRecord(List<CustomerDbDTO>);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();

            return loReturn;
        }



        public void TranScopeDemo(TranScopDemoParameterDTO poTranScopDemoParameterDTO)
        {
            R_Exception loException = new R_Exception();
            R_Db loDb = new R_Db();

            switch (poTranScopDemoParameterDTO.TransType)
            {
                case eTransType.WithoutTransaction:
                    {
                        //RemoveAllRecord(poTranScopDemoParameterDTO.RecordCount);
                        //AddAllRecord(poTranScopDemoParameterDTO.RecordCount);

                        break;
                    }

                case eTransType.AllTransaction:
                    {
                        try
                        {
                            using (TransactionScope TransScope = new TransactionScope(TransactionScopeOption.Suppress))
                            {
                                //RemoveAllRecord(poTranScopDemoParameterDTO.RecordCount);
                                //AddAllRecord(poTranScopDemoParameterDTO.RecordCount);

                                TransScope.Complete();
                            }
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }

                        break;
                    }
                case eTransType.PerRecordTransacton:
                    {
                        for (int lnCount = 1; lnCount <= poTranScopDemoParameterDTO.RecordCount; lnCount++)
                        {

                            try
                            {
                                using (TransactionScope TransScope = new TransactionScope(TransactionScopeOption.RequiresNew))
                                {
                                    RemoveRecord(lnCount);
                                    AddRecord(lnCount);

                                    TransScope.Complete();
                                }
                            }
                            catch (Exception ex)
                            {
                                loException.Add(ex);
                            }
                        }

                        break;
                    }
            }

        }

        private List<CustomerDbDTO> GetAllCustomer(int pnCount)
        {
            R_Exception loException = new R_Exception();
            R_Db loDb;
            List<CustomerDbDTO> loReturn = null;
            string lcCmd;
            string lcCust;
            try
            {
                lcCust = String.Format("Cust{0}", pnCount.ToString("0000"));
                lcCmd = "SELECT * from TestCustomer(nolock) WHERE CustomerID <= {0} ";

                loDb = new R_Db();
                loReturn = loDb.SqlExecObjectQuery<CustomerDbDTO>(lcCmd, lcCust);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

        EndBlock:
            loException.ThrowExceptionIfErrors();

            return loReturn;
        }
        private void RemoveAllRecord(List<CustomerDbDTO> poCustomer)
        {
            R_Exception loException = new R_Exception();
            R_Db loDb = new R_Db();
            DbConnection loConn = null;
            DbCommand loCommand;
            DbParameter loDbParameter;

            string lcCmd;
            try
            {
                loDb = new R_Db();
                loCommand = loDb.GetCommand();
                loConn = loDb.GetConnection();
                loDb.R_AddCommandParameter(loCommand, "Param1", DbType.String, 50, "");

                loDbParameter = loCommand.Parameters[0];
                foreach (var item in poCustomer)
                {
                    lcCmd = "DELETE TestCustomer WHERE CustomerId = @Param1";
                    loCommand.CommandText = lcCmd;
                    loDbParameter.Value = item.CustomerId;
                    loDb.SqlExecNonQuery(loConn, loCommand, false);

                    lcCmd = "INSERT into TestCustomerLog(log) values (@Param1)";
                    loCommand.CommandText = lcCmd;
                    loDbParameter.Value = $"Remove Cust --{item.CustomerId}--";
                    loDb.SqlExecNonQuery(loConn, loCommand, false);
                }
            }
            catch (Exception ex)
            {
                loException.Add(ex);
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
            loException.ThrowExceptionIfErrors();
        }

        private void AddAllRecord(List<CustomerDbDTO> poCustomer)
        {
            R_Exception loException = new R_Exception();
            R_Db loDb = new R_Db();
            DbConnection loConn = null;
            DbCommand loCommand;
            string lcCmd = null;
            DbParameter loDbParCustomerID;
            DbParameter loDbParCustomerName;
            DbParameter loDbParContactName;
            try
            {
                loDb = new R_Db();
                loCommand = loDb.GetCommand();
                loConn = loDb.GetConnection();
                loDb.R_AddCommandParameter(loCommand, "CustomerID", DbType.String, 50, "");
                loDb.R_AddCommandParameter(loCommand, "CustomerName", DbType.String, 50, "");
                loDb.R_AddCommandParameter(loCommand, "ContactName", DbType.String, 50, "");

                loDbParCustomerID = loCommand.Parameters["CustomerID"];
                loDbParCustomerName = loCommand.Parameters["CustomerName"];
                loDbParContactName = loCommand.Parameters["ContactName"];

                loCommand.CommandText = lcCmd;
                loDb.SqlExecNonQuery(loConn, loCommand, false);

                foreach (var item in poCustomer)
                {
                    lcCmd = "INSERT into TestCopyCustomer(Customer_Id, Customer_Name, Contact_Name) values (@Customer_Id, @Customer_Name, @Contact_Name)";
                    loCommand.CommandText = lcCmd;
                    loDbParCustomerID.Value = item.CustomerId;
                    loDbParCustomerName.Value = item.CustomerName;
                    loDbParContactName.Value = item.ContactName;

                    loDb.SqlExecNonQuery(loConn,loCommand, false);
                }
            }
            catch (Exception ex)
            {
                loException.Add(ex);
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
            loException.ThrowExceptionIfErrors();
        }
        private void RemoveRecord(int pnCount)
        {
            R_Exception loException = new R_Exception();
            R_Db loDb = new R_Db();

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
        private void AddRecord(int pnCount)
        {
            R_Exception loException = new R_Exception();
            R_Db loDb = new R_Db();

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
    }
}
