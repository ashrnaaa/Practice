using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using System.Collections;
using Microsoft.AspNetCore.Routing;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository _campRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public CampsController(ICampRepository campRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _campRepository = campRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }
        [HttpGet]
        public async Task<ActionResult<CampsModel[]>> GetCamps(bool includeTalks = false)
        {
            try
            {
                var result = await _campRepository.GetAllCampsAsync(includeTalks);

                return _mapper.Map<CampsModel[]>(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Something wrong on our end.");
            }
        }

        [HttpGet("{moniker}")]
        public async Task<ActionResult<CampsModel>> GetCamp(string moniker, bool includeTalks = false)
        {
            try
            {
                var result = await _campRepository.GetCampAsync(moniker, includeTalks);

                if (result == null)
                    return NotFound();
                return _mapper.Map<CampsModel>(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Something wrong on our end.");

            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<CampsModel[]>> SearchByDate(DateTime theDate, bool includeTalks = false)
        {
            try
            {
                var results = await _campRepository.GetAllCampsByEventDate(theDate, includeTalks);

                if (results == null)
                    return NotFound();

                return _mapper.Map<CampsModel[]>(results);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Something wrong on our end.");

            }
        }

        [HttpPost]
        public async Task<ActionResult<CampsModel>> CreateCamp(CampsModel model)
        {
            try
            {
                var campExists = _campRepository.GetCampAsync(model.Moniker);
                if (campExists != null)
                    return BadRequest("Moniker already in use!");
                var location = _linkGenerator.GetPathByAction($"GetCamp", $"Camps", new { moniker = model.Moniker });

                if (string.IsNullOrWhiteSpace(location))
                    return BadRequest("Could not use that moniker");
                var camp = _mapper.Map<Camp>(model);
                _campRepository.Add(camp);

                if (await _campRepository.SaveChangesAsync())
                {
                    return Created(location, _mapper.Map<CampsModel>(camp));
                }

            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Something wrong on our end.");

            }

            return BadRequest("wrong input");
        }

        [HttpPut("{moniker}")]
        public async Task<ActionResult<CampsModel>> UpdateCampModel(string moniker, CampsModel model)
        {
            try
            {
                var camp = await _campRepository.GetCampAsync(moniker);
                if (camp == null) return NotFound($"Could not find the camp with given moniker");

                _mapper.Map(model, destination: camp);

                if (await _campRepository.SaveChangesAsync())
                {
                    return _mapper.Map<CampsModel>(camp);
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Something wrong on our end.");
            }

            return BadRequest();
        }

        [HttpDelete("{moniker}")]
        public async Task<IActionResult> Delete(string moniker)
        {
            try
            {
                var campExist = await _campRepository.GetCampAsync(moniker);
                if (campExist == null)
                {
                    return NotFound("Camp with this moniker does not exist");
                }
                _campRepository.Delete(campExist);

                if (await _campRepository.SaveChangesAsync())
                    return Ok();

            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Something wrong on our end.");
            }

            return BadRequest("Failed to delete");
        }

    }
}
