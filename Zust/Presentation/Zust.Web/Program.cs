using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Zust.Application;
using Zust.Infrastructure;
using Zust.Persistence;
using Zust.Web.Hubs;

var builder = WebApplication.CreateBuilder(args);
var config = new MapperConfiguration(cfg => {
    cfg.AddMaps(typeof(Program));
    cfg.AddProfile<MappingProfile>();
});

IMapper mapper = config.CreateMapper();

builder.Services.AddSingleton(mapper);

builder.Services.AddPersistanceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation().AddNToastNotifyToastr().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseNToastNotify();


app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<ChatHub>("/chatHub");
app.Run();
