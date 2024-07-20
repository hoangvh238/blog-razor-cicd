using Core.Models;
using System.Threading.Tasks;

namespace Core.ExtensionService.BlogService
{
	public interface IBlogExtensionService
	{
		Task<string> AddBlogAsync(BlogAddViewModel model, string userId);
	}
}
