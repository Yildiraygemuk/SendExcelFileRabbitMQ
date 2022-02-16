using AutoMapper;
using Business.Abstract;
using Business.BusinessAspects;
using Business.Constants;
using Business.ValidationRules;
using Core.Aspects;
using Core.Aspects.Autofact;
using Core.Aspects.Autofact.Logging;
using Core.Aspects.Autofact.Performance;
using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Vms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace Business.Concrete
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public IDataResult<BookVm> GetById(Guid id)
        {
            var entity = _bookRepository.Get(x => x.Id == id);
            var dto = _mapper.Map<BookVm>(entity);
            return new SuccessDataResult<BookVm>(dto);
        }

        public IDataResult<IQueryable<BookVm>> GetList()
        {
            var entityList = _bookRepository.GetAllQueryable();
            var dtoList = _mapper.ProjectTo<BookVm>(entityList);
            return new SuccessDataResult<IQueryable<BookVm>>(dtoList);
        }
        [LogAspect(typeof(DatabaseLogger))]
        public IDataResult<IQueryable<BookVm>> GetListByCategory(Guid categoryId)
        {
            var entityList = _bookRepository.GetAllQueryable(x => x.CategoryId == categoryId);
            var dtoList = _mapper.ProjectTo<BookVm>(entityList);
            return new SuccessDataResult<IQueryable<BookVm>>(dtoList);
        }

        public IResult Update(BookPutDto book)
        {
            var dto = _mapper.Map<Book>(book);
            var entity = _bookRepository.GetById(dto.Id);
            if (entity == null)
                return new ErrorResult(Messages.BookNotFound);
            _mapper.Map(book, entity);
            _bookRepository.Update(entity);
            return new SuccessDataResult<Book>(entity);
        }
        [ValidationAspect(typeof(BookValidator))]
        public IResult Add(BookDto book)
        {
            IResult result = BusinessRules.Run(CheckIfbookNameExists(book.BookName));
            if (result != null)
            {
                return result;
            }
            var mappedDto = _mapper.Map<Book>(book);
            _bookRepository.Add(mappedDto);
            return new SuccessResult(Messages.BookAdded);
        }

        private IResult CheckIfbookNameExists(string bookName)
        {
            if (_bookRepository.Get(p => p.BookName == bookName) != null)
            {
                return new ErrorResult(Messages.BookNameAlreadyExists);
            }
            return new SuccessResult();
        }

        public IResult Delete(Guid id)
        {
            var model = _bookRepository.GetById(id);
            if (model == null)
                return new ErrorResult(Messages.BookNotFound);
            _bookRepository.Delete(model);
            return new SuccessResult(Messages.BookDeleted);
        }
        public void SendEmail(string filePath, string toEmail)
        {
            var subject = "title";
            var message = "message";
            var mailMessage = new MailMessage("from Email", toEmail)
            {
                Subject = subject,
                Body = message,
            };

            var fileNameWithSplit = System.IO.Directory.GetCurrentDirectory() + @"\Files\" + filePath;
            mailMessage.Attachments.Add(new System.Net.Mail.Attachment(fileNameWithSplit));
            EmailInformation(mailMessage);
        }
        public static void EmailInformation(MailMessage mail)
        {
            var client = new SmtpClient
            {
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = true,
                Host = "smtp.gmail.com",
                Credentials = new NetworkCredential("Email", "password")
            };

            client.Send(mail);
        }

    }
}
