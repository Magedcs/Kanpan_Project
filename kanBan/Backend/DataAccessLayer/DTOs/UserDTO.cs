using System;
using IntroSE.Kanban.Backend.DataAccessLayer;
namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
	public class UserDTO : DTO
	{

		public const string UsersEmailColumnName = "Email";
		public const string UsersPasswordColumnName = "Password";


		private string _email;
		public string Email { get { return _email; } set { _email = value; } }
		private string _password;
		public string Password { get { return _password; } set { _password = value; } }
		private bool _status;
		public bool Status { get { return _status; }  }

		private static int boolToInt(bool a)
		{
			if (a) return 1;
			return 0;
		}

		public UserDTO(string email , string password, bool status):base(new UserControllerDAL())
        {
			_email = email;
			_password = password;
			_status = status;
        }


		public void Save()
        {
			Email = _email;
			Password = _password;
        }

	}
}

