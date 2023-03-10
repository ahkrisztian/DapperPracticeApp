using DataAccessLibrary.Dbcontext.Data;
using DataAccessLibrary.DTOs.AddressDTOs;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Practice16122022.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly IAddressData addressData;
    private readonly ILogger<AddressesController> logger;

    public AddressesController(IAddressData addressData, ILogger<AddressesController> logger)
    {
        this.addressData = addressData;
        this.logger = logger;
    }

    // GET api/AddressesController/5
    /// <summary>
    /// Get a list of a users addresses
    /// </summary>
    /// <remarks>
    /// Sample Response:
    /// [
    /// {
    ///  "id": 1,
    ///  "country": "USA",
    ///  "city": "Milwaukee",
    ///  "zipCode": "5002",
    ///  "street": "Watt Str. 66"
    ///}    
    /// ]
    /// </remarks>
    /// <returns>A list of addresses</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ReadAddressDTO>> GetUserAddresses(int id)
    {
        try
        {
            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }
          
            var output = await addressData.GetUserAddresses(id);

            if (output.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            logger.LogInformation("The api/Addresses{id} was called", id);
            return Ok(output);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "The given Id of {id} was invalid", id);
            return BadRequest("The index was out of range.");
        }
    }

    // DELETE api/AddressesController/5
    /// <summary>
    /// Delete the address of a user
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUserAddresses(int id)
    {
        try
        {
            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            await addressData.DeleteAddress(id);

            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "The given Id of {id} was invalid", id);
            return BadRequest("The index was out of range.");
        }
    }

    // PUT api/AddressesController/5
    /// <summary>
    /// Update an address of a user
    /// </summary>
    /// <remarks>
    ///{
    ///"id": 1,
    ///"country": "USA",
    ///"city": "Boston",
    ///"zipCode": "2050",
    ///"street": "Williams Str. 12."
    ///}
    /// </remarks>
    /// <returns>List of addresses</returns>
[HttpPut("{id}")]
    public async Task<ActionResult<ReadAddressDTO>> UpdateAddress(int id, [FromBody] UpdateAddressDTO updatemodel)
    {       
        try
        {
            if (id < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var output = await addressData.UpdateAddress(id, updatemodel);

            return Ok(output);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "The given Id of {id} was invalid", id);
            return BadRequest("The index was out of range.");
        }
    }
}
