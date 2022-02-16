using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Dtos
{
    public class BookDto
    {
        public string BookName { get; set; }
        public string Description { get; set; }
        public bool IsRenting { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime? FinishRentTime { get; set; }
    }
}
