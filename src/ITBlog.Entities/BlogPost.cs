using System;

namespace Antropov.ITBlog.Entities;

public class BlogPost
{
	public string Id { get; set; } = null!;
	public string Title { get; set; } = null!;
	public string Body { get; set; } = null!;
	public DateTime? CreationDate { get; set; }
	public DateTime? LastUpdateDate { get; set; }
	public string[] Tags { get; set; } = null!;
	public string Author { get; set; } = null!;
	public bool IsDeleted { get; set; }
}