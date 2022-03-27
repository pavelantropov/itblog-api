using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Antropov.ITBlog.Entities;

namespace Antropov.ITBlog.DataAccess.CosmosDB.Abstractions;

public interface IBlogPostDataAccessObject
{
	Task<IReadOnlyCollection<BlogPost>> GetBlogPosts(
		Func<IQueryable<BlogPost>, IQueryable<BlogPost>> applyQuery,
		CancellationToken cancellation = default);

	Task<BlogPost?> GetBlogPost(string id, CancellationToken cancellation = default);
}