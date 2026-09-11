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
    public ActionResult<IEnumerable<Employee>> GetByShainNo(string shainNo)
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
        string tmpShainNo = _service.AssignShainNo();

        //400 BadRequestは入れるべき？→[ApiController]がついているので、DTOで付与した属性に応じてバリデーションエラーを自動で出してくれる

        //リファクタリング対象：採番処理（falseの場合、社員番号重複エラー）
        if (String.IsNullOrEmpty(tmpShainNo))
        {
            return Conflict("既に存在する社員番号です。"); //E001:既に存在する社員番号です。
        }

        _service.Register(tmpShainNo, body);
        return CreatedAtAction(
    nameof(GetByShainNo),
    new { shainNo = tmpShainNo },
    body);
    }

    [HttpPut("{shainNo}")]
    public IActionResult Update(string shainNo, [FromBody] UpdateEmployeeDto body)
    {
        if (_service.UpdateEmployee(shainNo, body))
        {
            return Ok();
        }
        else
        {
            return NotFound("社員が存在しません。");  //リファクタリング対象
        }
    }

    [HttpDelete("{shainNo}")]
    public IActionResult Delete(string shainNo)
    {
        if (_service.DeleteEmployee(shainNo))
        {
            return NoContent(); //204
        }
        else
        {
            return NotFound("社員が存在しません。");  //リファクタリング対象
        }
    }
}

