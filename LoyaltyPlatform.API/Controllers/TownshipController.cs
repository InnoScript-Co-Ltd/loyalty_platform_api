using LoyaltyPlatform.DataAccess.Implementation;
using LoyaltyPlatform.DataAccess.Interface;
using LoyaltyPlatform.Logging;
using LoyaltyPlatform.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace LoyaltyPlatform.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TownshipController : ControllerBase
    {
        LoggerHelper _logHelper;
        private readonly ITownshipRepository _townshipRepository;
        public TownshipController(ITownshipRepository townshipRepository)
        {
            _logHelper = LoggerHelper.Instance;
            _townshipRepository = townshipRepository;
        }
        //GET: api/<TownshipController>
        [HttpGet]
        public ActionResult<TownshipPagingDTO> Get([FromQuery] PageSortParam pageSortParam)
        {
            try
            {
                TownshipPagingDTO townshipPagingDTO = _townshipRepository.GetAllTownship(pageSortParam);
                Console.WriteLine(townshipPagingDTO);
                if (!townshipPagingDTO.Townships.Any())
                {
                    return NoContent();
                }
                
                // Add a custom header
                //Response.Headers.Add("X-Custom-Header", "foo");
                return Ok(townshipPagingDTO);

            }
            catch (Exception ex)
            {
                _logHelper.LogError(ex);
                return StatusCode(500, "An error occurred while processing your request.");

            }
        }
        [HttpGet("{id}")]
        public ActionResult<TownshipDTO> Get(int id)
        {
            try
            {
                _logHelper.LogInfo("test info log");
                if (id == 0)
                {
                    return BadRequest();
                }
                var result = _townshipRepository.GetTownship(id);
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

        // POST api/<TownshipController>
        [HttpPost]
        public ActionResult<TownshipDTO> Post([FromBody] TownshipDTO townshipDTO)
        {
            try
            {
                if (townshipDTO == null)
                {
                    return BadRequest();
                }
                var createdTownship = _townshipRepository.AddTownship(townshipDTO);
                return CreatedAtAction(nameof(Get), new { id = createdTownship.Id }, createdTownship);
            }
            catch (Exception ex)
            {
                _logHelper.LogError(ex);
                return StatusCode(500, "An error occurred while processing your request.");

            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] TownshipDTO townshipDTO)
        {
            try
            {
                if (townshipDTO == null || id != townshipDTO.Id)
                {
                    return BadRequest();
                }
                var result = _townshipRepository.UpdateTownship(townshipDTO);

                if (!result)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logHelper.LogError(ex);
                return StatusCode(500, "An error occurred while processing your request.");

            }

        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var result = _townshipRepository.DeleteTownship(id);
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
