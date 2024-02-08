using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer.Board;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;


namespace IntroSE.Kanban.Backend.BusinessLayer.Board
{
    /// <summary>
    /// This is a colum class and its used to organize the task according to the user.
    /// </summary>
   public  class Column
    {
        private int brdId;
        private string name;
        private int columnOrdinal;// backlog , inprogress, done indicator 0, 1 ,2 respectively
        private List<Task> tasks;
        private int taskslimit;




        private readonly TaskControllerDAL taskDal = new TaskControllerDAL();
        private readonly ColumnControllerDAL colDal = new ColumnControllerDAL();

        /// <summary>
        /// this is a constructor for the column class
        /// </summary>
        /// <param name="ColumnOrdinal">the column ordinal</param>
        /// <param name="name">the name we want to give for the column</param>
        /// <return>the function does not return anything</return>
        public Column(int ColumnOrdinal, string name, int brdid)
        {
            this.brdId = brdid;
            this.columnOrdinal = ColumnOrdinal;
            this.name = name;
            tasks = new List<Task>();
            taskslimit = -1;
        }

        /// <summary>
        /// this is a getter for the name of the column
        /// </summary>
        /// <returns>the name of the column</returns>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// C# setter and getter for the task limit 
        /// </summary>
        /// <returns>the limit of the column</returns>
        public int Tasklimit
        {
            get { return taskslimit; }
            set
            {
                taskslimit = value;
            } 
        }

        /// <summary>
        /// this is a setter for the column limit
        /// </summary>
        /// <param name="limit">the new limit we want to set to the column</param>
        /// <exception cref="Exception">if the column has more tasks than the limit</exception> 
        public void Limit(int limit) //limits number of tasks
        {
            if (limit == -1 || limit >= numOftsks())
            {
                this.taskslimit = limit;
                toDal().Save();

            }
            else { throw new Exception("Column contains tasks more than the limit!"); }
        }

        /// <summary>
        /// this function gets all the tasks in the column
        /// </summary>
        /// <returns>list of tasks in the column</returns>
        public List<Task> Tasks
        {
            get { return tasks; }
        }
        /// <summary>
        /// this function returns the number of tasks in column
        /// </summary>
        /// <returns>the number of tasks in the column</returns>
        public int numOftsks()
        {
            return tasks.Count();
        }

        /// <summary>
        /// this functions add a task to the column
        /// </summary>
        /// <param name="email">the users email who wantato add the task</param>
        /// <param name="Title">the title of the task </param>
        /// <param name="boardname">the board name than we want to add the task to</param>
        /// <param name="decription">the task decsription</param>
        /// <param name="duedate">the task duoDate</param>
        /// <param name="taskid">the task id </param>
        /// <param name="boardid">the board id the task in</param>
        /// <exception cref="Exception">if the column reached the limit</exception>
        public void addTask(string email, string Title, string boardname, string decription, DateTime duedate,int taskid,int boardid)
        {
            if (tasks.Count == taskslimit)
            {
                throw new Exception($"reached the maximum number of tasks in the column");
            }
            else
            {
                Task newtask = new Task(email, Title, decription, duedate, taskid, boardid);
                tasks.Add(newtask);
                taskDal.Insert(newtask.toDal());
            }
        }
        /// <summary>
        /// c# getter for the column ordinal
        /// </summary>
        public int ColumnOrdinal
        {
            get { return columnOrdinal; }
        }

        public void Prepare()
        {
            this.taskslimit = colDal.getTaskslimit(brdId, columnOrdinal);
            List<TaskDTO> tasks = taskDal.SelectcolsTasks(brdId, columnOrdinal);
            foreach(TaskDTO tsk in tasks)
            {
                this.tasks.Add(new Task(tsk));
            }

        }

        public ColumnDTO toDal()
        {
            return new ColumnDTO(brdId, ColumnOrdinal, Name, Tasklimit);
        }
    }
}
