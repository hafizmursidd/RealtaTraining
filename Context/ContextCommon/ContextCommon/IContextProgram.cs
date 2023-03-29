using System;
using System.Collections.Generic;
using System.Text;

namespace ContextCommon
{
    public interface IContextProgram
    {
        IAsyncEnumerable<SalesStreamDTO> GetSalesList();
        IAsyncEnumerable<OrderStreamDTO> GetOrderList();
    }
}
