using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Vms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Abstract
{
    public interface IBookService
    {
        IDataResult<BookVm> GetById(Guid id);
        IDataResult<IQueryable<BookVm>> GetList();
        IDataResult<IQueryable<BookVm>> GetListByCategory(Guid categoryId);
        IResult Add(BookDto book);
        IResult Delete(Guid id);
        IResult Update(BookPutDto book);
        void SendEmail(string filePath, string toEmail);
    }
}
