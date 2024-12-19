using GenericAPI.Entities;
using GenericAPI.IRepository;
using GenericAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GenericAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeControllerUoW : ControllerBase
    {
        private readonly IUnitofWork _unitofWork;

        public EmployeeControllerUoW(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
           var result=  await _unitofWork.GetRepository<Employee>().GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(EmployeeVM employeeVM)
        {
            try
            {
                using var transaction = _unitofWork.BeginTransactionAsync();
                //Should use Automapper for entity Mapping
                var empEntity = new Employee
                {
                    Name = employeeVM.Name,
                    Email = employeeVM.Email,
                    MobileNo = employeeVM.MobileNo,
                    Password = employeeVM.Password,
                    DOB = employeeVM.DOB
                };
               var result=  await _unitofWork.GetRepository<Employee>().AddAsync(empEntity);
                await _unitofWork.SaveChangesAsync();

               // Other Transactions to insert into other entities can be called here to consume inside  UOW.

                await _unitofWork.CommitAsync();
                return StatusCode((int)HttpStatusCode.Created,new { Id = result.Id});
            }
            catch (Exception)
            {
                await _unitofWork.RollBackAsync();
                throw;
            }
        }
    }
}
