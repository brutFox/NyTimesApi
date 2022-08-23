using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NyTimesData;
using NyTimesServices.Contracts;
using NyTimesSharedModels.Models;

namespace NyTimesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NyTimesTopNewsController : ControllerBase
    {
        private readonly INyTimesTopNewsServices _nyTimesTopNewsServices;
        private readonly NyTimesDBContext _context;
        public NyTimesTopNewsController(INyTimesTopNewsServices nyTimesTopNewsServices, NyTimesDBContext context)
        {
            _nyTimesTopNewsServices = nyTimesTopNewsServices;
            _context = context;
        }

        [HttpGet("GetNyTimesTopNewsBySection")]
        public async Task<ActionResult> GetNyTimesTopNewsBySection(string section)
        {
            try
            {
                var result = await _nyTimesTopNewsServices.GetDataFromNyTimesAsync(section);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}
