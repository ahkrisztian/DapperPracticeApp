using DataAccessLibrary.DTOs.AddressDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class UserModel
    {     
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int Id { get; set; }

        public List<AddressModel> AddressModels { get; set; } = new List<AddressModel>();
    }
}
