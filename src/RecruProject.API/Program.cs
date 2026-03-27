using RecruProject.API.Configuration;
using RecruProject.API.Mappings;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.Register();
builder.Services.AddAutoMapper(_ => {}, typeof(OrderProfile));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();