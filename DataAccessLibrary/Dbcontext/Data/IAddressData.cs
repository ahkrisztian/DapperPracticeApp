using DataAccessLibrary.DTOs.AddressDTOs;
using DataAccessLibrary.Models;

namespace DataAccessLibrary.Dbcontext.Data
{
    public interface IAddressData
    {
        Task<List<ReadAddressDTO>> GetUserAddresses(int id);

        Task DeleteAddress(int id);

        Task<ReadAddressDTO> UpdateAddress(int id, UpdateAddressDTO updatemodel);
    }

}