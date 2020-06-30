using BookStore_API.Contracts;
using BookStore_API.Data;
using BookStore_API.Modals;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.Services
{
    public class BookRepository : IBookRepository
    {

        private readonly ApplicationDbContext Context;
        public BookRepository(ApplicationDbContext context)
        {
            Context = context;
        }


        public async Task<bool> Create(Book entity)
        {
            await Context.Books.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Book entity)
        {
            Context.Books.Remove(entity);
            return await Save();
        }

        public async Task<bool> Exsists(int id)
        {
            return await Context.Books.AnyAsync(q=> q.Id == id);
        }

        public async Task<IList<Book>> FindAll()
        {
            var books = await Context.Books.ToListAsync();
            return books;
            
        }

        public async Task<Book> FindById(int Id)
        {
            var book = await Context.Books.FindAsync(Id);
            return book;
        }
        public async Task<bool> Save()
        {
            var changes = await Context.SaveChangesAsync();
            return changes > 0;
        }
        public async Task<bool> Update(Book entity)
        {
            Context.Update(entity);
            return await Save();
        }
    }
}
