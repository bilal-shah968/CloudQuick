using Microsoft.EntityFrameworkCore;

namespace CloudQuick.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CloudDbContext _dbContext;

        public StudentRepository(CloudDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateAsync(Student student)
        {
            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> DeleteAsync(Student student)
        {
            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _dbContext.Students.ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id, bool usenotracking = false)
        {
            if (usenotracking)
                return await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
            else
                return await _dbContext.Students.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<Student> GetByNameAsync(string name)
        {
           
            return await _dbContext.Students.FirstOrDefaultAsync(n => n.StudentName != null && n.StudentName.ToLower().Contains(name.ToLower()));
        }

        public async Task<int> UpdateAsync(Student student)
        {
            //var existingStudent = await _dbContext.Students.FirstOrDefaultAsync(s => s.Id == student.Id);
            //if (existingStudent == null)
                //throw new ArgumentNullException($"No student found with id: {student.Id}");

            //// Update fields explicitly....
            //existingStudent.StudentName = student.StudentName;
            //existingStudent.Email = student.Email;
            //existingStudent.Address = student.Address;
            //existingStudent.DOB = student.DOB;

            await _dbContext.SaveChangesAsync();
            return student.Id;
        }
    }
}
