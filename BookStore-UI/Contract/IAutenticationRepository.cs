using BookStore_UI.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI.Contract
{
    public interface IAutenticationRepository
    {
        public Task<bool> Register(RegistrationModel usr);

        public Task<bool> Login(LoginModel usr);

        public Task LogOut();

    }
}
