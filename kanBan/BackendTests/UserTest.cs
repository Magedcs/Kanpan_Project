//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Text.Json;
//using IntroSE.Kanban.Backend.ServiceLayer;

//namespace BackendTests
//{
//    /// <summary>
//    /// This class is used to test the user calss and its functionality.
//    /// </summary>
//    internal class UserTest
//    {
//        private readonly UserService userService = new UserService();

//        public UserTest()
//        {
//            this.userService = userService; 
//        }
//        public void RunAllTests()
//        {
//            RegisterTests();
//            LoginTests();
//            LogoutTests();
//        }

//        /// <summary>
//        /// This is a test function for Register .
//        /// </summary>
//        /// <example> For reg the function prints registered successfully</example>
//        /// <example> For reg2 the function prints email already exist</example>
//        /// <example> For reg3 the function prints email is null</example>
//        /// <example> For reg4 the function prints password should include a number</example>
//        /// <example> For reg5 the function prints password should include an upperCase letter</example>
//        /// <example> For reg6 the function prints password should include a lowerCase letter</example>
//        /// <example> For reg7 the function prints password should be less that 20 in length</example>
//        /// <example> For reg8 the function prints password should be more that 6 in length</example>
//        public void RegisterTests()
//        {
//            //email should be registered successfully
//            string reg = userService.Register("guy@gmail.com", "1235cA");
//            Response response = JsonSerializer.Deserialize<Response>(reg);
//            Console.WriteLine(reg);



//            //trying to register an existing user 
//            string reg2 = userService.Register("guy@gmail.com", "1235cAf");
//            Response response2 = JsonSerializer.Deserialize<Response>(reg2);
//            Console.WriteLine(reg2);

//            //email inout is null
//            string reg3 = userService.Register(null, "1235MMaa");
//            Response response3 = JsonSerializer.Deserialize<Response>(reg3);
//            Console.WriteLine(reg3);



//            //not legal password because there is no numbers
//            string reg4 = userService.Register("ali@gmail.com", "iloveSe");
//            Response response4 = JsonSerializer.Deserialize<Response>(reg4);
//            Console.WriteLine(reg4);


//            //not legal password because there is no uppercase
//            string reg5 = userService.Register("ali@gmail.com", "ilovese4");
//            Response response5 = JsonSerializer.Deserialize<Response>(reg5);
//            Console.WriteLine(reg5);


//            //not legal password because there us no lowercase
//            string reg6 = userService.Register("ali@gmail.com", "ILOVESE66");
//            Response response6 = JsonSerializer.Deserialize<Response>(reg6);
//            Console.WriteLine(reg6);




//            //password's length is bigger than 20
//            string reg7 = userService.Register("ali@gmail.com", "ILoveSe12355555milestone1");
//            Response response7 = JsonSerializer.Deserialize<Response>(reg7);
//            Console.WriteLine(reg7);


//            //password's length is smaller than 6
//            string reg8 = userService.Register("ali@gmail.com", "sd3S");
//            Response response8 = JsonSerializer.Deserialize<Response>(reg8);
//            Console.WriteLine(reg8);
//        }

//        /// <summary>
//        /// This is a test function for logIn
//        /// </summary>
//        /// <example> For log the function should print logIn successfully</example>
//        /// <example> For log2 the function should print password is wrong</example>
//        /// <example> For log3 the function should print email does not exist</example>
//        public void LoginTests()
//        {
//            string log = userService.Register("guy@gmail.com", "1235cA");
//            Response response = JsonSerializer.Deserialize<Response>(log);
//            Console.WriteLine(response.ErrorMessage);

//            //Password is incorrect for the specified email
//            string log2 = userService.Login("guy@gmail.com", "1235cAVAFA");
//            Response response2 = JsonSerializer.Deserialize<Response>(log2);
//            Console.WriteLine(response2.ErrorMessage);

//            //email not registered in the system
//            string log3 = userService.Login("batman@gmail.com", "1235cAVAFA");
//            Response response3 = JsonSerializer.Deserialize<Response>(log3);
//            Console.WriteLine(response3.ErrorMessage);
//        }

//        /// <summary>
//        /// This is a test function fot logOut
//        /// </summary>
//        /// <example> For logout the function should print logged Out successfully</example>
//        public void LogoutTests()
//        {
//            string logout = userService.Logout("guy@gmail.com");
//            Response response = JsonSerializer.Deserialize<Response>(logout);
//            Console.WriteLine(response.ErrorMessage);
//        }
//    }
     
//}
