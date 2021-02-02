using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using wolftrack.Database;
using wolftrack.DTO;
using wolftrack.Models;

namespace wolftrack.Services
{
    public class WolfService : IWolfService
    {
        private readonly WolftrackContext _context;
        private readonly IMapper _mapper;
        public WolfService(WolftrackContext wolftrackContext, IMapper mapper)
        {
            _context = wolftrackContext;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<List<GetWolfDTO>>> AddWolf(AddWolfDTO wolf)
        {
            ServiceResponse<List<GetWolfDTO>> serviceResponse = new ServiceResponse<List<GetWolfDTO>>();
            if (_context.Wolves.Any(e => e.Name == wolf.Name))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Name already exists !";
                return serviceResponse;
            }
            if (wolf.Birthday != null)
            {
                string pattern = @"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[13-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$";
                var match = Regex.Match(wolf.Birthday, pattern, RegexOptions.IgnoreCase);
                if (!match.Success == true)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Wrong date format ! Please use either dd.MM.yyyy or dd-MM-yyyy or dd/MM/yyyy";
                    return serviceResponse;
                }
            }
            Wolf wolf1 = _mapper.Map<Wolf>(wolf);
            await _context.Wolves.AddAsync(wolf1);
            await _context.SaveChangesAsync();
            serviceResponse.Data = (_context.Packs.Select(c => _mapper.Map<GetWolfDTO>(c))).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetWolfDTO>>> DeleteWolf(int id)
        {
            ServiceResponse<List<GetWolfDTO>> serviceResponse = new ServiceResponse<List<GetWolfDTO>>();
            try
            {
                Wolf wolf =await _context.Wolves.FirstAsync(c => c.WolfId == id);
                _context.Wolves.Remove(wolf);
                await _context.SaveChangesAsync();
                serviceResponse.Data = (_context.Wolves.Select(c => _mapper.Map<GetWolfDTO>(c))).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetWolfDTO>>> GetAllWolves()
        {
            ServiceResponse<List<GetWolfDTO>> serviceResponse = new ServiceResponse<List<GetWolfDTO>>();
            List<Wolf> dbWolves = await _context.Wolves.ToListAsync();
            serviceResponse.Data = dbWolves.Select(c => _mapper.Map<GetWolfDTO>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetWolfDTO>> GetWolfById(int id)
        {
            ServiceResponse<GetWolfDTO> serviceResponse = new ServiceResponse<GetWolfDTO>();
            Wolf dbWolf = await _context.Wolves.FirstOrDefaultAsync(c => c.WolfId == id);
            serviceResponse.Data = _mapper.Map<GetWolfDTO>(dbWolf);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetWolfDTO>> UpdateWolf(UpdateWolfDTO updateWolf , int id)
        {
            ServiceResponse<GetWolfDTO> serviceResponse = new ServiceResponse<GetWolfDTO>();
            try
            {
               Wolf wolf = await _context.Wolves.FirstOrDefaultAsync(c => c.WolfId == id);
                if (updateWolf.Name != null)
                {
                    wolf.Name = updateWolf.Name;
                }
                if (updateWolf.Birthday != null)
                {
                    wolf.Birthday = updateWolf.Birthday;
                }
                if (updateWolf.Gender != null)
                {
                    wolf.Gender = updateWolf.Gender;
                }
                if (updateWolf.Location != null)
                {
                    wolf.Location = Location(updateWolf);
                }
                _context.Wolves.Update(wolf);
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetWolfDTO>(wolf);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
        public string Location(UpdateWolfDTO updateWolf)
        {
            var loc = updateWolf.Location;
            string url = "https://geocode.xyz/?locate=" + loc + "&json=1";
            var request = System.Net.WebRequest.Create(url);

            using (WebResponse wrs = request.GetResponse())
            using (Stream stream = wrs.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream)) {
                string json = reader.ReadToEnd();
                var obj = JObject.Parse(json);
                string City = (string)obj["standard"]["city"];
                string Country = (string)obj["standard"]["countryname"];
                string CountryCode = (string)obj["standard"]["prov"];
                return (CountryCode + " - " + Country + "," + City);
        }
        }

    public async Task<ServiceResponse<GetWolfDTO>> WolfToPack(int id,string packname)
        {
            ServiceResponse<GetWolfDTO> serviceResponse = new ServiceResponse<GetWolfDTO>();
            Wolf wolf = await _context.Wolves.Include(p=>p.Pack).FirstOrDefaultAsync(c => c.WolfId == id);
            Pack pack = _context.Packs.Where(p => p.Name == packname).FirstOrDefault();
            wolf.Pack = pack;
            _context.Wolves.Update(wolf);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetWolfDTO>(wolf);
            return serviceResponse;
        }
    }
}
