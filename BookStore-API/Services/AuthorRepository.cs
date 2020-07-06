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
    public class AuthorRepository : IAuthorRepository
    {

        private readonly ApplicationDbContext Context;
        public AuthorRepository(ApplicationDbContext context)
        {
            Context = context;
        }



        public async Task<IList<Author>> FindAll()
        {
            var authors = await Context.Authors.Include(q=> q.ListBooks).ToListAsync();
            return authors;
        }




        public async Task<bool> Create(Author entity)
        {
            await Context.Authors.AddAsync(entity);
            return await Save();
        }



        public async Task<Author> FindById(int id)
        {
            var author = await Context.Authors.Include(q => q.ListBooks).FirstOrDefaultAsync(q=> q.Id==id);
            return author;
        }

        public async Task<bool> Save()
        {
            var changes = await Context.SaveChangesAsync();
            return changes > 0;
        }




        public async Task<bool> Update(Author entity)
        {
             Context.Update(entity);
            return await Save();
        }



        public async Task<bool> Delete(Author entity)
        {
            Context.Remove(entity);
            return await Save();
        }

        public async Task<bool> Exsists(int id)
        {
            return await Context.Authors.AnyAsync(q => q.Id == id);
        }
    }
}
