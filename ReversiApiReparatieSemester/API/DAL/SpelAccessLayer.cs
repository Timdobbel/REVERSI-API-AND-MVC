using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Model;

namespace API.DAL
{
    public class SpelAccessLayer : ISpelRepository
    {
        private SpelContext _context;

        public SpelAccessLayer(SpelContext context) { _context = context; }

        public void AddSpel(Spel spel)
        {
            _context.Spellen.Add(spel);
            _context.SaveChanges();
        }

        public List<Spel> GetSpellen()
        {
            return _context.Spellen.ToList();
        }

        public Spel GetSpel(string spelToken)
        {
            return _context.Spellen.First(spel => spel.Token == spelToken);
        }

        public void Delete(Spel spel)
        {
            _context.Spellen.Remove(spel);
            _context.SaveChanges();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
