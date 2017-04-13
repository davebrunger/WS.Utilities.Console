using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace WS.Utilities.Console.Tabulation
{
    internal class TabulatorRowWriter
    {
        private readonly ImmutableList<Column> _sizedColumns;
        private readonly IOutputWriter _outputWriter;
        private readonly string _format;
        private readonly bool _highlightHeadings;

        public TotalColumns TotalColumns { get; }

        public TabulatorRowWriter(IEnumerable<Column> sizedColumns, IOutputWriter outputWriter, TotalColumnNames totalColumnNames, bool highlightHeadings)
        {
            _sizedColumns = sizedColumns.ToImmutableList();
            _outputWriter = outputWriter;
            _highlightHeadings = highlightHeadings;
            _format = string.Join(" ", _sizedColumns.Select(c => c.ToString()));
            TotalColumns = totalColumnNames != null ? new TotalColumns(_sizedColumns, totalColumnNames) : null;
        }

        public void WriteDataRow(IEnumerable<string> row, bool highlight)
        {
            _outputWriter.WriteLine(string.Format(_format, row.Select(s => (object)s).ToArray()), highlight);
        }

        public void WriteSeparatorRow(char separatorCharacter)
        {
            var line = string.Join(" ", _sizedColumns.Select(c => new string(separatorCharacter, c.Width)));
            _outputWriter.WriteLine(line, _highlightHeadings);
        }

        public void WriteHeaderRow()
        {
            _outputWriter.WriteLine(string.Join(" ", _sizedColumns.Select(s => (object)s.FormattedHeader).ToArray()), _highlightHeadings);
        }

        public void WriteTotalRow(string heading, double total)
        {
            if (TotalColumns == null)
            {
                return;
            }
            var finalTotalData = _sizedColumns.Select(s => "").ToList();
            finalTotalData[TotalColumns.HeaderColumn.DataIndex] = heading;
            finalTotalData[TotalColumns.TotalColumn.DataIndex] = total.ToString(CultureInfo.CurrentCulture);
            _outputWriter.WriteLine(string.Format(TotalColumns.TotalFormat, finalTotalData.Select(s => (object) s).ToArray()), _highlightHeadings);
        }
    }
}
