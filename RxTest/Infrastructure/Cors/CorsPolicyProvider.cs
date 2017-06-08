using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace RxTest.Infrastructure.Cors
{
    public class CorsPolicyProvider : ICorsPolicyProvider
    {
        public async Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var policy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true,
                SupportsCredentials = true
            };

            if (request.Headers.Referrer != null)
            {
                var origin = request.Headers.Referrer.Scheme + "://" + request.Headers.Referrer.Authority;
                policy.Origins.Add(origin);
            }
            return policy;
        }
    }
}