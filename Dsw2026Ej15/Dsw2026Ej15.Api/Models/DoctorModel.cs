using Dsw2026Ej15.Domain.Entities;

namespace Dsw2026Ej15.Api.Models;

public record DoctorModel
{
    public record Request(string Name, string LicenseNumber, Guid SpecialityId);
    public record ResponseList(Guid DoctorId, string Name, string LicenseNumber, string SpecialityName, bool IsActive)
    {
        public static ResponseList DoctorList(Doctor doctor) =>
            new(doctor.Id, doctor.Name, doctor.LicenseNumber, doctor.Speciality?.Name ?? "", doctor.IsActive);
    }
    public record ResponseId(string Name, string LicenseNumber, string SpecialityName)
    {
        public static ResponseId DoctorId(Doctor doctor) =>
            new(doctor.Name, doctor.LicenseNumber, doctor.Speciality?.Name ?? "");
    }
}
