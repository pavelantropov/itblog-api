using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Antropov.ITBlog.DataAccess.CosmosDB.Abstractions;
using Antropov.ITBlog.UseCases.Abstractions;
using Antropov.ITBlog.UseCases.Dto;

namespace Antropov.ITBlog.UseCases;

public class GetListOfBlogPostsUseCase : IGetListOfBlogPostsUseCase
{
	#region props

	private readonly IBlogPostDataAccessObject _blogPostDao;

	#endregion

	#region ctors

	public GetListOfBlogPostsUseCase(
		IBlogPostDataAccessObject blogPostDao
		)
	{
		_blogPostDao = blogPostDao;
	}

	#endregion

	public async Task<BlogPostsDto> Invoke(
		string title,
		CancellationToken cancellationToken)
	{
		var blogPosts = await _blogPostDao.GetBlogPosts(
			q =>  
				q.Where(bp => bp.Title.Contains(title, StringComparison.InvariantCultureIgnoreCase)),
			cancellationToken);

		var totalCount = blogPosts.Count;

		return new BlogPostsDto
		{
			BlogPosts = blogPosts.ToArray(),
			BlogPostsCount = totalCount
		};
	}
}