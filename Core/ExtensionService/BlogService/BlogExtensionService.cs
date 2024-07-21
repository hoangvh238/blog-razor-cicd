using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using Core.Models;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core.ExtensionService.BlogService
{
	public class BlogExtensionService : IBlogExtensionService
	{
		private readonly BlogManager _blogManager;
		private readonly WriterManager _writerManager;
		private readonly UserManager<User> _userManager;

		public BlogExtensionService(BlogManager blogManager, WriterManager writerManager, UserManager<User> userManager)
		{
			_blogManager = blogManager;
			_writerManager = writerManager;
			_userManager = userManager;
		}

		public async Task<string> AddBlogAsync(BlogAddViewModel model, string userId)
		{
			bool isJpg = string.Equals(model.BlogImage?.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase);
			bool isJpeg = string.Equals(model.BlogImage?.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase);
			bool isPng = string.Equals(model.BlogImage?.ContentType, "image/png", StringComparison.OrdinalIgnoreCase);

			if (model.BlogImage == null)
			{
				return "Lütfen bir profil resmi yükleyiniz.";
			}
			else if (!isJpg && !isJpeg && !isPng)
			{
				return "Lütfen geçerli bir profil resmi yükleyiniz.";
			}

			var writer = _writerManager.GetWriterBySession(userId);

			var extension = Path.GetExtension(model.BlogImage?.FileName);
			var newImageName = Guid.NewGuid() + extension;

			Blog blog = new()
			{
				BlogTitle = model.BlogTitle,
				BlogContent = model.BlogContent,
				BlogStatus = true,
				BlogCreatedAt = DateTime.Now,
				BlogImage = "/WriterBlogFiles/" + newImageName,
				CategoryID = model.CategoryID,
				WriterID = writer.WriterID,
			};

			BlogValidator blogValidator = new();
			ValidationResult result = blogValidator.Validate(blog);

			if (result.IsValid && model.BlogImage != null && (isJpg || isJpeg || isPng))
			{
				var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/WriterBlogFiles/");

				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}

				var location = Path.Combine(directory + newImageName);
				var stream = new FileStream(location, FileMode.Create);
				model.BlogImage.CopyTo(stream);
				stream.Close();

				_blogManager.AddEntity(blog);

				return "Blog added successfully.";
			}
			else
			{
				var errorMessage = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
				return errorMessage;
			}
		}
	}
}
