using Microsoft.AspNetCore.Http;

namespace SportsStore.Infrastructure {
    public static class UrlExtensions {
        public static string PathAndQuery(this HttpRequest request) {
            return string.Concat(request.Path.Value, request.QueryString.Value);
        }
    }
}