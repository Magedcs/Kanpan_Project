using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using IntroSE.Kanban.Backend.BusinessLayer.Board;
using Task = IntroSE.Kanban.Backend.BusinessLayer.Board;
using IntroSE.Kanban.Backend.ServiceLayer.ToReturn;
using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService
    {
        private readonly BoardController boardController;
        /// <summary>
        /// This is a constructor for the BoardService Class.
        /// <param > It has no Parametrs</param>
        /// </summary>
        public BoardService()
        {
            this.boardController = new BoardController();
        }

         BoardService(BoardController boardController)
        {
            this.boardController = boardController;
        }

        /// <summary>
        /// This function loads the data needed for the Board Service.
        /// <param> It has no Parametrs</param>
        /// </summary>
        /// <returns> A Response with a message, unless an error occurs</returns>
        public string LoadData()
        {
            string json;
            Response res;
            try
            {
                boardController.LoadData();
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
                boardController.DeleteData();
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
        /// This function adds a new board to the user. 
        /// <param name="email" > This is the users email that we will add the Board to ,And its unique</param>
        /// <param name="boardname"> This is the board name that we want to give to the new Board we created</param>
        /// </summary>
        /// <returns> A Response with a message that the board added successfully,unless an error occurs</returns>
        public string AddBoard(string email, string boardname)
        {
            Response res;
            string json;
            try
            {
                boardController.AddBoard(email, boardname);
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
        /// This function remove a board from the users Boards.
        /// </summary>
        /// <param name="email" > This is the users email that we will remove the Board from ,and its unique</param>
        /// <param name="boardname"> This is the board name that we want to remove</param>
        /// </summary>
        /// <returns> A Response a message that the board removed successfully,unless an error occurs </returns>
        public string RemoveBoard(string email, string boardname)
        {
            Response res;
            string json;
            try
            {
                boardController.RemoveBoard(email, boardname);
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
        /// This function shows the user all the in progress tasks.
        /// </summary>
        /// <param name="email" > This is the users email that we will show the task to,and its unique</param>
        /// </summary>
        /// <returns> A Response with all the in progress , unless an error occurs </returns>
        public string InProgressTasks(string email)
        {
            Response res;
            string json;
            try
            {
                List<Task.Task> tasks = boardController.InProgessTasks(email);
                List<ToReturn.Task> temp = new List<ToReturn.Task>();
                foreach (Task.Task curr in tasks)
                {
                    ToReturn.Task toRet = new ToReturn.Task(curr);
                    temp.Add(toRet);
                }
                ToReturn.Task [] toReturntoArr = temp.ToArray();
                res = new Response<ToReturn.Task []>(toReturntoArr);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            json = ToJson(res);
            return json;

        }



        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public string LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            Response res;
            string json;
            try
            {
                boardController.LimitColumn(email, boardName, columnOrdinal, limit);
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
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>Response with column limit value, unless an error occurs </returns>
        public string GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            Response res;
            string json;
            try
            {
                int limit = boardController.GetColumnLimit(email, boardName, columnOrdinal);
                res = new Response<int>(limit);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            json = ToJson(res);
            return json;
        }

        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>Response with column name value, unless an error occurs </returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            Response res;
            string json;
            try
            {
                string name = boardController.GetColumnName(email, boardName, columnOrdinal);
                res = new Response<string>(name);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            json = ToJson(res);
            return json;

        }

        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>Response with  a list of the column's tasks, unless an error occurs</returns>
        public string GetColumn(string email, string boardName, int columnOrdinal)
        {
            Response res;
            string json;
            try
            {
                List<Task.Task> tasks = boardController.GetColumn(email, boardName, columnOrdinal);
                List<ToReturn.Task> toReturn = new List<ToReturn.Task>();
                foreach (Task.Task temp in tasks)
                {
                    ToReturn.Task toRet = new ToReturn.Task(temp);
                    toReturn.Add(toRet);
                }
                ToReturn.Task[] Column = toReturn.ToArray();
                res = new Response<ToReturn.Task[]>(Column);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
            }
            json = ToJson(res);
            return json;
        }

            /// <summary>
            /// This function adds a new task to the users Board . 
            /// <param name="email" > This is the users email thah we will add the task to ,and its unique</param>
            /// <param name="Title">This is the Title we will set for the task after adding it</param>
            /// <param name="boardname"> This is the board name that we will add the task to</param>
            /// <param name="decription"> This is a decription for the task wrote by the user</param>
            /// <param name="duedate"> this is the due date for the task</param>
            /// </summary>
            /// <returns>An empty Resopne, unless an error occurs</returns>
            public string AddTask(string email, string boardname , string Title, string decription, DateTime duedate)
            {
                Response res;
                string json;
                try
                {
                    boardController.AddTask(email, boardname, Title, decription, duedate);
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
        /// This function move a task from a column to another.
        /// </summary>
        /// <param name="email" > This is the users email that we will use to know which board to use to move the task , and its unique</param>
        /// <param name="boardname"> This is the board name that the task will be moved in</param>
        /// <param name="columnOrdinal"> This is the column Ordinal that we want to change.</param>
        /// <param name="taskid">This is the task id that we want to work on</param>
        /// </summary>
        /// <returns>, unless an error occurs</returns>
        public string MoveTask(String email, String boardname, int columnOrdinal, int taskid)
            {
                Response res;
                string json;
                try
                {
                    boardController.MoveTask(email, boardname, columnOrdinal, taskid);
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
            /// This function updates a task title. 
            /// <param name="email" > This is the users email that we will use to know which board to use to change the task , and its unique</param>
            /// <param name="boardname"> This is the board name that the task will be changed in</param>
            /// <param name="taskid">This is the task id that we want to work on</param>
            /// <param name="columnOrdinal"> This is the column ordinal that we will use to locate the task</param>
            /// <param name="newTitle">the new title of the task</param> 
            /// </summary>
            /// <returns>An empty Resopne, unless an error occurs</returns>
            public string UpdateTaskTitle(string email, string boardname, int columnOrdinal, int taskid, string newTitle)
            {
                Response res;
                string json;
                try
                {
                    boardController.UpdateTaskTitle(email, boardname,columnOrdinal, taskid,newTitle);
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
            /// This function updates the description of a task.
            /// </summary>
            /// <param name="email" > This is the users email that the tasks belonge to, and its unique</param>
            /// <param name="boardname"> This is the board name that the task will be updated in</param>
            /// <param name="newDescription"> This is the new description we want to give for the task</param>
            /// <param name="columnOrdinal"> This is the column ordinal that we want to work on.</param>
            /// <param name="taskid">This is the task id that we will work on</param>
            /// </summary>
            /// <returns>The string "{}", unless an error occurs</returns>
            public string UpdateTaskDescription(string email, string boardname, int columnOrdinal, int taskid, string newDescription)
            {
                Response res;
                string json;
                try
                {
                    boardController.UpdateTaskDescription(email, boardname,columnOrdinal, taskid, newDescription);
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
            /// This function updates the duo date of a task.
            /// </summary>
            /// <param name="email" > This is the users email that the tasks belonge to, and its unique</param>
            /// <param name="boardname"> This is the board name that the task will be updated in</param>
            /// <param name="newDate"> This is the new duo date we want to set for the task</param>
            /// <param name="columnOrdinal"> This is the column ordinal that we want to work on.</param>
            /// <param name="taskid">This is the task id that we will work on</param>
            /// </summary>
            /// <returns>The string "{}", unless an error occurs</returns>
            public string UpdateTaskDueDate(string email, string boardname,  int columnOrdinal, int taskid, DateTime newDate)
            {
                Response res;
                string json;
                try
                {
                    boardController.UpdateTaskDueDate(email, boardname, columnOrdinal, taskid,newDate);
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
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A response with a list of IDs of all user's boards, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetUserBoards(string email)
        {
            Response res;
            string json;
            try
            {
                List<int> ids = boardController.GetUserBoards(email);
                int [] idsarry = ids.ToArray();
                res = new Response<int []>(idsarry);
            }catch (Exception ex)
            {
                res = new Response(ex.Message);

            }
            json = ToJson(res);
            return json;
        }

        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        /// <returns>A response with the board's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetBoardName(int boardId)
        {
            Response res;
            string json;
            try
            {
                string name = boardController.GetBoardName(boardId);
                res = new Response<string>(name,null);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);

            }
            json = ToJson(res);
            return json;
        }

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string JoinBoard(string email, int boardID)
        {
            Response res;
            string json;
            try
            {
                boardController.JoinBoard(email, boardID);
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
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LeaveBoard(string email, int boardID)
        {
            Response res;
            string json;
            try
            {
                boardController.LeaveBoard(email,boardID);
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
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            Response res;
            string json;
            try
            {
                boardController.TransferOwnership(currentOwnerEmail,newOwnerEmail,boardName);
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
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            Response res;
            string json;
            try
            {
                boardController.AssignTask(email,boardName,columnOrdinal,taskID,emailAssignee);
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);

            }
            json = ToJson(res);
            return json;
        }

        public string ToJson(object obj)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            string json = JsonSerializer.Serialize(obj,obj.GetType(),options);
            return json;
        }

        public BoardController getController()
        {
            return boardController;
        }

        public void adduser(string email)
        {
            boardController.addUser(email);
        }
        public void setstatus(string email,int i)
        {
            boardController.setstatus(email,i);
        }
    }

}
