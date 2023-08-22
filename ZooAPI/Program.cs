
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddXmlDataContractSerializerFormatters();

builder.Services.AddCors(p=> p.AddPolicy("LocalhostCorsPolicy",builder =>
{
    builder.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback).AllowAnyHeader().AllowAnyMethod();

}));builder.Services.AddCors(p=> p.AddPolicy("CorsPolicy",builder =>
{
    builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().AllowCredentials().WithOrigins("https://localhost:7016");


}));

builder.Services.AddAutoMapper(typeof(AnimalMappingProfile),typeof(CategoryMappingProfile), typeof(CommentMappingProfile));
string connectionString = builder.Configuration["ZooConnection"]!;
builder.Services.AddDbContext<ZooContext>(options => options.UseLazyLoadingProxies().UseSqlServer(connectionString));
builder.Services.AddScoped<IRepository<Category>, CategoryRepo>();
builder.Services.AddScoped<IRepository<Animal>, AnimalRepo>();
builder.Services.AddScoped<IRepository<Comment>, CommentRepo>();
builder.Services.AddScoped<IRepository<Image<Animal>>, AnimalImageRepo>();
builder.Services.AddScoped<AnimalsService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CommentService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.UseCors("CorsPolicy");
app.MapHub<CommentHub>("/hubs/comment");
app.MapControllers();


app.Run();
