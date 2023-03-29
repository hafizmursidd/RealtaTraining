using R_APICommonDTO;
using System;
using System.Text;

namespace ExceptionCommon
{
    public class CustomerResultDTO : R_APIResultBaseDTO
    {
        public CustomerDTO data { get; set; }
    }
}
