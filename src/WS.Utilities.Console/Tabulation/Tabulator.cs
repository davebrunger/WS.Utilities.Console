using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace WS.Utilities.Console.Tabulation
{
    public static class Tabulator
    {
        public static void Tabulate<T>(this IEnumerable<T> source, IOutputWriter outputWriter, bool headerRow, int separatorRow, TotalColumnNames totalColumnNames = null)
        {
            var tabulator = new Tabulator<T>(headerRow, separatorRow, totalColumnNames);
            tabulator.Tabulate(source, outputWriter);
        }

        public static void Tabulate(this IEnumerable<IEnumerable<string>> source, IEnumerable<Column> columnDefinitions, IOutputWriter outputWriter, bool headerRow, int separatorRow, TotalColumnNames totalColumnNames = null)
        {
            var tabulator = new BasicTabulator(columnDefinitions, headerRow, separatorRow, totalColumnNames);
            tabulator.Tabulate(source, outputWriter);
        }
    }

    internal class Tabulator<T>
    {
        private readonly BasicTabulator _tabulator;
        private readonly ImmutableList<PropertyInfo> _properties;

        public Tabulator(bool headerRow, int separatorRow, TotalColumnNames totalColumnNames = null)
        {
            _properties = typeof(T).GetTypeInfo()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public).ToImmutableList();
            var columns = _properties
                .Select(
                    (p, i) =>
                            new Column(i, p.Name, GetDefaultAlignment(p.PropertyType), GetDefaultFormat(p.PropertyType)));
            _tabulator = new BasicTabulator(columns, headerRow, separatorRow, totalColumnNames);
        }

        public void Tabulate(IEnumerable<T> data, IOutputWriter outputWriter)
        {
            var dataAsStrings = data.Select(item => _properties.Select(p => p.GetValue(item)?.ToString() ?? "").ToList());
            _tabulator.Tabulate(dataAsStrings, outputWriter);
        }

        private static string GetDefaultFormat(Type propertyType)
        {
            if ((propertyType == typeof(decimal)) || (propertyType == typeof(decimal?)))
            {
                return "C";
            }
            return null;
        }

        private static ColumnAlignment GetDefaultAlignment(Type propertyType)
        {
            if ((propertyType == typeof(int)) || (propertyType == typeof(decimal)) || (propertyType == typeof(decimal?)))
            {
                return ColumnAlignment.Right;
            }
            return ColumnAlignment.Left;
        }
    }
}
