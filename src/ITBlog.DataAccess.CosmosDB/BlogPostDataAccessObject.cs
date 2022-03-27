using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Antropov.ITBlog.DataAccess.CosmosDB.Abstractions;
using Antropov.ITBlog.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Antropov.ITBlog.DataAccess.CosmosDB;

public class BlogPostDataAccessObject : IBlogPostDataAccessObject
{
	private readonly CosmosClient _cosmosClient;
	private readonly CosmosDbOptions _options;
	private readonly ILogger _logger;

	public BlogPostDataAccessObject(
		CosmosClient cosmosClient,
		IOptions<CosmosDbOptions> options,
		ILogger<BlogPostDataAccessObject> logger)
	{
		_cosmosClient = cosmosClient;
		_options = options.Value;
		_logger = logger;
	}

	public async Task<IReadOnlyCollection<BlogPost>> GetBlogPosts(
		Func<IQueryable<BlogPost>, IQueryable<BlogPost>> applyQuery,
		CancellationToken cancellation = default)
	{
		Container container = _cosmosClient.GetContainer(_options.DatabaseName, _options.BlogPostsContainerName);
		var blogPosts = new List<BlogPost>();
		
		using FeedIterator<BlogPost> it = applyQuery(container.GetItemLinqQueryable<BlogPost>()).ToFeedIterator();
		while (it.HasMoreResults)
		{
			blogPosts.AddRange(await it.ReadNextAsync(cancellation));
		}

		return blogPosts;
	}

	public async Task<BlogPost?> GetBlogPost(string id, CancellationToken cancellation = default)
	{
		var container = _cosmosClient.GetContainer(_options.DatabaseName, _options.BlogPostsContainerName);
		return await container.ReadItemAsync<BlogPost?>(id, new PartitionKey(id), cancellationToken: cancellation);
	}
}