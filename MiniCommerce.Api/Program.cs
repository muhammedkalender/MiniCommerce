using MiniCommerce.Api.App.Configurations;
using MiniCommerce.Api.App.Middlewares;
using MiniCommerce.Infrastructure.Configurations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

SerilogConfiguration.ConfigureSerilogAsLogger(builder.Configuration);
builder.Host.UseSerilog();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.MapAppDependencies()
    .AddAndMapFluentValidation()
    .ConfigureControllersAndJsonSettings()
    .AddSwaggerWithProjectContext()
    .AddBasicAuthentication()
    .AddRedisAsCache(builder.Configuration)
    .AddPostgresAsDb(builder.Configuration)
    .AddRabbitAsQueue(builder.Configuration);

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

app.UseSerilogRequestLogging();

app.Run();