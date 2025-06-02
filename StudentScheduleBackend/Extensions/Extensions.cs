using System.Reflection;
using System.Text;
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

        //why the fuck this is not in base c#...
        public static string ElementsToString<T>(this ICollection<T> collection)
        {
            var msg = new StringBuilder();

            foreach (var item in collection)
            {
                if (item is System.Collections.DictionaryEntry entry)
                {
                    msg.AppendLine($"{entry.Key} - {entry.Value}");
                }
                else if (item != null && item.GetType().IsGenericType &&
                         item.GetType().GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
                {
                    dynamic dynItem = item;
                    msg.AppendLine($"{dynItem.Key} - {dynItem.Value}");
                }
                else
                {
                    msg.AppendLine(item?.ToString());
                }
            }

            return msg.ToString();
        }
    }
}
