using Senior2.Api.Models;

namespace Senior2.Api.Extensions
{
    public static class AdvertisementQueryExtensions
    {
        public static IQueryable<Advertisement> Active(this IQueryable<Advertisement> query, DateTimeOffset? nowUtc = null)
        {
            var now = nowUtc ?? DateTimeOffset.UtcNow;

            return query.Where(a =>
                a.Status == AdvertisementStatus.Approved &&
                a.StartDateUtc <= now &&
                a.EndDateUtc >= now);
        }
    }
}