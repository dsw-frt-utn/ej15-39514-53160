using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Data;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Dsw2026Ej15.Api.Controllers;

public class DoctorsController : AppController
{
    private readonly IPersistence _persistence;

    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }

    [HttpPost("doctors")]
    public async Task<IActionResult> CreateDoctor(DoctorModel.Request request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.LicenseNumber))
            throw new ValidationException("Nombre y matrícula son requeridos");

        var speciality = await _persistence.GetSpecialityById(request.SpecialityId);
        if (speciality is null)
            throw new ValidationException("La especialidad no existe");

        var doctor = new Doctor(request.Name, request.LicenseNumber, true, speciality);
        await _persistence.SaveDoctor(doctor);

        return Created();  
    }

    [HttpGet("doctors")]
    public async Task<IActionResult> ReadActiveDoctors()
    {
        var doctors = await _persistence.GetAllDoctors();
        var responseList = doctors.Where(d => d.IsActive).Select(d => DoctorModel.ResponseList.DoctorList(d));

        return Ok(responseList); 
    }

    [HttpGet("doctors/{id}")]
    public async Task<IActionResult> ReadDoctorById(Guid id)
    {
        var doctor = await _persistence.GetDoctorById(id);

        if (doctor is null || !doctor.IsActive)
            throw new ValidationException("No se encuentra el médico o no está activo");

        var responseId = DoctorModel.ResponseId.DoctorId(doctor);
        return Ok(responseId); 
    }

    [HttpDelete("doctors/{id}")]
    public async Task<IActionResult> DeactivateDoctor(Guid id)
    {
        var doctor = await _persistence.GetDoctorById(id);

        if (doctor is null || !doctor.IsActive)
            throw new ValidationException("No se encuentra el médico o no está activo");

        doctor.Deactivate();

        if (_persistence is PersistenceEf ef)
            await ef.UpdateDoctor(doctor);
        else
            await _persistence.SaveDoctor(doctor); 

        return NoContent();
    }
}
