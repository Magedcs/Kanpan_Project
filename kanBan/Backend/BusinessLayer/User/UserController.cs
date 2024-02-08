using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using IntroSE.Kanban.Backend.BusinessLayer.Board;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.Backend.BusinessLayer.User
{
    /// <summary>
    /// This our User manager, it manges all the new/exists Users of the Kanpan board!
    /// </summary>
   public class UserController
    {
        private Dictionary<string, User> users;
        private readonly UserControllerDAL userDal;
     
     
        private const int MaxLength = 20;
        private const int MinLength = 6;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// This is a constructor for the UserController Class.
        /// </summary>
        /// <param > It has no Parametrs</param>
        /// <return>the function does not return anything</return>
        public UserController()
        {
            users = new Dictionary<string, User>();
            userDal = new UserControllerDAL();

        }
        /// <summary>
        /// This function loads the data needed for the User Service.
        /// <param> It has no Parametrs</param>
        /// </summary>
        /// <returns> a json string but since we didnt implement it yet we dont know what the json string contains</returns>
        public void LoadData()
        {
            List<UserDTO> a = userDal.allusers();
            foreach(UserDTO userdto in a)
            {
                users.Add(userdto.Email, new User(userdto));
            }
        }


        public void DeleteData()
        {
            userDal.DeleteData();
        }
        /// <summary>
        /// This function is used to make a new user in the system.
        /// </summary>
        /// <param name="Email">This is the new email for the new user</param>
        /// <param name="Password">This is the new password for the new user</param>
        /// <exception cref="Exception">if the email is null </exception>
        /// <exception cref="Exception">if the email has invalid syntax </exception>
        /// <exception cref="Exception">if the user is already registered</exception>
        /// <return>the function does not return anything</return>
        public void Register(string Email, string Password)
        {
            if (Email is null)
            {
                log.Warn("attempt to register using a null email");
                throw new Exception("cannot register null email!");
            }
            if (!ValidateEmail(Email))
            {
                log.Warn("attempt to register a user with an invalid email");
                throw new Exception("invalid email syntax!");
            }
            if (users.ContainsKey(Email))
            {
                log.Warn("attempt to register an already registered user");
                throw new Exception("User is already registered!");
            }
            checkPass(Password);
            User newUser = new User(Email, Password,true);
            users.Add(Email, newUser);
            userDal.Insert(newUser.ToDAL());
            log.Info("a new user with registered");
        }
        /// <summary>
        /// This function is used to log in to the system.
        /// </summary>
        /// <param name="Email">This is the email used to log in</param>
        /// <param name="Password">This is the password used to log in</param>
        /// <exception cref="Exception">if the email is null </exception>
        /// <exception cref="Exception">if the user is already logged in </exception>
        /// <exception cref="Exception">if the user is not already registered</exception>
        /// <return>the function does not return anything</return>
        public void Login(string Email, string Password)
        {
            if (Email is null)
            {
                log.Warn("attempt to login using null email");
                throw new Exception("Email is null!");
            }
            if (!users.ContainsKey(Email))
            {
                log.Warn("attempt to login with a non registered arguments");
                throw new Exception("User hasnt registered!");
            }
            if (users[Email].Status) { log.Warn("attempt to login a loggedin user"); throw new Exception("User is already logged in!"); }
            users[Email].login(Password);
            log.Info("User loggedin successfully");
        }
        /// <summary>
        /// This function is uesd to log out from the system.
        /// </summary>
        /// <param name="Email">This is the email that we want to logout from.</param>
        /// <exception cref="Exception">if the email is null</exception>
        /// <exception cref="Exception">if the user is not registered </exception>
        /// <exception cref="Exception">if the user is already logged out </exception>
        /// <return>the function does not return anything</return>
        public void Logout(string Email)
        {
            if (Email is null)
            {
                throw new Exception("email is null!");
            }
            if (!users.ContainsKey(Email)) { log.Warn("attempt to logout a non exsisting user"); throw new Exception("User doesn't exist!"); }
            if (!users[Email].Status) { log.Warn("attempt to logout a user that is logged out"); throw new Exception("User is already logged out"); }
            users[Email].logout();
            log.Info("User logged out successfully");


        }

        
        /// <summary>
        /// This function is validate the syntax of an email.
        /// </summary>
        /// <param name="email">This is the email that we want to check.</param>
        /// <exception cref="Exception">if the email is invalid</exception>
        /// <return>the function returns if the email is valid or not</return>
        private bool ValidateEmail(string email)
        {

            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// This function is validate a password.
        /// </summary>
        /// <param name="Email">This is the password that we want to check.</param>
        /// <exception cref="Exception">if the password length is shorter than 6 </exception>
        /// <exception cref="Exception">if the password length is longer than 20 </exception>
        /// <exception cref="Exception">if the password does not contain a number </exception>
        /// <exception cref="Exception">if the password does not contain an uppercase letter </exception>
        /// <exception cref="Exception">if the password does not contain a lowercase letter </exception>
        /// <return>the function returns if the password is valid or not</return>
        private bool checkPass(string Password)
        {
            bool upper = false;
            bool lower = false;
            bool numb = false;
            if (Password.Length < MinLength)
            {
                log.Warn("attempt to register using password length shorter than 6");
                throw new Exception("Passwords length cant be shorter than 6!");
            }
            if (Password.Length > MaxLength)
            {
                log.Warn("attempt to register using password length longer than 20");
                throw new Exception("Passwords length cant be longer than 20!");
            }
            foreach (char c in Password)
            {
                if (!numb)
                {
                    if (c >= 48 & c <= 57)
                    {
                        numb = true;
                    }
                }
                if (!upper)
                {
                    if (c >= 65 & c <= 90)
                    {
                        upper = true;
                    }
                }
                if (!lower)
                {
                    if (c >= 97 & c <= 122)
                    {
                        lower = true;
                    }
                }

            }
            if (!numb)
            {
                log.Warn("attempt to register using password that doesn't contain a number");
                throw new Exception("password must contain a number!");
            }
            if (!upper)
            {
                log.Warn("attempt to register using password that doesn't contain an uppercase");
                throw new Exception("password must contain an uppercase letter!");
            }
            if (!lower)
            {
                log.Warn("attempt to register using password that doesn't contain a lowercase");
                throw new Exception("password must contain a lowercase letter!");
            }


            return true;

        }

    }



}
