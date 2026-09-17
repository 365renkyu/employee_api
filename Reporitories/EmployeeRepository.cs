using EmployeeModel;

namespace EmployeeRepositories
{
    public class EmployeeRepository
    {
        /// 社員リスト
        /// 便宜上、DBの代用とする
        /// </summary>
        public static List<Employee> EmployeeList = new()
        {
            new Employee { ShainNo = "10001", Name = "山田太郎", Busho = "人事部", Age = 28, Hobby = "写真", IsDeleted = false },
            //new Employee { ShainNo = "10001", Name = "山田太郎（例外動作確認用）", Busho = "人事部", Age = 28, Hobby = "写真", IsDeleted = false },
            new Employee { ShainNo = "10002", Name = "田中次郎", Busho = "開発部", Age = 34, IsDeleted = false },
            new Employee { ShainNo = "10003", Name = "山本花子", Busho = "営業部", Age = 30, Hobby = "散歩", IsDeleted = true }
        };

        /// <summary>
        /// 社員情報取得処理
        /// </summary>
        /// <param name="shainNo"></param>
        /// <returns></returns>
        public List<Employee> GetAll()
        {
            return EmployeeList;
        }

        /// <summary>
        /// 社員情報取得処理
        /// </summary>
        /// <param name="shainNo"></param>
        /// <returns></returns>
        public Employee? GetByShainNo(string shainNo)
        {
            var empListByShainNo = GetAll().SingleOrDefault(e => e.ShainNo == shainNo && !e.IsDeleted);

            return empListByShainNo; //複数件数取得時は例外へ
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <returns></returns>
        public void Register(string shainNo, CreateEmployeeDto body)
        {
            //登録
            var employee = new EmployeeModel.Employee
            {
                ShainNo = shainNo,
                Name = body.Name,
                Busho = body.Busho,
                Age = body.Age,
                Hobby = body.Hobby,
                IsDeleted = body.IsDeleted
            };

            GetAll().Add(employee);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <returns></returns>
        public void UpdateEmployee(Employee employee, UpdateEmployeeDto body)
        {
            //更新
            employee.Name = body.Name;
            employee.Busho = body.Busho;
            employee.Age = body.Age;
            employee.Hobby = body.Hobby;
            employee.IsDeleted = body.IsDeleted;
        }

        /// <summary>
        /// 削除処理（論理削除）
        /// </summary>
        /// <returns></returns>
        public void LogicalDelete(Employee employee)
        {
            //削除（論理削除）
            employee.IsDeleted = true;
        }

        /// <summary>
        /// 削除処理（物理削除）
        /// </summary>
        /// <returns></returns>
        public void PhysicalDelete(Employee employee)
        {
            EmployeeList.Remove(employee);
        }
    }


}