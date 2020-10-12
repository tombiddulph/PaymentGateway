using System;

namespace PaymentGateway.Models
{
    public static class DateTimeProvider
    {
        public static Func<DateTime> Now = () => DateTime.Now;
        public static void SetDateTime(DateTime dateTime) => Now = () => dateTime;
        public static void ResetDateTime() => Now = () => DateTime.Now;
    }
}