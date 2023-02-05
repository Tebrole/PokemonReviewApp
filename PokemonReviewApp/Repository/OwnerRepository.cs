using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners.Where(x=>x.Id==ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfPokemon(int id)
        {
            return _context.PokemonOwners.Where(x=>x.Pokemon.Id==id).Select(x=>x.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(x => x.Owner.Id == ownerId).Select(x => x.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _context.Owners.Any(x=>x.Id== ownerId);
        }
    }
}
