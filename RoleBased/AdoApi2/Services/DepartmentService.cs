using AdoApi2.Repositories.Interfaces;
using PocoClasses;
using PocoClasses.Dto;

namespace AdoApi2.Services
{
    public class DepartmentService(IDepartmentRepository repo)
    {
        private readonly IDepartmentRepository _repo = repo;

        public Task<List<DepartmentDto>> GetDepartments()
        {
            return _repo.GetDepartments();
        }

        public Task<List<User>> GetUsersByDepartment(Guid departmentId)
        {
            return _repo.GetUsersByDepartment(departmentId);
        }
    }
}