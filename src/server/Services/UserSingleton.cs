using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
	public static class UserSingleton
	{
		public static User User { get; private set; } = new User("admin");
	}
}
