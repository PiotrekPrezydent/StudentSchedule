using System.Reflection;
using Microsoft.IdentityModel.Tokens;

namespace StudentScheduleBackend.Extensions
{
    public static class Extensions
    {
        public static IEnumerable<T> PanDa5ZaTenSuperFilter<T>(this ICollection<T> collection, List<KeyValuePair<string,string>> filters)
        {
            return collection.Where(e =>
            {
                foreach (var filter in filters)
                {
                    if (filter.Value.IsNullOrEmpty())
                        continue;
                    PropertyInfo prop = e.GetType().GetProperty(filter.Key)!;
                    if (prop == null)
                        return false;

                    string value = prop.GetValue(e)!.ToString()!;

                    if (value.IsNullOrEmpty())
                        continue;

                    if (value !=filter.Value)
                        return false;
                }
                return true;
            });
        }
    }
}
