//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;
//using IntroSE.Kanban.Backend.ServiceLayer;
//using IntroSE.Kanban.Backend.BusinessLayer.Board;
//using IntroSE.Kanban.Backend.BusinessLayer.User;

//namespace BackendTests
//{
//    /// <summary>
//    /// This class is used to test the Board calss and its functionality.
//    /// </summary>
//      class BoardTest
//    {
//        private readonly BoardController Bcontroller;
//        private readonly BoardService boardservice;
//        private readonly UserController userController;

//        public BoardTest()
//        {
//            Bcontroller = new BoardController();
//            this.boardservice = new BoardService(Bcontroller);
//            userController = new UserController();
//        }


//        public void RunAllTests()
//        {
//            AddBoardTest();
//            RemoveBoardTest();
//            InProgressTasksTest();
//            LimitColumnTest();
//            GetColumnLimitTest();
//            GetColumnNameTest();
//            GetColumnTest();

//        }
//        /// <summary>
//        /// This is a test function for AddBoard
//        /// </summary>
//        /// <example> For json1 the function should print Board added succesfully</example>
//        /// <example> For json2 the function should print Board name already taken</example>
//        /// <example> For json3 the function should print Board added successfully</example>
//        public void AddBoardTest()
//        {

//            // Board should be added successfully
//            string json1 = boardservice.AddBoard("hmode@gmail.com", "somaya");
//            Response response1 = JsonSerializer.Deserialize<Response>(json1);
//            Console.WriteLine(response1.ErrorMessage);

//            // Board should not be added succesfully because of repetition of board name with the same user   
//            string json2 = boardservice.AddBoard("hmode@gmail.com", "somaya");
//            Response response2 = JsonSerializer.Deserialize<Response>(json2);
//            Console.WriteLine(response1.ErrorMessage);

//            // Board should  be added succesfully
//            string json3 = boardservice.AddBoard("moshhmode@gmail.com", "somaya");
//            Response response3 = JsonSerializer.Deserialize<Response>(json3);
//            Console.WriteLine(response1.ErrorMessage);
//        }
//        /// <summary>
//        /// This is a test function for RemoveBoard
//        /// </summary>
//        /// <example> For json1 the function should print Board removed successfully</example>
//        /// <example> For json2 the function should throw an exception stating that the email is not the owner's/example>
//        public void RemoveBoardTest() 
//        {
//            string json1 = boardservice.RemoveBoard("hmode@gmail.com", "somaya");
//            Response response1 = JsonSerializer.Deserialize<Response>(json1);
//            Console.WriteLine(response1.ErrorMessage);


//            string json2 = boardservice.RemoveBoard("maged@gmail.com", "somaya");
//            Response response2 = JsonSerializer.Deserialize<Response>(json2);
//            Console.WriteLine(response2.ErrorMessage);
//        }
//        /// <summary>
//        /// This is a test function for InProgressTask
//        /// </summary>
//        /// <example> For json the function should print InProgress Tasks returned successfully</example>
//        public void InProgressTasksTest()
//        {
//            string json = boardservice.InProgressTasks("hmode@gmail.com");
//            Response response = JsonSerializer.Deserialize<Response>(json);
//            Console.WriteLine(response.ErrorMessage);

//        }
//        /// <summary>
//        /// This is a test function for LimitColumn 
//        /// </summary>
//        /// <example> For json1,json2,json3 the function should print column limit set successfully</example>
//        /// <example> Json4 should throw an exception as the given user is not the owner</example>
//        public void LimitColumnTest()
//        {
//            string json1 = boardservice.LimitColumn("hmode@gmail.com", "somaya", 2, 56);
//            Response response = JsonSerializer.Deserialize<Response>(json1);
//            Console.WriteLine(response.ErrorMessage);

//            string json2 = boardservice.LimitColumn("hmode@gmail.com", "somaya", 1, 56);
//            Response response2 = JsonSerializer.Deserialize<Response>(json2);
//            Console.WriteLine(response2.ErrorMessage);

