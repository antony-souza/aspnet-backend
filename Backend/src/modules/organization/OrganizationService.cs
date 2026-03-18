using Backend.Common.database;
using Backend.Interfaces;
using Backend.modules.user.Dto;
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

        public async Task<Organization> Update(Guid id, UpdateOrganizationDto updateDto)
        {
            var organization = await _context.Organizations.FirstOrDefaultAsync(org => org.Id == id);

            if (organization == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(updateDto.Name))
                organization.Name = updateDto.Name;

            if (!string.IsNullOrEmpty(updateDto.Slug))
            {
                var existingSlug = await _context.Organizations.AnyAsync(org => org.Slug == updateDto.Slug && org.Id != id);

                if (existingSlug)
                {
                    return null;
                }
                organization.Slug = updateDto.Slug;
            }

            if (!string.IsNullOrEmpty(updateDto.Cnpj))
                organization.Cnpj = updateDto.Cnpj;

            organization.UpdatedAt = DateTime.UtcNow;

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
                    Cnpj = organization.Cnpj,
                    Users = organization.Users!.Select(u => new UserListDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email!
                    }).ToList()
                })
                .ToListAsync();

            return new BaseReturnPagination<ListOrganizationDto>(page, perPage, total, items);
        }

        public async Task<bool> SoftDelete(Guid id)
        {
            var rows = await _context.Organizations
                .Where(org => org.Id == id)
                .ExecuteUpdateAsync(set =>
                    set.SetProperty(org => org.DeletedAt, DateTime.UtcNow)
                );

            return rows > 0;
        }
    }
}
