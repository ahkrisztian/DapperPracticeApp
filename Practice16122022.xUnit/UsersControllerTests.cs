using DataAccessLibrary.Dbcontext.Data;
using DataAccessLibrary.DTOs;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Microsoft.Extensions.Logging;
using Practice16122022.Controllers;
using Xunit.Abstractions;
using Xunit;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.HttpSys;
using DataAccessLibrary.DTOs.AddressDTOs;
using Microsoft.Identity.Client;
using System.Reflection;

namespace Practice16122022.xUnit
{
    public class UsersControllerTests : IDisposable
    {
        Mock<IUserData> mockRepo;
        public UsersControllerTests()
        {
            mockRepo= new Mock<IUserData>();
        }

        public void Dispose()
        {
            mockRepo = null;
        }

        [Fact]
        public void GetUsers_ReturnsZeroUsers_BadRequestObj400_WhenDbIsEmpty()
        {

            //Arrange
            mockRepo.Setup(repo => repo.GetAllUsers()).Returns(GetUsers(0));

            var controller = new UsersController(mockRepo.Object, new NullLogger<UsersController>());

            //Act
            var result = controller.Get();


            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result.Result);
        }

        [Fact]
        public void GetUsers_ReturnsOneUser_OkObjResult200()
        {

            //Arrange
            mockRepo.Setup(repo => repo.GetAllUsers()).Returns(GetUsers(1));

            var controller = new UsersController(mockRepo.Object, new NullLogger<UsersController>());

            //Act
            var result = controller.Get();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result.Result);
        }

        [Fact]
        public void GetUserWithAddress_ReturnsOneUserWithOneAddress_OkObjResultOK()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetUserWithAddresses(1)).Returns(UserWithAddress(1));

            var controller = new UsersController(mockRepo.Object, new NullLogger<UsersController>());

            //Act
            var result = controller.Get(1);

            //Assert
            Assert.IsType<OkObjectResult>(result.Result.Result);

        }

        [Fact]
        public void GetUserWithaddress_ReturnsBadRequestObj()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetUserWithAddresses(0)).Returns(UserWithAddress(0));

            var controller = new UsersController(mockRepo.Object, new NullLogger<UsersController>());

            //Act
            var result = controller.Get(0);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result.Result);
        }

        [Fact]
        public void PostUserModel_ReturnsUserModel_OkObjResult()
        {
            //Arrange
            mockRepo.Setup(repo => repo.CreateUserWithAddress(createUser()[0])).Returns(CreateUserWithAddress(createUser()[0]));

            var controller = new UsersController(mockRepo.Object, new NullLogger<UsersController>());

            //Act
            var result = controller.Post(createUser()[0]);

            //Assert
            Assert.IsType<OkObjectResult>(result.Result.Result);

        }

        [Fact]
        public void PostUserModel_ReturnsBadRequestObj()
        {
            //Arrange
            mockRepo.Setup(repo => repo.CreateUserWithAddress(createUser()[1])).Returns(CreateUserWithAddress(createUser()[1]));

            var controller = new UsersController(mockRepo.Object, new NullLogger<UsersController>());

            //Act
            var result = controller.Post(createUser()[1]);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result.Result);
        }

        [Fact]
        public void DeleteUser_ReturnOkObj200()
        {
            //Arrange
            mockRepo.Setup(repo => repo.DeleteUser(1)).Returns(DeleteUser(1));

            var controller = new UsersController(mockRepo.Object, new NullLogger<UsersController>());

            //Act
            var result = controller.Delete(1);

            //Assert
            Assert.IsType<OkResult>(result.Result);
        }

        [Fact]
        public void DeleteUser_ReturnsBadRequestObj400()
        {
            //Arrange
            mockRepo.Setup(repo => repo.DeleteUser(0)).Returns(DeleteUser(0));

            var controller = new UsersController(mockRepo.Object, new NullLogger<UsersController>());

            //Act
            var result = controller.Delete(0);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateUser_ReturnOkObj200()
        {
            //Arrange
            mockRepo.Setup(repo => repo.UpdateUser(1, updateUser()[0])).Returns(UpdateUser(1, updateUser()[0]));

            var controller = new UsersController(mockRepo.Object, new NullLogger<UsersController>());

            //Act
            var result = controller.Put(1, updateUser()[0]);

            //Assert
            Assert.IsType<OkObjectResult>(result.Result.Result);
        }

        [Fact]
        public void UpdateUser_ReturnsBadRequestObj400()
        {
            //Arrange
            mockRepo.Setup(repo => repo.UpdateUser(1, updateUser()[1])).Returns(UpdateUser(1, updateUser()[1]));

            var controller = new UsersController(mockRepo.Object, new NullLogger<UsersController>());

            //Act
            var result = controller.Put(1, updateUser()[1]);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result.Result);
        }


        private List<CreateUserModelDTO> createUser()
        {
            List<CreateUserModelDTO> models = new List<CreateUserModelDTO>();

            models.Add(new CreateUserModelDTO()
            {
                FirstName = "Test",
                LastName = "Test",

                AddressModels = new List<CreateAddressDTO>()
                {
                    new CreateAddressDTO
                    {
                        Country = "Test",
                        City= "Test",
                        Street= "Test",
                        ZipCode= "Test"
                    }
                }
            });

            models.Add(new CreateUserModelDTO()
            {
                FirstName = "string",
                LastName = "string",

                AddressModels = new List<CreateAddressDTO>()
                {
                    new CreateAddressDTO
                    {
                        Country = "Test",
                        City= "Test",
                        Street= "Test",
                        ZipCode= "Test"
                    }
                }
            });

            return models;
        }

        private List<UpdateUserModelDTO> updateUser()
        {
            List<UpdateUserModelDTO> models = new List<UpdateUserModelDTO>();

            models.Add(new UpdateUserModelDTO()
            {
                FirstName = "Test",
                LastName = "Test"
            });

            models.Add(new UpdateUserModelDTO()
            {
                FirstName = "string",
                LastName = "string"
            });

            return models;
        }

        private async Task<List<ReadUserModelDTO>> GetUsers(int id)
        {
            List<ReadUserModelDTO> userempty = new List<ReadUserModelDTO>();

            if (id > 0)
            {
                List<UserModel> users = new List<UserModel>();

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
                        }
                    }
                });

                List<ReadUserModelDTO> readUserModelDTOs = new List<ReadUserModelDTO>();

                foreach (var user in users)
                {
                    readUserModelDTOs.Add(new ReadUserModelDTO
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName

                    });
                }

                return readUserModelDTOs;
            }       

            return userempty;
        }

        private async Task<IEnumerable<UserModel>> UserWithAddress(int id)
        {
            List<UserModel> users = new List<UserModel>();

            if (id > 0)
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
                        }
                    }
                });
            }

            return users;
        }

        private async Task<IEnumerable<UserModel>> CreateUserWithAddress(CreateUserModelDTO model)
        {
            List<CreateUserModelDTO> createUserModelDTOs= new List<CreateUserModelDTO>();

            createUserModelDTOs.Add(model);

            List<UserModel> users = new List<UserModel>();

            if (model.FirstName != "string" && model.LastName != "string")
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
                        }
                    }
                });
            }

            return users;
        }

        private async Task DeleteUser(int id)
        {
            if(id < 1)
            {
                return;
            }
        }

        private async Task<UserModel> UpdateUser(int id, UpdateUserModelDTO model)
        {

            if (id > 0 && model.FirstName != "string" && model.LastName != "string")
            {
                var output = new UserModel
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                return output;
            }

            return null;
        }

        
    }
}
