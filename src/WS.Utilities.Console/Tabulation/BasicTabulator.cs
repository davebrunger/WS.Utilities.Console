using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace WS.Utilities.Console.Tabulation
{
    internal class BasicTabulator
    {
        private readonly ImmutableList<Column> _columnDefinitions;
        private readonly bool _headerRow;
        private readonly TotalColumnNames _totalColumnNames;
        private readonly int _separatorRow;

        public BasicTabulator(IEnumerable<Column> columnDefinitions, bool headerRow, int separatorRow, TotalColumnNames totalColumnNames = null)
        {
            _columnDefinitions = columnDefinitions.ToImmutableList();
            _headerRow = headerRow;
            _totalColumnNames = totalColumnNames;
            _separatorRow = separatorRow;
        }

        public void Tabulate(IEnumerable<IEnumerable<string>> data, IOutputWriter outputWriter)
        {
            var columnList = _columnDefinitions.ToList();
            if (columnList.GroupBy(c => c.DataIndex).Any(g => g.Count() > 1))
            {
                throw new ArgumentException("Column Data Indexes must be unique", nameof(_columnDefinitions));
            }
            var dataAsList = data.Select(row => row.ToList()).ToList();

            var sizedColumns = dataAsList.Count > 0
                ? columnList
                    .Select(c => c.WithWidth(dataAsList.Select(r => r[c.DataIndex].Length).Max()))
                    .Select(c => c.WithWidth(c.Name == "Total" ? c.Width + 2 : c.Width))
                    .ToList()
                : columnList;

            var tabulatorRowWriter = new TabulatorRowWriter(sizedColumns, outputWriter, _totalColumnNames, _separatorRow > 0);

            var total = 0D;
            string lastHeading = null;
            var firstRow = true;

            if (_headerRow)
            {
                tabulatorRowWriter.WriteHeaderRow();
                tabulatorRowWriter.WriteSeparatorRow('=');
            }
            var rowIndex = 0;
            foreach (var row in dataAsList)
            {
                if (tabulatorRowWriter.TotalColumns != null)
                {
                    var currentHeading = row[tabulatorRowWriter.TotalColumns.HeaderColumn.DataIndex];
                    if ((currentHeading != lastHeading) && !firstRow)
                    {
                        tabulatorRowWriter.WriteSeparatorRow('-');
                        tabulatorRowWriter.WriteTotalRow(lastHeading, total);
                        tabulatorRowWriter.WriteSeparatorRow('-');
                        total = 0;
                        rowIndex = 0;
                    }
                    lastHeading = currentHeading;
                    if (!string.IsNullOrEmpty(row[tabulatorRowWriter.TotalColumns.TotalColumn.DataIndex]))
                    {
                        total = total + double.Parse(row[tabulatorRowWriter.TotalColumns.TotalColumn.DataIndex]);
                    }
                }
                firstRow = false;
                var highlight = (_separatorRow > 0) && (rowIndex > 0) && (rowIndex % _separatorRow == 0);
                tabulatorRowWriter.WriteDataRow(row, highlight);
                rowIndex = rowIndex + 1;
            }
            if ((tabulatorRowWriter.TotalColumns == null) || firstRow)
            {
                return;
            }
            tabulatorRowWriter.WriteSeparatorRow('-');
            tabulatorRowWriter.WriteTotalRow(lastHeading, total);
            tabulatorRowWriter.WriteSeparatorRow('=');
        }
    }
}
