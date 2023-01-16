using DataAccessLibrary.DTOs;
using DataAccessLibrary.DTOs.AddressDTOs;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Dbcontext.Data
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess dataAccess;

        public UserData(ISqlDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public Task<List<ReadUserModelDTO>> GetAllUsers()
        {
            return dataAccess.LoadData<ReadUserModelDTO, dynamic>(
                    storedProcedure: "dbo.SelectAllUsers",
                    new {  },
                    connectionStringName: "Default");
        }


        public  Task<IEnumerable<UserModel>> GetUserWithAddresses(int id)
        {
            return  dataAccess.LoadUserWithAddress<UserModel, dynamic>(
                    storedProcedure: "dbo.SelectUser",
                    new { UserId = id },
                    connectionStringName: "Default");
        }

        public async Task<IEnumerable<UserModel>> CreateUserWithAddress(CreateUserModelDTO model)
        {
            var result = await dataAccess.LoadData<UserModel, dynamic>(
                storedProcedure: "dbo.InsertUser",
                new 
                { 
                    FirstName = model.FirstName, 
                    LastName = model.LastName,

                },
                connectionStringName: "Default");

            foreach (var address in model.AddressModels)
            {
                await dataAccess.LoadData<CreateAddressDTO, dynamic>(
                storedProcedure: "dbo.CreateAddress",
                new
                {
                    Country = address.Country,
                    City = address.City,
                    Street = address.Street,
                    ZipCode = address.ZipCode,
                    UserId = result.FirstOrDefault().Id

                },
                connectionStringName: "Default");
            }

            

            var usermodelwithaddress = await GetUserWithAddresses(result.FirstOrDefault().Id);

            return usermodelwithaddress;
        }

        public Task DeleteUser(int id)
        {
            return dataAccess.SaveData<dynamic>(
                storedProcedure: "dbo.DeleteUser",
                new { UserId = id },
                connectionStringName: "Default");
        }

        public async Task<UserModel> UpdateUser(int id, UpdateUserModelDTO model)
        {
            var result = await dataAccess.LoadData<UserModel, dynamic>(
                storedProcedure: "dbo.UpdateUser",
                new { UserId = id, FirstName = model.FirstName, LastName = model.LastName },
                connectionStringName: "Default");

            //var task = GetUserWithAddresses(id);

            //var user = await task;

            //return user.FirstOrDefault();

            return result.FirstOrDefault();
        }
    }
}
