using Backend.modules.user.Dto;

namespace Backend.src.modules.organization.dtos
{
    public class ListOrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public ICollection<UserListDto>? Users { get; set; }
    }
}
