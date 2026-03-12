using Backend.Common.database;

namespace Backend.src.modules.organization
{
    public class OrganizationService
    {
        private readonly AppDbContext _context;

        public OrganizationService(AppDbContext context)
        {
            _context = context;
        }
    }
}
