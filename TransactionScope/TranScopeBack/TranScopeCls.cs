﻿using R_BackEnd;
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
using System.Transactions;

namespace TranScopeBack
{
    public class TranScopeCls
    {
        public TranScopeDemoDataDTO ProcessWithoutTransactionDB(int poProcessRecordCount)
        {
            R_Exception loException = new R_Exception();
            TranScopeDemoDataDTO loReturn = new TranScopeDemoDataDTO();
            List<CustomerDbDTO> Customer;

            try
            {
                Customer = GetAllCustomer(poProcessRecordCount);
                RemoveAllRecord(Customer);
                AddAllRecord(Customer);

                loReturn.IsSuccess = true;
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();

            return loReturn;
        }
        public TranScopeDemoDataDTO ProcessAllWithTransactionDB(int poProcessRecordCount)
        {
            R_Exception loException = new R_Exception();
            TranScopeDemoDataDTO loReturn = new TranScopeDemoDataDTO();
            List<CustomerDbDTO> Customer;

            try
            {
                Customer = GetAllCustomer(poProcessRecordCount);
                using (TransactionScope TransScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    RemoveAllRecord(Customer);
                    AddAllRecord(Customer);
                    TransScope.Complete();
                }

                loReturn.IsSuccess = true;
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
            int lnCount;

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

                //loCommand.CommandText = lcCmd;
                //loDb.SqlExecNonQuery(loConn, loCommand, false);
                lcCmd = "INSERT into TestCopyCustomer(CustomerID, CustomerName, ContactName) values (@CustomerID, @CustomerName, @ContactName)";
                loCommand.CommandText = lcCmd;
                lnCount = 1;

                foreach (var item in poCustomer)
                {
                    if ((lnCount % 3) == 0)
                    {
                        loException.Add("001", $"Error at {lnCount} data");
                        goto EndBlock;
                    }

                    loDbParCustomerID.Value = item.CustomerId;
                    loDbParCustomerName.Value = item.CustomerName;
                    loDbParContactName.Value = item.ContactName;

                    loDb.SqlExecNonQuery(loConn, loCommand, false);
                    lnCount++;
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
