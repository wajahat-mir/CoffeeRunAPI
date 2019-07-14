using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoffeeRunAPI.Filters;
using CoffeeRunAPI.Models;
using CoffeeRunAPI.Repositories;
using CoffeeRunAPI.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeRunAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunController : ControllerBase
    {
        private readonly IRunRepository _runRepository;
        private readonly IMapper _mapper;

        public RunController(IRunRepository runRepository, IMapper mapper)
        {
            _runRepository = runRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RunViewModel>>> Get()
        {
            var runs = await _runRepository.GetAllRuns();
            if (runs == null)
                return NotFound();

            var runViewModels = _mapper.Map<IEnumerable<RunViewModel>>(runs);
            return Ok(runViewModels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RunViewModel>> Get(int id)
        {
            var run = await _runRepository.GetRun(id);
            if (run == null)
                return NotFound();

            var runViewModel = _mapper.Map<RunViewModel>(run);
            return Ok(runViewModel);
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateModelAttribute))]
        public async Task<ActionResult> Patch(int id, [FromBody]JsonPatchDocument<RunViewModel> runPatch)
        {
            var run = await _runRepository.GetRun(id);
            if (run == null)
                return NotFound();

            RunViewModel runViewModel = _mapper.Map<RunViewModel>(run);
            runPatch.ApplyTo(runViewModel);

            var patchedRun = _mapper.Map<Run>(runViewModel);

            if (await _runRepository.UpdateRun(patchedRun))
                return Ok(runViewModel);
            return BadRequest();
        }
    }
}
