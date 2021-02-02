using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wolftrack.Database;
using wolftrack.DTO;
using wolftrack.Models;
using wolftrack.Services;

namespace wolftrack.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PackController : ControllerBase
    {
        private readonly WolftrackContext _context;
        private readonly IPackService _packService;
        public PackController(WolftrackContext context, IPackService packService)
        {
            _context = context;
            _packService = packService;

        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPacks()
        {
            return Ok(await _packService.GetAllPacks());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackById(int id)
        {
            ServiceResponse<GetPackDTO> response = await _packService.GetPackById(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> AddPack(AddPackDTO pack)
        {
            return Ok(await _packService.AddPack(pack));
        }
        [HttpPut("{packname}/{wolfname}")]
        public async Task<IActionResult> RemoveWolf(string packname , string wolfname)
        {
            ServiceResponse<GetWolfDTO> response = await _packService.RemoveWolf(packname, wolfname);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePack(UpdatePackDTO updatePack , int id)
        {
            ServiceResponse<GetPackDTO> response = await _packService.UpdatePack(updatePack , id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePack(int id)
        {
            ServiceResponse<List<GetPackDTO>> response = await _packService.DeletePack(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}