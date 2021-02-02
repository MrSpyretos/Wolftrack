using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wolftrack.Database;
using wolftrack.DTO;
using wolftrack.Models;
using wolftrack.Services;

namespace wolftrack.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WolfController : ControllerBase
    {
        private readonly WolftrackContext _context;
        private readonly IWolfService _wolfService;
        public WolfController(WolftrackContext context, IWolfService wolfService)
        {
            _context = context;
            _wolfService = wolfService;

        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllWolves()
        {
            return Ok(await _wolfService.GetAllWolves());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWolfById(int id)
        {
            ServiceResponse<GetWolfDTO> response = await _wolfService.GetWolfById(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> AddWolf(AddWolfDTO wolf)
        {
            return Ok(await _wolfService.AddWolf(wolf));
        }
        [HttpPut("{id}/{location}")]
        public async Task<IActionResult> UpdateLocation(int id, string location)
        {
            if (_context.Wolves.Where(w => w.WolfId == id).FirstOrDefault() != null)
            {
                UpdateWolfDTO updateWolf = new UpdateWolfDTO();
                updateWolf.Location = location;
                ServiceResponse<GetWolfDTO> response = await _wolfService.UpdateWolf(updateWolf, id);
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut("{id}/in/{packname}")]
        public async Task<IActionResult> UpdatePack(int id,string packname)
        {
            if (_context.Wolves.Where(w => w.WolfId == id).FirstOrDefault() != null)
            {
                ServiceResponse<GetWolfDTO> response = await _wolfService.WolfToPack(id ,packname);
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWolf(UpdateWolfDTO updateWolf , int id)
        {
            ServiceResponse<GetWolfDTO> response = await _wolfService.UpdateWolf(updateWolf, id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWolf(int id)
        {
            ServiceResponse<List<GetWolfDTO>> response = await _wolfService.DeleteWolf(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
