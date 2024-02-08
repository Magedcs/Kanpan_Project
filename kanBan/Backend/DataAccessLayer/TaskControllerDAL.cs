using System;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System.Linq;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
	public class TaskControllerDAL : ControllerDAL
	{
		private const string TaskTable = "Tasks";
		public TaskControllerDAL() : base(TaskTable) {}


		/// <summary>
		/// an a sql query to delete board's tasks from tasks table
		/// </summary>
		/// <param name="boardID"> boardId to specify the row</param>
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
					command.CommandText = $"DELETE FROM {TaskTable} WHERE BoardID = @id";

					SQLiteParameter Pram1 = new SQLiteParameter(@"id", boardID);
					command.Parameters.Add(Pram1);

					command.Prepare();
					res = command.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
					log.Error("Error deleting to dataBase");
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
		/// an a sql query to update in the tasks table
		/// </summary>
		/// <param name="id"> taskid to specify the row</param>
		/// <param name="brdid"> task's boardid</param>
		/// <param name="attributeName"> the column name in the tasks table</param>
        /// <param name="attributeValue"> the new value to set in the table</param>>
		/// <returns></returns>
		public bool Update(int id, int brdid, string attributeName, string attributeValue)
		{

			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				int res = -1;
				try
				{
					connection.Open();
					command.CommandText = $"UPDATE {TaskTable} SET [{attributeName}] = @attributevalue WHERE TaskID = @ID AND BoardID = @brdId";
					SQLiteParameter attrib = new SQLiteParameter(@"attributevalue" , attributeValue);
					SQLiteParameter idPar = new SQLiteParameter(@"ID", id);
					SQLiteParameter brdPar = new SQLiteParameter(@"brdId", brdid);
					command.Parameters.Add(idPar);
					command.Parameters.Add(brdPar);
					command.Parameters.Add(attrib);
					command.Prepare();
					res = command.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
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



		/// <summary>
		/// an a sql query to update in the tasks table
		/// </summary>
		/// <param name="id"> taskid to specify the row</param>
		/// <param name="brdid"> task's boardid</param>
		/// <param name="attributeName"> the column name in the tasks table</param>
		/// <param name="attributeValue"> the new value to set in the table</param>>
		/// <returns></returns>
		public bool Update(int id, int brdid, string attributeName, long attributeValue)
		{

				using (var connection = new SQLiteConnection(_connection))
				{
					SQLiteCommand command = new SQLiteCommand(null, connection);
					int res = -1;
					try
					{
						connection.Open();
						command.CommandText = $"UPDATE {TaskTable} SET [{attributeName}]=@attributevalue WHERE TaskID=@id AND BoardID = @brdid";
					    SQLiteParameter attrPar = new SQLiteParameter(@"attributevalue", attributeValue);
 						SQLiteParameter idPar = new SQLiteParameter(@"id", id);
						SQLiteParameter brdPar = new SQLiteParameter(@"brdid", brdid);
					    command.Parameters.Add(attrPar);
						command.Parameters.Add(idPar);
						command.Parameters.Add(brdPar);
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


		/// <summary>
		/// an a sql query to add a task in the tasks table
		/// </summary>
		/// <param name="tsk"> task dto to add its feilds to the tasks table</param>
		/// <returns></returns>
		public bool Insert(TaskDTO tsk)
		{
			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				int res = -1;
				try
				{
					connection.Open();
					command.CommandText = $"INSERT INTO {TaskTable}({TaskDTO.TasksBoardIDColumnName}, {TaskDTO.TasksOrdColumnName} ,{TaskDTO.TasksIDColumnName}, {TaskDTO.TasksEmailColumnName}, {TaskDTO.TasksTitleColumnName}, {TaskDTO.TasksDescreptionColumnName},{TaskDTO.TaskDueDateColumnName},{TaskDTO.TasksCreationTimeColumnName})"
						+ $"VALUES(@brdid ,@Ordinal, @tskid, @email , @title, @descreption , @duedate, @creationtime)";
					SQLiteParameter Param1 = new SQLiteParameter(@"brdid", tsk.BoardID);
					SQLiteParameter Param2 = new SQLiteParameter(@"Ordinal", tsk.ColumnOrdinal);
					SQLiteParameter Param3 = new SQLiteParameter(@"tskid", tsk.TaskId);
					SQLiteParameter Param4 = new SQLiteParameter(@"email", tsk.Assignee);
					SQLiteParameter Param5 = new SQLiteParameter(@"title", tsk.Title);
					SQLiteParameter Param6 = new SQLiteParameter(@"descreption", tsk.Descreption);
					SQLiteParameter Param7 = new SQLiteParameter(@"duedate", tsk.DueDate);
					SQLiteParameter Param8 = new SQLiteParameter(@"creationtime", tsk.CreationTime);
					command.Parameters.Add(Param1);
					command.Parameters.Add(Param2);
					command.Parameters.Add(Param3);
					command.Parameters.Add(Param4);
					command.Parameters.Add(Param5);
					command.Parameters.Add(Param6);
					command.Parameters.Add(Param7);
					command.Parameters.Add(Param8);
					command.Prepare();
					res = command.ExecuteNonQuery();

				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
					log.Error("error occured while updating the database");
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
		/// an a sql query to delete all tasks table data
		/// </summary>
		/// <returns></returns>
		public bool DeleteAll()
		{
			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				int res = -1;
				try
				{
					connection.Open();
					command.CommandText = $"DELETE FROM {TaskTable}";

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


		protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
		{
			TaskDTO result = new TaskDTO(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), Convert.ToDateTime(reader.GetString(6)), Convert.ToDateTime(reader.GetString(7)));
			return result;
		}

        public List<TaskDTO> alltasks()
        {
            List<TaskDTO> result = Select().Cast<TaskDTO>().ToList();
            return result;
        }



		/// <summary>
		/// an a sql query to get all column's tasks to load the data
		/// </summary>
		/// <param name="brdid"> task's boardid</param>
		/// <param name="ord"> column ordinal</param>
		/// <returns></returns>
		public List<TaskDTO> SelectcolsTasks(int brdid, int ord)
		{
			List<DTO> results = new List<DTO>();
			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				command.CommandText = $"select * from {TaskTable} WHERE BoardID = @brdid and Ordinal = @ord;";
				SQLiteDataReader dataReader = null;
				try
				{
					connection.Open();
					SQLiteParameter Pram1 = new SQLiteParameter(@"brdid", brdid);
					SQLiteParameter Pram2 = new SQLiteParameter(@"ord", ord);
					command.Parameters.Add(Pram1);
					command.Parameters.Add(Pram2);
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
			List<TaskDTO> res2 = results.Cast<TaskDTO>().ToList();
			return res2;
		}






	}



}

