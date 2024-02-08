using System;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
	public class ColumnDTO : DTO
	{

		public const string ColumnsBoardIdColumnName = "BoardID";
		public const string ColumnsOrdinalColumnName = "Ordinal";
		public const string ColumnsNameColumnName = "Name";
		public const string ColumnsLimitColumnName = "TasksLimit";



		private int _brdId;
		public int BoardID { get { return _brdId; } set { _brdId = value;} }


		private string _name;
		public string Name { get { return _name; } set { _name = value;} }

		private int _columnordinal;// backlog , inprogress, done indicator 0, 1 ,2 respectively
		public int  ColumnOrdinal{get {return _columnordinal;} set { _columnordinal = value; } }

		private int _taskslimit;
		public int Taskslimit { get { return _taskslimit;} set { _taskslimit = value; _controller.GetColumnDal().Update(_brdId, _columnordinal, ColumnsLimitColumnName, value); } }

		public ColumnDTO(int brdId,int Ordinal , string name , int limit) : base(new ColumnControllerDAL())
		{
			_columnordinal = Ordinal;
			_name = name;
			_brdId = brdId;
			_taskslimit = limit;
		}

		public void Save()
		{
			ColumnOrdinal = _columnordinal;
			Taskslimit = _taskslimit;
			Name = _name;
			BoardID = _brdId;
		}


	}
}

