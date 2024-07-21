using BusinessLayer.Concrete;
using ClosedXML.Excel;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

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
            var worksheet = workbook.Worksheets.Add("Danh Sách Blog");
            var currentRow = 1;

            worksheet.Cell(currentRow, 1).Value = "Mã Blog";
            worksheet.Cell(currentRow, 2).Value = "Tiêu Đề Blog";
            worksheet.Cell(currentRow, 3).Value = "Nội Dung Blog";
            worksheet.Cell(currentRow, 4).Value = "Ngày Tạo Blog";
            worksheet.Cell(currentRow, 5).Value = "Thể Loại Blog";
            worksheet.Cell(currentRow, 6).Value = "Tác Giả Blog";

            var blogs = _blogManager.GetDetailedBlogList();

            foreach (var blog in blogs)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = blog.BlogID;
                worksheet.Cell(currentRow, 2).Value = blog.BlogTitle;
                worksheet.Cell(currentRow, 3).Value = Truncate(blog.BlogContent);
                worksheet.Cell(currentRow, 4).Value = blog.BlogCreatedAt.ToString("dd-MM-yyyy");
                worksheet.Cell(currentRow, 5).Value = blog.Category.CategoryName;
                worksheet.Cell(currentRow, 6).Value = blog.Writer.WriterNameSurname;
            }

            FormatWorksheet(worksheet);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            var date = DateTime.Now.ToString("yyyyMMddHHmmss");

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Danh_Sach_Blog_{date}.xlsx");
        }

        public IActionResult ExportDynamicUserToExcel()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Danh Sách Người Dùng");
            var currentRow = 1;

            worksheet.Cell(currentRow, 1).Value = "Mã Người Dùng";
            worksheet.Cell(currentRow, 2).Value = "Tên Người Dùng";
            worksheet.Cell(currentRow, 3).Value = "Họ Tên";
            worksheet.Cell(currentRow, 4).Value = "Email";
            worksheet.Cell(currentRow, 5).Value = "Thông Tin";
            worksheet.Cell(currentRow, 6).Value = "Đường Dẫn Ảnh";

            var writers = _writerManager.GetEntities();

            foreach (var writer in writers)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = writer.WriterID;
                worksheet.Cell(currentRow, 2).Value = writer.WriterUserName;
                worksheet.Cell(currentRow, 3).Value = writer.WriterNameSurname;
                worksheet.Cell(currentRow, 4).Value = writer.WriterMail;
                worksheet.Cell(currentRow, 5).Value = writer.WriterAbout;
                worksheet.Cell(currentRow, 6).Value = writer.WriterImage;
            }

            FormatWorksheet(worksheet);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            var date = DateTime.Now.ToString("yyyyMMddHHmmss");

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Danh_Sach_Nguoi_Dung_{date}.xlsx");
        }

        public IActionResult ExportDynamicCommentToExcel()
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Danh Sách Bình Luận");
            var currentRow = 1;

            worksheet.Cell(currentRow, 1).Value = "Mã Bình Luận";
            worksheet.Cell(currentRow, 2).Value = "Tên Người Dùng";
            worksheet.Cell(currentRow, 3).Value = "Tiêu Đề Bình Luận";
            worksheet.Cell(currentRow, 4).Value = "Nội Dung Bình Luận";
            worksheet.Cell(currentRow, 5).Value = "Tiêu Đề Blog";
            worksheet.Cell(currentRow, 6).Value = "Tác Giả Blog";

            var comments = _commentManager.GetCommentsWithBlogAndWriter();

            foreach (var comment in comments)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = comment.CommentID;
                worksheet.Cell(currentRow, 2).Value = comment.CommentUserName;
                worksheet.Cell(currentRow, 3).Value = comment.CommentTitle;
                worksheet.Cell(currentRow, 4).Value = comment.CommentContent;
                worksheet.Cell(currentRow, 5).Value = comment.Blog.BlogTitle;
                worksheet.Cell(currentRow, 6).Value = comment.Blog.Writer.WriterNameSurname;
            }

            FormatWorksheet(worksheet);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            var date = DateTime.Now.ToString("yyyyMMddHHmmss");

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Danh_Sach_Binh_Luan_{date}.xlsx");
        }

		private void FormatWorksheet(IXLWorksheet worksheet)
		{
			// Adjust column widths
			worksheet.Columns().AdjustToContents();

			// Set header style
			var headerRow = worksheet.Row(1);
			headerRow.Style.Font.Bold = true;
			headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
			headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
			headerRow.Style.Font.FontSize = 12;
			headerRow.Style.Font.FontColor = XLColor.DarkBlue;

			// Set borders for the header row
			headerRow.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
			headerRow.Style.Border.OutsideBorderColor = XLColor.DarkBlue;

			// Apply alternating row colors
			bool isEvenRow = false;
			foreach (var row in worksheet.RowsUsed().Skip(1))
			{
				isEvenRow = !isEvenRow;
				if (isEvenRow)
				{
					row.Style.Fill.BackgroundColor = XLColor.AliceBlue;
				}
				else
				{
					row.Style.Fill.BackgroundColor = XLColor.White;
				}

				// Set border style for all rows
				row.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
				row.Style.Border.OutsideBorderColor = XLColor.LightGray;
			}

			// Center align all columns
			worksheet.Columns().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


			// Set borders for the entire used range
			worksheet.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
			worksheet.RangeUsed().Style.Border.InsideBorder = XLBorderStyleValues.Thin;
			worksheet.RangeUsed().Style.Border.OutsideBorderColor = XLColor.LightGray;
			worksheet.RangeUsed().Style.Border.InsideBorderColor = XLColor.LightGray;
		}

		private string Truncate(string value, int maxLength = 32767)
		{
			if (string.IsNullOrEmpty(value)) return value;
			return value.Length <= maxLength ? value : value.Substring(0, maxLength);
		}
	}
}
