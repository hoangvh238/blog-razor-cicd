using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using DateTimeExtensions;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.ViewComponents.Writer
{
    public class WriterMessageNotification : ViewComponent
    {
        private readonly MessageManager _manager = new(new EfMessageRepository());
        private readonly WriterManager _writerManager = new(new EfWriterRepository());
        private readonly UserManager<User> _userManager;

        public WriterMessageNotification(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var writer = GetWriterID().Result;

            var values = _manager.GetReceivedMessagesByWriter(writer.WriterID);
            values.RemoveAll(x => x.SenderID == writer.WriterID);
            values = values.OrderByDescending(x => x.MessageDate).ToList();

            foreach (var item in values)
            {
                var now = DateTime.Now;
                var tempDate = item.MessageDate;
                var relativeDate = tempDate.ToNaturalText(now, false);

                relativeDate = LocalizeRelativeDateTerm(relativeDate);

                item.MessageDetails = relativeDate;
            }

            return View(values);
        }

        private async Task<EntityLayer.Concrete.Writer> GetWriterID()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            string userId = await _userManager.GetUserIdAsync(user);
            var writer = _writerManager.GetWriterBySession(userId);

            return writer;
        }

        private static string LocalizeRelativeDateTerm(string relativeDate)
        {
            if (relativeDate.Contains("second"))
            {
                if (relativeDate.Contains("seconds"))
                {
                    relativeDate = relativeDate.Replace("seconds", "Cách đây vài giây");
                }
                relativeDate = relativeDate.Replace("second", "Cách đây vài giây");
            }
            else if (relativeDate.Contains("minute"))
            {
                if (relativeDate.Contains("minutes"))
                {
                    relativeDate = relativeDate.Replace("minutes", "một phút trước đây");
                }
                relativeDate = relativeDate.Replace("minute", "một phút trước đây");
            }
            else if (relativeDate.Contains("hour"))
            {
                if (relativeDate.Contains("hours"))
                {
                    relativeDate = relativeDate.Replace("hours", "một giờ trước");
                }
                relativeDate = relativeDate.Replace("hour", "một giờ trước");
            }
            else if (relativeDate.Contains("day"))
            {
                if (relativeDate.Contains("days"))
                {
                    relativeDate = relativeDate.Replace("days", "một ngày trước");
                }
                relativeDate = relativeDate.Replace("day", "một ngày trước");
            }
            else if (relativeDate.Contains("week"))
            {
                if (relativeDate.Contains("weeks"))
                {
                    relativeDate = relativeDate.Replace("weeks", "một tuần trước");
                }
                relativeDate = relativeDate.Replace("week", "một tuần trước");
            }
            else if (relativeDate.Contains("month"))
            {
                if (relativeDate.Contains("months"))
                {
                    relativeDate = relativeDate.Replace("months", "một tháng trước");
                }
                relativeDate = relativeDate.Replace("month", "một tháng trước");
            }
            else if (relativeDate.Contains("year"))
            {
                if (relativeDate.Contains("years"))
                {
                    relativeDate = relativeDate.Replace("years", "một năm trước");
                }
                relativeDate = relativeDate.Replace("year", "một năm trước");
            }

            return relativeDate;
        }
    }
}
