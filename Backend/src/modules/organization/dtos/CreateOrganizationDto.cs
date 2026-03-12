namespace Backend.src.modules.organization.dtos
{
    public class CreateOrganizationDto
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
    }
}
