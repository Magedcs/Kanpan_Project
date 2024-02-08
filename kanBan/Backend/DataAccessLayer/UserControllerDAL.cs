using System;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System.Linq;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
	public class UserControllerDAL : ControllerDAL
	{
		private const string UserTable = "Users";
		public UserControllerDAL(): base(UserTable) { }


		/// <summary>
		/// an a sql query to add a user to the users table
		/// </summary>
		/// <param name="user"> user dto to add its fields to the users table</param>
		/// <returns></returns>
		public bool Insert(UserDTO user) {
			using (var connection = new SQLiteConnection(_connection))
			{
				SQLiteCommand command = new SQLiteCommand(null, connection);
				int res = -1;
				try
				{
					connection.Open();
					command.CommandText = $"INSERT INTO {UserTable}({UserDTO.UsersEmailColumnName},{UserDTO.UsersPasswordColumnName})" 
						+ $"VALUES(@email,@password)";

					SQLiteParameter Pram1 = new SQLiteParameter(@"email" , user.Email);
					SQLiteParameter Pram2 = new SQLiteParameter(@"password" , user.Password);
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
		/// an a sql query to delete all data from the users table
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
					command.CommandText = $"DELETE FROM {UserTable}";

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
			UserDTO result = new UserDTO(reader.GetString(0), reader.GetString(1),false);
			return result;
		}

        public List<UserDTO> allusers()
        {
            List<UserDTO> result = Select().Cast<UserDTO>().ToList();
            return result;
        }

        public bool intTobool(int a)
        {
			if(a == 1) return true;
			return false;
        }



	}
}

