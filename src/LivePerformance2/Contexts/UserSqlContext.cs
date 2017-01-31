using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using LivePerformance2.Exceptions;
using LivePerformance2.Models;
using LivePerformance2.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LivePerformance2.Contexts
{
    public class UserSQLContext : IUserContext
    {
        private IDatabaseService DatabaseService { get; set; }
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public UserSQLContext(IDatabaseService dbService)
        {
            DatabaseService = dbService;
        }

        public void Add(User user)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandText = "CreateUser"
            };
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            DatabaseService.RunCommandNonQuery(cmd);
        }

        public void Delete(User user)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandText = "DELETE FROM [User]" +
                              " WHERE [User].Id = @Id"
            };
            cmd.Parameters.AddWithValue("@Id", user.Id);
            DatabaseService.RunCommandNonQuery(cmd);
        }

        public void Update(User user)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandText =
                    "UPDATE [User] SET Username=@Username, Password=@Password, Street=@Street, Place=@Place, Name=@Name" +
                    " WHERE [User].Id = @Id"
            };
            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Id", user.Id);
            DatabaseService.RunCommandNonQuery(cmd);
        }

        public ICollection<User> Read()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM [User]";
            DbDataReader reader = null;

            try
            {
                using (reader = DatabaseService.RunCommand(cmd))
                {
                    ICollection<User> userList = new List<User>();
                    while (reader != null && reader.Read())
                    {
                        User user = new User
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Email = reader.GetString(2),
                            Password = reader.GetString(3)
                        };
                        userList.Add(user);
                    }
                }
            }
            catch (SqlException e)
            {
                Log.Information(e.ToString());
            }
            return new List<User>();
        }
        /// <summary>
        /// Logs in the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The ID of the logged in user, or an exception</returns>
        public int Login(User user)
        {
            return Login(user.Username, user.Password);
        }

        /// <summary>
        /// Logs in the user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>The ID of the logged in user, or an exception</returns>
        public int Login(string username, string password)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandText = "Login"
            };
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", password);
            SqlParameter id = cmd.Parameters.Add("@UserId", SqlDbType.Int);
            id.Direction = ParameterDirection.Output;
            try
            {

                DbDataReader reader = DatabaseService.RunCommand(cmd);
                reader.Read();
                return (int)cmd.Parameters["@UserId"].Value;
            }
            catch (SqlException e)
            {
                if (e.State == 1)
                {
                    throw new UsernameNotFoundException();
                } else if (e.State == 2)
                {
                    throw new WrongPasswordException();
                }
                else Log.Error(e.ToString());
                return -1; //if we reached this, something went EXTREMELY wrong.
            }
        }

        public bool UserNameExists(string username)
        {
            SqlCommand cmd = new SqlCommand
            {
                CommandText = "SELECT COUNT(*) FROM [User] WHERE [User].Username = @Username"
            };
            cmd.Parameters.AddWithValue("@Username", username);
            var count =  DatabaseService.RunScalar(cmd);
            if (count != null && ((int)count) > 0)
            {
                return true;
            } else return false;
        }
    }
}
