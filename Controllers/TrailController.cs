using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailController : Controller
    {
        private ITrailRepository trailRepo;
        private readonly IMapper mapper;
        public TrailController(ITrailRepository trailRepo, IMapper mapper)
        {
            this.trailRepo = trailRepo;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllTrails()
        {
            var trailList = trailRepo.GetTrails();
            var trailDto = new List<TrailDto>();
            foreach(var obj in trailList)
            {
                trailDto.Add(mapper.Map<TrailDto>(obj));
            }
            return Ok(trailDto);
        }
        /*[HttpGet("{trailId:int}", Name = "GetTrail")]
        public IActionResult GetTrail(int trailId)
        {
            var trail = trailRepo.GetTrail(trailId);
            if (trail == null)
                return NotFound();
            var trailDto = new TrailDto();
            trailDto = mapper.Map<TrailDto>(trail);
            return Ok(trailDto);
        }*/
        [HttpPost]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if(trailDto == null)
            {
                return BadRequest(ModelState);
            }
            if(trailRepo.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists!");
                return StatusCode(404, ModelState);
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var trail = mapper.Map<Trail>(trailDto);
            if(!trailRepo.CreateTrail(trail))
            {
                ModelState.AddModelError("", $"Something went wrong while saving the record {trail.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetTrail", new { trailId = trail.Id}, trail);
        }
        [HttpPatch]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trailDto)
        {
            if (trailDto == null || trailId != trailDto.Id)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var trail = mapper.Map<Trail>(trailDto);
            if(!trailRepo.UpdateTrail(trail))
            {
                ModelState.AddModelError("", $"Something went wrong while updating the record {trail.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete]
        public IActionResult DeleteTrail(int trailId)
        {
            if(!trailRepo.TrailExists(trailId))
            {
                ModelState.AddModelError("", $"Trail does not exists with id {trailId}");
                return StatusCode(404, ModelState);
            }
            if(!trailRepo.DeleteTrail(trailId))
            {
                ModelState.AddModelError("", $"Something went wrong while deleting Trail with Id : {trailId}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
