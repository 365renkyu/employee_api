
//GET
using Microsoft.AspNetCore.Mvc;
using EmployeeModel;
using System.ComponentModel;

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

//POST
[ApiController]
[Route("employees")]
public class PostEmployeeController : ControllerBase
{
    [HttpPost]
    public IActionResult Create([FromBody] CreateEmployeeDto dto)
    {
        //リクエストボディ　存在チェック
        if (dto is null) {
            return BadRequest("Requiest Bodyが存在しません。");
        }

        //採番（あとでDB対応）
        //自動採番（社員番号最大値＋１）
        //既存の社員がいるにもかかわらず0+1=1になる場合、後続の社員番号重複エラーでハンドリング
        dto.ShainNo = (int.Parse(EmployeeServices.Employees.EmployeeList.Max(e => e.ShainNo)) + 1).ToString();

        var exists = EmployeeServices.Employees.EmployeeList
            .Any(e => e.ShainNo == dto.ShainNo);

        //社員番号重複エラー
        if (exists)
        {
            return Conflict("既に存在する社員番号です。");
        }
        //登録
        var employee = new Employee
        {
            ShainNo = dto.ShainNo,  
            Name = dto.Name,
            Busho = dto.Busho
        };

        EmployeeServices.Employees.EmployeeList.Add(employee);

        // ⑤ 201 Created
        return CreatedAtAction(
            nameof(Create),
            new { shainNo = employee.ShainNo },
            employee
        );
    }
}

//PUT
[ApiController]
[Route("api/[controller]")]
public class PutEmployeeController : ControllerBase
{
    //社員番号
    [HttpPut("{shainNo}")]
    public IActionResult Update(string shainNo, [FromBody] UpdateEmployeeDto dto)
    {
        Console.WriteLine($"PUT URL shainNo = {shainNo}");
        //Console.WriteLine($"PUT BODY shainNo = {updShainNo?.ShainNo}");

        //リクエストボディ　存在チェック
        if (dto is null) {
            return BadRequest("Requiest Bodyが存在しません。");
        }

        //パラメータの社員番号未入力
        if (String.IsNullOrEmpty(shainNo))
        {
            return BadRequest("パラメータが設定されていません。");
        }

        //更新先データ存在チェック
        var employeeListByShainNo = EmployeeServices.Employees.EmployeeList
            .FirstOrDefault(e => e.ShainNo == shainNo);
        if (employeeListByShainNo is null)
        {
            return NotFound("社員が存在しません。");
        }

        //更新
        employeeListByShainNo.Name = dto.Name;
        employeeListByShainNo.Busho = dto.Busho;

        return Ok(employeeListByShainNo); //200（意図：調査用）
    }

}
