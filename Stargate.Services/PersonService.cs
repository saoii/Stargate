using Stargate.Services.Data;
using Stargate.Services.Models;
using Stargate.Services.Repos;

namespace Stargate.Services
{
    public interface IPersonService
    {
        public Task<Person> CreateAsync(Person person);
        public Task<Person> ReadAsync(string name);
    }

    public class PersonService : IPersonService
    {
        private readonly StargateContext? _context;
        private IRepository<Person>? _personRepo;
        public IRepository<Person> PersonRepo => _personRepo ??= new PersonRepo(_context);

        public PersonService(StargateContext context)
        {
            _context = context;
        }

        public async Task<Person> CreateAsync(Person person)
        {
            //check for duplicates
            var dupe = await PersonRepo.FindAsync(p => p.ToString() == person.ToString());

            if (dupe == null)
                return await PersonRepo.AddAsync(person);
            else
            {
                return null;
            }
        }

        public async Task<Person> ReadAsync(string name)
        {
            //var result = await PersonRepo.FindAsync(p => p.LastName.Contains(name) || p.FirstName.Contains(name));

            var result = await PersonRepo.FindAsync(p => p.LastName == name);

            return result.FirstOrDefault();
        }
    }
}
