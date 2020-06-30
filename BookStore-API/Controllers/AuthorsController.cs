using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    /// EndPoint Used to intereact with the author in the book stores database
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILoggerServices _logger;
        private readonly IMapper _map;


        public AuthorsController(IAuthorRepository authorRepository, ILoggerServices logger,
            IMapper map)
        {
            _authorRepository = authorRepository;
            _logger = logger;
            _map = map;
        }

        

        /// <summary>
        /// Http action Get All authors
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            try
            {
                _logger.LogInfo("Attemped get all Authors");
                var authors = await _authorRepository.FindAll();
                if (authors == null)
                {
                    _logger.LogWarn("No authors Avalible");
                    return NotFound();
                }
                var response = _map.Map<IList<AuthorDTO>>(authors);
                _logger.LogInfo("Succesfully Got All Authors");
                return Ok(response);
            }
            catch (Exception e)
            {
                return internalError($"{e.Message}-{e.InnerException}");
            } 
        }

        /// <summary>
        /// Get An author by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An authors information</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(int id)
        {  
            try
            {
                _logger.LogInfo($"Attemped get one author with id:{id}");
                var author = await _authorRepository.FindById(id);
                if (author == null)
                {
                    _logger.LogWarn($"Author with id:{id} Not found");
                    return NotFound();
                }
                var response = _map.Map<AuthorDTO>(author);
                return Ok(response);
            }
            catch (Exception e)
            {
                return internalError($"{e.Message}-{e.InnerException}");
            }
        }


        /// <summary>
        /// action Http post that creates a author
        /// </summary>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize (Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreatePost([FromBody] AuthorCreateDTO authorDTO)
        {
            try
            {
                _logger.LogInfo($"Author submition attempeted");
                if (authorDTO == null)
                {
                    _logger.LogWarn($"Empty Request was submited");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"Author data incomplete or not accepted");
                    return BadRequest(ModelState);
                }
                var authorInfo = _map.Map<Author>(authorDTO);
                var isSuccess = await _authorRepository.Create(authorInfo);
                if (!isSuccess)
                {
                    return internalError($"Author creation failed");
                }

                return Created("Create", new { authorInfo});
            }
            catch (Exception e)
            {

                return internalError($"{e.Message}-{e.InnerException}");
            }
        }





        /// <summary>
        /// UPDATES an author
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> UpdatePut(int id ,[FromBody] AuthorUpdateDTO authorDTO)
        {
            try
            {
                _logger.LogInfo($"Author Update attempted");
                if (id < 1 || authorDTO == null || id != authorDTO.Id)
                {
                    _logger.LogInfo($"Id {id} does not exist! Or ");
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInfo($"Author data incomplete or not accepted");
                    return BadRequest(ModelState);
                }
                var authorUpdate = _map.Map<Author>(authorDTO);
                var isSuccess = await _authorRepository.Update(authorUpdate);
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInfo($"Author Delete attempted");
                if (id < 1)
                {
                    _logger.LogInfo($"Id {id} does not exist! Or ");
                    return BadRequest();
                }
                var author = await _authorRepository.FindById(id);
                if (author == null)
                {
                    return NotFound();
                }


                var isSuccess = await _authorRepository.Delete(author);
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




        private ObjectResult internalError(string message) {
            _logger.LogError($"{message}");
            return StatusCode(500, "Server error");
        }

    }
}
