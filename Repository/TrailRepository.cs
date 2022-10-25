using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext db;
        public TrailRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public bool CreateTrail(Trail trail)
        {
            db.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(int id)
        {
            if(TrailExists(id))
            {
                Trail trail = GetTrail(id);
                db.Trails.Remove(trail);
            }
            return Save();
        }

        public Trail GetTrail(int trailId)
        {
            return db.Trails.Include(c => c.NationalPark).FirstOrDefault(trail => trail.Id == trailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return db.Trails.Include(c => c.NationalPark).OrderBy(trail => trail.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            return db.Trails.Any(trail => trail.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool TrailExists(int id)
        {
            return db.Trails.Any(trail => trail.Id == id);
        }

        public bool Save()
        {
            return db.SaveChanges() >= 0 ? true : false;                
        }

        public bool UpdateTrail(Trail trail)
        {
            db.Trails.Update(trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int nationalParkId)
        {
            return db.Trails.Include(c => c.NationalPark).Where(c => c.NationalParkId == nationalParkId).ToList();
        }
    }
}
