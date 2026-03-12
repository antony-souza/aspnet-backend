using Backend.Base;
using Backend.modules.user;
using Backend.src.modules.organization.dtos;
using Backend.src.modules.user.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.src.modules.organization
{
    [Controller]
    [Route("organization")]
    [Authorize]
    public class OrganizationController : ControllerBase
    {
        private readonly OrganizationService _organizationService;

        public OrganizationController(OrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrganizationDto createOrganizationDto)
        {
            var result = await _organizationService.Create(createOrganizationDto);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> FindAll([FromQuery] BaseQueryControllerPagination query)
        {
            int page = query.Page;
            int perPage = query.PerPage;

            var data = await _organizationService.FindAll(page, perPage);

            return Ok(data);
        }
    }
}
