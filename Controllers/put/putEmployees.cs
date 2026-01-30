//社員情報登録API（新規登録・更新）
using Microsoft.AspNetCore.Mvc;
using EmployeeModel;

public class PutEmployeeController : ControllerBase
{
    //社員番号
    [HttpPut("{shainNo}")]
    public IActionResult Update(string shainNo, Employee updShainNo)
    {

        var employeeListByShainNo = EmployeeServices.Employees.EmployeeList
            .FirstOrDefault(e => e.ShainNo == updShainNo.ToString());

        if (employeeListByShainNo is null)  //未取得
        {
            return NotFound();
        }

        // 更新
        employeeListByShainNo.Name = updShainNo.Name;
        employeeListByShainNo.Busho = updShainNo.Busho;

        return Ok(employeeListByShainNo); // 200（意図：調査用）
    }

/*
    //追加予定
    ①新規追加

    ②エラーハンドリング
    ・名称が空（名前・部署）
    ・URLのIDとBodyのIDの不一致（更新対象ミス防止）
*/

}
