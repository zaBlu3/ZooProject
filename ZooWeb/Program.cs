
var builder = WebApplication.CreateBuilder(args);
#region configiration
string apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? throw new InvalidOperationException("Api settings string 'BaseUrl' not found."); ;
string zooConnectionString = builder.Configuration.GetConnectionString("ZooConnection") ?? throw new InvalidOperationException("Connection string 'ZooConnection' not found.");
string userConnectionString = builder.Configuration.GetConnectionString("UsersConnection") ?? throw new InvalidOperationException("Connection string 'UsersConnection' not found.");

#endregion
#region httpClients

builder.Services.AddHttpClient("CategoriesApiClient", client =>
{
	client.BaseAddress = new Uri(apiBaseUrl + "/category/");
});
builder.Services.AddHttpClient("AnimalsApiClient", client =>
{
	client.BaseAddress = new Uri(apiBaseUrl + "/animal/");
});
#endregion

#region DBs
builder.Services.AddDbContext<ZooContext>(options => options.UseLazyLoadingProxies().UseSqlServer(zooConnectionString));
builder.Services.AddDbContext<UsersContext>(options => options.UseSqlServer(userConnectionString));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireDigit = false;
	options.Password.RequireLowercase = false;
	options.Password.RequiredLength = 4;
})
	.AddEntityFrameworkStores<UsersContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Identity/Account/Login";
	options.LogoutPath = "/Identity/Account/Logout";
	options.AccessDeniedPath = "/Identity/Account/AccessDenied";

});
#endregion

#region injections

builder.Services.AddScoped<IEmailSender, ZooWeb.EmailSender>();
builder.Services.AddScoped<IRepository<Animal>, AnimalRepo>();
builder.Services.AddScoped<IRepository<Category>, CategoryRepo>();
builder.Services.AddScoped<IRepository<Comment>, CommentRepo>();
builder.Services.AddScoped<IRepository<Image<Animal>>, AnimalImageRepo>();
builder.Services.AddScoped<AnimalsService>();
builder.Services.AddScoped<CategoryService>();

#endregion


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();




if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");

	app.UseHsts();
}

using (var scope = app.Services.CreateScope())

{
	var ctx = scope.ServiceProvider.GetRequiredService<ZooContext>();
	ctx.Database.EnsureDeleted();
	ctx.Database.EnsureCreated();
	var ctx1 = scope.ServiceProvider.GetRequiredService<UsersContext>();
	ctx1.Database.EnsureDeleted();
	ctx1.Database.EnsureCreated();
	var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>()!;
	var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>()!;
	var signInManager = scope.ServiceProvider.GetService<SignInManager<IdentityUser>>()!;
	if (!roleManager.RoleExistsAsync(Roles.Admin).Result)
	{
		roleManager.CreateAsync(new IdentityRole(Roles.Admin)).Wait();
		roleManager.CreateAsync(new IdentityRole(Roles.User)).Wait();
	}
	if (userManager.FindByNameAsync("admin").Result == null)
	{
		var adminUser = new ZooUser { UserName = "admin", Email = "admin@admin.com" };
		var regularUser = new ZooUser { UserName = "user", Email = "user@user.com" };
		var resultAdmin = userManager.CreateAsync(adminUser, "1234").Result;
		var resultUser = userManager.CreateAsync(regularUser, "1234").Result;

		if (resultAdmin.Succeeded && resultUser.Succeeded)
		{
			userManager.AddToRoleAsync(adminUser, Roles.Admin).Wait();
			userManager.AddToRoleAsync(regularUser, Roles.User).Wait();
		}

	}



}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();




app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id=1}");
app.MapRazorPages();
app.Run();

