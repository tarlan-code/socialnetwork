namespace Zust.Application.Extensions
{
    public static class DateExtension
    {
        public static string DateDifference(this DateTime date1, DateTime date2)
        {
            TimeSpan diff = date2.Subtract(date1); 
            int seconds = (int)diff.TotalSeconds; 

            if (seconds < 60)
            {
                return seconds + " Seconds"; 
            }
            else if (seconds < 60 * 60)
            {
                int minutes = seconds / 60;
                return minutes + " Minites"; 
            }
            else if (seconds < 60 * 60 * 24)
            {
                int hours = seconds / (60 * 60);
                return hours + " Hours"; 
            }
            else
            {
                int days = seconds / (60 * 60 * 24);
                return days + " Days"; 
            }
        }
    }
}
