
//社員情報取得API
using Microsoft.AspNetCore.Mvc;
using EmployeeModel;

[ApiController]
[Route("api/[controller]")]
public class GetEmployeeController : ControllerBase
{
    //全情報取得
    [HttpGet]
    public ActionResult<List<Employee>> GetAll()
    {
        return EmployeeServices.Employees.EmployeeList;
    }

    //社員番号
    [HttpGet("{shainNo}")]
    public ActionResult<Employee> GetById(string shainNo)
    {
        var employeeListByShainNo = EmployeeServices.Employees.EmployeeList
            .FirstOrDefault(e => e.ShainNo == shainNo);

        if (employeeListByShainNo is null)  //未取得
        {
            return NotFound();
        }

        return employeeListByShainNo;
    }
}
