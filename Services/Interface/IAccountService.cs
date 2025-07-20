using BusinessObjects;
using System.Collections.Generic;

namespace Services.Interface
{
    public interface IAccountService
    {
        IEnumerable<User> GetAllAccounts();
        User GetAccountById(int id);
        void AddAccount(User user);
        void UpdateAccount(User user);
        void DeleteAccount(int id);
    }
} 