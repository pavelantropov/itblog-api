using System.Threading;
using System.Threading.Tasks;
using Antropov.ITBlog.UseCases.Dto;

namespace Antropov.ITBlog.UseCases.Abstractions;

public interface IGetListOfBlogPostsUseCase
{
	Task<BlogPostsDto> Invoke(
		string title,
		CancellationToken cancellationToken
		);
}