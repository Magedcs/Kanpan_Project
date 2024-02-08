using System;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System.Linq;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
	public class BoardControllerDAL : ControllerDAL
	{
		private const string BoardTable = "Boards";
		private const string JoineesbrdTable = "Joinees";
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public BoardControllerDAL() : base(BoardTable) { }



		public bool DeleteAll()
        {
			return DeleteAllboards() & DeleteAlljoinees();
        }



		/// <summary>
		/// an a sql query to get usersbrds id's
		/// </summary>
		/// <param name="email"> the user email</param>
		/// <returns></returns>
		public List<BoardDTO> Getusersbrdslist(string email)
		{
			List<DTO> results = new List<DTO>();
			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				command.CommandText = $"SELECT Boards.BoardID,Boards.Email,BoardName,TaskIDcounter from {BoardTable} join {JoineesbrdTable} on Joinees.BoardID = Boards.BoardID and Joinees.Email = @email";
				SQLiteDataReader dataReader = null;
				try
				{
					connection.Open();
					SQLiteParameter Param1 = new SQLiteParameter(@"email" , email);
					command.Parameters.Add(Param1);
					command.Prepare();
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
			List<BoardDTO> res2 = new List<BoardDTO>();
			res2 = results.Cast<BoardDTO>().ToList();
			return res2;
		}



		/// <summary>
		/// an a sql query to delete the joinees table
		/// </summary>
		/// <returns></returns>
		public bool DeleteAlljoinees()
		{
			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				int res = -1;
				try
				{
					connection.Open();
					command.CommandText = $"DELETE FROM {JoineesbrdTable}";

					command.Prepare();
					res = command.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					log.Error("Error deleting from dataBase");
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

		/// <summary>
		/// an a sql query to delete boards table
		/// </summary>
		/// <returns></returns>
		public bool DeleteAllboards()
		{
			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				int res = -1;
				try
				{
					connection.Open();
					command.CommandText = $"DELETE FROM {BoardTable}";

					command.Prepare();
					res = command.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					log.Error("Error deleting from dataBase");
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



		/// <summary>
		/// an a sql query to delete joinees from joinees table
		/// </summary>
		/// <param name="boardID"> the board id needed to delete its joinees </param>
		/// <returns></returns>
		public bool DeleteJoinees(int boardID)
		{
			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				int res = -1;
				try
				{
					connection.Open();
					command.CommandText = $"DELETE FROM {JoineesbrdTable} WHERE BoardID = @id";

					SQLiteParameter Pram1 = new SQLiteParameter(@"id", boardID);
					command.Parameters.Add(Pram1);

					command.Prepare();
					res = command.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					log.Error("Error deleting from dataBase");
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

		/// <summary>
		/// an a sql query to delete 1 board
		/// </summary>
		/// <param name="boardID"> the board id wanted to be deleted</param>
		/// <returns></returns>
		public bool Delete(int boardID)
		{
			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				int res = -1;
				try
				{
					connection.Open();
					command.CommandText = $"DELETE FROM {BoardTable} WHERE BoardID = @id";

					SQLiteParameter Pram1 = new SQLiteParameter(@"id", boardID);
					command.Parameters.Add(Pram1);

					command.Prepare();
					res = command.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					log.Error("Error deleting from dataBase");
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

		/// <summary>
		/// an a sql query to insert a board to boards table
		/// </summary>
		/// <param name="board"> dto object to add its fields to the boards table</param>
		/// <returns></returns>
		public bool Insert(BoardDTO board)
		{
			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				int res = -1;
				try
				{
					connection.Open();
					command.CommandText = $"INSERT INTO {BoardTable}({BoardDTO.BoardsIDColumnName},{BoardDTO.BoardsEmailColumnName},{BoardDTO.BoardsNameColumnName},{BoardDTO.BoardstaskcurrColumnName})"
						+ $"VALUES(@id,@email,@boardname,@taskidCounter)";

					SQLiteParameter Pram1 = new SQLiteParameter(@"id", board.BoardId);
					SQLiteParameter Pram2 = new SQLiteParameter(@"email", board.UserEmail);
					SQLiteParameter Pram3 = new SQLiteParameter(@"boardname",board.BoardName );
					SQLiteParameter Pram4 = new SQLiteParameter(@"taskidCounter", board.Currtaskid);
					command.Parameters.Add(Pram1);
					command.Parameters.Add(Pram2);
					command.Parameters.Add(Pram3);
					command.Parameters.Add(Pram4);

					command.Prepare();
					res = command.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					log.Error("Error inserting to dataBase");
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


		/// <summary>
		/// an a sql query to insert a new row to the joinees table
		/// </summary>
		/// <param name="email"> the user email</param>
        /// <param name="boardID"> the joined board id</param>>
		/// <returns></returns>
		public bool Insert(string email, int boardID)
        {
			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				int res = -1;
				try
				{
					connection.Open();
					command.CommandText = $"INSERT INTO {JoineesbrdTable}(Email,BoardID)"
						+ $"VALUES(@email,@boardID)";

					SQLiteParameter Pram1 = new SQLiteParameter(@"email", email);
					SQLiteParameter Pram2 = new SQLiteParameter(@"boardID", boardID);
					command.Parameters.Add(Pram1);
					command.Parameters.Add(Pram2);

					command.Prepare();
					res = command.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					log.Error("Error inserting to dataBase");
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


		/// <summary>
		/// an a sql query to update in the board table
		/// </summary>
		/// <param name="brdId"> boardId to specify the row</param>
		/// <param name="attributeName"> column name in the table</param>
		/// <param name="brdId"> the new value to set</param>
		/// <returns></returns>
		public bool Update(int brdId, string attributeName, string attributeValue)
	     {   
			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				int res = -1;
				try
				{
					connection.Open();
					command.CommandText = $"UPDATE {BoardTable} SET [{attributeName}]=@attributevalue WHERE BoardID=@brdId";
					SQLiteParameter attribpram = new SQLiteParameter(@"attributevalue", attributeValue);
					SQLiteParameter Pram1 = new SQLiteParameter(@"brdId", brdId);
					command.Parameters.Add(attribpram);
					command.Parameters.Add(Pram1);
					command.Prepare();
					res = command.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					log.Error("Error in updating dataBase");
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




		protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
		{
			BoardDTO result = new BoardDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),reader.GetInt32(3));
			return result;
		}
        public List<BoardDTO> allboards()
        {
            List<BoardDTO> result = Select().Cast<BoardDTO>().ToList();
            return result;
        }




		/// <summary>
		/// an a sql query to get the BoardId Counter for the boardcontroller
		/// </summary>
		/// <returns></returns>
		public int BCidCounter()
        {
			int idcounter = 0;
			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				command.CommandText = $"SELECT max(BoardID) FROM Boards";
				SQLiteDataReader dataReader = null;


				try
				{
					connection.Open();

					dataReader = command.ExecuteReader();
					while (dataReader.Read())
					{
						try
						{
							idcounter = dataReader.GetInt32(0) + 1;
						}catch(Exception e)
                        {
							idcounter = 0;
                        }

					}
				}
				catch (Exception e)
				{
					log.Debug(e.Message);
					Console.WriteLine(e.Message);

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
			return idcounter;
		}



		/// <summary>
		/// an a sql query to remove 1 joinee from the joinees table
		/// </summary>
		/// <param name="email"> the users email</param>
		/// <param name="boardID"> the boardID to delete its joinee</param>
		/// <returns></returns>
		public bool Delete1Joinee(string email,int boardID)
		{
			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				int res = -1;
				try
				{
					connection.Open();
					command.CommandText = $"DELETE FROM {JoineesbrdTable} WHERE BoardID = @id AND Email = @email";

					SQLiteParameter Pram1 = new SQLiteParameter(@"id", boardID);
					SQLiteParameter Pram2 = new SQLiteParameter(@"email", email);
					command.Parameters.Add(Pram1);
					command.Parameters.Add(Pram2);

					command.Prepare();
					res = command.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					log.Error("Error deleting from dataBase");
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

