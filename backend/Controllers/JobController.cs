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
    public class JobController: ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IManagementRepository _repo;

        public JobController(IMapper mapper, IManagementRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }
        


        [HttpPost]
        public async Task<IActionResult> AddJob(int userId, JobForCreationDto jobForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var job = _mapper.Map<Job>(jobForCreationDto);

            var partInfo = await _repo.GetPart(userId, jobForCreationDto.PartNum);

            job.PartNum = partInfo.PartNumber;
            job.userId = userId;

            _repo.Add(job);

            if (await _repo.SaveAll())
            {
                var jobToReturn = _mapper.Map<JobForReturnDto>(job);
                return CreatedAtRoute("GetJob", new {jobNum = job.JobNumber}, jobToReturn);
            }
                
            throw new Exception("Creation of job lot failed on save");
        }

        [HttpPut("{jobNum}")]
        public async Task<IActionResult> UpdateJob(int userId, string jobNum, JobForCreationDto jobForUpdateDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var jobFromRepo = await _repo.GetJob(userId, jobNum);

            _mapper.Map(jobForUpdateDto, jobFromRepo);

            if (await _repo.SaveAll())
                return CreatedAtRoute("GetJob", new {jobNum = jobFromRepo.JobNumber}, jobForUpdateDto);

            var newData = _mapper.Map(jobForUpdateDto, jobFromRepo);

            if (jobFromRepo == newData)
                return Ok(jobForUpdateDto);

            throw new Exception($"Updating job lot {jobNum} failed on save");
        }

        [HttpGet("{jobNum}", Name = "GetJob")]
        public async Task<IActionResult> GetJob(int userId, string jobNum)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            Job job = await _repo.GetJob(userId, jobNum);
            JobForReturnDto jobForReturn = _mapper.Map<JobForReturnDto>(job);
            return Ok(jobForReturn);
        }


        [HttpGet]
        public async Task<IActionResult> GetJobs(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            IEnumerable<Job> directJobs = await _repo.GetJobs(userId);

            var jobs = _mapper.Map<IEnumerable<JobForReturnDto>>(directJobs);

            return Ok(jobs);
        }

        [HttpGet("part={partNum}")]
        public async Task<IActionResult> GetJobsByJob(int userId, string partNum)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            IEnumerable<Job> directJobs = await _repo.GetJobsByPart(userId, partNum);

            var jobs = _mapper.Map<IEnumerable<JobForReturnDto>>(directJobs);

            return Ok(jobs);
        }

        [HttpDelete("{jobNum}")]
        public async Task<IActionResult> DeleteJob(int userId, string jobNum)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var jobToDelete = await _repo.GetJob(userId, jobNum);
            
            if (userId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                _repo.Delete(jobToDelete);
                await _repo.SaveAll();
                return Ok(
                            "Operation "
                            + jobToDelete.Operation
                            + " for job# " 
                            + jobToDelete.JobNumber 
                            +" was deleted, along with related production lots and hourly counts!"
                        );
        }
        
    }
}