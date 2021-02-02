using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wolftrack.DTO;
using wolftrack.Models;

namespace wolftrack.Services
{
    public interface IPackService
    {
        Task<ServiceResponse<List<GetPackDTO>>> GetAllPacks();
        Task<ServiceResponse<GetPackDTO>> GetPackById(int id);
        Task<ServiceResponse<List<GetPackDTO>>> AddPack(AddPackDTO pack);
        Task<ServiceResponse<GetPackDTO>> UpdatePack(UpdatePackDTO updatePack , int id);
        Task<ServiceResponse<List<GetPackDTO>>> DeletePack(int id);
        Task<ServiceResponse<GetWolfDTO>> RemoveWolf(string packname , string wolfname);
    }
}
