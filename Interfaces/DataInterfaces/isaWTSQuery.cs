namespace UMDGeneral.DataManager.Interfaces
{
    public interface isaWTSQuery
    {
        string SQL { get; set; }
        string FileName { get; set; }
        string DateField { get; set; }
        string AdditionalConditions { get; set; }
    }
}
