using Microsoft.AspNetCore.Mvc;
using EmployeeServices;

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
    public IActionResult GetAll()
    {
        return Ok(_service.GetAll());
    }

    [HttpGet("{shainNo}")]
    public IActionResult GetByShainNo(string shainNo)
    {
        var empListByShainNo = _service.GetByShainNo(shainNo);

        if (empListByShainNo is null)
        {
            return NotFound(); //404
        }
        return Ok(empListByShainNo);
    }

    [HttpPost]
    public IActionResult CreateController([FromBody] CreateEmployeeDto body)
    {
        string tmpShainNo = _service.AssignShainNo();

        //400 BadRequestは入れるべき？

        //採番処理（falseの場合、社員番号重複エラー）
        if (String.IsNullOrEmpty(tmpShainNo))
        {
            return Conflict("既に存在する社員番号です。"); //E001:既に存在する社員番号です。
                                              //return Conflict(message.MessageTxt); //E001:既に存在する社員番号です。
        }

        //登録
        try
        {
            _service.Register(tmpShainNo, body);
            return CreatedAtAction(nameof(CreateController), body); //201 Created
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "サーバーエラーが発生しました。", errorCode = "INTERNAL_SERVER_ERROR" });
        }

    }

    [HttpPut("{shainNo}")]
    public IActionResult Update(string shainNo, [FromBody] UpdateEmployeeDto body)
    {
        try
        {
            if (_service.UpdateEmployee(shainNo, body))
            {
                return Ok();
            }
            else
            {
                return NotFound("社員が存在しません。");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "サーバーエラーが発生しました。", errorCode = "INTERNAL_SERVER_ERROR" });
        }
    }

    [HttpDelete("{shainNo}")]
    public IActionResult Delete(string shainNo)
    {
        try
        {
            if (_service.DeleteEmployee(shainNo))
            {
                return NoContent(); //204
            }
            else
            {
                return NotFound("社員が存在しません。");
            }

        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "サーバーエラーが発生しました。", errorCode = "INTERNAL_SERVER_ERROR" });
        }
    }
}

