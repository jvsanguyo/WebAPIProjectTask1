using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestProject.Models;
using TestProject.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.IRepository;
using TestProject.Data;
using Microsoft.AspNetCore.Authorization;

namespace TestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidSummaryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CovidSummaryController(IUnitOfWork unitOfWork,
           IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSummary()
        {
            var summary = await _unitOfWork.Summaries.GetAll(new List<string> { "Global","Countries" });
            var result = _mapper.Map<List<CovidSummary>>(summary);
            return Ok(result);
        }

        [HttpGet("{id:guid}", Name = "GetSummary")]
        public async Task<IActionResult> GetSummary(Guid id)
        {
            var summary = await _unitOfWork.Summaries.Get(q => q.ID == id, new List<string> { "Countries" });
            var result = _mapper.Map<CovidSummary>(summary);
            return Ok(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> CreateSummary([FromBody] CovidSummary covidSummary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var summary = _mapper.Map<Summary>(covidSummary);
            await _unitOfWork.Summaries.Insert(summary);
            await _unitOfWork.Save();

            return CreatedAtRoute("GetSummary", new { id = summary.ID }, covidSummary);
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateSummary(Guid id, [FromBody] CovidSummary covidSummary)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var summary = await _unitOfWork.Summaries.Get(q => q.ID == id, new List<string> { "Global", "Countries" });
                _mapper.Map(covidSummary, summary);
                _unitOfWork.Summaries.Update(summary);

                _unitOfWork.Global.Update(summary.Global);

                foreach (var item in summary.Countries)
                {
                    _unitOfWork.Countries.Update(item);
                }

                await _unitOfWork.Save();
                return Ok("Summary has been updated...");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSummary(Guid id)
        {
            try
            {
                var country = await _unitOfWork.Summaries.Get(q => q.ID == id);
                await _unitOfWork.Summaries.Delete(id);
                await _unitOfWork.Save();
                return Ok("Summary has been deleted...");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
