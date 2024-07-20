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
	
	}
}
