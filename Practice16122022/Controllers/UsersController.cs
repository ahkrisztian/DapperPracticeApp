using DataAccessLibrary.Dbcontext.Data;
using DataAccessLibrary.DTOs;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Practice16122022.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserData userData;
        private readonly ILogger logger;

        public UsersController(IUserData userData, ILogger<UsersController> logger)
        {
            this.userData = userData;
            this.logger = logger;
        }

        // GET: api/UsersController
        /// <summary>
        /// Get all users from the database
        /// </summary>
        /// <remarks>
        /// {
        ///"firstName": "Tim",
        ///"lastName": "Burton",
        ///"id": 2007
        ///},
        ///{
        ///"firstName": "Sandor",
        ///"lastName": "Toth",
        ///"id": 2008
        ///}
        /// </remarks>
        /// <returns>List of users</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadUserModelDTO>>> Get()
        {
            try
            {
                 var output = await userData.GetAllUsers();

                if (output == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(output.Count));
                }

                logger.LogInformation("The api/Users was called");
                return Ok(output);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "The API could not fetch any data.");
                return BadRequest("No Data.");
            }

        }

        // GET api/UsersController/5
        /// <summary>
        /// Get a User and the addresses of this user
        /// </summary>
        /// <remarks>
        /// [
          ///{
           ///"firstName": "Ferenc",
            ///"lastName": "József",
            ///"id": 1,
            ///"addressModels": [
              ///{
                ///"id": 1,
                ///"country": "USA",
                ///"city": "Milwaukee",
                ///"zipCode": "5002",
                ///"street": "Watt Str. 66",
                ///"userId": 1
             ///},
              ///{
                ///"id": 2,
                ///"country": "USA",
                ///"city": "Boston",
                ///"zipCode": "1001",
                ///"street": "Marbel Str 11",
                ///"userId": 1
              ///}
            ///]
          ///}
        ///]
        /// </remarks>
        /// <returns>A user and list of users addresses</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AddressModel>> Get(int id)
        {
            try
            {
                if (id < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(id));
                }

                var output = await userData.GetUserWithAddresses(id);

                logger.LogInformation("The api/UsersController/{id} was called", id);
                return Ok(output);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "The given Id of {id} was invalid", id);
                return BadRequest("The index was out of range.");
            }

        }

        // POST api/UsersController
        /// <summary>
        /// Create a user with an address
        /// </summary>
        /// <returns>Returns a user model</returns>
        [HttpPost]
        public async Task<ActionResult<UserModel>> Post([FromBody] CreateUserModelDTO model)
        {
            try
            {
                if(model.FirstName != "string" && model.LastName != "string")
                {
                    var output = await userData.CreateUserWithAddress(model);

                    logger.LogInformation("The POST api/UsersController was called. Model: {FirstName} {LastName}", model.FirstName, model.LastName);
                    return Ok(output);
                }

                throw new ArgumentException(model.FirstName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "The user did not changed the string values {FirstName} {LastName}", model.FirstName, model.LastName);
                return BadRequest("The given Model was invalid.");
            }
        }

        // PUT api/<UsersController>/5
        /// <summary>
        /// Update a first and last name of a user
        /// </summary>
        /// <returns>Returns a user model</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ReadUserModelDTO>> Put(int id, [FromBody] UpdateUserModelDTO model)
        {
            try
            {
                if (id < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(id));
                }

                if (model.FirstName != "string" && model.LastName != "string")
                {
                    var output = await userData.UpdateUser(id, model);

                    logger.LogInformation("The PUT api/UsersController was called. Model: {FirstName} {LastName}", model.FirstName, model.LastName);
                    return Ok(output);
                }

                throw new ArgumentException(model.FirstName);

            }
            catch (ArgumentOutOfRangeException ex)
            {
                logger.LogError(ex, "The given Id of {id} was invalid", id);
                return BadRequest("The index was out of range.");
            }
            catch(ArgumentException ex)
            {
                logger.LogError(ex, "The user did not changed the string values {FirstName} {LastName}", model.FirstName, model.LastName);
                return BadRequest("The given Model was invalid.");
            }

            
        }

        // DELETE api/UsersController/5
        /// <summary>
        /// Delete a user
        /// </summary>
        /// <returns>return Ok</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (id < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(id));
                }

                await userData.DeleteUser(id);

                logger.LogInformation("The DELETE api/UsersController/{id} was called. The Deleted Id was {deletedId}", id, id);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "The given Id of {id} was invalid", id);
                return BadRequest("The index was out of range.");
            }

        }
        
    }
}
