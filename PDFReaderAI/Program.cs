using Microsoft.EntityFrameworkCore;
using PDFReaderAI.Data;
using PDFReaderAI.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add services to the container.


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<ChatDbContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
var AllowedOrigins = "AllowAllOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowedOrigins,
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IChatAIService, ChatAIService>();
builder.Services
       .AddHttpClient<IChatAIService, ChatAIService>(client =>
       {
           client.BaseAddress = new Uri("http://localhost:11434");
       });


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "PDFReaderAI",
        Version = "v1"
    });
});
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PDFReaderAI v1"));
    app.UseSwagger();
}
app.UseCors(AllowedOrigins);



app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
