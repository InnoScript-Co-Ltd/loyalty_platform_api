using LoyaltyPlatform.DataAccess.Interface;
using LoyaltyPlatform.Logging;
using LoyaltyPlatform.Model.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoyaltyPlatform.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        LoggerHelper _logHelper;
        private readonly ICountryRepository _countryRepository;        

        public CountryController(ICountryRepository countryRepository)
        {
            _logHelper = LoggerHelper.Instance;
            _countryRepository = countryRepository;
        }

        //Get, Post, Put, Delete

        // GET: api/<CountryController>
        [HttpGet]
        public ActionResult<IEnumerable<CountryDTO>> Get()
        {
            try
            {                
                IEnumerable<CountryDTO> lstCountry = _countryRepository.GetAllCountry();
                if (!lstCountry.Any())
                {
                    return NoContent();
                }
                // Add a custom header
                //Response.Headers.Add("X-Custom-Header", "foo");
                return Ok(lstCountry);

            }
            catch (Exception ex)
            {
                _logHelper.LogError(ex);
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }

        // GET: api/<CountryController>/5
        [HttpGet("{id}")]
        public ActionResult<CountryDTO> Get(int id)
        {
            try
            {
                _logHelper.LogInfo("test info log");
                if (id == 0)
                {                    
                    return BadRequest();
                }                
                var result = _countryRepository.GetCountry(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logHelper.LogError(ex);
                return StatusCode(500, "An error occurred while processing your request.");
            }            
        }
                
        // POST api/<CountryController>
        [HttpPost]
        public ActionResult<CountryDTO> Post([FromBody] CountryDTO countryDTO)
        {
            try
            {
                if (countryDTO == null)
                {
                    return BadRequest();
                }
                var createdCountry = _countryRepository.AddCountry(countryDTO);

                return CreatedAtAction(nameof(Get), new { id = createdCountry.Id }, createdCountry);

            }
            catch (Exception ex)
            {
                _logHelper.LogError(ex);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/<CountryController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] CountryDTO countryDTO)
        {
            try
            {
                if (countryDTO == null || id != countryDTO.Id)
                {
                    return BadRequest();
                }

                var result = _countryRepository.UpdateCountry(countryDTO);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logHelper.LogError(ex);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // DELETE api/<CountryController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var result = _countryRepository.DeleteCountry(id);
                if (!result)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logHelper.LogError(ex);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
