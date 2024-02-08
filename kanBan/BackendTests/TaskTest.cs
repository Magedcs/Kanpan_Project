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
//    /// This class is used to test the task calss and its functionality.
//    /// </summary>
//    internal class TaskTest
//    {

//        private readonly BoardService taskService = new BoardService();


//        public TaskTest()
//        {
//            this.taskService = taskService;
//        }
//        public void RunAllTests()
//        {
//            addTaskTest();
//            moveTaskTest();
//            UpdateTaskDescriptionTest();
//            UpdateTaskDueDateTest();
//            UpdateTaskTitleTest();
//        }
//        /// <summary>
//        /// This is a test function for addTask
//        /// </summary>
//        /// <example> For add1 & add6 the function should print added task successfully</example>
//        /// <example> For add2 the function should print task title should not be empty</example>
//        /// <example> For add3 the function should print task title should be less than 50 characters</example>
//        /// <example> For add4 the function should print task description should be less than 300 characters</example>
//        /// <example> For add5 the function should print task duoDate is expired</example>
//        public void addTaskTest()
//        {
//            //adding a task successfully
//            String add1 = taskService.AddTask("ali@gmail.com", "house","cleaning the room", "must clean the room", new DateTime(2022, 5, 27));
//            Response response1 = JsonSerializer.Deserialize<Response>(add1);
//            Console.WriteLine(response1.ErrorMessage);
            
//            //adding a task with an empty title
//            String add2 = taskService.AddTask("ali@gmail.com",  "house", "", "must clean the room", new DateTime(2022, 5, 27));
//            Response response2 = JsonSerializer.Deserialize<Response>(add2);
//            Console.WriteLine(response2.ErrorMessage);

//            //adding a task with more than 50 characters in the title
//            String add3 = taskService.AddTask("ali@gmail.com",  "house", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "must clean the room", new DateTime(2022, 4, 27));
//            Response response3 = JsonSerializer.Deserialize<Response>(add3);
//            Console.WriteLine(response3.ErrorMessage);

//            //adding a tsak with more that 300 characters int the description 
//            String add4 = taskService.AddTask("ali@gmail.com", "cleaning the room", "house", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa ", new DateTime(2022, 5, 27));
//            Response response4 = JsonSerializer.Deserialize<Response>(add4);
//            Console.WriteLine(response4.ErrorMessage);

//            //adding a task with a exp duoDate
//            String add5 = taskService.AddTask("ali@gmail.com", "cleaning the room", "house", "must clean the room", new DateTime(2022, 4, 10));
//            Response response5 = JsonSerializer.Deserialize<Response>(add5);
//            Console.WriteLine(response5.ErrorMessage);

//            //adding a task successfully but without a description
//            String add6 = taskService.AddTask("ali@gmail.com", "cleaning the room", "house", "", new DateTime(2022, 5, 27));
//            Response response6 = JsonSerializer.Deserialize<Response>(add6);
//            Console.WriteLine(response6.ErrorMessage);
//        }
//        /// <summary>
//        /// This is a test function for moveTask
//        /// </summary>
//        /// <example> For move1 the function should print task moved successfully</example>
//        /// <example> For move2 the function should print Cant move a task that is done</example>
//        public void moveTaskTest()
//        {
//            //moving a task successfully from the first column to the second
//            String move1 = taskService.MoveTask("ali@gmail.com", "house", 0, 10);
//            Response response1 = JsonSerializer.Deserialize<Response>(move1);
//            Console.WriteLine(response1.ErrorMessage);

//            //cant move a task from the last column
//            String move2 = taskService.MoveTask("ali@gmail.com", "house", 2, 11);
//            Response response2 = JsonSerializer.Deserialize<Response>(move2);
//            Console.WriteLine(response2.ErrorMessage);
//        }
//        /// <summary>
//        /// This is a test function for UpdateTaskTitle
//        /// </summary>
//        /// <example> For title1 the function should print updated task title successfully</example>
//        /// <example> For title2 the function should print task title sould not be empty</example>
//        /// <example> For title3 the function should print task title should not be more that 50 characters</example>
//        /// <example> For title4 the function should print Cant Update a task title that is done</example>
//        public void UpdateTaskTitleTest()
//        {
//            //Updating the task title susuccessfully
//            String title1 = taskService.UpdateTaskTitle("ali@gmail.com", "house",0, 10, "school");
//            Response response1 = JsonSerializer.Deserialize<Response>(title1);
//            Console.WriteLine(response1.ErrorMessage);

