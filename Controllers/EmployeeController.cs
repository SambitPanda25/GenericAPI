using GenericAPI.Entities;
using GenericAPI.IRepository;
using GenericAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenericAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IGenericRepository<Employee> _empRepository;

        public EmployeeController(IGenericRepository<Employee> empRepository)
        {
            _empRepository = empRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var emp = await _empRepository.GetAllAsync();
            return Ok(emp);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var emp = await _empRepository.GetByIDAsync(Id);
            return Ok(emp);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmployeeVM employeeVM)
        {
            var empEntity = new Employee()
            {
                Name = employeeVM.Name,
                Email = employeeVM.Email,
                MobileNo = employeeVM.MobileNo,
                DOB = employeeVM.DOB,
                Password = employeeVM.Password
            };
            var createdEmpRepsonse = await _empRepository.AddAsync(empEntity);
            return CreatedAtAction(nameof(GetById), new { id = createdEmpRepsonse.Id }, createdEmpRepsonse);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(int Id, [FromBody] EmployeeVM employeeVM)
        {
            var empEntity = await _empRepository.GetByIDAsync(Id);
            if (empEntity == null)
            {
                return NotFound();
            }
            empEntity.Name = employeeVM.Name;
            empEntity.Email = employeeVM.Email;
            empEntity.MobileNo = employeeVM.MobileNo;
            empEntity.DOB = employeeVM.DOB;
            empEntity.Password = employeeVM.Password;

            await _empRepository.UpdateAsync(empEntity);
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var empEntity = await _empRepository.GetByIDAsync(Id);
            if (empEntity == null)
            {
                return NotFound();
            }
            await _empRepository.DeleteAsync(empEntity);
            return NoContent();
        }
    }
}
