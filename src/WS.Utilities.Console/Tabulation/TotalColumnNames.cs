namespace WS.Utilities.Console.Tabulation
{
    public class TotalColumnNames
    {
        public string HeaderColumn { get; }
        public string TotalColumn { get; }

        public TotalColumnNames(string headerColumn, string totalColumn)
        {
            HeaderColumn = headerColumn;
            TotalColumn = totalColumn;
        }
    }
}