//            //Updating the task with an empty title
//            String title2 = taskService.UpdateTaskTitle("ali@gmail.com", "house",  0, 10, "");
//            Response response2 = JsonSerializer.Deserialize<Response>(title2);
//            Console.WriteLine(response2.ErrorMessage);

//            //Updating the task title with a title that has more than 50 characters
//            String title3 = taskService.UpdateTaskTitle("ali@gmail.com", "house", 0, 10, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
//            Response response3 = JsonSerializer.Deserialize<Response>(title3);
//            Console.WriteLine(response3.ErrorMessage);

//            //Updating a task title that its done
//            String title4 = taskService.UpdateTaskTitle("ali@gmail.com", "house", 2 , 10 ,"school");
//            Response response4 = JsonSerializer.Deserialize<Response>(title4);
//            Console.WriteLine(response4.ErrorMessage);
//        }
//        /// <summary>
//        /// This is a test function for UpdateTaskDescription
//        /// </summary>
//        /// <example> For dec1 & dec2 the function should print updated task description successfully</example>
//        /// <example> For dec3 the function should print task description should not be more that 300 characters</example>
//        /// <example> For dec4 the function should print Cant Update a task description that is done</example>
//        public void UpdateTaskDescriptionTest()
//        {
//            //Updating the task description susuccessfully
//            String dec1 = taskService.UpdateTaskDescription("ali@gmail.com", "house", 0, 10, "must clean the room");
//            Response response1 = JsonSerializer.Deserialize<Response>(dec1);
//            Console.WriteLine(response1.ErrorMessage);

//            //Updating the task successfully with an empty description
//            String dec2 = taskService.UpdateTaskDescription("ali@gmail.com", "house", 0, 10, "");
//            Response response2 = JsonSerializer.Deserialize<Response>(dec2);
//            Console.WriteLine(response2.ErrorMessage);

//            //Updating the task title with a description that has more than 300 characters
//            String dec3 = taskService.UpdateTaskDescription("ali@gmail.com", "house", 0, 10, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
//            Response response3 = JsonSerializer.Deserialize<Response>(dec3);
//            Console.WriteLine(response3.ErrorMessage);

//            //Updating a task description that its done
//            String dec4 = taskService.UpdateTaskDescription("ali@gmail.com", "house", 2, 10, "must clean the room");
//            Response response4 = JsonSerializer.Deserialize<Response>(dec4);
//            Console.WriteLine(response4.ErrorMessage);
//        }
//        /// <summary>
//        /// This is a test function for UpdateTaskDueDate
//        /// </summary>
//        /// <example> For date1 the function should print updated task duo Date successfully</example>
//        /// <example> For date2 the function should print task duo Date is expired</example>
//        /// <example> For date3 the function should print Cant Update a task duo Date that is done</example>
//        public void UpdateTaskDueDateTest()
//        {
//            //updating the duoDate successfully
//            String date1 = taskService.UpdateTaskDueDate("ali@gmail.com", "house", 0, 10, new DateTime(2022, 5, 30));
//            Response response1 = JsonSerializer.Deserialize<Response>(date1);
//            Console.WriteLine(response1.ErrorMessage);

//            //Updating the duoDate with an exp Date
//            String date2 = taskService.UpdateTaskDueDate("ali@gmail.com", "house", 0, 10, new DateTime(2022, 4, 10));
//            Response response2 = JsonSerializer.Deserialize<Response>(date2);
//            Console.WriteLine(response2.ErrorMessage);

//            //Updating a task duoDate that its done
//            String date3 = taskService.UpdateTaskDueDate("ali@gmail.com", "house",2, 10, new DateTime(2022, 5, 30));
//            Response response3 = JsonSerializer.Deserialize<Response>(date3);
//            Console.WriteLine(response3.ErrorMessage);
//        }

//        /// <summary>
//        /// This is a test function for AssignTask
//        /// </summary>
//        /// <example> For json2 the function should print task assigned successfully</example>
//        public void AssignTaskTest()
//        {
//            string json2 = taskService.AssignTask("hmode@gmail.com", "somaya",2,23, "e3tdal@gmail.com");
//            Response response2 = JsonSerializer.Deserialize<Response>(json2);
//            Console.WriteLine(response2.ErrorMessage);

//        }
//    }
  
//}
