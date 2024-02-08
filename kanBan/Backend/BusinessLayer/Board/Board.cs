using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer.Board
{
    /// <summary>
    /// This is a Board class to use for the kanpan board.
    /// <summary>
   public  class Board
    {
        private string userEmail;
        private string boardname;
        private int boardId;
        private int currtaskid;
        private List<Column> columns;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        private readonly ColumnControllerDAL columnDal = new ColumnControllerDAL();
        private readonly TaskControllerDAL taskDal = new TaskControllerDAL();

        /// <summary>
        /// This is a constructor for the Board Class.
        /// </summary>
        /// <param name="Email">this is the email for the user how created the board</param>
        /// <param name="boardname"> the board name we want to give for the board</param>
        /// <param name="id">the board id and its unique</param>
        /// <return>the function does not return anything</return>
        public Board(string Email, string boardname, int id)
        {
            this.boardId = id;
            this.userEmail = Email;
            this.boardname = boardname;
            this.currtaskid = 0;
            columns = new List<Column>();
            Column curr = new Column(0, "backlog", boardId);
            columns.Add(curr);
            columnDal.Insert(curr.toDal());
            curr = new Column(1, "in progress", boardId);
            columns.Add(curr);
            columnDal.Insert(curr.toDal());
            curr = new Column(2, "done", boardId);
            columns.Add(curr);
            columnDal.Insert(curr.toDal());
        }

        /// <summary>
        /// this is a constructor for the board calss
        /// </summary>
        /// <param name="board">board dto to build the  board from</param>
        public Board(BoardDTO board)
        {
            this.userEmail = board.UserEmail;
            this.boardname = board.BoardName;
            this.boardId = board.BoardId;
            this.currtaskid = board.Currtaskid;
            columns = new List<Column>();
            columns.Add(new Column(0, "backlog", boardId));
            columns.Add(new Column(1, "in progress", boardId));
            columns.Add(new Column(2, "done", boardId));
        }


        public BoardDTO toDal()
        {
            return new BoardDTO(BoardId, UserEmail, Boardname, currtaskid);
        }

        /// <summary>
        /// this function gets a column from a board
        /// </summary>
        /// <param name="Ordinal">the column ordinal we want to return</param>
        /// <returns> a column from the board</returns>
        public Column getCol(int Ordinal)
        {
            return columns[Ordinal];
        }

        /// <summary>
        /// this function gets a column name
        /// </summary>
        /// <param name="Ordinal">the column ordinal than we want to return the name of</param>
        /// <returns>the name of the column in the board</returns>
        public string getcolname(int Ordinal)
        {
            return columns[Ordinal].Name;
        }

        /// <summary>
        /// C# setter and getters for the userEmail
        /// </summary>
        public string UserEmail
        {
            get { return userEmail; }
            set { userEmail = value; }
        }

        /// <summary>
        /// C# setter and getters for the baord name
        /// </summary>
        public string Boardname
        {
            get { return boardname; }
            set { boardname = value; }
        }

        /// <summary>
        /// this function gets the limit of a column in the board
        /// </summary>
        /// <param name="Ordinal">the column ordinal we want to get the limit of</param>
        /// <returns>the limit of a column in the board</returns>
        public int getlimit(int Ordinal)
        {
            return columns[Ordinal].Tasklimit;
        }

        /// <summary>
        /// this function sets a limit to a column in the board
        /// </summary>
        /// <param name="limit">the limit we want to set for the column</param>
        /// <param name="Ordinal">the column ordinal we want to set the limit to</param>
        /// <return>the function does not return anything</return>
        public void limitcol(int limit , int Ordinal)
        {
            Column temp = columns[Ordinal];
            temp.Limit(limit);
        }

        /// <summary>
        /// this function sets a limit to all the columns in the board
        /// </summary>
        /// <param name="limit">the limit we want to set to all the columns</param>
        /// <return>the function does not return anything</return>
        public void limitAll(int limit)
        {
          foreach(Column col in columns)
            {
                col.Limit(limit);
            }
        }

        /// <summary>
        /// this function gets all the in progress task for the user in the board
        /// </summary>
        /// <returns>list of all the in progress task in this board</returns>
        public List<Task> InProgressTasks()
        {
            return columns[1].Tasks;
        }
        /// <summary>
        /// C# getters for the board id
        /// </summary>
        public int BoardId
        {
            get { return boardId; }
        }

        public void Prepare()
        {
            foreach(Column col in columns)
            {
                col.Prepare();
            }
        }
        /// <summary>
        /// C# setter and getters for the current task id
        /// </summary>
        public int Currtaskid
        {
            get { return currtaskid; }
            set { currtaskid = value; }
        }
        /// <summary>
        /// this function moves a tasks from a coulmn to the next one
        /// </summary>
        /// <param name="task">the task we want to move</param>
        /// <param name="columnordinal">the column ordinal the task in</param>
        /// <exception cref="Exception">if the coulmn has already reached its limit</exception>
        public void moveTask(Task task,int columnordinal)
        {
            List<Task> tasks=columns[columnordinal+1].Tasks;
            if(tasks.Count == getlimit(columnordinal + 1))
            {
                log.Warn("max amount of tasks in the column");
                throw new Exception("reached the maximum number of tasks in the column");
            }
            columns[columnordinal].Tasks.Remove(task);
            tasks.Add(task);
            log.Info("task moved successfully");
        }

        /// <summary>
        /// this function sets all the tasks who is the email is assignee to them in the baord and sets them to unassigned
        /// </summary>
        /// <param name="email">the email who want to leave the board</param>
        public void freeTasks(string email)
        {
            foreach(Column col in columns)
            {
                foreach(Task task in col.Tasks)
                {
                    if (task.Assignee.Equals(email)) { task.Assignee = "unassigned"; task.toDal().Assignee = "unassigned"; }
                }
            }
        }
    }
}
