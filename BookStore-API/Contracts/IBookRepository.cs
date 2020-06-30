﻿using BookStore_API.Contracts;
using BookStore_API.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_API.Contracts
{
    public interface IBookRepository :IRepositoryBase<Book>
    {
    }
}
