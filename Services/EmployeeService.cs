using EmployeeModel;

namespace EmployeeServices
{

    public static class Employees
    {
        public static List<Employee> EmployeeList = new()
    {

        new Employee { ShainNo = "10001", Name = "山田太郎", Busho = "人事部" },
        new Employee { ShainNo = "10002", Name = "田中次郎", Busho = "開発部" }
    };
    };

};
