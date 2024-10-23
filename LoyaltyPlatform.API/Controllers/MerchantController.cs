using LoyaltyPlatform.DataAccess.Interface;
using LoyaltyPlatform.Logging;
using LoyaltyPlatform.Model.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LoyaltyPlatform.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        private readonly LoggerHelper _logHelper;
        private readonly IMerchantRepository _merchantRepository;

        public MerchantController(IMerchantRepository merchantRepository)
        {
            _logHelper = LoggerHelper.Instance;
            _merchantRepository = merchantRepository;
        }

        // GET: api/<MerchantController>
        [HttpGet]
        public ActionResult<IEnumerable<MerchantDTO>> Get([FromQuery] PageSortParam pageSortParam)
        {
            try
            {
                var merchantPagingDTO = _merchantRepository.GetAllMerchant(pageSortParam);
                if (!merchantPagingDTO.Merchants.Any())
                {
                    return NoContent();
                }
                return Ok(merchantPagingDTO);
            }
            catch (Exception ex)
            {
                _logHelper.LogError(ex);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // GET: api/<MerchantController>/5
        [HttpGet("{id}")]
        public ActionResult<MerchantDTO> Get(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var result = _merchantRepository.GetMerchant(id);
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

        // POST api/<MerchantController>
        [HttpPost]
        public ActionResult<MerchantDTO> Post([FromBody] MerchantDTO merchantDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (merchantDTO == null)
                {
                    return BadRequest();
                }
                var createdMerchant = _merchantRepository.AddMerchant(merchantDTO);
                return CreatedAtAction(nameof(Get), new { id = createdMerchant.Id }, createdMerchant);
            }
            catch (Exception ex)
            {
                _logHelper.LogError(ex);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/<MerchantController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] MerchantDTO merchantDTO)
        {
            try
            {
                if (merchantDTO == null || id != merchantDTO.Id)
                {
                    return BadRequest();
                }

                var result = _merchantRepository.UpdateMerchant(merchantDTO);
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

        // DELETE api/<MerchantController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var result = _merchantRepository.DeleteMerchant(id);
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
