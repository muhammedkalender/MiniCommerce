using MiniCommerce.Api.App.Configurations;
using MiniCommerce.Api.App.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.MapAppDependencies()
    .AddAndMapFluentValidation()
    .ConfigureControllersAndJsonSettings()
    .AddSwaggerWithProjectContext()
    .AddBasicAuthentication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();
app.UseAuthorization();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();