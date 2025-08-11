using System.Text.Json;

namespace Equinox.Models.Infrastructure
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session,
            string key, T value)
        {
            var valuse = JsonSerializer.Serialize(value);
            session.SetString(key, valuse);
        }

        public static T? GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return string.IsNullOrEmpty(value) ? default :
                JsonSerializer.Deserialize<T>(value);
        }
    }
}
