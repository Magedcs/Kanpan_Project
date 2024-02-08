using System;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
	public abstract class ControllerDAL
	{
		protected readonly string _connection;
		private readonly string _table;
		protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public ControllerDAL(string table)
		{
			string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
			this._connection = $"Data Source={path}; Version=3";
			_table = table;
		}

		public UserControllerDAL GetUserDal()
		{
			return new UserControllerDAL();
		}
		public BoardControllerDAL GetBoardDal()
		{
			return new BoardControllerDAL();
		}
        public ColumnControllerDAL GetColumnDal()
        {
            return new ColumnControllerDAL();
        }
        public TaskControllerDAL GetTaskDal()
		{
			return new TaskControllerDAL();
		}



		/// <summary>
		/// converts SQLiteReader to the corresponding DTO object so we can handle it and deal with it!
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		protected abstract DTO ConvertReaderToObject(SQLiteDataReader reader);




		/// <summary>
		/// Reads data from a wanted table
		/// </summary>
		/// <returns></returns>
		protected List<DTO> Select()
		{
			List<DTO> results = new List<DTO>();
			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				command.CommandText = $"select * from {_table};";
				SQLiteDataReader dataReader = null;
				try
				{
					connection.Open();
					dataReader = command.ExecuteReader();

					while (dataReader.Read())
					{
						results.Add(ConvertReaderToObject(dataReader));

					}
				}
				finally
				{
					if (dataReader != null)
					{
						dataReader.Close();
					}

					command.Dispose();
					connection.Close();
				}

			}
			return results;
		}


		/// <summary>
		/// deletes all data in a wanted table
		/// </summary>
		/// <returns></returns>
		public bool DeleteData()
		{
			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				int res = -1;
				try
				{
					command.CommandText = $"DELETE  FROM {_table}";
					command.Prepare();
					connection.Open();
					res = command.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					log.Error("error connecting to the dataBase");
					log.Debug(e.Message);
				}
				finally
				{
					command.Dispose();
					connection.Close();
				}
				return res > -1;
			}

		}
	}
}

