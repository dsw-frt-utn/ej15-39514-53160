using Dsw2026Ej15.Domain.Entities;

namespace Dsw2026Ej15.Domain.Interfaces;

public interface IPersistence
{
    IEnumerable<Doctor> GetAllDoctors();
    Doctor? GetDoctorById(Guid id);
    Speciality? GetSpecialityById(Guid id);
    void SaveDoctor(Doctor doctor);
}
