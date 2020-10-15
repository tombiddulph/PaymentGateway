using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace PaymentGateway.Api
{
    public static class Extensions
    {
        public static string GetLoggedInUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            first = list.Count > 0 ? list[0] : default;
            second = list.Count > 1 ? list[1] : default;
        }
    }
}