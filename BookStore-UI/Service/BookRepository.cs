using Blazored.LocalStorage;
using BookStore_UI.Contract;
using BookStore_UI.ViewModals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BookStore_UI.Service
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private readonly IHttpClientFactory client;
        private readonly ILocalStorageService localStorage;

        public BookRepository(IHttpClientFactory _client, ILocalStorageService _localStorage) : base(_client, _localStorage)
        {
            client = _client;
            localStorage = _localStorage;
        }
    }
}
