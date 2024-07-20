using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Core.ExtensionService.BlogService;
using Microsoft.AspNetCore.Identity;
using EntityLayer.Concrete;
using Core.Models;
using System.Collections.Generic;
using System.IO;
using System;

namespace Core.Hubs
{
	public class PostHub : Hub
	{
		private readonly UserManager<User> _userManager;

		public PostHub(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public override async Task OnConnectedAsync()
		{
			await Clients.All.SendAsync("UserStatusChanged", $"{Context.ConnectionId}" + "StateChangedLoadLatestChatList");
		}

		public async Task SendBlogData(Dictionary<string, string> formData)
		{
			//var user = await _userManager.FindByNameAsync(Context.User.Identity.Name);
			//string userId = await _userManager.GetUserIdAsync(user);

			//var model = new BlogAddViewModel
			//{
			//	BlogTitle = formData["BlogTitle"],
			//	BlogContent = formData["BlogContent"],
			//	CategoryID = int.Parse(formData["CategoryID"]),
			//	BlogImage = new FormFile(
			//		new MemoryStream(Convert.FromBase64String(formData["BlogImage"])),
			//		0,
			//		Convert.FromBase64String(formData["BlogImage"]).Length,
			//		"BlogImage",
			//		formData["BlogImageName"]
			//	)
			//};

			//var result = blogExtensionService.AddBlogAsync(model, userId);

			//await Clients.All.SendAsync("BlogDataReceived", result);

			await Clients.All.SendAsync("BlogDataReceived", "oke");
		}

	
	}
}
