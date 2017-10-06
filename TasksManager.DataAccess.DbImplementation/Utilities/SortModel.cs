using System.Linq;

namespace TasksManager.DataAccess.DbImplementation.Utilities
{
    internal class SortModel
    {
        internal string Property { get; set; }
        internal bool Descending { get; set; }

        internal static SortModel CreateFromString(string source)
        {
            if (string.IsNullOrWhiteSpace(source)) return null;

            SortModel result = new SortModel();
            char firstChar = source.First();

            result.Descending = firstChar.Equals('-');
            result.Property = firstChar.Equals('-') || firstChar.Equals('+') ? source.Substring(1) : source;

            return result;
        }
    }
}
