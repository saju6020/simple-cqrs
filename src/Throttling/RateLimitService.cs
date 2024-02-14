namespace Platform.Infrastructure.Throttling
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AspNetCoreRateLimit;

    public class RateLimitService : IRateLimitService
    {
        private readonly IRateLimitProcessor processor;
        private readonly ClientRateLimitOptions options;
        private readonly IRateLimitConfiguration config;

        public RateLimitService(
            ClientRateLimitOptions options,
            IRateLimitProcessor processor,
            IRateLimitConfiguration config)
        {
            this.options = options;
            this.processor = processor;
            this.config = config;
            this.config.RegisterResolvers();
        }

        public async Task<RateLimitResponse> IsAllowedToProceed(string clientId)
        {
            // compute identity from request
            var identity = new ClientRequestIdentity()
            {
                ClientId = clientId,
            };

            RateLimitResponse rateLimitResponse = new RateLimitResponse()
            {
                IsAllowedToProceed = true,
            };

            var rules = await this.processor.GetMatchingRulesAsync(identity, new System.Threading.CancellationToken(false));

            var rulesDict = new Dictionary<RateLimitRule, RateLimitCounter>();

            foreach (var rule in rules)
            {
                // increment counter
                var rateLimitCounter = await this.processor.ProcessRequestAsync(identity, rule, new System.Threading.CancellationToken(false));

                if (rule.Limit > 0)
                {
                    // check if key expired
                    if (rateLimitCounter.Timestamp + rule.PeriodTimespan.Value < DateTime.UtcNow)
                    {
                        continue;
                    }

                    // check if limit is reached
                    if (rateLimitCounter.Count > rule.Limit)
                    {
                        //compute retry after value
                        var retryAfter = rateLimitCounter.Timestamp.RetryAfterFrom(rule);

                        // break execution
                        rateLimitResponse.Message = this.ReturnQuotaExceededResponse(rule, retryAfter);
                        rateLimitResponse.IsAllowedToProceed = false;

                        return rateLimitResponse;
                    }
                }

                rulesDict.Add(rule, rateLimitCounter);
            }

            return rateLimitResponse;
        }

        public string ReturnQuotaExceededResponse(RateLimitRule rule, string retryAfter)
        {
            //Use Endpoint QuotaExceededResponse
            if (rule.QuotaExceededResponse != null)
            {
                this.options.QuotaExceededResponse = rule.QuotaExceededResponse;
            }

            return string.Format(
                this.options.QuotaExceededResponse?.Content ??
                this.options.QuotaExceededMessage ??
                "API calls quota exceeded! maximum admitted {0} per {1}.", rule.Limit, rule.Period, retryAfter);
        }
    }
}