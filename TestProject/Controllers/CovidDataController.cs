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

namespace TestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidDataController : ControllerBase
    {
        private readonly IDataClient _commandDataClient;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CovidDataController(IDataClient commandDataClient,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _commandDataClient = commandDataClient;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("getcovidsummary")]
        public async Task<IActionResult> GetCovidSummary()
        {
            if (!_unitOfWork.HasSummaryData())
            {
                var result = await _commandDataClient.GetSummaries();
                var summary = _mapper.Map<Summary>(result);
                await _unitOfWork.Summaries.Insert(summary);
                await _unitOfWork.Save();
                return Ok("Covid Summary Data has been saved...");
            }
            return NoContent();
        }

        [HttpGet]
        [Route("getcovidhistory")]
        public async Task<IActionResult> GetCovidHistory(string country)
        {
            if (!_unitOfWork.HasHistoryData())
            {
                var result = await _commandDataClient.GetHistory(country);
                var histories = _mapper.Map<List<History>>(result);
                await _unitOfWork.Histories.InsertRange(histories);
                await _unitOfWork.Save();
                return Ok("Covid History Data has been saved...");
            }

            return NoContent();
        }

    }
}
