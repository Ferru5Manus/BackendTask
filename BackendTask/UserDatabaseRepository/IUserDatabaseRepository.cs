namespace BackendTask
{
    public interface IUserDatabaseRepository
    {
        string CreateUser(ValidatedUserDto user);
        string UpdateUser(ValidatedUserDto user);
        string DeleteUser(int id);
        ValidatedUserDto GetUserByName(string username);
        ValidatedUserDto GetUserById(int id);
    }
}
