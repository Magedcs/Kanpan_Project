using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.Backend.BusinessLayer.User
{
    /// <summary>
    /// This is a User class we use to make new users so they could use the kanpan board
    /// </summary>
     public  class User
    {
        private string email;
        private string password;
        private bool status;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This is a constructor for the User Class.
        /// </summary>
        /// <param name="email">This is the new email for the new user</param>
        /// <param name="password">This is the new password for the new user</param>
        /// /// <param name="id">This is the nwe id for the new user</param>
        /// <return>the function does not return anything</return>

        public User(string email, string password, bool status)
        {
            this.email = email;
            this.password = password;
            this.status = status;
        }
        public User(UserDTO user) {

            email = user.Email;
            password = user.Password;
        }

        public UserDTO ToDAL()
        {
            return new UserDTO(email, password, false);
        }

        /// <summary>
        /// c# getter for the email.
        /// </summary>
        public string Email
        {
            get { return email; }
        }
        /// <summary>
        /// C# getter for the status
        /// </summary>
        public bool Status
        {
            get { return status; }
        }
        /// <summary>
        /// This function is used for logging a user in.
        /// </summary>
        /// <param name="password">This is the password of the user</param>
        /// <exception cref="Exception">if the password is invalid </exception>
        /// <return>the function does not return anything</return>
        public void login(string password)
        {
            if (!password.Equals(this.password))
            {
                log.Warn("attempt to login using an incorrect password");
                throw new Exception("incorrect password!");
            }

            this.status = true;

        }
        /// <summary>
        /// This function is used for logging a user out.
        /// </summary>
        /// <param > It has no Parametrs</param>
        /// <return>the function does not return anything</return>
        public void logout()
        {
            this.status = false;
        }
        /// <summary>
        /// C# getter for the password field.
        /// </summary>
        public string Password
        {
            get { return password; }
        }

    }
}
