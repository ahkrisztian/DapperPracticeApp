using DataAccessLibrary.DTOs.AddressDTOs;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.DTOs
{
    public class CreateUserModelDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public List<CreateAddressDTO> AddressModels { get; set; } = new List<CreateAddressDTO>();
    }
}
