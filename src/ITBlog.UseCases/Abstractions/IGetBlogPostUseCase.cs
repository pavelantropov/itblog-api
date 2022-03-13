using System.Threading;
using System.Threading.Tasks;
using Antropov.ITBlog.Entities;

namespace Antropov.ITBlog.UseCases.Abstractions;

public interface IGetBlogPostUseCase
{
	Task<BlogPost?> Invoke(
		string blogPostId,
		CancellationToken cancellationToken
	);
}