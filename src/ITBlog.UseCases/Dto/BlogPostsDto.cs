using Antropov.ITBlog.Entities;

namespace Antropov.ITBlog.UseCases.Dto;

public class BlogPostsDto
{
	public BlogPost[] BlogPosts { get; set; } = null!;
	public int BlogPostsCount { get; set; }
}