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
            var result = await _commandDataClient.GetSummaries();
            var data = (await _unitOfWork.Summaries.Get(q => q.ID == result.ID));
            if (data != null)
                return NoContent();

            var summary = _mapper.Map<Summary>(result);
            await _unitOfWork.Summaries.Insert(summary);
            await _unitOfWork.Save();
            return Ok("Covid Summary Data has been saved...");
        }

        [HttpGet]
        [Route("getcovidhistory")]
        public async Task<IActionResult> GetCovidHistory(string country)
        {
            var results = await _commandDataClient.GetHistory(country);
            foreach (var item in results)
            {
                var data = await _unitOfWork.Histories.Get(q => q.ID == item.ID);
                if (data == null)
                {
                    var history = _mapper.Map<History>(item);
                    await _unitOfWork.Histories.Insert(history);
                    await _unitOfWork.Save();
                }
            }
            return Ok("Covid History Data has been saved...");
        }

    }
}
