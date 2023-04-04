using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GSM00200Common
{
    public interface IGSM00200 : R_IServiceCRUDBase<GSM00200DTO>
    {
        IAsyncEnumerable<GSM00200DTOnon> GetTableHDList();
    }

}
