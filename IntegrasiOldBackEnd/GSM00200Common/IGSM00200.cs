using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GSM00200Common
{
    public interface IGSM00200 : R_IServiceCRUDBase<GSM00200DTO>
    {
        IAsyncEnumerable<GSM00200DTOnon> GetTableHDList();
        Task<GSM00200DTOnon> GetTableList();
    }

}
