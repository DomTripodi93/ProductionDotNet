using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BackEnd.Data;
using BackEnd.Dtos;
using BackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Authorize]
    [Route("api/{userId}/[controller]")]
    [ApiController]
    public class PartController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IManagementRepository _repo;

        public PartController(IMapper mapper, IManagementRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }
        


        [HttpPost]
        public async Task<IActionResult> AddPart(int userId, PartForCreationDto partForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var part = _mapper.Map<Part>(partForCreationDto);

            part.userId = userId;

            _repo.Add(part);

            if (await _repo.SaveAll())
            {
                var partToReturn = _mapper.Map<PartForCreationDto>(part);
                return CreatedAtRoute("GetPart", new {part = part.PartNumber}, partToReturn);
            }
                
            throw new Exception("Creation of part lot failed on save");
        }

        [HttpPut("{part}")]
        public async Task<IActionResult> UpdatePart(int userId, string part, PartForCreationDto partForUpdateDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var partFromRepo = await _repo.GetPart(userId, part);

            _mapper.Map(partForUpdateDto, partFromRepo);

            if (await _repo.SaveAll())
                return CreatedAtRoute("GetPart", new {part = partFromRepo.PartNumber}, partForUpdateDto);

            throw new Exception($"Updating part {part} failed on save");
        }

        [HttpGet("{part}", Name = "GetPart")]
        public async Task<IActionResult> GetPart(int userId, string part)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var partFromRepo = await _repo.GetPart(userId, part);

            PartForReturnDto partForReturn = _mapper.Map<PartForReturnDto>(partFromRepo);

            return Ok(partForReturn);
        }


        [HttpGet]
        public async Task<IActionResult> GetParts(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            IEnumerable<Part> directParts = await _repo.GetParts(userId);

            var parts = _mapper.Map<IEnumerable<PartForReturnDto>>(directParts);

            return Ok(parts);
        }

        [HttpGet("job={job}")]
        public async Task<IActionResult> GetPartByJob(int userId, string job)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            Part directPart = await _repo.GetPartByJob(userId, job);

            var part = _mapper.Map<PartForReturnDto>(directPart);

            return Ok(part);
        }


        [HttpDelete("{part}")]
        public async Task<IActionResult> DeletePart(int userId, string part)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var partToDelete = await _repo.GetPart(userId, part);
            
            if (userId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                _repo.Delete(partToDelete);
                await _repo.SaveAll();
                return Ok(
                            partToDelete.PartNumber
                            +" and any associated Jobs, Ops, Production, and Hourly Tracking was deleted!"
                        );
        }
    }
}