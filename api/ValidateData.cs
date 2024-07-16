using System.Collections.Generic;
using System.Text.Json;
namespace api
{
    public static class ValidateData
    {
        public static bool IsUuid(string uuid)
        {
            return Guid.TryParse(uuid, out _);
        }

        public static bool IsInList<T>(T item, List<T> list)
        {
            int pos = list.IndexOf(item);
            return (pos != -1);
        }

        public static void PrintJson<T>(T obj)
        {
            var json = JsonSerializer.Serialize(obj);
            Console.WriteLine(json);
        }
    }
}
