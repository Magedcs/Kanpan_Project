using System;
using IntroSE.Kanban.Backend.BusinessLayer.User;
namespace IntroSE.Kanban.Backend.ServiceLayer.ToReturn
{
 public class User
	{

		public string email { set; get; }
		public bool status { get; set; }


		public User(BusinessLayer.User.User prevUser)//Copy constructor
		{
			email = prevUser.Email;
			status = prevUser.Status;
		}
	}
}

