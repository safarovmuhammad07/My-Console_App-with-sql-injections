using Infrastructure.Models;

namespace Infrastructure.Interface;

public interface IDoctorService
{
    List<Doctor> GetAllDoctors();
    Doctor GetDoctorById(int id);
    int AddDoctor(Doctor doctor);
    bool UpdateDoctor(Doctor doctor);
    bool DeleteDoctor(int id);
}
