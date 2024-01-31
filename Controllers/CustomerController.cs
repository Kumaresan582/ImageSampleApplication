using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Repo.Interface;
using Repo.MapperModel;

namespace SampleApplication.Controllers
{
    [EnableRateLimiting("fixedwindow")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _customerService.getall();
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("GetbyId")]
        public async Task<IActionResult> Getbycode(int code)
        {
            var data = await _customerService.getById(code);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CustomerMapper _data)
        {
            var data = await _customerService.Create(_data);
            return Ok(data);
        }

        [AllowAnonymous]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(CustomerMapper _data, int code)
        {
            var data = await _customerService.Update(_data, code);
            return Ok(data);
        }

        [AllowAnonymous]
        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(int code)
        {
            var data = await _customerService.Remove(code);
            return Ok(data);
        }
    }
}