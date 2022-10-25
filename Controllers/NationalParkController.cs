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
    public class NationalParkController : Controller
    {
        private INationalParkRepository npRepo;
        private readonly IMapper mapper;
        public NationalParkController(INationalParkRepository npRepo, IMapper mapper)
        {
            this.npRepo = npRepo;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllNationalParks()
        {
            var nationalParkList = npRepo.GetNationalParks();
            var nationalParkDto = new List<NationalParkDto>();
            foreach(var obj in nationalParkList)
            {
                nationalParkDto.Add(mapper.Map<NationalParkDto>(obj));
            }
            return Ok(nationalParkDto);
        }
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var nationalPark = npRepo.GetNationalPark(nationalParkId);
            if (nationalPark == null)
                return NotFound();
            var nationalParkDto = new NationalParkDto();
            nationalParkDto = mapper.Map<NationalParkDto>(nationalPark);
            return Ok(nationalParkDto);
        }
        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if(nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }
            if(npRepo.NationalParkExists(nationalParkDto.Id))
            {
                ModelState.AddModelError("", "National Park Exists!");
                return StatusCode(404, ModelState);
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var nationalPark = mapper.Map<NationalPark>(nationalParkDto);
            if(!npRepo.CreateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong while saving the record {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalPark.Id}, nationalPark);
        }
        [HttpPatch]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || nationalParkId != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var nationalPark = mapper.Map<NationalPark>(nationalParkDto);
            if(!npRepo.UpdateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong while updating the record {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if(!npRepo.NationalParkExists(nationalParkId))
            {
                ModelState.AddModelError("", $"National Park does not exists with id {nationalParkId}");
                return StatusCode(404, ModelState);
            }
            if(!npRepo.DeleteNationalPark(nationalParkId))
            {
                ModelState.AddModelError("", $"Something went wrong while deleting National Park with Id : {nationalParkId}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
