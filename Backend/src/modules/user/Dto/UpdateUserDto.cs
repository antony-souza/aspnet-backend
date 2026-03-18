namespace Backend.src.modules.user.Dto
{
    public class UpdateUserDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Cpf { get; set; }
        public Guid? OrganizationId { get; set; }
    }
}
