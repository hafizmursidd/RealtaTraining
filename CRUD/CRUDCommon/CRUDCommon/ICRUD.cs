using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUDCommon
{
    public interface ICRUD : R_IServiceCRUDBase<CustomerDTO>
    {
        IAsyncEnumerable<CustomerStreamDTO> CustomerList();
    }
}
