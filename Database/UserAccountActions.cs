using BlogWebApp.Interfaces;
using BlogWebApp.Models;

namespace BlogWebApp.Database
{
    public class UserAccountActions : IUserAccountActions
    {
        public void AddNewUserAccount(UserAccount userToAdd)
        {
            using (var _context = new UserAccountDbContext())
            {
                if (CheckIfUsernameAlreadyExists(userToAdd.Username) == true)
                {
                    throw new Exception("Unable to add user, user already exists");

                }
                else
                {
                    _context.UserAccounts.Add(userToAdd);
                    _context.SaveChanges();
                }
            }
        } 
        public UserAccount GetUserAccountByUsername(string username)
        {
            using (var _context = new UserAccountDbContext())
            {
                if(CheckIfUsernameAlreadyExists(username) != true)
                {
                    throw new Exception("unable to locate user by that username");
                }
                else 
                { 
                    return _context.UserAccounts.FirstOrDefault<UserAccount>(p => p.Username == username);
                } 
            }
        }
        public bool CheckIfUsernameAlreadyExists(string username)
        {
            using (var _context = new UserAccountDbContext()) { 
                if (_context.UserAccounts.Any(p => p.Username == username) != true)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public bool CheckIfPasswordCorrect(string username, string passwordToCheck)
        {
            using(var _context = new UserAccountDbContext())
            {
                if(GetUserAccountByUsername(username).Password != passwordToCheck)
                {
                    throw new Exception("Password does not match this username");
                }
                else
                { 
                    return true;
                }
            }
        }
        public void ChangeUserPassword(UserAccount existingAccountInfo, string newPassword)
        {
            using (var _context = new UserAccountDbContext())
            {
                
                try
                {
                    var accountToUpdate = GetUserAccountByUsername(existingAccountInfo.Username);
                    CheckIfPasswordCorrect(existingAccountInfo.Username, existingAccountInfo.Password);
                    accountToUpdate.Password = newPassword;
                    _context.SaveChanges();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public int GetUserIdFromUsername(string username)
        {
            using (var _context = new UserAccountDbContext())
            {
                UserAccount founduser = _context.UserAccounts.FirstOrDefault<UserAccount>(p => p.Username == username);
                return founduser.UserId;
            }
        }

        public UserAccount GetUserById(int Id)
        {
            using (var _context = new UserAccountDbContext())
            {
                var s = from i in _context.UserAccounts where i.UserId == Id select i;
                if(!s.Any())
                {
                    throw new Exception("Unable to find account with given Id");
                }

                return s.First();
            }
        }

        public void DeleteUserByUsername(string username)
        {
            using(var _context = new UserAccountDbContext())
            {
                _context.UserAccounts.Remove(GetUserAccountByUsername(username));
                _context.SaveChanges();
            }
        }
    }
}
