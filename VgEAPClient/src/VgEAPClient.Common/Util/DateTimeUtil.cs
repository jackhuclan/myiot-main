// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common
{
    public static class DateTimeUtil
    {
        public const double BEIJING_HOUR_OFFSET = 8.0;

        /// <summary>
        /// 注意这是UNIX-TIMESTAMP的最小值
        /// </summary>
        readonly static public DateTime BeginOf1970 = new DateTime(1970, 1, 1);

        /// <summary>
        /// 本项目中将其视为 有效时间值的上限（不可到达）
        /// </summary>
        readonly static public DateTime Future3000 = new DateTime(3000, 1, 1);


        //本项目中记录的有效时间都应该在下面两个值的范围内
        readonly static public DateTime BeginOf2000 = new DateTime(2000, 1, 1);
        readonly static DateTime BeginOf2999 = new DateTime(2999, 1, 1);


        public static bool IsValidTime(DateTime dt)
        {
            return dt > BeginOf2000 && dt < BeginOf2999;
        }

        /// <summary>
        /// 把分钟表达的时长变为 HH:mm:ss 的格式
        /// </summary>
        /// <param name="dur_min"></param>
        /// <returns></returns>
        public static string DurMinToHHMMSS(double dur_min)
        {
            string hour_part = ((long)dur_min / 60).ToString().PadLeft(2, '0');
            string min_part = ((long)dur_min % 60).ToString().PadLeft(2, '0');
            string sec_part = (((long)(dur_min * 60.0)) % 60).ToString().PadLeft(2, '0');

            return $"{hour_part}:{min_part}:{sec_part}";
        }


        public static DateTime UtcToLocalTime(DateTime utc)
        {
            TimeZoneInfo currentTimeZone = TimeZoneInfo.Local;
            return utc.Add(currentTimeZone.BaseUtcOffset);
        }

        public static DateTime GetDayBegin(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        public static string DateTimeToDBString(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }


        public static DateTime DBStringToDateTime(string dt_s, DateTime def_value)
        {
            try
            {
                return Convert.ToDateTime(dt_s);
            }
            catch
            {
                return def_value;
            }
        }

        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (long)(dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
        }


        public static DateTime UnixTimestampToDateTime(long unix_times_tamp, DateTime def_value)
        {
            if (unix_times_tamp <= 0)
            {
                return def_value;
            }

            return new DateTime(1970, 1, 1).AddSeconds((double)unix_times_tamp);
        }


        public static DateTime BeijingToUtcTime(DateTime bj_time)
        {
            bj_time.AddHours(-BEIJING_HOUR_OFFSET);
            return bj_time;
        }

        public static DateTime UtcToBeijingTime(DateTime utc_time)
        {
            utc_time.AddHours(BEIJING_HOUR_OFFSET);
            return utc_time;
        }

        public static bool IsAlmosEqual(DateTime t1, DateTime t2, double maxOffsetSec)
        {
            if (IsValidTime(t1) && IsValidTime(t2))
            {
                double sec = Math.Abs((t1 - t2).TotalSeconds);
                return sec < maxOffsetSec;
            }
            else
                return false;
        }
    }
}
