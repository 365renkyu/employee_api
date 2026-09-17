using EmployeeModel;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRepositories

{
    public class EmployeeRepository_DbSample
    {
        private readonly EmployeeDbContext _context;

        public EmployeeRepository_DbSample(EmployeeDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAll()
        {
            return await _context.Employees.ToListAsync();
        }

        /// <summary>
        /// 社員情報取得処理
        /// </summary>
        /// <param name="shainNo"></param>
        /// <returns></returns>
        public async Task<Employee?> GetByShainNo(string shainNo)
        {
            return await _context.Employees.SingleOrDefaultAsync(e =>
                                e.ShainNo == shainNo &&
                                !e.IsDeleted);
        }

        /// <summary>
        /// 登録
        /// </summary>
        /// <returns></returns>
        public async Task Register(string shainNo, CreateEmployeeDto body)
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

            _context.Employees.Add(employee);
            //await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <returns></returns>
        public async Task UpdateEmployee(Employee employee, UpdateEmployeeDto body)
        {
            //更新
            employee.Name = body.Name;
            employee.Busho = body.Busho;
            employee.Age = body.Age;
            employee.Hobby = body.Hobby;
            employee.IsDeleted = body.IsDeleted;

            //await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 削除処理（論理削除）
        /// </summary>
        /// <returns></returns>
        public async Task LogicalDelete(Employee employee)
        {
            //削除（論理削除）
            employee.IsDeleted = true;
            //await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 削除処理（物理削除）
        /// </summary>
        /// <returns></returns>
        public async Task PhysicalDelete(Employee employee)
        {
            _context.Employees.Remove(employee);
            //await _context.SaveChangesAsync();
        }
    }


}