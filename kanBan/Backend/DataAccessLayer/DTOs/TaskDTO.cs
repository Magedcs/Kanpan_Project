using System;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
	public class TaskDTO : DTO
	{

		public const string TasksBoardIDColumnName = "BoardID";

		public const string TasksOrdColumnName = "Ordinal";

		public const string TasksIDColumnName = "TaskID";

		public const string TasksEmailColumnName = "Email";

		public const string TasksTitleColumnName = "Title";

		public const string TasksDescreptionColumnName = "Descreption";

		public const string TaskDueDateColumnName = "DueDate";

		public const string TasksCreationTimeColumnName = "CreationTime";


    	private int _taskId;
		public int TaskId { get { return _taskId; } set { _taskId = value; _controller.GetTaskDal().Update(_taskId, _boardID, TasksIDColumnName, value); } }

		private int _boardID;
		public int BoardID { get { return _boardID; } set { _boardID = value; _controller.GetTaskDal().Update(_taskId, _boardID, TasksBoardIDColumnName, value); } }

		private string _assignee;
		public string Assignee { get { return _assignee; } set { _assignee = value; _controller.GetTaskDal().Update(_taskId,_boardID,TasksEmailColumnName, value); } }

		private string _title;
		public string Title { get { return _title; } set { _title = value;_controller.GetTaskDal().Update(_taskId, _boardID, TasksTitleColumnName, value); } }

		private string _descreption;
		public string Descreption { get { return _descreption; } set {_descreption = value; _controller.GetTaskDal().Update(_taskId, _boardID, TasksDescreptionColumnName, value); } }

		private  DateTime _creationTime;
		public DateTime CreationTime { get { return _creationTime; } set { _creationTime = value; _controller.GetTaskDal().Update(_taskId, _boardID, TasksCreationTimeColumnName, value.ToString()); } }

		private DateTime _dueDate;
		public DateTime DueDate { get { return _dueDate; } set { _dueDate = value; _controller.GetTaskDal().Update(_taskId, _boardID, TaskDueDateColumnName, value.ToString()); } }

		private int _columnOrdinal;
		public int ColumnOrdinal { get { return _columnOrdinal; } set { _columnOrdinal = value; _controller.GetTaskDal().Update(_taskId, _boardID, TasksOrdColumnName, value); } }
		public TaskDTO(int boardId,int Ordinal,  int taskid, string assignee , string title , string descreption, DateTime date , DateTime CT  ) : base (new TaskControllerDAL())
		{ 
			_taskId = taskid;
			_boardID = boardId;
			_assignee = assignee;
			_title = title;	
			_descreption = descreption;
			_creationTime = CT;
			_dueDate = date;
			_columnOrdinal = Ordinal;
		}

		public void Save()
        {
			TaskId = _taskId;
			BoardID = _boardID;
			Assignee = _assignee;
			Title = _title;
			Descreption = _descreption;
			CreationTime = _creationTime;
			DueDate = _dueDate;
			ColumnOrdinal = _columnOrdinal;
		}
	}
}

