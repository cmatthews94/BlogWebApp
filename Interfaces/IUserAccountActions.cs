using BlogWebApp.Models;
namespace BlogWebApp.Interfaces
{
    public interface IUserAccountActions
    {
        bool CheckIfUsernameAlreadyExists(string username);
        void AddNewUserAccount(UserAccount userToAdd);
        UserAccount GetUserAccountByUsername(string username);
        bool CheckIfPasswordCorrect(string username, string password);
        void ChangeUserPassword(UserAccount existingAccountInfo, string newPassword);
        int GetUserIdFromUsername(string username);
        UserAccount GetUserById(int Id);
        void DeleteUserByUsername(string username);
        
    }
}