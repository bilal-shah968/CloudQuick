namespace CloudQuick.Data.Repository
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync();
        Task<Student> GetByIdAsync(int id, bool usenotracking = false);
        Task<Student> GetByNameAsync(string name); // Changed "Name" to "name"
        Task<int> CreateAsync(Student student);
        Task<int> UpdateAsync(Student student);
        Task<bool> DeleteAsync(Student student);
    }
}
