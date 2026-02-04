using EmployeeModel;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using EmployeeServices;

[ApiController]
[Route("api/[controller]")]
public class GetEmployeeController : ControllerBase
{
    ////< summary>
    ///全ての社員情報を取得
    ///</summary>
    [HttpGet]
    public ActionResult<List<Employee>> GetAll()
    {
        return EmployeeServices.Employees.EmployeeList;
    }

    ///< summary>
    ///社員番号に紐づく社員情報を取得
    ///</summary>
    [HttpGet("{shainNo}")]
    public ActionResult<Employee> GetById(string shainNo)
    {
        var service = new EmployeeService();
        var empListByShainNo = service.GetByShainNo(shainNo);

        if (empListByShainNo is null)
        {
            return NotFound();
        }

        return empListByShainNo;
    }
}

///< summary>
///POST
///</summary>
[ApiController]
[Route("employees")]
public class PostEmployeeController : ControllerBase
{
    ///< summary>
    ///新規登録
    ///</summary>
    [HttpPost]
    public IActionResult Create([FromBody] CreateEmployeeDto body)
    {
        /*
        //リクエストボディ　存在チェック→不要？
        if (body is null)
        {
            return BadRequest("Requiest Bodyが存在しません。");
        }
        */

        /*
                //バリデーション処理→不要？
                if (!ModelState.IsValid)
                {
                    return BadRequest("登録内容に不備があります。　詳細：" + ModelState);
                }
                */

        var service = new EmployeeService();
        string tmpShainNo = service.AssignShainNo();

        //採番処理（falseの場合、社員番号重複エラー）
        if (String.IsNullOrEmpty(tmpShainNo))
        {
            return Conflict("既に存在する社員番号です。");
        }

        //登録
        if (service.Register(tmpShainNo, body))
        {
            return CreatedAtAction(nameof(Create), body); //201 Created
        }
        else
        {
            return StatusCode(500, "登録に失敗しました");
        }
    }
}

///< summary>
///PUT
///</summary>
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
        //Console.WriteLine($"PUT URL shainNo = {shainNo}");
        //Console.WriteLine($"PUT BODY shainNo = {updShainNo?.ShainNo}");

        /*
                //リクエストボディ　存在チェック→不要？
                if (body is null)
                {
                    return BadRequest("Requiest Bodyが存在しません。");
                }
                */

        /*
        //パラメータ.社員番号未入力
        if (String.IsNullOrEmpty(shainNo))
        {
            return BadRequest("パラメータが設定されていません。");
        }

                //バリデーション処理
                if (!ModelState.IsValid)
                {
                    return BadRequest("登録内容に不備があります。　詳細：" + ModelState);
                }
                */

        var service = new EmployeeService();

        //更新
        if (!service.UpdateEmployee(shainNo, body))
        {
            return NotFound("社員が存在しません。");
        }
        else
        {
            service.UpdateEmployee(shainNo, body);
            return Ok();
        }

    }
}
