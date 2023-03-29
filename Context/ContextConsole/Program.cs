using R_APIClient;
using ContextCommon;
using R_ContextFrontEnd;
using R_ContextEnumAndInterface;

namespace ContextConsole
{
    internal partial class Program
    {
        static HttpClient loHttpClient = null;
        static R_ContextHeader loContextHeader;

        static void Main(string[] args)
        {
            loHttpClient = new HttpClient
            {
                BaseAddress = new Uri("Http://localhost:5023/")
            };

            loContextHeader = new R_ContextHeader();
            //seting internal context
            loContextHeader.R_Context._SetInternalContext(R_InternalContextVarEnumerator.COMPANY_ID, "PT LIMBOTO");

            R_HTTPClient.R_CreateInstanceWithName("DEFAULT", loHttpClient, loContextHeader);

             Task.Run(() => GetSalesAsync());
            //Task.Run(() => GetOrderAsync());
            Console.ReadKey();

        }



        private static async Task GetSalesAsync()
        {
            ProgramContextDTO loProgramContextDTO;
            GetSalesListContextDTO loGetSalesListContextDTO;
            List<SalesStreamDTO> loSalesList;
            NotifySales notifiySales;

            //soapkan program context
            loProgramContextDTO = new ProgramContextDTO()
            {
                DepartmentId = "RnD"
            };
            loContextHeader.R_Context.R_SetContext(ContextConstant.PROGRAM_CONTEXT, loProgramContextDTO);

            //siapkan salesstreamContext
            loGetSalesListContextDTO = new GetSalesListContextDTO()
            {
                SalesCount = 5
            };
            loContextHeader.R_Context.R_SetStreamingContext(ContextConstant.SALES_STREAM_CONTEXT, loGetSalesListContextDTO);

            //loClient = R_HTTPClient.R_GetInstanceWithName("DEFAULT");
            try
            {
            notifiySales = new NotifySales();
            loSalesList = await R_HTTPClientWrapper.R_APIRequestStreamingObject<SalesStreamDTO>("api/Context",
                nameof(IContextProgram.GetSalesList), plSendWithContext: true, plSendWithToken: false, poNotify:notifiySales);

                //foreach (SalesStreamDTO item in loSalesList)
                //{
                //    Console.WriteLine($"Company : {item.CompanyId} SalesId : {item.SalesId} SalesName : {item.SalesName}");
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        private static async Task GetOrderAsync()
        {
            ProgramContextDTO loProgramContextDTO;
            GetOrderListContextDTO loGetOrderListContextDTO;
            List<OrderStreamDTO> loOrderList;


            try
            {
                //soapkan program context
                loProgramContextDTO = new ProgramContextDTO()
                {
                    DepartmentId = "RnD"
                };
                loContextHeader.R_Context.R_SetContext(ContextConstant.PROGRAM_CONTEXT, loProgramContextDTO);

                //siapkan salesstreamContext
                loGetOrderListContextDTO = new GetOrderListContextDTO()
                {
                    OrderCount = 5

                };

                loContextHeader.R_Context.R_SetStreamingContext(ContextConstant.ORDER_STREAM_CONTEXT, loGetOrderListContextDTO);

                //loClient = R_HTTPClient.R_GetInstanceWithName("DEFAULT");


                loOrderList = await R_HTTPClientWrapper.R_APIRequestStreamingObject<OrderStreamDTO>("api/Context",
                    nameof(IContextProgram.GetOrderList), plSendWithContext: true, plSendWithToken: false, poNotify: null);


                foreach (OrderStreamDTO item in loOrderList)
                {
                    Console.WriteLine($"Company : {item.CompanyId} SalesId : {item.DepartmentId}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
    }
}