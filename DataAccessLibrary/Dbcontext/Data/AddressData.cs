using Dapper;
using DataAccessLibrary.DTOs;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.DTOs.AddressDTOs;
using System.Globalization;

namespace DataAccessLibrary.Dbcontext.Data
{
    public class AddressData : IAddressData
    {
        private readonly ISqlDataAccess access;

        public AddressData(ISqlDataAccess access)
        {
            this.access = access;
        }

        public Task<List<ReadAddressDTO>> GetUserAddresses(int id)
        {
            return access.LoadData<ReadAddressDTO, dynamic>(
                        storedProcedure: "dbo.GetAddressFromUser",
                        new { UserId = id },
                        connectionStringName: "Default");
        }

        public Task DeleteAddress(int id)
        {
            return access.SaveData<dynamic>(
                storedProcedure: "dbo.DeleteAddress",
                new { AddressId = id },
                connectionStringName: "Default");
        }

        public async Task<ReadAddressDTO> UpdateAddress(int id, UpdateAddressDTO updatemodel)
        {
            var result = await access.LoadData<ReadAddressDTO, dynamic>(
                            storedProcedure: "dbo.UpdateAddress",
                            new
                            {
                                AddressId = id,
                                Country = updatemodel.Country,
                                City = updatemodel.City,
                                Street = updatemodel.Street,
                                ZipCode = updatemodel.ZipCode
                            },
                            connectionStringName: "Default");

            return result.FirstOrDefault();
        }
    }
}
