using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStore_API.Contracts;
using BookStore_API.DTOs;
using BookStore_API.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore_API.Controllers
{
    /// <summary>
    /// Interacts whit the books table! 
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoggerServices _logger;
        private readonly IMapper _map;


        public BooksController(IBookRepository bookRepository, ILoggerServices logger,
            IMapper map)
        {
            _bookRepository = bookRepository;
            _logger = logger;
            _map = map;
        }


        /// <summary>
        /// Get All Books
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks()
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempet Call");
                var books = await _bookRepository.FindAll();
                var response = _map.Map<IList<BookDTO>>(books);
                _logger.LogInfo($"{location}: Attempet Call Succesful");
                return Ok(response);
            }
            catch (Exception e)
            {
                return internalError($"{location}:{e.Message}-{e.InnerException}");
            }
        }



        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooksById(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attemped Call  for id {id}");
                var book = await _bookRepository.FindById(id);
                var response = _map.Map<BookDTO>(book); 
                if (book == null)
                {
                    _logger.LogWarn($"{location} Faild to Find id {id}");
                    return NotFound();
                }
                _logger.LogInfo($"{location}: Attempet Call Succesful");
                return Ok(response);
            }
            catch (Exception e)
            {
                return internalError($"{location}:{e.Message}-{e.InnerException}");
            }
        }

        /// <summary>
        /// Creates a new Book
        /// </summary>
        /// <param name="bookDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBook([FromBody] BookCreateDTO bookDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}:Create Attempted");
                if (bookDTO == null)
                {
                    BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var book = _map.Map<Book>(bookDTO);
                var isSuccess = await _bookRepository.Create(book);
                return Created("Created", new { book });
            }
            catch (Exception e)
            {
                return internalError($"{location}:{e.Message}-{e.InnerException}");
            }
        }






        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> UpdatePut(int id, [FromBody] BookUpdateDTO bookDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"Author Update attempted");
                if (id < 1 || bookDTO == null || id != bookDTO.Id)
                {
                    _logger.LogInfo($"Id {id} does not exist! Or ");
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInfo($"Author data incomplete or not accepted");
                    return BadRequest(ModelState);
                }
                var bookUpdate = _map.Map<Book>(bookDTO);
                var isSuccess = await _bookRepository.Update(bookUpdate);
                if (!isSuccess)
                {
                    return internalError($"Author update failed");
                }

                return NoContent();
            }
            catch (Exception e)
            {

                return internalError($"{e.Message}-{e.InnerException}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Delete(int id)
        {
            var location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Delete Attempted on Id: {id}");
                if (id < 1)
                {
                    _logger.LogInfo($"Id {id} does not exist! Or ");
                    return BadRequest();
                }
                var author = await _bookRepository.FindById(id);
                if (author == null)
                {
                    return NotFound();
                }


                var isSuccess = await _bookRepository.Delete(author);
                if (!isSuccess)
                {
                    return internalError($"Author delete failed");
                }

                return NoContent();
            }
            catch (Exception e)
            {

                return internalError($"{e.Message}-{e.InnerException}");
            }
        }



        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ActionName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return $"{controller} - {action}";
        }


        private ObjectResult internalError(string message)
        {
            _logger.LogError($"{message}");
            return StatusCode(500, "Server error");
        }

    }
}
