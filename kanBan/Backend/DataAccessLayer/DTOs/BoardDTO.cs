using System;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
	public class BoardDTO : DTO
	{
		public const string BoardsIDColumnName = "BoardID";
		public const string BoardsEmailColumnName = "Email";
		public const string BoardsNameColumnName = "BoardName";
		public const string BoardstaskcurrColumnName = "TaskIDcounter";


		private string _userEmail;
		public string UserEmail { get { return _userEmail; } set { _userEmail = value; _controller.GetBoardDal().Update(_boardId, BoardsEmailColumnName, value); } }

		private string _boardname;
		public string BoardName { get { return _boardname; } set { _boardname = value; _controller.GetBoardDal().Update(_boardId, BoardsNameColumnName, value);} }

		private int _boardId;
		public int BoardId { get { return _boardId; } set { _boardId = value;} }

		private int _currtaskid;
		public int Currtaskid { get { return _currtaskid; } set { _currtaskid = value; _controller.GetBoardDal().Update(_boardId, BoardstaskcurrColumnName, value.ToString()); } }
		public BoardDTO(int brdid, string email, string brdname, int currtaskid) : base (new BoardControllerDAL())
		{
			_boardId = brdid;
			_boardname = brdname;
			_userEmail = email;
			_currtaskid = currtaskid;
		}

		public void Save()
        {
			UserEmail = _userEmail;
			BoardName = _boardname;
			BoardId = _boardId;
			Currtaskid = _currtaskid;
        }
	}
}

