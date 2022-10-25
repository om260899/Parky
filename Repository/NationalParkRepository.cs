using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext db;
        public NationalParkRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public bool CreateNationalPark(NationalPark nationalPark)
        {
            db.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(int id)
        {
            if(NationalParkExists(id))
            {
                NationalPark nationalPark = GetNationalPark(id);
                db.NationalParks.Remove(nationalPark);
            }
            return Save();
        }

        public NationalPark GetNationalPark(int nationalParkId)
        {
            return db.NationalParks.FirstOrDefault(nationalPark => nationalPark.Id == nationalParkId);
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return db.NationalParks.OrderBy(nationalPark => nationalPark.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            return db.NationalParks.Any(nationalPark => nationalPark.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool NationalParkExists(int id)
        {
            return db.NationalParks.Any(nationalPark => nationalPark.Id == id);
        }

        public bool Save()
        {
            return db.SaveChanges() >= 0 ? true : false;                
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            db.NationalParks.Update(nationalPark);
            return Save();
        }
    }
}
