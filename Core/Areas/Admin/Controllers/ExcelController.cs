using BusinessLayer.Concrete;
using ClosedXML.Excel;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace Core.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class ExcelController : Controller
	{
		private readonly BlogManager _blogManager = new(new EfBlogRepository());
		private readonly WriterManager _writerManager = new(new EfWriterRepository());
		private readonly CommentManager _commentManager = new(new EfCommentRepository());

		public IActionResult BlogListToExcel()
		{
			return View();
		}

		public IActionResult ExportDynamicBlogToExcel()
		{
			using var workbook = new XLWorkbook();
			var worksheet = workbook.Worksheets.Add("Danh sách Blog");
			worksheet.Cell(1, 1).Value = "Blog ID";
			worksheet.Cell(1, 2).Value = "Tiêu đề Blog";
			worksheet.Cell(1, 3).Value = "Nội dung Blog";
			worksheet.Cell(1, 4).Value = "Ngày tạo Blog";
			worksheet.Cell(1, 5).Value = "Danh mục Blog";
			worksheet.Cell(1, 6).Value = "Tác giả Blog";

			int blogRowCount = 2;

			var blogs = _blogManager.GetDetailedBlogList();

			foreach (var blog in blogs)
			{
				worksheet.Cell(blogRowCount, 1).Value = blog.BlogID;
				worksheet.Cell(blogRowCount, 2).Value = blog.BlogTitle;
				worksheet.Cell(blogRowCount, 3).Value = blog.BlogContent;
				worksheet.Cell(blogRowCount, 4).Value = blog.BlogCreatedAt;
				worksheet.Cell(blogRowCount, 5).Value = blog.Category.CategoryName;
				worksheet.Cell(blogRowCount, 6).Value = blog.Writer.WriterNameSurname;
				blogRowCount++;
			}

			using var stream = new MemoryStream();
			workbook.SaveAs(stream);
			var content = stream.ToArray();
			var date = DateTime.Now.ToString();

			return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Danh sách Blog - {date}.xlsx");
		}

		public IActionResult ExportDynamicUserToExcel()
		{
			using var workbook = new XLWorkbook();
			var worksheet = workbook.Worksheets.Add("Danh sách Người dùng");
			worksheet.Cell(1, 1).Value = "Người dùng ID";
			worksheet.Cell(1, 2).Value = "Tên đăng nhập";
			worksheet.Cell(1, 3).Value = "Tên đầy đủ";
			worksheet.Cell(1, 4).Value = "Email";
			worksheet.Cell(1, 5).Value = "Thông tin về người dùng";
			worksheet.Cell(1, 6).Value = "Đường dẫn hình ảnh người dùng";

			int writerRowCount = 2;

			var writers = _writerManager.GetEntities();

			foreach (var writer in writers)
			{
				worksheet.Cell(writerRowCount, 1).Value = writer.WriterID;
				worksheet.Cell(writerRowCount, 2).Value = writer.WriterUserName;
				worksheet.Cell(writerRowCount, 3).Value = writer.WriterNameSurname;
				worksheet.Cell(writerRowCount, 4).Value = writer.WriterMail;
				worksheet.Cell(writerRowCount, 5).Value = writer.WriterAbout;
				worksheet.Cell(writerRowCount, 6).Value = writer.WriterImage;
				writerRowCount++;
			}

			using var stream = new MemoryStream();
			workbook.SaveAs(stream);
			var content = stream.ToArray();
			var date = DateTime.Now.ToString();

			return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Danh sách Người dùng - {date}.xlsx");
		}

		public IActionResult ExportDynamicCommentToExcel()
		{
			using var workbook = new XLWorkbook();
			var worksheet = workbook.Worksheets.Add("Danh sách Bình luận");
			worksheet.Cell(1, 1).Value = "Bình luận ID";
			worksheet.Cell(1, 2).Value = "Tên người dùng";
			worksheet.Cell(1, 3).Value = "Tiêu đề Bình luận";
			worksheet.Cell(1, 4).Value = "Nội dung Bình luận";
			worksheet.Cell(1, 5).Value = "Tiêu đề Blog liên quan";
			worksheet.Cell(1, 6).Value = "Tác giả Blog liên quan";

			int commentRowCount = 2;

			var comments = _commentManager.GetCommentsWithBlogAndWriter();

			foreach (var comment in comments)
			{
				worksheet.Cell(commentRowCount, 1).Value = comment.CommentID;
				worksheet.Cell(commentRowCount, 2).Value = comment.CommentUserName;
				worksheet.Cell(commentRowCount, 3).Value = comment.CommentTitle;
				worksheet.Cell(commentRowCount, 4).Value = comment.CommentContent;
				worksheet.Cell(commentRowCount, 5).Value = comment.Blog.BlogTitle;
				worksheet.Cell(commentRowCount, 6).Value = comment.Blog.Writer.WriterNameSurname;
				commentRowCount++;
			}

			using var stream = new MemoryStream();
			workbook.SaveAs(stream);
			var content = stream.ToArray();
			var date = DateTime.Now.ToString();

			return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Danh sách Bình luận - {date}.xlsx");
		}
	}
}
