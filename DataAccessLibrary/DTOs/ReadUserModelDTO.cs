using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.DTOs
{
    public class ReadUserModelDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public int Id { get; set; }
    }
}
