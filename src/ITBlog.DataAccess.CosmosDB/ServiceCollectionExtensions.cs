using System;
using Antropov.ITBlog.DataAccess.CosmosDB.Abstractions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Antropov.ITBlog.DataAccess.CosmosDB;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddCosmosDb(this IServiceCollection sc)
	{
		sc.ConfigureCosmosDbOptions();
		sc.AddSingleton<CosmosClient>(ImplementationFactory);

		sc.AddScoped<IBlogPostDataAccessObject, BlogPostDataAccessObject>();

		return sc;
	}

	private static IServiceCollection ConfigureCosmosDbOptions(this IServiceCollection sc)
	{
		sc.Configure<CosmosDbOptions>(c =>
		{
			var connectionString = Environment.GetEnvironmentVariable(
				"COSMOSDB_ConnectionString",
				EnvironmentVariableTarget.Process);
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				throw new ArgumentNullException(nameof(connectionString), "Please specify the CosmosDB connection string.");
			}

			var databaseName = Environment.GetEnvironmentVariable(
				"COSMOSDB_DatabaseName",
				EnvironmentVariableTarget.Process);
			if (string.IsNullOrWhiteSpace(databaseName))
			{
				throw new ArgumentNullException(nameof(databaseName), "Please specify the CosmosDB database name.");
			}

			var blogPostsContainerName = Environment.GetEnvironmentVariable(
				"COSMOSDB_BlogPostsContainerName",
				EnvironmentVariableTarget.Process);
			if (string.IsNullOrWhiteSpace(blogPostsContainerName))
			{
				throw new ArgumentNullException(nameof(blogPostsContainerName), "Please specify the CosmosDB blog posts container name.");
			}

			c.ConnectionString = connectionString;
			c.DatabaseName = databaseName;
			c.BlogPostsContainerName = blogPostsContainerName;
		});
		return sc;
	}

	private static readonly Func<IServiceProvider, CosmosClient> ImplementationFactory = sp =>
	{
		var cosmosOptions = sp.GetRequiredService<IOptions<CosmosDbOptions>>().Value;
		var configurationBuilder = new CosmosClientBuilder(cosmosOptions.ConnectionString)
			.WithBulkExecution(false)
			.WithThrottlingRetryOptions(TimeSpan.FromSeconds(30), 9)
			.WithSerializerOptions(new CosmosSerializationOptions
			{
				PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
			});

		return configurationBuilder.Build();
	};
}