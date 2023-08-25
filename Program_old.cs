//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using ActeAdministratif.Data;
//using Microsoft.AspNetCore.Identity;
//using Identity.Models;



//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<SNDIContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString") ?? throw new InvalidOperationException("Connection string 'SNDIContext' not found.")));


//builder.Services.AddDbContext<AuthContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString") ?? throw new InvalidOperationException("Connection string 'AuthContext' not found.")));

//builder.Services.AddDefaultIdentity<ActeAdministratif.Areas.Identity.Data.SNDIUser>(options => options.SignIn.RequireConfirmedAccount = false)
//    .AddEntityFrameworkStores<AuthContext>();





//builder.Services.AddDbContext<AppIdentityDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString") ?? throw new InvalidOperationException("Connection string 'AuthContext' not found.")));

///*uilder.Services.AddIdentity<SNDI.Areas.Identity.Data.SNDIUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
//*/

//// Add services to the container.
//builder.Services.AddControllersWithViews();

//builder.Services.AddRazorPages();
//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.Password.RequireUppercase = false;
//});

//var app = builder.Build();


//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();
//app.UseAuthentication(); ;

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();

//app.Run();



















//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using ActeAdministratif.Data;
//using Microsoft.AspNetCore.Identity;
//using Identity.Models;



//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<SNDIContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString") ?? throw new InvalidOperationException("Connection string 'SNDIContext' not found.")));


//builder.Services.AddDbContext<AuthContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString") ?? throw new InvalidOperationException("Connection string 'AuthContext' not found.")));

//builder.Services.AddDefaultIdentity<ActeAdministratif.Areas.Identity.Data.SNDIUser>(options => options.SignIn.RequireConfirmedAccount = false)
//    .AddEntityFrameworkStores<AuthContext>();





//builder.Services.AddDbContext<AppIdentityDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString") ?? throw new InvalidOperationException("Connection string 'AuthContext' not found.")));

///*uilder.Services.AddIdentity<SNDI.Areas.Identity.Data.SNDIUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
//*/

//// Add services to the container.
//builder.Services.AddControllersWithViews();

//builder.Services.AddRazorPages();
//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.Password.RequireUppercase = false;
//});

//var app = builder.Build();


//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();
//app.UseAuthentication();;

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();

//app.Run();
