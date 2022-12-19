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
