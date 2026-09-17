using EmployeeModel;
using EmployeeRepositories;

namespace EmployeeServices
{
    public class EmployeeService
    {
        //ファクタリング対象：業務ロジック強化
        private readonly EmployeeRepository _repository;

        public EmployeeService(EmployeeRepository repositories)
        {
            _repository = repositories;
        }

        public List<Employee> GetAll()
        {
            return _repository.GetAll();
        }

        public Employee? GetByShainNo(string shainNo)
        {
            return _repository.GetByShainNo(shainNo);
        }


        /// <summary>
        /// 社員番号採番処理（あとでDB対応）
        /// 自動採番（社員番号最大値＋１）
        /// 10001番からスタート
        /// 既存の社員がいるにもかかわらず0+1=1になる場合、後続の社員番号重複エラーでハンドリング
        /// </summary>
        public AssignShainNoResult AssignShainNo()
        {
            var empList = _repository.GetAll();
            string tmpShainNo = "";

            if (empList is null || !empList.Any())
            {
                tmpShainNo = "10001"; // 初回登録の場合
            }
            else
            {
                tmpShainNo = (int.Parse(empList.Max(e => e.ShainNo)) + 1).ToString();
                var exists = empList.Any(e => e.ShainNo == tmpShainNo);

                //社員番号重複エラー
                if (exists)
                {
                    return new AssignShainNoResult
                    {
                        Result = EmployeeResult.Conflict,
                        ShainNo = tmpShainNo
                    };
                }
            }

            return new AssignShainNoResult
            {
                Result = EmployeeResult.Success,
                ShainNo = tmpShainNo
            };
        }

        public EmployeeResult Register(string shainNo, CreateEmployeeDto body)
        {
            //登録データ存在チェック
            var employee = _repository.GetByShainNo(shainNo);

            if (employee is not null)
            {
                return EmployeeResult.Conflict;
            }

            _repository.Register(shainNo, body);
            return EmployeeResult.Success;
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <returns></returns>
        public EmployeeResult UpdateEmployee(string shainNo, UpdateEmployeeDto body)
        {
            //更新対象データ存在チェック
            var employee = _repository.GetByShainNo(shainNo);
            if (employee is null)
            {
                return EmployeeResult.NotFound;
            }

            //更新
            _repository.UpdateEmployee(employee, body);
            return EmployeeResult.Success;
        }

        /// <summary>
        /// 削除処理（論理削除）
        /// </summary>
        /// <returns></returns>
        public EmployeeResult LogicalDelete(string shainNo)
        {
            //削除対象データ存在チェック
            var employee = _repository.GetByShainNo(shainNo);
            if (employee is null)
            {
                return EmployeeResult.NotFound;
            }

            //削除（論理削除）
            _repository.LogicalDelete(employee);
            return EmployeeResult.Success;
        }

        /// <summary>
        /// 削除処理（物理削除）
        /// </summary>
        /// <returns></returns>
        public EmployeeResult PhysicalDelete(string shainNo)
        {
            //削除対象データ存在チェック
            var employee = _repository.GetByShainNo(shainNo);
            if (employee is null)
            {
                return EmployeeResult.NotFound;
            }

            //削除（論理削除）
            _repository.PhysicalDelete(employee);
            return EmployeeResult.Success;
        }
    };
};
