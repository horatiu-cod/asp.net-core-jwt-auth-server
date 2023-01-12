using AuthServerApi.Models;
using AuthServerApi.Services;

var builder = WebApplication.CreateBuilder(args);

var authenticationConfiguration = new AuthenticationConfiguration();

builder.Configuration.Bind("Authentication", authenticationConfiguration);

// Add services to the container.
builder.Services.RegisterServices();
builder.Services.AddSingleton(authenticationConfiguration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.ConfigureSwagger();

app.UseHttpsRedirection();

app.MapAuthEndpoints();

app.Run();
