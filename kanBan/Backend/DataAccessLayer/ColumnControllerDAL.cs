using System;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System.Linq;


namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class ColumnControllerDAL : ControllerDAL
    {
        private const string ColumnTable = "Columns";

        public ColumnControllerDAL() : base(ColumnTable) { }



		/// <summary>
		/// an a sql query to get the column's task limit
		/// </summary>
		/// <param name="brdid"> boardId to specify the row</param>
		/// <param name="ord"> column ord to specify the board's column</param>
		/// <returns></returns>
		public int getTaskslimit(int brdid , int ord)
        {
			int limit = 0;
			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				command.CommandText = $"select * from {ColumnTable} WHERE BoardID = @brdid and Ordinal = @ord;";
				SQLiteDataReader dataReader = null;


				try
				{
					connection.Open();
					SQLiteParameter Pram1 = new SQLiteParameter(@"brdid",brdid );
					SQLiteParameter Pram2 = new SQLiteParameter(@"ord", ord);
					command.Parameters.Add(Pram1);
					command.Parameters.Add(Pram2);
					command.Prepare();


					dataReader = command.ExecuteReader();
					while (dataReader.Read())
					{
						try
						{
							limit = dataReader.GetInt32(3);
						}catch(Exception e)
                        {
							limit = -1;
                        }

					}
					
				}
				catch(Exception e)
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
			return limit;
		}




		/// <summary>
		/// an a sql query to delete all data in columns table
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
					command.CommandText = $"DELETE FROM {ColumnTable}";

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
		/// an a sql query to delete board's columns from columns table
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
					command.CommandText = $"DELETE FROM {ColumnTable} WHERE BoardID = @id";

					SQLiteParameter Pram1 = new SQLiteParameter(@"id", boardID);
					command.Parameters.Add(Pram1);

					command.Prepare();
					res = command.ExecuteNonQuery();
				}
				catch (Exception e)
				{
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
		/// an a sql query to add a row to columns table
		/// </summary>
		/// <param name="column"> column dto to add its fields to the table</param>
		/// <returns></returns>
		public bool Insert(ColumnDTO column)
		{
			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				int res = -1;
				try
				{
					connection.Open();
					command.CommandText = $"INSERT INTO {ColumnTable}({ColumnDTO.ColumnsBoardIdColumnName},{ColumnDTO.ColumnsOrdinalColumnName},{ColumnDTO.ColumnsNameColumnName},{ColumnDTO.ColumnsLimitColumnName})"
						+ $"VALUES(@id,@Ordinal,@Colname,@limit)";

					SQLiteParameter Pram1 = new SQLiteParameter(@"id", column.BoardID);
					SQLiteParameter Pram2 = new SQLiteParameter(@"Ordinal",column.ColumnOrdinal);
					SQLiteParameter Pram3 = new SQLiteParameter(@"Colname", column.Name);
					SQLiteParameter Pram4 = new SQLiteParameter(@"limit", column.Taskslimit);
					command.Parameters.Add(Pram1);
					command.Parameters.Add(Pram2);
					command.Parameters.Add(Pram3);
					command.Parameters.Add(Pram4);

					command.Prepare();
					res = command.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
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
		/// an a sql query to update in the columns table
		/// </summary>
		/// <param name="id"> boardId to specify the row</param>
		/// <param name="Ordinal"> wanted column ordinal</param>
		/// <param name="attributeName"> the column name in the table</param>
        /// <param name="attributeValue"> the new value to set in the tavle</param>>
		/// <returns></returns>
		public bool Update(int id, int Ordinal, string attributeName, int attributeValue)
		{

			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				int res = -1;
				try
				{
					connection.Open();
					command.CommandText = $"UPDATE {ColumnTable} SET [{attributeName}] = @attributevalue WHERE BoardID = @boardId AND Ordinal = @ColOrdinal";
					SQLiteParameter attribpar = new SQLiteParameter(@"attributevalue", attributeValue);
					SQLiteParameter idPar = new SQLiteParameter(@"boardId", id);
					SQLiteParameter ordPar = new SQLiteParameter(@"ColOrdinal", Ordinal);
					command.Parameters.Add(attribpar);
					command.Parameters.Add(idPar);
					command.Parameters.Add(ordPar);
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
		ColumnDTO result = new ColumnDTO(reader.GetInt32(0), reader.GetInt32(1),reader.GetString(2), reader.GetInt32(3));
		return result;
	    }

		public List<ColumnDTO> allcolumns()
		{
			List<ColumnDTO> result = Select().Cast<ColumnDTO>().ToList();
			return result;
		}
	}
}

