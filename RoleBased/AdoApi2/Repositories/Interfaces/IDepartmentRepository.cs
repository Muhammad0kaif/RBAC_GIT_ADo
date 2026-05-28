using PocoClasses;
using PocoClasses.Dto;

namespace AdoApi2.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<List<DepartmentDto>> GetDepartments();

        Task<List<User>> GetUsersByDepartment(Guid departmentId);
    }
}