using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using log4net;
using log4net.Config;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer.User;
using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
     public class UserService

    {
        private readonly UserController userController;
        /// <summary>
        /// This is a constructor for the UserService Class.
        /// </summary>
        /// <param > It has no Parametrs</param>
        /// <return>the function does not return anything</return>
        public UserService()
        {
            userController = new UserController();
        }
        /// <summary>
        /// This function loads the data needed for the User Service.
        /// <param> It has no Parametrs</param>
        /// </summary>
        /// <returns> a json string but since we didnt implement it yet we dont know what the json string contains</returns>
        public string LoadData()
        {
            string json;
            Response res;
            try
            {
                userController.LoadData();
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }

            json = ToJson(res);
            return json;
        }

        public string DeleteData()
        {
            string json;
            Response res;
            try
            {
                userController.DeleteData();
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }

            json = ToJson(res);
            return json;
        }
        /// <summary>
        /// This function is used to make a new user in the system.
        /// </summary>
        /// <param name="Email">This is the new email for the new user</param>
        /// <param name="Password">This is the new password for the new user</param>
        /// <returns>a json string containing a response if the user is registered succesfully or what was the error</returns>
        public string Register(string Email,string Password)
        {
            Response res;
            string json;
            try
            {
                userController.Register(Email, Password);
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            json = ToJson(res);
            return json;
        }
        /// <summary>
        /// This function is used to log in to the system.
        /// </summary>
        /// <param name="Email">This is the email used to log in</param>
        /// <param name="Password">This is the password used to log in</param>
        /// <returns>a json string containing a response if the user has logged in succesfully or what was the error</returns>
        public string Login(string Email, string Password)
        {
            Response res;
            string json;
            try
            {
                userController.Login(Email, Password);
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            json = ToJson(res);
            return json;
        }
        /// <summary>
        /// This function is uesd to log out from the system.
        /// </summary>
        /// <param name="Email">This is the email that we want to logout from.</param>
        /// <returns>a json string containing a response if the user has logged out succesfully or what was the error</returns>
        public string Logout(String Email)
        {
            Response res;
            string json;
            try
            {
                userController.Logout(Email);
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            json = ToJson(res);
            return json;
        }
        /// <summary>
        /// This function is uesd to convert a string to a json string
        /// </summary>
        /// <param name="obj">This is string that needs to be converted.</param>
        /// <returns>a json string according to the string that was converted</returns>

        public string ToJson(object obj)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            string json = JsonSerializer.Serialize(obj, obj.GetType(), options);
            return json;
        }

    }
}
