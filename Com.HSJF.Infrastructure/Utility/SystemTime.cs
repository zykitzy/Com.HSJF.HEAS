namespace Com.HSJF.Infrastructure
{
    using System;

    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.UtcNow;
    }
}