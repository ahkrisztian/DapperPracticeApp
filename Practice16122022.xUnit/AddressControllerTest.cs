using DataAccessLibrary.Dbcontext.Data;
using DataAccessLibrary.DTOs.AddressDTOs;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Practice16122022.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice16122022.xUnit
{
    public class AddressControllerTest
    {
        public List<UserModel> users = new List<UserModel>();

        Mock<IAddressData> mockRepo;
        public AddressControllerTest()
        {
            mockRepo = new Mock<IAddressData>();
        }

        [Fact]
        public void ListOfUsersAddresses_ReturnOkObj200()
        {
            //Arrange
            var mockRepo = new Mock<IAddressData>();
            mockRepo.Setup(repo => repo.GetUserAddresses(1)).Returns(GetUserAddresses(1));

            var controller = new AddressesController(mockRepo.Object, new NullLogger<AddressesController>());

            //Act

            var result = controller.GetUserAddresses(1);

            //Assert

            Assert.IsType<OkObjectResult>(result.Result.Result);
        }

        [Fact]
        public void ListOfUsersAddresses_ReturnsBadRequestObj400()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetUserAddresses(3)).Returns(GetUserAddresses(3));

            var controller = new AddressesController(mockRepo.Object, new NullLogger<AddressesController>());

            //Act

            var result = controller.GetUserAddresses(3);

            //Assert

            Assert.IsType<BadRequestObjectResult>(result.Result.Result);
        }

        [Fact]
        public void DeleteAddressWithId_ReturnsOk200()
        {
            //Arrange
            mockRepo.Setup(repo => repo.DeleteAddress(1)).Returns(GetUserAddresses(1));

            var controller = new AddressesController(mockRepo.Object, new NullLogger<AddressesController>());

            //Act

            var result = controller.DeleteUserAddresses(1);

            //Assert

            Assert.IsType<OkResult>(result.Result);
        }

        [Fact]
        public void DeleteAddressWithId_ReturnsBadRequest400()
        {
            //Arrange
            mockRepo.Setup(repo => repo.DeleteAddress(0)).Returns(GetUserAddresses(0));

            var controller = new AddressesController(mockRepo.Object, new NullLogger<AddressesController>());

            //Act

            var result = controller.DeleteUserAddresses(0);

            //Assert

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateAddressWithId_ReturnsOkObj200()
        {
            //Arrange
            mockRepo.Setup(repo => repo.UpdateAddress(1, updateAddress())).Returns(UserUpdate(1, updateAddress()));

            var controller = new AddressesController(mockRepo.Object, new NullLogger<AddressesController>());

            //Act

            var result = controller.UpdateAddress(1, updateAddress());

            //Assert

            Assert.IsType<OkObjectResult>(result.Result.Result);
        }

        [Fact]
        public void UpdateAddressWithId_ReturnsBadRequestObj400()
        {
            //Arrange
            mockRepo.Setup(repo => repo.UpdateAddress(0, updateAddress())).Returns(UserUpdate(0, updateAddress()));

            var controller = new AddressesController(mockRepo.Object, new NullLogger<AddressesController>());

            //Act

            var result = controller.UpdateAddress(0, updateAddress());

            //Assert

            Assert.IsType<BadRequestObjectResult>(result.Result.Result);
        }

        public UpdateAddressDTO updateAddress()
        {
            return new UpdateAddressDTO()
            {                
                Country = "Marocco",
                City = "Rabat",
                Street = "Babab Str. 11",
                ZipCode = "11"
            };
        }
        public void Users()
        {
            users.Add(new UserModel
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Test",
                AddressModels = new List<AddressModel>
                    {
                        new AddressModel
                        {
                            Id= 1,
                            Country = "Test",
                            City= "Test",
                            Street= "Test",
                            ZipCode= "Test",
                            UserId= 1
                        },
                        new AddressModel
                        {
                            Id= 2,
                            Country = "Test2",
                            City= "Test2",
                            Street= "Test2",
                            ZipCode= "Test2",
                            UserId= 1
                        }
                    }
            });

        }

        public async Task<List<ReadAddressDTO>> GetUserAddresses(int id)
        {
            Users();

            List<ReadAddressDTO> userAddresses = new List<ReadAddressDTO>();

            if (id < 1 || !users.Any(x => x.Id == id))
            {
                return userAddresses;
            }

            users.ForEach(m => m.AddressModels.ForEach
              (a => userAddresses.Add
              (new ReadAddressDTO
              {
                  Id = a.Id,
                  Country = a.Country,
                  City = a.City,
                  Street = a.Street,
                  ZipCode = a.ZipCode
              })));

            return userAddresses;

        }

        public async Task DeleteUserAddress(int id)
        {
            if (id < 1)
            {
                return;
            }
        }

        public async Task<ReadAddressDTO> UserUpdate(int id, UpdateAddressDTO model)
        {
            Users();


            if (id < 1 || !users.Any(x => x.Id == id))
            {
                return null;
            }


            var output = users.FirstOrDefault().AddressModels.SingleOrDefault(x => x.Id == id);

            if (output != null)
            {
                output.Country = model.Country;
                output.City = model.City;
                output.Street = model.Street;
                output.ZipCode = model.ZipCode;

                //var addressModel = users.FirstOrDefault().AddressModels.Where(x => x.Id == id).FirstOrDefault();

                //var index = users.FirstOrDefault().AddressModels.IndexOf(addressModel);

                //users.FirstOrDefault().AddressModels.RemoveAt(index);
                //users.FirstOrDefault().AddressModels.Insert(index, output);
            }


            var readnewaddress = users.FirstOrDefault().AddressModels.SingleOrDefault(x => x.Id == id);


            var readAddressDTO = new ReadAddressDTO
            {
                Id = readnewaddress.Id,
                Country = readnewaddress.Country,
                City = readnewaddress.City,
                Street = readnewaddress.Street,
                ZipCode = readnewaddress.ZipCode
            };

            return readAddressDTO;
        }

    }
}
