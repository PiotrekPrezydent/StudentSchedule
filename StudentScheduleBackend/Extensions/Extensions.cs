using System.Reflection;

namespace StudentScheduleBackend.Extensions
{
    public static class Extensions
    {
        public static IEnumerable<T> PanDa5ZaTenSuperFilter<T>(this ICollection<T> collection, List<KeyValuePair<string,object>> filters)
        {
            return collection.Where(e =>
            {
                foreach (var filter in filters)
                {
                    PropertyInfo prop = e.GetType().GetProperty(filter.Key);
                    if (prop == null)
                        return false;

                    var value = prop.GetValue(e);

                    if (!object.Equals(value,filter.Value))
                        return false;
                }
                return true;
            });
        }
    }
}
