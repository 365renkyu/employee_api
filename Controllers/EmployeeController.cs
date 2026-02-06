using EmployeeModel;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using EmployeeServices;
using MessageModel;

[ApiController]
[Route("api/[controller]")]
public class GetEmployeeController : ControllerBase
{
    ////< summary>
    ///全ての社員情報を取得
    ///</summary>
    [HttpGet]
    public ActionResult<Employee> GetControllerByAll()
    {
        var service = new EmployeeService();
        return Ok(service.GetAll());
    }

    ///< summary>
    ///社員番号に紐づく社員情報を取得
    ///</summary>
    [HttpGet("{shainNo}")]
    public ActionResult<Employee> GetControllerByShainNo(string shainNo)
    {
        var service = new EmployeeService();
        var empListByShainNo = service.GetByShainNo(shainNo);

        if (empListByShainNo is null)
        {
            return NotFound(); //404
        }
        return Ok(empListByShainNo);
    }
}

[ApiController]
[Route("employees")]
public class PostEmployeeController : ControllerBase
{
    ///< summary>
    ///新規登録
    ///</summary>
    [HttpPost]
    public IActionResult CreateController([FromBody] CreateEmployeeDto body)
    {
        var service = new EmployeeService();
        var message = new Message();

        string tmpShainNo = service.AssignShainNo();

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
            service.Register(tmpShainNo, body);
            return CreatedAtAction(nameof(CreateController), body); //201 Created
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "サーバーエラーが発生しました。", errorCode = "INTERNAL_SERVER_ERROR" });
        }
    }
}

[ApiController]
[Route("api/[controller]")]
public class PutEmployeeController : ControllerBase
{
    ///< summary>
    ///社員番号に紐づく社員情報の全項目を更新
    ///</summary>
    [HttpPut("{shainNo}")]
    public IActionResult Update(string shainNo, [FromBody] UpdateEmployeeDto body)
    {
        var service = new EmployeeService();

        //更新
        try
        {
            if (service.UpdateEmployee(shainNo, body))
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
}

[ApiController]
[Route("api/[controller]")]
public class DeleteEmployeeController : ControllerBase
{
    ///< summary>
    ///社員番号に紐づく社員情報を削除
    ///</summary>
    [HttpPut("{shainNo}")]
    public IActionResult Delete(string shainNo)
    {
        var service = new EmployeeService();

        //削除
        try
        {
            if (service.DeleteEmployee(shainNo))
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
