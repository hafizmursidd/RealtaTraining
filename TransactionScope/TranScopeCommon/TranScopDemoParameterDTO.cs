namespace TranScopeCommon
{
    public class TranScopDemoParameterDTO
    {
        public int RecordCount { get; set; }
        public eTransType TransType { get; set; }
    }

    public enum eTransType
    {
        WithoutTransaction,
        AllTransaction,
        PerRecordTransacton
    }
}
