using Microsoft.AspNetCore.Mvc;
using EmployeeServices;
using EmployeeModel;

[ApiController]
[Route("api/[controller]")]

public class EmployeeController : ControllerBase
{
    private readonly EmployeeService _service;

    public EmployeeController(EmployeeService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Employee>> GetAll()
    {
        return Ok(_service.GetAll());
    }

    [HttpGet("{shainNo}")]
    public ActionResult<Employee> GetByShainNo(string shainNo)
    {
        var empListByShainNo = _service.GetByShainNo(shainNo);

        if (empListByShainNo is null)
        {
            return NotFound(); //404
        }
        return Ok(empListByShainNo);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateEmployeeDto body)
    {
        var result = _service.AssignShainNo();

        //400 BadRequestは入れるべき？→[ApiController]がついているので、DTOで付与した属性に応じてバリデーションエラーを自動で出してくれる

        //リファクタリング対象：採番処理（falseの場合、社員番号重複エラー）
        if (result.Result is EmployeeResult.Conflict)
        {
            return Conflict("既に存在する社員番号です。");
        }

        var register = _service.Register(result.ShainNo, body);

        return CreatedAtAction(
            nameof(GetByShainNo),
            new { shainNo = result.ShainNo },
            body);
    }

    [HttpPut("{shainNo}")]
    public IActionResult Update(string shainNo, [FromBody] UpdateEmployeeDto body)
    {
        var result = _service.UpdateEmployee(shainNo, body);

        return result switch
        {
            EmployeeResult.Success => Ok(),
            EmployeeResult.NotFound => NotFound("社員が存在しません。")
        };
    }

    [HttpDelete("{shainNo}")]
    public IActionResult Delete(string shainNo)
    {
        var result = _service.DeleteEmployee(shainNo);

        return result switch
        {
            EmployeeResult.Success => Ok(),
            EmployeeResult.NotFound => NoContent()
        };
    }
}

