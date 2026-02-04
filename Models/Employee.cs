namespace EmployeeModel
{
    public class Employee
    {
        public string ShainNo { get; set; } = "";  //社員番号
        public string Name { get; set; } = "";  //社員名
        public string Busho { get; set; } = ""; //部署
        public int Age { get; set; } //年齢
        public string? Hobby { get; set; } = ""; //趣味
        public bool IsDeleted { get; set; } = false; //論理削除フラグ
    };
}