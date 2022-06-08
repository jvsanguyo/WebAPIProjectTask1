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
    public class CovidHistoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CovidHistoryController(IUnitOfWork unitOfWork,
           IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHistory()
        {
            var history = await _unitOfWork.Histories.GetAll();
            var result = _mapper.Map<List<CovidHistory>>(history);
            return Ok(result);
        }

        [HttpGet("{id:int}", Name = "GetHistory")]
        public async Task<IActionResult> GetHistory(int id)
        {
            var history = await _unitOfWork.Histories.Get(q => q.Id == id);
            var result = _mapper.Map<CovidHistory>(history);
            return Ok(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> CreateHistory([FromBody] CovidHistory covidHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var history = _mapper.Map<History>(covidHistory);
            await _unitOfWork.Histories.Insert(history);
            await _unitOfWork.Save();

            return CreatedAtRoute("GetHistory", new { id = history.Id }, covidHistory);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateHistory(int id, [FromBody] CovidHistory covidHistory)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var history = await _unitOfWork.Histories.Get(q => q.Id == id);
                _mapper.Map(covidHistory, history);
                _unitOfWork.Histories.Update(history);
                await _unitOfWork.Save();
                return Ok("History has been updated...");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteHistory(int id)
        {
            try
            {
                var country = await _unitOfWork.Histories.Get(q => q.Id == id);
                await _unitOfWork.Histories.Delete(id);
                await _unitOfWork.Save();
                return Ok("History has been deleted...");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
