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
    /// This is a task class and it allows the user to make new tasks for the board. 
    /// </summary>
    public class Task
    {
        private int taskId;
        private int boardID;
        private string assignee;
        private string title;
        private string description;
        private readonly DateTime creationTime;
        private DateTime dueDate;
        private int columnOrdinal;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const int MaxTitle = 50;
        private const int MaxDesc = 300;


        /// <summary>
        /// this is a constructor for the task class
        /// </summary>
        /// <param name="userEmail">the user email who created the task</param>
        /// <param name="Title">the task title</param>
        /// <param name="Description">the task decsription</param>
        /// <param name="duoDate">the task duoDate</param>
        /// <param name="taskid">the task id and its unique</param>
        public Task(string userEmail, string Title, string Description, DateTime duoDate, int taskid, int BoardID)
        {
            this.assignee = "unassigned";
            this.creationTime = DateTime.Now;
            this.taskId = taskid;
            this.columnOrdinal = 0;
            this.boardID = BoardID;
            this.Title = Title;
            this.Description = Description;
            DueDate = duoDate;
        }
        /// <summary>
        /// this is a constructor for the task class
        /// </summary>
        /// <param name="task">a task dto</param>
        public Task(TaskDTO task)
        {
            this.assignee = task.Assignee;
            this.creationTime = task.CreationTime;
            this.taskId = task.TaskId;
            this.columnOrdinal = task.ColumnOrdinal;
            this.boardID = task.BoardID;
            this.Title = task.Title;
            this.Description = task.Descreption;
            DueDate = task.DueDate;
        }

        public TaskDTO toDal()
        {
            return new TaskDTO(BoardID, ColumnOrdinal, Taskid, Assignee, Title, Description, dueDate, creationTime);
        }

        /// <summary>
        /// C# getter for the task id
        /// </summary>
        /// <returns>the task id</returns>
        public int Taskid
        {
            get { return this.taskId; }
        }

        /// <summary>
        /// C# setter and getter for the task title
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { if (checktitle(value))
                {
                    this.title = value;
                }
            }
        }

        /// <summary>
        /// C# setter and getter for the task decsription
        /// </summary>
        public string Description
        {
            get { return this.description; }
            set {
                if (checkdesc(value))
                {
                    this.description = value;
                }
            }
        }

        /// <summary>
        /// C# setter and getter for the task dueDate
        /// </summary>
        public DateTime DueDate
        {
            get { return dueDate; }
            set { if (checkdtae(value))
                {
                    this.dueDate = value;
                }
            }
        }

        /// <summary>
        /// C# setter and getter for the column ordinal
        /// </summary>
        public int ColumnOrdinal
        {
            get { return columnOrdinal; }
            set
            {
                columnOrdinal = value;
            }
        }
        /// <summary>
        /// C# getter for the task creation time
        /// </summary>
        public DateTime creation() { return creationTime; }
        /// <summary>
        /// C# getter for the baord id
        /// </summary>
        public int BoardID
        {
            get { return boardID; }
        }
        /// <summary>
        /// C# setter and getter for the task assignee
        /// </summary>
        public string Assignee
        {
            get { return assignee; }
            set
            {
                assignee = value;
            }
        }


        /// <summary>
        /// this function checks if a title is valid
        /// </summary>
        /// <param name="title">the title we want to check</param>
        /// <returns>ture if the title is valid otherwise false</returns>
        /// <exception cref="Exception">if the title is not valid</exception>
        public bool checktitle(string title)
        {
            if (title is null)
            {
                log.Warn("title is null");
                throw new Exception("title is null");
            }
            if (title.Length == 0)
            {
                log.Warn("title sould not be empty");
                throw new Exception("title is empty");
            }
            if (title.Length > MaxTitle)
            {
                log.Warn("title sould not be more than 50 in length");
                throw new Exception("title length more than 50");
            }
            return true;
        }
        /// <summary>
        /// this function checks if a description is valid
        /// </summary>
        /// <param name="desc">the desc we want to check</param>
        /// <returns>ture if the desc is valid otherwise false</returns>
        /// <exception cref="Exception">if the desc is not valid</exception>
        public bool checkdesc(string desc)
        {
            if (desc == null)
            {
                log.Warn("decsription is null");
                throw new Exception("decsription is null");
            }
            if (desc.Length > MaxDesc)
            {
                log.Warn("description sould not be more than 300 in length");
                throw new Exception("description length more than 300");
            }
            return true;
        }
        /// <summary>
        /// this function checks if a date is valid
        /// </summary>
        /// <param name="title">the date we want to check</param>
        /// <returns>ture if the date is valid otherwise false</returns>
        /// <exception cref="Exception">if the date is not valid</exception>
        public bool checkdtae(DateTime date)
        {
            if (date == null)
            {
                log.Warn("date is null");
                throw new Exception("date is null");
            }
            if (DateTime.Now > date)
            {
                log.Warn("the date sould not be expired");
                throw new Exception("duoDate is expired");
            }
            return true;
        }
    }
}
