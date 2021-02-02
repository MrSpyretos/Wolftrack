using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wolftrack.Database;
using wolftrack.DTO;
using wolftrack.Models;

namespace wolftrack.Services
{
    public class PackService : IPackService
    {
        private readonly WolftrackContext _context;
        private readonly IMapper _mapper;
        public PackService(WolftrackContext wolftrackContext , IMapper mapper)
        {
            _context = wolftrackContext;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<List<GetPackDTO>>> AddPack(AddPackDTO pack)
        {
            ServiceResponse<List<GetPackDTO>> serviceResponse = new ServiceResponse<List<GetPackDTO>>();
            if (_context.Packs.Any(e => e.Name == pack.Name))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Name already exists !";
                return serviceResponse;
            }
            Pack pack1 = _mapper.Map<Pack>(pack);
            await _context.Packs.AddAsync(_mapper.Map<Pack>(pack1));
            await _context.SaveChangesAsync();
            serviceResponse.Data = (_context.Packs.Select(c=>_mapper.Map<GetPackDTO>(c))).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPackDTO>>> DeletePack(int id)
        {
            ServiceResponse<List<GetPackDTO>> serviceResponse = new ServiceResponse<List<GetPackDTO>>();
            try
            {
                Pack pack = _context.Packs.First(c => c.PackId == id);
                _context.Packs.Remove(pack);
                await _context.SaveChangesAsync();
                serviceResponse.Data = (_context.Packs.Select(c => _mapper.Map<GetPackDTO>(c))).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPackDTO>>> GetAllPacks()
        {
            ServiceResponse<List<GetPackDTO>> serviceResponse = new ServiceResponse<List<GetPackDTO>>();
            List<Pack> dbPacks = await _context.Packs.Include(w=>w.Wolves).ToListAsync();
            serviceResponse.Data = dbPacks.Select(c => _mapper.Map<GetPackDTO>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPackDTO>> GetPackById(int id)
        {
            ServiceResponse<GetPackDTO> serviceResponse = new ServiceResponse<GetPackDTO>();
            Pack dbPack = await _context.Packs.Include(w=>w.Wolves).FirstOrDefaultAsync(c => c.PackId == id);
            serviceResponse.Data= _mapper.Map<GetPackDTO>(dbPack);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetWolfDTO>> RemoveWolf(string packname ,string wolfname)
        {
            ServiceResponse<GetWolfDTO> serviceResponse = new ServiceResponse<GetWolfDTO>();
            Pack pack = _context.Packs.Where(p => p.Name == packname).FirstOrDefault();
            if (pack != null)
            {
                Wolf dbWolf = _context.Wolves.Where(w => w.Name == wolfname).SingleOrDefault();
                dbWolf.Pack = null;
                _context.Wolves.Update(dbWolf);
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetWolfDTO>(dbWolf);
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "No such Pack name in Database ";
            }
                return serviceResponse;
        }

        public async Task<ServiceResponse<GetPackDTO>> UpdatePack(UpdatePackDTO updatePack , int id)
        {
            ServiceResponse<GetPackDTO> serviceResponse = new ServiceResponse<GetPackDTO>();
            try
            {
                Pack pack = _context.Packs.FirstOrDefault(c => c.PackId == id);
                pack.Name = updatePack.Name;
                _context.Packs.Update(pack);
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetPackDTO>(pack);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
