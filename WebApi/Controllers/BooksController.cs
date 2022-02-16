using Business.Abstract;
using Core.Helpers;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Vms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IHttpAccessorHelper _httpAccessorHelper;
        private readonly RabbitMQPublisher _rabbitMQPublisher;
        public BooksController(IBookService bookService, IHttpAccessorHelper httpAccessorHelper, RabbitMQPublisher rabbitMQPublisher)
        {
            _bookService = bookService;
            _httpAccessorHelper = httpAccessorHelper;
            _rabbitMQPublisher = rabbitMQPublisher;
        }
        /// <summary>
        /// Gets the book list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetList()
        {
            var result = _bookService.GetList();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
        /// <summary>
        /// Returns the book with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(Guid id)
        {
            var result = _bookService.GetById(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
        /// <summary>
        /// Returns the books of the given category with the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetListByCategory/{id}")]
        public IActionResult GetListByCategory(Guid id)
        {
            var result = _bookService.GetListByCategory(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
        /// <summary>
        /// Performs category addition
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(BookDto book)
        {
            var result = _bookService.Add(book);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        /// <summary>
        /// Performs book rentals
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Update(BookPutDto book)
        {
            var result = _bookService.Update(book);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        /// <summary>
        /// Deletes the book with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            var result = _bookService.Delete(id);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
        /// <summary>
        /// It establishes the connection with RabbitMQ and creates the queue. Sends the e-mail address to the queue as a message
        /// </summary>
        /// <returns></returns>
        [HttpGet("CreateProductExcel")]
        public async Task<IActionResult> CreateProductExcel()
        {
            var userMail = _httpAccessorHelper.GetUserEMail();

            _rabbitMQPublisher.Publish(new CreateExcelMessage()
            {
                UserMail = userMail
            });
            return Ok();
        }

        [HttpPost("Upload")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Upload(IFormFile file, string toEmail)
        {
            if (file.Length < 0) return BadRequest();
            var filePath = "product-excel" + Path.GetExtension(file.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Files", filePath);
            using FileStream stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            stream.Close();
            _bookService.SendEmail(filePath, toEmail);
            return Ok();
        }
    }
}
