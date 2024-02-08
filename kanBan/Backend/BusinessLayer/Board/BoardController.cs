using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.Backend.BusinessLayer.Board
{
    /// <summary>
    /// This our board manager, it manages all the new/existing boards of the users!
    /// </summary>
    public class BoardController
    {
        private Dictionary<int, Board> boards;
        private Dictionary<string, List<Board>> usersboards;
        private Dictionary<string, bool> userStatus;
        private int boardId;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        private readonly UserControllerDAL userDal;
        private readonly BoardControllerDAL boardDal;
        private readonly ColumnControllerDAL columnDal;
        private readonly TaskControllerDAL taskDal;



     
        /// <summary>
        /// this is a constructor for board Controller
        /// </summary>
        public BoardController()
        {

            userDal = new UserControllerDAL();
            boardDal = new BoardControllerDAL();
            columnDal = new ColumnControllerDAL();
            taskDal = new TaskControllerDAL();
          
          
            boards = new Dictionary<int, Board>();
            usersboards = new Dictionary<string, List<Board>>();
            userStatus = new Dictionary<string, bool>();
            boardId = 0;
        }

        /// <summary>
        /// this function loads the data from the data base
        /// </summary>
        public void LoadData()
        {
            LoadLists();
            LoadBoards();
            LoadStatus();
        }
        /// <summary>
        /// this function delete all the data from the data base and the dictionarys
        /// </summary>
        public void DeleteData()
        {
            boardDal.DeleteAll();
            columnDal.DeleteAll();
            taskDal.DeleteAll();
            boards = new Dictionary<int, Board>();
            usersboards = new Dictionary<string, List<Board>>();
            userStatus = new Dictionary<string, bool>();
        }
        /// <summary>
        /// this function loads all the lists from the data base
        /// </summary>
        public void LoadLists()
        {
            this.boardId = boardDal.BCidCounter();
            List<UserDTO> users = userDal.allusers();
            foreach(UserDTO u in users)
            {
                List<Board> usrboards = new List<Board>();
                string email = u.Email;
                List<BoardDTO> boardDTOs = boardDal.Getusersbrdslist(email);
                foreach(BoardDTO dalBoardobj in boardDTOs)
                {
                    Board curr = new Board(dalBoardobj);
                    curr.Prepare();
                    usrboards.Add(curr);
                }
                usersboards.Add(email, usrboards);
            }
        }

        //loads users status
        public void LoadStatus()
        {
            List<UserDTO> users = userDal.allusers();
            foreach(UserDTO user in users)
            {
                userStatus.Add(user.Email, false);
            }
        }
        //loads boards
        public void LoadBoards()
        {
            List<BoardDTO> boards = boardDal.allboards();
            foreach(BoardDTO board in boards)
            {
                Board curr = new Board(board);
                curr.Prepare();
                this.boards.Add(board.BoardId, curr);
            }

        }


        /// <summary>
        /// This function checks if a user board can be added if so ... add the board
        /// </summary>
        /// <param name="email" > This is the users email that we will add the Board to ,And its unique</param>
        /// <param name="boardname"> This is the board name that we want to give to the new Board we created</param>
        /// <exception cref="Exception">if the email is invalid </exception>
        /// <exception cref="Exception">if the board name is null or empty</exception>
        /// <exception cref="Exception">if the user is offline</exception>
        /// <exception cref="Exception">if the boardname exsist already </exception>
        /// <exception cref="Exception">if the email is not registered</exception>
        /// <return> this function does not return anything</return>
        public void AddBoard(string email, string boardname)
        {
            if (string.IsNullOrWhiteSpace(boardname))
            {
                throw new Exception("boardname not valid");
            }
            if (checkEmail(email, "attempt to add a board"))
            {
                if (usersboards.ContainsKey(email))
                {
                    List<Board> boards = usersboards[email];
                    foreach (Board board in boards)
                    {
                        if (board.Boardname.Equals(boardname))
                        {
                            log.Warn("adding aboard with an exsiting boardname");
                            throw new Exception("The given boardname already exists!");
                        }
                    }        
                    log.Info("board added to user successfully");
                    Board toadd = new Board(email, boardname, boardId);
                    boards.Add(toadd);
                    this.boards.Add(boardId, toadd);
                    boardDal.Insert(toadd.toDal());
                    boardDal.Insert(email, toadd.BoardId);
                    boardId += 1;    
                    return;
                }
                log.Warn("attempt to add a board to a non registered Email");
                throw new Exception("Email not registered!");
            }
        }

        /// <summary>
        /// This function removes a board from the useus boards.
        /// </summary>
        /// <param name="email" > This is the users email that we will add the Board to ,And its unique</param>
        /// <param name="boardname"> This is the board name that we want to remove</param>
        /// <exception cref="Exception">if the email is invalid </exception>
        /// <exception cref="Exception">if the email is not the owner of the board</exception>
        /// <exception cref="Exception">if the user is offline</exception>
        /// <exception cref="Exception">if the board does not exsist</exception>
        /// <return> this function does not return anything</return>
        public void RemoveBoard(string email, string boardname)
        {
            if (checkEmail(email, "attempt to remove a board using"))
            {
                Board toremove = GetBoard(email, boardname);
                if (!toremove.UserEmail.Equals(email))
                {
                    log.Warn("the user is not the owner of the board");
                    throw new Exception("the user is not the owner of the board");
                }
                boards.Remove(toremove.BoardId);
                List<string> keys = new List<string>(usersboards.Keys);
                foreach (string key in keys)
                {
                    usersboards[key].Remove(toremove);
                }
                boardDal.DeleteJoinees(toremove.BoardId);
                boardDal.Delete(toremove.BoardId);
                columnDal.Delete(toremove.BoardId);
                taskDal.Delete(toremove.BoardId);
                log.Info("board removed");
                return;
            }
        }

        /// <summary>
        /// This function sets a limited (max) number of tasks to a column.
        /// </summary>
        /// <param name="email">This is the users email that the column belonge to, and its unique</param>
        /// <param name="boardname">This is the board name that the column will be in</param>
        /// <param name="columnOrdinal">This is the column ordinal that we want to work on</param>
        /// <param name="limit">This is the number of the maximum tasks in the column</param>
        /// <exception cref="Exception">if the email is invalid </exception>
        /// <exception cref="Exception">if the user is offline</exception>
        /// <exception cref="Exception">if the boardname does not exsist </exception>
        /// <exception cref="Exception">if the email is not registered</exception>
        /// <return> this function does not return anything</return>
        public void LimitColumn(string email, string boardname, int columnOrdinal, int limit)
        {
            checkOrd(columnOrdinal);
            if (checkEmail(email, "attempt to limit a column using a"))
            {
                Board board = GetBoard(email, boardname);
                board.limitcol(limit, columnOrdinal);
            }
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardname">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <exception cref="Exception">if the column has no limit</exception>
        /// <exception cref="Exception">if the email is not valid</exception>
        /// <exception cref="Exception">if the user is offline</exception>
        /// <exception cref="Exception">if the email is not registered</exception>
        /// <returns> int with the column limit</returns>
        public int GetColumnLimit(string email, string boardname, int columnOrdinal)
        {
            checkOrd(columnOrdinal);
            if (checkEmail(email, "attempt to get a column limit using an"))
            {
                Board board = GetBoard(email, boardname);
                return board.getlimit(columnOrdinal);
            }
            return -50;
        }


        /// <summary>
        /// This function shows the user all the in progress tasks he assignned to. 
        /// <param name="email" > This is the users email that we will show the task to,and its unique</param>
        /// </summary>
        /// <exception cref="Exception">if the email is not valid</exception>
        /// <exception cref="Exception">if the user is offline</exception>
        /// <exception cref="Exception">if the email is not registered</exception>
        /// <returns>List of all in progress task to the user</returns>
        public List<Task> InProgessTasks(string email)
        {
            List<Task> InProgress = new List<Task>();
            if (checkEmail(email, "attempt to list tasks using an"))
            {
                if (usersboards.ContainsKey(email))
                {
                    List<Board> boards = usersboards[email];
                    foreach (Board board in boards)
                    {
                        List<Task> InProgtemp = board.InProgressTasks();
                        foreach (Task task in InProgtemp)
                        {
                            if (task.Assignee.Equals(email))
                            {
                                InProgress.Add(task);
                            }
                        }
                    }
                }
                else { log.Warn("attempt to list tasks from a non registered email"); throw new Exception("email is not registered"); }
            }
            return InProgress;
        }

        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardname">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <exception cref="Exception">if the email is not valid</exception>
        /// <exception cref="Exception">if the user is offline</exception>
        /// <exception cref="Exception">if the email is not registered</exception>
        /// <exception cref="Exception">if the board does not exsist</exception>
        /// <returns>string with column name value</returns>
        public string GetColumnName(string email, string boardname, int columnOrdinal)
        {
            checkOrd(columnOrdinal);
            if (checkEmail(email, "attempt to get a column name using a"))
            {
                Board board = GetBoard(email, boardname);
                return board.getcolname(columnOrdinal);
            }
            return null;
        }

        /// <summary>
        /// This method gets column of a specific board
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardname">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <exception cref="Exception">if the email is not valid</exception>
        /// <exception cref="Exception">if the user is offline</exception>
        /// <exception cref="Exception">if the email is not registered</exception>
        /// <exception cref="Exception">if the board does not exsist</exception>
        /// <returns>List of task in the column</returns>
        public List<Task> GetColumn(string email, string boardname, int columnOrdinal)
        {
            checkOrd(columnOrdinal);
            if (checkEmail(email, "attempt to get column's tasks using a"))
            {
                Board board = GetBoard(email, boardname);
                return board.getCol(columnOrdinal).Tasks;
            }
            return null;
        }


        /// <summary>
        /// This function adds a new task to the users Board . 
        /// <param name="email" > This is the users email thah we will add the task to ,and its unique</param>
        /// <param name="Title">This is the Title we will set for the task after adding it</param>
        /// <param name="boardname"> This is the board name that we will add the task to</param>
        /// <param name="decription"> This is a decription for the task wrote by the user</param>
        /// <param name="duedate"> this is the due date for the task</param>
        /// </summary>
        /// <exception cref="Exception">if the email is invalid</exception>
        /// <exception cref="Exception">if the email is not the assignee of the task</exception>
        /// <exception cref="Exception">if the user is offline</exception>
        /// <exception cref="Exception">if the board does not exsist</exception>
        /// <exception cref="Exception>">if the user is not registered</exception>
        /// <returns>the function does not return anything</returns>
        public void AddTask(string email, string boardname, string Title, string decription, DateTime duedate)
        {
            if (checkEmail(email, "attempt to add a task to"))
            {
                Board board = GetBoard(email, boardname);
                Column column = board.getCol(0);
                column.addTask(email, Title, boardname, decription, duedate, board.Currtaskid, board.BoardId);
                board.Currtaskid += 1;
                board.toDal().Save();
                log.Info("Task added to the board successfully");
            }
        }


        /// <summary>
        /// This function move a task from a column to another. 
        /// <param name="email" > This is the users email that we will use to know which board to use to move the task , and its unique</param>
        /// <param name="boardname"> This is the board name that the task will be moved in</param>
        /// <param name="columnOrdinal"> This is the column Ordinal that we want to change.</param>
        /// <param name="taskid">This is the task id that we want to work on</param>
        /// </summary>
        /// <exception cref="Exception">if the email is invalid</exception>
        /// <exception cref="Exception">if the email is not the assignee of the task</exception>
        /// <exception cref="Exception">if the user is offline</exception>
        /// <exception cref="Exception">if the board does not exsist</exception>
        /// <exception cref="Exception">if the column reach its limit</exception>
        /// <exception cref="Exception">if task id does not exsist</exception>
        /// <exception cref="Exception">if the task is done</exception>
        /// <exception cref="Exception>">if the user is not registered</exception>
        /// <return>the function does not return anything</return>
        public void MoveTask(string email, string boardname, int columnOrdinal, int taskid)
        {
            checkOrd(columnOrdinal);
            if (checkEmail(email, "attempt to move a task to"))
            {
                if (columnOrdinal != 2)
                {
                    Board board = GetBoard(email, boardname);
                    Task task = GetTask(email, boardname, columnOrdinal, taskid);
                    if (task.Assignee.Equals(email))
                    {
                        board.moveTask(task, columnOrdinal);
                        task.ColumnOrdinal += 1;
                        task.toDal().Save();
                        return;
                    }
                    log.Warn("the user is not the assignee of the task");
                    throw new Exception("the user is not the assignee of the task");
                }
                else
                {
                    log.Warn("cant move a task that is done");
                    throw new Exception("cant move a task that is done");
                }
            }
        }


        /// <summary>
        /// This function update a task title. 
        /// <param name="email" > This is the users email that we will use to know which board to use to change the task , and its unique</param>
        /// <param name="boardname"> This is the board name that the task will be changed in</param>
        /// <param name="taskid">This is the task id that we want to work on</param>
        /// <param name="columnOrdinal"> This is the column ordinal that we will use to locate the task</param>
        /// <param name="newTitle">the new title of the task</param>
        /// </summary>
        /// <exception cref="Exception">if the email is invalid</exception>
        /// <exception cref="Exception">if the user is offline</exception>
        /// <exception cref="Exception">if the email is not the assignee of the task</exception>
        /// <exception cref="Exception">if the board does not exsist</exception>
        /// <exception cref="Exception">if task id does not exsist</exception>
        /// <exception cref="Exception">if the task is done</exception>
        /// <exception cref="Exception>">if the user is not registered</exception>
        /// <return>the function does not return anything</return>
        public void UpdateTaskTitle(string email, string boardname, int columnOrdinal, int taskid, string newTitle)
        {
            checkOrd(columnOrdinal);
            if (checkEmail(email, "attempt to update a task title"))
            {
                Task task = GetTask(email, boardname, columnOrdinal, taskid);
                if (task.Assignee.Equals(email))
                {
                    if (columnOrdinal != 2)
                    {
                        task.Title = newTitle;
                        task.toDal().Title = newTitle;
                        return;
                    }
                    log.Warn("task is done should not be changed");
                    throw new Exception("cant change a task that is done");
                }
                log.Warn("the user is not the assignee of the task");
                throw new Exception("the user is not the assignee of the task");
            }
        }
        /// <summary>
        /// This function updates the description of a task. 
        /// <param name="email" > This is the users email that the tasks belonge to, and its unique</param>
        /// <param name="boardname"> This is the board name that the task will be updated in</param>
        /// <param name="newDescription"> This is the new description we want to give for the task</param>
        /// <param name="columnOrdinal"> This is the column ordinal that we want to work on.</param>
        /// <param name="taskid">This is the task id that we will work on</param>
        /// </summary>
        /// <exception cref="Exception">if the email is invalid</exception>
        /// <exception cref="Exception">if the user is offline</exception>
        /// <exception cref="Exception">if the email is not the assignee of the task</exception>
        /// <exception cref="Exception">if the board does not exsist</exception>
        /// <exception cref="Exception">if task id does not exsist</exception>
        /// <exception cref="Exception">if the task is done</exception>
        /// <exception cref="Exception>">if the user is not registered</exception>
        /// <return>the function does not return anything</return>
        public void UpdateTaskDescription(string email, string boardname, int columnOrdinal, int taskid, string newDescription)
        {
            checkOrd(columnOrdinal);
            if (checkEmail(email, "attempt to update a task decsription"))
            {
                Task task = GetTask(email, boardname, columnOrdinal, taskid);
                if (task.Assignee.Equals(email))
                {
                    if (columnOrdinal != 2)
                    {
                        task.Description = newDescription;
                        task.toDal().Descreption = newDescription;
                        return;
                    }
                    log.Warn("task is done should not be changed");
                    throw new Exception("cant change a task that is done");
                }
                log.Warn("the user is not the assignee of the task");
                throw new Exception("the user is not the assignee of the task");
            }
        }

        /// <summary>
        /// This function updates the duo date of a task. 
        /// <param name="email" > This is the users email that the tasks belonge to, and its unique</param>
        /// <param name="boardname"> This is the board name that the task will be updated in</param>
        /// <param name="newDate"> This is the new duo date we want to set for the task</param>
        /// <param name="columnOrdinal"> This is the column ordinal that we want to work on.</param>
        /// <param name="taskid">This is the task id that we will work on</param>
        /// </summary>
        /// <exception cref="Exception">if the email is invalid</exception>
        /// <exception cref="Exception">if the user is offline</exception>
        /// <exception cref="Exception">if the email is not the assignee of the task</exception>
        /// <exception cref="Exception">if the board does not exsist</exception>
        /// <exception cref="Exception">if task id does not exsist</exception>
        /// <exception cref="Exception">if the task is done</exception>
        /// <exception cref="Exception>">if the user is not registered</exception>
        /// <return>the function does not return anything</return> 
        public void UpdateTaskDueDate(string email, string boardname, int columnOrdinal, int taskid, DateTime newDate)
        {
            checkOrd(columnOrdinal);
            if(checkEmail(email, "attempt to update a task duoDate"))
            {
                Task task = GetTask(email, boardname, columnOrdinal, taskid);
                if (task.Assignee.Equals(email))
                {
                    if (columnOrdinal != 2)
                    {
                        task.DueDate = newDate;
                        task.toDal().DueDate = newDate;
                        return;
                    }
                    log.Warn("task is done should not be changed");
                    throw new Exception("cant change a task that is done");
                }
                log.Warn("the user is not the assignee of the task");
                throw new Exception("the user is not the assignee of the task");
            }
        }
        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email">the email we want to return the list of boards id for</param>
        /// <returns>A rlist of IDs of all user's boards, unless an error occurs</returns>
        public List<int> GetUserBoards(string email)
        {
            if (!ValidateEmail(email))
            {
                log.Warn("attempt to get the users boards with an invalid email");
                throw new Exception("The email is not valid!");
            }
            if (usersboards.ContainsKey(email))
            {
                List<int> output = new List<int>();
                List<Board> boards = usersboards[email];
                foreach (Board board in boards)
                {
                    output.Add(board.BoardId);
                }
                return output;
            }
            log.Warn("attempt to get list of ids using a non registered email");
            throw new Exception("User email is not registered");
        }

        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        /// <returns>A string with the board's name, unless an error occurs</returns>
        public string GetBoardName(int boardId)
        {
            if (boards.ContainsKey(boardId))
            {
                return boards[boardId].Boardname;
            }
            log.Warn("board does not exists");
            throw new Exception("board does not exists");
        }

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs</returns>
        public void JoinBoard(string email, int boardID)
        {
            if(checkEmail(email , "JoinBoard is used"))
            {
                if (boards.ContainsKey(boardID))
                {
                    Board b=boards[boardID];
                    List<Board> brds = usersboards[email];
                    foreach (Board board in brds)
                    {
                        if (board.Boardname.Equals(b.Boardname))
                        {
                            if (board.BoardId == b.BoardId)
                            {
                                log.Warn("you already are a member of the board");
                                throw new Exception("cant join an already joined board");
                            }
                            else
                            {
                                log.Warn("you already are a member of the board");
                                throw new Exception("cant join a board with the same name of an already joined board");
                            }
                        }
                    }
                    usersboards[email].Add(boards[boardID]);
                    boardDal.Insert(email, boardID);
                    return;
                }
                log.Warn("attempt to joinboard using a non valid boardID");
                throw new Exception("Board does not exist!");
            }
        }

        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public void LeaveBoard(string email, int boardID)
        {
            if (checkEmail(email,"using LeaveBoard"))
            {
                if (boards.ContainsKey(boardID))
                {
                    if (boards[boardID].UserEmail.Equals(email))
                    {
                        log.Warn("a board owner attempted to leave the board");
                        throw new Exception("Board owner cannot leave the board!");
                    }
                    Board toLeave = boards[boardID];
                    toLeave.freeTasks(email);
                    usersboards[email].Remove(toLeave);
                    boardDal.Delete1Joinee(email,boardID);
                    return;
                }
                log.Warn("attempt to leave a non existent board");
                throw new Exception("Board doesnt exist!");
            }
        }

        /// <summary>
        /// This method transfers a board ownership.
        /// </summary>
        /// <param name="currentOwnerEmail">Email of the current owner. Must be logged in</param>
        /// <param name="newOwnerEmail">Email of the new owner</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            if(checkEmail(currentOwnerEmail, "attempt to set a task assignee") & ValidateEmail(newOwnerEmail))
            {
                Board board=GetBoard(currentOwnerEmail, boardName);
                if (board.UserEmail.Equals(currentOwnerEmail))
                {
                    if (usersboards[newOwnerEmail].Contains(board))
                    {
                        board.UserEmail = newOwnerEmail;
                        board.toDal().Save();
                        return;
                    }
                    log.Warn("the new owner is not a member in the board");
                    throw new Exception("the new owner is not a member in the board");
                }
                log.Warn("the user is not the owner of the board");
                throw new Exception("the user is not the owner of the board");
            }
            log.Warn("attempt to transfer the owner ship from or to an invalid email");
            throw new Exception("The email is not valid!");
        }


        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public void AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            checkOrd(columnOrdinal);
            if (checkEmail(email, "cant assigne a task from a") & ValidateEmail(emailAssignee))
            {
                if (columnOrdinal != 2)
                {
                    Board board = GetBoard(email, boardName);
                    Task task = GetTask(email, boardName, columnOrdinal, taskID);
                    if (usersboards[emailAssignee].Contains(board))
                    {
                        if (task.Assignee.Equals("unassigned"))
                        {
                            task.Assignee = emailAssignee;
                            task.toDal().Assignee = emailAssignee;
                            return;
                        }
                        else
                        {
                            if (task.Assignee.Equals(email))
                            {
                                task.Assignee = emailAssignee;
                                task.toDal().Assignee = emailAssignee;
                                return;
                            }
                            log.Warn("this user is not the assignee of the task");
                            throw new Exception("The user is not the assigne of this task");
                        }
                    }
                    log.Warn("the email assignee is not a member of the board");
                    throw new Exception("the email assignee is not a member of the board");
                }
                log.Warn("cant change the assignee of a task that is done");
                throw new Exception("cant change a task assignee that is done");
            }
            log.Warn("attempt to set a task assignee to an invalid email");
            throw new Exception("The email is not valid!");
        }


        /// <summary>
        /// this function checks if the email is valid
        /// </summary>
        /// <param name="email">the email we have to check</param>
        /// <returns>true if the email is valid</returns>
        /// <exception cref="Exception">if the email is not valid</exception>
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
        /// this function checks if the useu is online
        /// </summary>
        /// <param name="email">the users email we have to check</param>
        /// <exception cref="Exception">if the user is not registered</exception>
        /// <returns>true if the user is online otherwise false</returns>
        public bool checkStatus(string email)//or user...
        {
            if (userStatus.ContainsKey(email))
            {
                return userStatus[email];
            }
            throw new Exception("User is not registered");
        }
        /// <summary>
        /// this function check if the column ordinal is valid
        /// </summary>
        /// <param name="a"></param>
        /// <exception cref="Exception">if the column ordinal is valid</exception>
        public void checkOrd(int a)
        {
            if (a < 0 || a > 2) { throw new Exception("invalid column ordinal!"); }
        }


        /// <summary>
        /// this function adds a user to the dictionary
        /// </summary>
        /// <param name="email">the eamil we want to add</param>
        public void addUser(string email)
        {
            usersboards.Add(email, new List<Board>());
            userStatus.Add(email, true);
        }



        /// <summary>
        /// this function sets the status of a user
        /// </summary>
        /// <param name="email">the email of the user</param>
        /// <param name="a">1,0 to know what to set the status</param>
        public void setstatus(string email, int a)
        {
            if (a == 1)
            {
                userStatus[email] = true;
            }
            else
            {
                userStatus[email] = false;
            }
        }


        /// <summary>
        /// this function checks the email 
        /// </summary>
        /// <param name="email">the email we want to check</param>
        /// <param name="ex">the exception we will print if some error occurs</param>
        /// <returns>ture if email is valid and online false otherwise</return>
        public bool checkEmail(string email, string ex)
        {
            if (!ValidateEmail(email))
            {
                log.Warn($"{ex} to an invalid email");
                throw new Exception($"The email is not valid!");
            }
            if (!checkStatus(email))
            {
                log.Warn($"{ex} to an offline user");
                throw new Exception($"User is not logged in!");
            }
            return true;
        }


        /// <summary>
        /// this function gets a board
        /// </summary>
        /// <param name="email">the email who wants the board</param>
        /// <param name="Boardname">the board name</param>
        /// <returns>a board</returns>
        /// <exception cref="Exception">if the board does npt exsists</exception>
        public Board GetBoard(string email, string Boardname)
        {
            if (usersboards.ContainsKey(email))
            {
                List<Board> boards = usersboards[email];
                foreach (Board board in boards)
                {
                    if (board.Boardname.Equals(Boardname))
                    {
                        return board;
                    }
                }
                log.Warn("Board name is not in the dictionary");
                throw new Exception("Board does not exsist");
            }
            log.Warn("the user is not registered");
            throw new Exception("user not Registered");
        }


        /// <summary>
        /// this function gets a task
        /// </summary>
        /// <param name="email">the user email</param>
        /// <param name="Boardname">the boardname </param>
        /// <param name="columnordinal">the column ordinal the task in</param>
        /// <param name="taskid">the task id we want to return</param>
        /// <returns>a task</returns>
        /// <exception cref="Exception">if the task does not exsists</exception>
        public Task GetTask(string email, string Boardname, int columnordinal, int taskid)
        {
            Board b = GetBoard(email, Boardname);
            Column column = b.getCol(columnordinal);
            List<Task> tasks = column.Tasks;
            foreach (Task task in tasks)
            {
                if (task.Taskid == taskid)
                {
                    return task;
                }
            }
            log.Warn("task id does not exsist");
            throw new Exception("task does not exsist");
        }
    }
}
