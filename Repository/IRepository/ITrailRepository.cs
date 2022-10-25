
using ParkyAPI.Models;
using System.Collections.Generic;

namespace ParkyAPI.Repository.IRepository
{
    public interface ITrailRepository
    {
        ICollection<Trail> GetTrails();
        ICollection<Trail> GetTrailsInNationalPark(int nationalParkId);
        Trail GetTrail(int nationalParkId);
        bool TrailExists(string name);
        bool TrailExists(int id);
        bool CreateTrail(Trail nationalPark);
        bool UpdateTrail(Trail nationalPark);
        bool DeleteTrail(int id);
        bool Save();

    }
}
