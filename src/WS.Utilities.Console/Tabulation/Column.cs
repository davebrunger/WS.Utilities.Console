using System;

namespace WS.Utilities.Console.Tabulation
{
    public class Column
    {

        public int DataIndex { get; }
        public string Name { get; }
        public ColumnAlignment ColumnAlignment { get; }
        public string Format { get; }
        public int Width { get; }

        public Column(int dataIndex, string name, ColumnAlignment columnAlignment, string format = null, int width = -1)
        {
            if (dataIndex < 0)
            {
                throw new ArgumentException("DataIndex must be 0 or greater", nameof(dataIndex));
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            DataIndex = dataIndex;
            Name = name;
            ColumnAlignment = columnAlignment;
            Format = format;
            Width = Math.Max(name.Length, width);
        }

        public override string ToString()
        {
            string alignment;
            switch (ColumnAlignment)
            {
                case ColumnAlignment.Left:
                    alignment = "-";
                    break;
                case ColumnAlignment.Right:
                    alignment = string.Empty;
                    break;
                default:
                    throw new InvalidOperationException($"Unrecognised alignment: {ColumnAlignment}");
            }
            var format = string.IsNullOrEmpty(Format) ? "" : $":{Format}";
            return $"{{{DataIndex},{alignment}{Width}{format}}}";
        }

        public string FormattedHeader
        {
            get
            {
                switch (ColumnAlignment)
                {
                    case ColumnAlignment.Left:
                        return Name.PadRight(Width);
                    case ColumnAlignment.Right:
                        return Name.PadLeft(Width);
                    default:
                        throw new InvalidOperationException($"Unrecognised alignment: {ColumnAlignment}");
                }
            }
        }

        public Column WithWidth(int width)
        {
            return new Column(DataIndex, Name, ColumnAlignment, Format, width);
        }
    }
}
