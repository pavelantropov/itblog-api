using System;
using System.Linq;
using System.Threading;
using Antropov.ITBlog.DataAccess.CosmosDB;
using Antropov.ITBlog.UseCases;
using Antropov.ITBlog.UseCases.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCosmosDb();
builder.Services.AddScoped<IGetListOfBlogPostsUseCase, GetListOfBlogPostsUseCase>();
builder.Services.AddScoped<IGetBlogPostUseCase, GetBlogPostUseCase>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "Antropov IT Blog",
		Description = "An educational blog by Pavel Antropov on full-stack .NET development",
		Contact = new OpenApiContact
		{
			Name = "Pavel Antropov",
			Url = new Uri("https://www.linkedin.com/in/pavelantropov/")
		},
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
	options.SwaggerEndpoint("./swagger/v1/swagger.json", "Antropov IT Blog");
	options.RoutePrefix = string.Empty;
});

// app.UseHttpsRedirection();

var summaries = new[]
{
	"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
	var forecast = Enumerable.Range(1, 5).Select(index =>
	   new WeatherForecast
	   (
		   DateTime.Now.AddDays(index),
		   Random.Shared.Next(-20, 55),
		   summaries[Random.Shared.Next(summaries.Length)]
	   ))
		.ToArray();
	return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet(
	"api/blogPosts",
	async ([FromQuery] string? title, IGetListOfBlogPostsUseCase useCase, CancellationToken cancellationToken) =>
	await useCase.Invoke(title, cancellationToken));
app.MapGet(
	"api/blogPosts/{blogPostId}",
	async ([FromRoute] string blogPostId, IGetBlogPostUseCase useCase, CancellationToken cancellationToken) =>
	await useCase.Invoke(blogPostId, cancellationToken));

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}