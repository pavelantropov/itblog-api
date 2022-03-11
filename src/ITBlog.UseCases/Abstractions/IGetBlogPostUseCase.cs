using System.Threading;
using System.Threading.Tasks;
using Antropov.ITBlog.UseCases.Dto;

namespace Antropov.ITBlog.UseCases.Abstractions;

public interface IGetBlogPostUseCase
{
	Task<BlogPostDto> Invoke(
		string blogPostId,
		CancellationToken cancellationToken
	);
}