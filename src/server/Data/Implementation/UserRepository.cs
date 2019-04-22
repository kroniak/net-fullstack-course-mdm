using Server.Data.Interface;
using Server.Models;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Data.Implementation
{
	public class UserRepository : IUserRepository
	{
		public User GetCurrentUser(string userName) =>
			DataBase.UserList?.Where(x => x.UserName == userName).FirstOrDefault();
	}
}
