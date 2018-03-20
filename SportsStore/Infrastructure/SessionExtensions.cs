using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SportsStore.Infrastructure {
    public static class SessionExtensions {
        public static void SetJson(this ISession session, string key, object value) {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetJson<T>(this ISession session, string key) {
            string value = session.GetString(key);
            if (!String.IsNullOrEmpty(value)) {
                return JsonConvert.DeserializeObject<T>(value);
            } else {
                return default(T);
            }
        }
    }
}