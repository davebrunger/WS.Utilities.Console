using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace WS.Utilities.Console.Tabulation
{
    internal class TotalColumns
    {
        public Column TotalColumn { get; }
        public Column HeaderColumn { get; }
        public string TotalFormat { get; }

        public TotalColumns(IEnumerable<Column> sizedColumns, TotalColumnNames totalColumnNames)
        {
            var sizedColumnsList = sizedColumns.ToImmutableList();
            TotalColumn = sizedColumnsList.Single(c => c.Name == totalColumnNames.TotalColumn);
            HeaderColumn = sizedColumnsList.Single(c => c.Name == totalColumnNames.HeaderColumn);
            TotalFormat = string.Join(" ",
                sizedColumnsList.Select(
                    c => c.Equals(HeaderColumn) || c.Equals(TotalColumn) ? c.ToString() : new string(' ', c.Width)));
        }
    }
}
