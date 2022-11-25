using Dapper;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BackendTask
{
    public class UserDatabaseRepository : IUserDatabaseRepository
    {   
        private bool IsTableExist()
        {
            bool exists = false;
            try
            {
                using (MySqlConnection cnx = new MySqlConnection(Environment.GetEnvironmentVariable("DB_URL")))
                {
                    var result = cnx.Query<int>("select case when exists((select * from information_schema.tables where table_name = \"userTable\")) then 1 else 0 end").FirstOrDefault();
                    if (result != 0)
                    {
                        return true;
                    }
                    else
                    {
                        
                        return false;
                    }
                }
                
            }
            catch 
            {
                exists = false;
                return exists;
            }
           
        }
        public void ConfigureDb()
        {
            if (!IsTableExist())
            {
                using (IDbConnection db = new MySqlConnection(Environment.GetEnvironmentVariable("DB_URL")))
                {
                    string sqlQuery1 = "CREATE TABLE userTable (id int not null unique,username varchar(50) not null unique,password varchar(50) not null,created_at DateTime not null,updated_at DateTime not null)";
                    int rowsAffected = db.Execute(sqlQuery1);
                }
             
            }
        }
      
        public string CreateUser(ValidatedUserDto user)
        {
            try
            {
                using (IDbConnection db = new MySqlConnection(Environment.GetEnvironmentVariable("DB_URL")))
                {
                    string sqlQuery1 = "INSERT INTO userTable (id,username,password,created_at,updated_at) VALUES(@id,@username,@password,@created_at,@updated_at)";
                    db.Execute(sqlQuery1, user);
                    return "Added user with id: "+user.id.ToString();
                }
               
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        } 
        public string UpdateUser(ValidatedUserDto user) 
        {
            try
            {
                
                using (IDbConnection db = new MySqlConnection(Environment.GetEnvironmentVariable("DB_URL")))
                {

                    string sqlQuery1 = "UPDATE userTable SET username=@username,password=@password,updated_at=@updated_at WHERE id LIKE @id;";

                    db.Execute(sqlQuery1, user);
                    return "Updated user!";
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        public string DeleteUser(int id)
        {
            try
            {
                using (IDbConnection db = new MySqlConnection(Environment.GetEnvironmentVariable("DB_URL")))
                {
                    string sqlQuery1 = "DELETE FROM userTable WHERE id="+id.ToString()+";" ;
                    Console.WriteLine("Deleted user");
                    db.Execute(sqlQuery1);
                    return "Deleted user!";
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        public ValidatedUserDto GetUserByName(string username)
        {
            try
            {
                using (MySqlConnection cnx = new MySqlConnection(Environment.GetEnvironmentVariable("DB_URL")))
                {
                    var result = cnx.Query<ValidatedUserDto>("select * from userTable where username=@username", new { username }).FirstOrDefault();
                 
                    if (result != null)
                    {
                        return result;
                    }
                    else
                    {
                        Console.WriteLine("No such user with that username");
                        return null;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public ValidatedUserDto GetUserById(int id)
        {
            try
            {
                using (MySqlConnection cnx = new MySqlConnection(Environment.GetEnvironmentVariable("DB_URL")))
                {
                    var result = cnx.Query<ValidatedUserDto>("select * from userTable where id=@id", new { id }).FirstOrDefault();
                    if (result != null)
                    {
                        return result;
                    }
                    else
                    {
                        Console.WriteLine("No such user with that id!");
                        return null;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
