using System.Threading;
using System.Threading.Tasks;
using Antropov.ITBlog.DataAccess.CosmosDB.Abstractions;
using Antropov.ITBlog.Entities;
using Antropov.ITBlog.UseCases.Abstractions;

namespace Antropov.ITBlog.UseCases;

public class GetBlogPostUseCase : IGetBlogPostUseCase
{
	#region props

	private readonly IBlogPostDataAccessObject _blogPostDao;

	#endregion

	#region ctors

	public GetBlogPostUseCase(IBlogPostDataAccessObject blogPostDao)
	{
		_blogPostDao = blogPostDao;
	}

	#endregion

	public async Task<BlogPost?> Invoke(string blogPostId, CancellationToken cancellationToken) => 
		await _blogPostDao.GetBlogPost(blogPostId, cancellationToken);
}