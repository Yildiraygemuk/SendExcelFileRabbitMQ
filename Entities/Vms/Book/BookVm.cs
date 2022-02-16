using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Vms
{
    public class BookVm
    {
        public Guid Id { get; set; }
        public string BookName { get; set; }
        public string Description { get; set; }
        public bool IsRenting { get; set; }
        public string CategoryName { get; set; }
        public DateTime? FinishRentTime { get; set; }
    }
}
