namespace Zust.Application.Extensions
{
    public static class CheckDate
    {
        public static string? CheckBirthday(this DateTime date)
        {
            if (DateTime.Now.AddYears(-13) < date)
                return "You do have to be a minimum of 13 years old to use the site";
            else if (DateTime.Now.AddYears(-150) > date)
                return "Wrong date of birth. Max age 150";
            return null;
        }
    }
}
