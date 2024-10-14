using LoyaltyPlatform.DataAccess.Implementation;
using LoyaltyPlatform.DataAccess.Interface;
using LoyaltyPlatform.Logging;
using LoyaltyPlatform.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;

namespace LoyaltyPlatform.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {

        LoggerHelper _logHelper; 
        private readonly IStateRepository _stateRepository;
            
        public StateController(IStateRepository stateRepository)
        {
            _logHelper = LoggerHelper.Instance;
            _stateRepository = stateRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<StatePagingDTO>> Get([FromQuery] PageSortParam pageSortParam)
        {
            try
            {
                StatePagingDTO statePagingDTO = _stateRepository.GetAllState(pageSortParam);
                if (!statePagingDTO.States.Any())
                {
                    return NoContent();
                }
               
                return Ok(statePagingDTO);

            }
            catch (Exception ex)
            {
                _logHelper.LogError(ex);
                return StatusCode(500, "An error occurred while processing your request.");

            }
        }

        [HttpGet("{id}")]
        public ActionResult<StateDTO> Get(int id)
        {
            try
            {
                _logHelper.LogInfo("test info log");
                if (id == 0)
                {
                    return BadRequest();
                }
                var result = _stateRepository.GetState(id);
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

        [HttpPost]
        public ActionResult<StateDTO> AdddState([FromBody] StateDTO stateDTO)
        {
            try
            {
                if (stateDTO == null)
                {
                    return BadRequest();
                }
                var createdState = _stateRepository.AddState(stateDTO);
                return CreatedAtAction(nameof(Get), new { id = createdState.Id }, createdState);
            }
            catch (Exception ex)
            {
                _logHelper.LogError(ex);
                return StatusCode(500, "An error occurred while processing your request.");

            }

        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] StateDTO stateDTO)
        {
            try
            {
                if (stateDTO == null || id != stateDTO.Id)
                {
                    return BadRequest();
                }
                var result = _stateRepository.UpdateState(stateDTO);
                if (!result)
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

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var result = _stateRepository.DeleteState(id);
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