namespace TranScopeCommon
{
    public interface ITranScope
    {
        // TranScopeDemoResultDTO TranScopeDemo(TranScopDemoParameterDTO poTranScopeDemoParameter);

        TranScopeDemoResultDTO ProcessWithoutTransaction(int poProcessRecord);
        TranScopeDemoResultDTO ProcessAllWithTransaction(int poProcessRecord);
        TranScopeDemoResultDTO ProcessPerRecordTransaction(int poProcessRecord);
    }
}
