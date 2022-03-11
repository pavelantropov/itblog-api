namespace Antropov.ITBlog.DataAccess.CosmosDB;

public class CosmosDbOptions
{
	public string ConnectionString { get; set; } = null!;
	public string DatabaseName { get; set; } = null!;
	public string BlogPostsContainerName { get; set; } = null!;
}