using System;
namespace LCARS.CoreUi.Helpers
{
    /// <summary>
    /// Contains methods for handling and converting stardates
    /// </summary>
    /// <remarks>Previously in StardateLibrary.dll. Moved here for simplicity. Generally only the static methods are used.</remarks>
    public class StarDateMath
    {
        DateTime mydate;
        double mystardate;
        int @base = 2323;
        /// <summary>
        /// Create a new stardate object from a standard date
        /// </summary>
        /// <param name="newdate">Standard date</param>
        /// <remarks></remarks>
        public StarDateMath(DateTime newdate)
        {
            mydate = newdate;
            mystardate = getStardate(newdate);
        }
        /// <summary>
        /// Create a new stardate object from a decimal stardate
        /// </summary>
        /// <param name="newstardate">Decimal representation of a stardate.</param>
        /// <remarks>There's no validation for this, but for current dates it will be negative.</remarks>
        public StarDateMath(double newstardate)
        {
            mystardate = newstardate;
            mydate = GetEarthDate(this);
        }
        /// <summary>
        /// Returns the stardate as a string with two decimal places
        /// </summary>
        /// <returns>Approximate string representation of stardate</returns>
        /// <remarks>Saves you some work, and gives you the date as it would normally be spoken.</remarks>
        public override string ToString()
        {
            return StarDate.ToString("F2");
        }
        /// <summary>
        /// Returns a stardate from the given standard date, using the supplied datebase
        /// </summary>
        /// <param name="convertdate">Date to convert</param>
        /// <param name="datebase">Year to base stardate on. Generally 2323 is used.</param>
        /// <returns>Decimal representation of stardate.</returns>
        /// <remarks>One of the two functions you are likely to use.</remarks>
        public static double getStardate(DateTime convertdate, int datebase = 2323)
        {
            int x = 0;
            if (DateTime.IsLeapYear(convertdate.Year))
            {
                x = 366;
            }
            else
            {
                x = 365;
            }
            double earthdatetime = convertdate.Year + 1 / x * (convertdate.DayOfYear - 1 + convertdate.Hour / 24 + convertdate.Minute / 1440 + convertdate.Second / 86400);
            return 1000 * (earthdatetime - datebase);
        }
        /// <summary>
        /// Returns a standard date from the given stardate, using the supplied datebase
        /// </summary>
        /// <param name="convertdate">Stardate to be converted.</param>
        /// <param name="datebase">Datebase used for stardate.</param>
        /// <returns>Standard date equivalent of the stardate</returns>
        /// <remarks> If you used something other than 2323, set the datebase!</remarks>
        public static DateTime GetEarthDate(StarDateMath convertdate, int datebase = 2323)
        {
            double earthdatetime = convertdate.StarDate / 1000 + datebase;
            int myyear = (int)Math.Floor(earthdatetime);
            int x = 365;
            if (DateTime.IsLeapYear(myyear))
            {
                x = 366;
            }
            earthdatetime = (earthdatetime - myyear) * x;
            int myday = (int)Math.Floor(earthdatetime);
            earthdatetime = (earthdatetime - myday) * 24;
            int myhour = (int)Math.Floor(earthdatetime);
            earthdatetime = (earthdatetime - myhour) * 60;
            int myminute = (int)Math.Floor(earthdatetime);
            earthdatetime = (earthdatetime - myminute) * 60;
            int mysecond = (int)Math.Floor(earthdatetime);
            int month = 1;

            DoTransformation(x, ref myday, ref month);
            DateTime mynewdate = new DateTime(myyear, month, myday, myhour, myminute, mysecond);
            return mynewdate;
        }

        private static void DoTransformation(int x, ref int myday, ref int month)
        {
            if ((myday - 31 <= 0)) return;
            myday -= 31;
            month += 1;
            if ((myday - (28 + x - 365) <= 0)) return;
            myday -= (28 + x - 365);
            month += 1;
            if ((myday - 31 <= 0)) return;
            myday -= 31;
            month += 1;
            if ((myday - 30 <= 0)) return;
            myday -= 30;
            month += 1;
            if ((myday - 31 <= 0)) return;
            myday -= 30;
            month += 1;
            if ((myday - 30 <= 0)) return;
            myday -= 30;
            month += 1;
            if ((myday - 31 <= 0)) return;
            myday -= 30;
            month += 1;
            if ((myday - 31 <= 0)) return;
            myday -= 30;
            month += 1;
            if ((myday - 30 <= 0)) return;
            myday -= 30;
            month += 1;
            if ((myday - 31 <= 0)) return;
            myday -= 30;
            month += 1;
            if ((myday - 30 <= 0)) return;
            myday -= 30;
            month += 1;
            if ((myday - 31 <= 0)) return;
            myday -= 30;
            month += 1;
        }

        /// <summary>
        /// Returns a standard date from the given stardate, using the supplied datebase
        /// </summary>
        /// <param name="convertdate">Decimal representation of the stardate</param>
        /// <param name="datebase">Datebase used for stardate.</param>
        /// <returns>Standard date equivalent of the stardate</returns>
        /// <remarks>Stub to call overloaded function. No savings to use one or the other.</remarks>
        public static DateTime GetEarthDate(double convertdate, int datebase = 2323)
        {
            return GetEarthDate(new StarDateMath(convertdate), datebase);
        }
        /// <summary>
        /// Returns the stardate stored by this object as a decimal
        /// </summary>
        /// <value>Decimal representation of stardate.</value>
        /// <returns>Decimal representation of stardate.</returns>
        /// <remarks></remarks>
        public double StarDate
        {
            get { return mystardate; }
            set
            {
                mystardate = value;
                mydate = GetEarthDate(this);
            }
        }
        /// <summary>
        /// Returns the date stored by this object as a date
        /// </summary>
        /// <value>Standard date</value>
        /// <returns>Standard date</returns>
        /// <remarks></remarks>
        public DateTime EarthDate
        {
            get { return mydate; }
            set
            {
                mydate = value;
                mystardate = getStardate(value);
            }
        }
        /// <summary>
        /// Determines if the two stardates are equal
        /// </summary>
        /// <param name="obj">Stardate to be compared</param>
        /// <returns>True if both are stardates and are equal</returns>
        /// <remarks></remarks>
        public override bool Equals(object obj)
        {
            if (obj.GetType().Equals(GetType()))
            {
                if (this.StarDate == ((StarDateMath)obj).StarDate)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
