using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Zust.Application.Abstractions.Services;
using Zust.Application.UnitOfWorks;
using Zust.Domain.Entities;
using Zust.Persistence.Contexts;
using Zust.Persistence.Services;
using Zust.Persistence.UnitOfWorks;
namespace Zust.Persistence
{
    public static class ServiceRegistration
    {
        

        public static void AddPersistanceServices(this IServiceCollection service)
        {

            service.AddAuthentication().AddGoogle(opt =>
            {
                opt.ClientId = Configuration.GoogleClientId;
                opt.ClientSecret = Configuration.GoogleSecret;
              
            });

            service.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.ConnectionString);
            });

            service.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = false;
                opt.SignIn.RequireConfirmedEmail = true;
                opt.User.RequireUniqueEmail = true;
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._";
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
            service.AddScoped<IUnitOfWork, UnitOfwork>();
            service.AddScoped<IGenderService, GenderService>();
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<IPathService, PathService>();
            service.AddScoped<ICountryService, CountryService>();
            service.AddScoped<ICityService, CityService>();
            service.AddScoped<IRelationService, RelationService>();
            service.AddScoped<IPrivacySettingService, PrivacySettingService>();
            service.AddScoped<IFriendService, FriendService>();
            service.AddScoped<IPostService, PostService>();
            service.AddScoped<IReactionService, ReactionService>();
            service.AddScoped<ICommentService, CommentService>();
            service.AddScoped<IMessageService, MessageService>();
            service.AddScoped<INotificationService, NotificationService>();
            service.AddScoped<IBirthdayService, BirthdayService>();
            service.AddScoped<IEventService, EventService>();
            service.AddScoped<ISupportService, SupportService>();


        }
    }
}
