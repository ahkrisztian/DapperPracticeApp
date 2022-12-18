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

        public UsersController(IUserData userData)
        {
            this.userData = userData;
        }

        // GET: api/UsersController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadUserModelDTO>>> Get()
        {
            var output = await userData.GetAllUsers();

            return Ok(output);
        }

        // GET api/UsersController/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AddressModel>> Get(int id)
        {
            var output = await userData.GetUserWithAddresses(id);

            return Ok(output);
        }

        // POST api/UsersController
        [HttpPost]
        public async Task<ActionResult<UserModel>> Post([FromBody] CreateUserModelDTO model)
        {
            var output = await userData.CreateUserWithAddress(model);

            return Ok(output);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ReadUserModelDTO>> Put(int id, [FromBody] UpdateUserModelDTO model)
        {
            var output = await userData.UpdateUser(id, model);

            return Ok(output);
        }

        // DELETE api/UsersController/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await userData.DeleteUser(id);

            return Ok();
        }
        
    }
}
