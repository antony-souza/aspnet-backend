using Backend.Common.database;
using Backend.Interfaces;
using Backend.src.modules.organization.dtos;
using Backend.src.modules.organization.entitiy;
using Microsoft.EntityFrameworkCore;

namespace Backend.src.modules.organization
{
    public class OrganizationService
    {
        private readonly AppDbContext _context;

        public OrganizationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Organization> Create(CreateOrganizationDto createOrganizationDto)
        {
            var exist = await _context.Organizations.AnyAsync(org => org.Slug == createOrganizationDto.Slug);

            if (exist)
            {
                return null;
            }

            var organization = new Organization
            {
                Name = createOrganizationDto.Name,
                Slug = createOrganizationDto.Slug,
                Cnpj = createOrganizationDto.Cnpj,
            };

            await _context.Organizations.AddAsync(organization);

            await _context.SaveChangesAsync();

            return organization;
        }

        public async Task<IBaseReturnPagination<ListOrganizationDto>> FindAll(int page, int perPage)
        {
            var skip = (page - 1) * perPage;

            var total = await _context.Organizations.CountAsync();

            var items = await _context.Organizations
                .AsNoTracking()
                .Skip(skip)
                .Take(perPage)
                .Select(organization => new ListOrganizationDto
                {
                    Id = organization.Id,
                    Name = organization.Name,
                    Cnpj = organization.Cnpj
                })
                .ToListAsync();

            return new BaseReturnPagination<ListOrganizationDto>(page, perPage, total, items);
        }

        public async Task<bool> SoftDelete(Guid userId)
        {
            var rows = await _context.Users
                .Where(user => user.Id == userId)
                .ExecuteUpdateAsync(set =>
                    set.SetProperty(user => user.DeletedAt, DateTime.UtcNow)
                );

            return rows > 0;
        }
    }
}
