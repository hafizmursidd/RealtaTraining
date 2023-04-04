using R_CommonFrontBackAPI;
using System.Collections.Generic;

namespace GSM00200Common
{
    public interface IGSM00210 : R_IServiceCRUDBase<GSM00210DTO>
    {
        IAsyncEnumerable<GSM00210DTOnon> GetTableDTList();
    }

}
