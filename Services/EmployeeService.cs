using EmployeeModel;

namespace EmployeeServices
{
    /// <summary>
    /// Employeeクラス（リスト型）
    /// 便宜上、DBの代用とする
    /// </summary>
    public static class Employees
    {
        public static List<Employee> EmployeeList = new()
        {
            new Employee { ShainNo = "10001", Name = "山田太郎", Busho = "人事部", Age = 28, Hobby = "写真", IsDeleted = false },
            new Employee { ShainNo = "10002", Name = "田中次郎", Busho = "開発部", Age = 34, IsDeleted = false },
            new Employee { ShainNo = "10003", Name = "山本花子", Busho = "営業部", Age = 30, Hobby = "散歩", IsDeleted = true }
        };
    };

    public class EmployeeService
    {

        /// <summary>
        /// 社員情報取得処理
        /// </summary>
        /// <param name="shainNo"></param>
        /// <returns></returns>
        public List<Employee> GetAll()
        {
            return Employees.EmployeeList;
        }

        /// <summary>
        /// 社員情報取得処理
        /// </summary>
        /// <param name="shainNo"></param>
        /// <returns></returns>
        public Employee GetByShainNo(string shainNo)
        {
            var empListByShainNo = EmployeeServices.Employees.EmployeeList
                                  .FirstOrDefault(e => e.ShainNo == shainNo && !e.IsDeleted);

            return empListByShainNo;
        }


        /// <summary>
        /// 社員番号採番処理（あとでDB対応）
        /// 自動採番（社員番号最大値＋１）
        /// 10001番からスタート
        /// 既存の社員がいるにもかかわらず0+1=1になる場合、後続の社員番号重複エラーでハンドリング
        /// </summary>
        public string AssignShainNo()
        {
            var empList = Employees.EmployeeList;
            string tmpShainNo = "";

            if (empList is null || !empList.Any())
            {
                tmpShainNo = "10001"; // 初回登録の場合
            }
            else
            {
                tmpShainNo = (int.Parse(empList.Max(e => e.ShainNo)) + 1).ToString();
            }

            var exists = empList.Any(e => e.ShainNo == tmpShainNo);

            //社員番号重複エラー
            if (exists)
            {
                return "";
            }

            return tmpShainNo;
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <returns></returns>
        public bool Register(string shainNo, CreateEmployeeDto body)
        {
            var empList = Employees.EmployeeList;

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

            empList.Add(employee);
            return true;
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <returns></returns>
        public bool UpdateEmployee(string shainNo, UpdateEmployeeDto body)
        {
            var empList = Employees.EmployeeList;

            //更新対象データ存在チェック
            var empListByShainNo = empList.FirstOrDefault(e => e.ShainNo == shainNo && !e.IsDeleted);
            if (empListByShainNo is null)
            {
                return false;  //★あとでエラーコード返すように変更したい「社員が存在しません」
            }

            //更新
            empListByShainNo.Name = body.Name;
            empListByShainNo.Busho = body.Busho;
            empListByShainNo.Age = body.Age;
            empListByShainNo.Hobby = body.Hobby;
            empListByShainNo.IsDeleted = body.IsDeleted;

            return true;
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns></returns>
        public bool DeleteEmployee(string shainNo)
        {
            var empList = Employees.EmployeeList;

            //削除対象データ存在チェック
            var empListByShainNo = empList.FirstOrDefault(e => e.ShainNo == shainNo && !e.IsDeleted);
            if (empListByShainNo is null)
            {
                return false;  //★あとでエラーコード返すように変更したい「社員が存在しません」
            }

            //削除（論理削除）
            //★トランザクション追記する
            empListByShainNo.IsDeleted = true;

            return true;
        }
    };
};
