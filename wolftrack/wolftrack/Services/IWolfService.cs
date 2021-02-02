using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wolftrack.DTO;
using wolftrack.Models;

namespace wolftrack.Services
{
    public interface IWolfService
    {
        Task<ServiceResponse<List<GetWolfDTO>>> GetAllWolves();
        Task<ServiceResponse<GetWolfDTO>> GetWolfById(int id);
        Task<ServiceResponse<List<GetWolfDTO>>> AddWolf(AddWolfDTO wolf);
        Task<ServiceResponse<GetWolfDTO>> UpdateWolf(UpdateWolfDTO updateWolf , int id);
        Task<ServiceResponse<List<GetWolfDTO>>> DeleteWolf(int id);
        Task<ServiceResponse<GetWolfDTO>> WolfToPack(int id, string packname);
    }
}