//            string json3 = boardservice.LimitColumn("hmode@gmail.com", "somaya", 1, 56);
//            Response response3 = JsonSerializer.Deserialize<Response>(json3);
//            Console.WriteLine(response3.ErrorMessage);

//            string json4 = boardservice.LimitColumn("maged@gmail.com", "somaya", 0, 56);
//            Response response4 = JsonSerializer.Deserialize<Response>(json4);
//            Console.WriteLine(response4.ErrorMessage);
//        }
//        /// <summary>
//        /// This is a test function for GetColumnLimit
//        /// </summary>
//        /// <example> For json the function should print the column Limit returned successfully</example>
//        public void GetColumnLimitTest()
//        {
//            string json = boardservice.GetColumnLimit("hmode@gmail.com", "somaya", 2);
//            Response response = JsonSerializer.Deserialize<Response>(json);
//            Console.WriteLine(response.ErrorMessage);
//        }
//        /// <summary>
//        /// This is a test function for GetCoulnmName
//        /// </summary>
//        /// <example> For json the function should print in progress</example>
//        public void GetColumnNameTest()
//        {
//            string json = boardservice.GetColumnName("hmode@gmail.com", "somaya", 2);
//            Response response = JsonSerializer.Deserialize<Response>(json);
//            Console.WriteLine(response.ErrorMessage);
//        }
//        /// <summary>
//        /// This is a test function for GetCoulmn
//        /// </summary>
//        /// <example> For json the function should print column retured successfully</example>
//        public void GetColumnTest()
//        {
//            string json = boardservice.GetColumn("hmode@gmail.com", "somaya", 2);
//            Response response = JsonSerializer.Deserialize<Response>(json);
//            Console.WriteLine(response.ErrorMessage);
//        }


//        /// <summary>
//        /// This is a test function for GetUserBoards
//        /// </summary>
//        /// <example> For json the function should print user's Boards returned successfully</example>
//        public void GetUserBoardsTest()
//        {
//            string json = boardservice.GetUserBoards("hmode@gmail.com");
//            Response response = JsonSerializer.Deserialize<Response>(json);
//            Console.WriteLine(response.ErrorMessage);

//        }

//        /// <summary>
//        /// This is a test function for GetBoardName
//        /// </summary>
//        /// <example> For json the function should print Board's name returned successfully</example>
//        public void GetBoardName()
//        {
//            string json = boardservice.GetBoardName(24);
//            Response response = JsonSerializer.Deserialize<Response>(json);
//            Console.WriteLine(response.ErrorMessage);


//        }

//        /// <summary>
//        /// This is a test function for JoinBoardTest
//        /// </summary>
//        /// <example> For json the function should print user joined the board sucessfully</example>
//        public void JoinBoardTest()
//        {
//            string json = boardservice.JoinBoard("maged@gmail.com", 2);
//            Response response = JsonSerializer.Deserialize<Response>(json);
//            Console.WriteLine(response.ErrorMessage);


//        }

//        /// <summary>
//        /// This is a test function for LeaveBoard
//        /// </summary>
//        /// <example> For json the function should print user left the board sucessfully</example>
//        public void LeaveBoardTest()
//        {
//            string json = boardservice.LeaveBoard("maged@gmail.com", 2);
//            Response response = JsonSerializer.Deserialize<Response>(json);
//            Console.WriteLine(response.ErrorMessage);

//        }

//        /// <summary>
//        /// This is a test function for TransferOwnership
//        /// </summary>
//        /// <example> For json the function should print board ownership transferred sucessfully</example>
//        /// <example> For json2 the function should print the given user is not the owner of the board</example>
//        public void TransferOwnership()
//        {
//            string json = boardservice.TransferOwnership("hmode@gmail.com" ,"maged@gmail.com", "somaya");
//            Response response = JsonSerializer.Deserialize<Response>(json);
//            Console.WriteLine(response.ErrorMessage);


//            string json2 = boardservice.TransferOwnership("maged@gmail.com", "e3tdal@gmail.com", "somaya");
//            Response response2 = JsonSerializer.Deserialize<Response>(json2);
//            Console.WriteLine(response2.ErrorMessage);

//        }

//    }
//}
