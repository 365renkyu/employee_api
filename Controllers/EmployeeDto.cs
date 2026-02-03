//DTO

//POST用
public class CreateEmployeeDto
{
    public string ShainNo { get; set; } = ""; //社員番号
    public string Name { get; set; } = ""; //社員名
    public string Busho { get; set; } = ""; //部署
}

//PUT用
public class UpdateEmployeeDto
{
    public string Name { get; set; } = ""; //社員名
    public string Busho { get; set; } = ""; //部署
}
