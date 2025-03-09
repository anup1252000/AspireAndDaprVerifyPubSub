using AspireAndDaprVerify.Actors;
using Dapr.Actors.Client;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDaprClient();
builder.Services.AddActors(options =>
{
    options.Actors.RegisterActor<SampleActor>();
    // options.Actors.RegisterActor<Actor2>();
});
builder.Services.AddSingleton<ActorProxyFactory>();
var app = builder.Build();
app.UseCloudEvents();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseAuthorization();
app.MapActorsHandlers();

app.MapControllers();
app.MapSubscribeHandler();
app.Run();
