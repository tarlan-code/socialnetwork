using Microsoft.EntityFrameworkCore;
using Zust.Application.Abstractions.Services;
using Zust.Domain.Entities;

namespace Zust.Persistence.Services
{
    public class BirthdayService : IBirthdayService
    {

        public async Task<List<AppUser>> ComingBirthdays(List<AppUser> friends)
        {
            List<AppUser> usersList = new();
            foreach (var user in friends)
            {
                DateTime ThisYearbirthday = new DateTime(DateTime.Now.Year, user.Birthday.Month, user.Birthday.Day);
                if (ThisYearbirthday < DateTime.Now)
                {
                    ThisYearbirthday = ThisYearbirthday.AddYears(1);
                }

                int days = (ThisYearbirthday - DateTime.Now).Days;

                if (days < 5)
                {
                    usersList.Add(user);
                }
            }

            return usersList;
        }

        public async Task<List<AppUser>> TodayBirthdays(List<AppUser> friends)
        {
            return friends.Where(u=>u.Birthday == new DateTime(u.Birthday.Year, DateTime.Now.Month, DateTime.Now.Day)).ToList();
        }
    }
}
