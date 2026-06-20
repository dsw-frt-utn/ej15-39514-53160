using Dsw2026Ej15.Api.Models;
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
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.LicenseNumber))
        {
            return BadRequest("Nombre y matricula son requeridos");
        }

        var speciality = _persistence.GetSpecialityById(request.SpecialityId);
        if (speciality is null)
        {
            return BadRequest("Especialidad no existe");
        }

        var doctor = new Doctor(request.Name, request.LicenseNumber, true, speciality);
        _persistence.SaveDoctor(doctor);

        return Created();
    }


    [HttpGet("doctors")]
    public async Task<IActionResult> ReadActiveDoctors()
    {
        var responseList = _persistence
            .GetAllDoctors()
            .Where(d => d.IsActive)
            .Select(d => DoctorModel.ResponseList.DoctorList(d));

        return Ok(responseList);
    }


    [HttpGet("doctors/{id}")]
    public async Task<IActionResult> ReadDoctorById(Guid id)
    {
        var doctor = _persistence.GetDoctorById(id);

        if (doctor is null || !doctor.IsActive)
        {
            throw new ValidationException("No se encuentra el medico o no esta activo");
            //return NotFound("No se encuentra el medico o no esta activo");
        }

        var responseId = DoctorModel.ResponseId.DoctorId(doctor);
        return Ok(responseId);
    }

    [HttpDelete("doctors/{id}")]

    public async Task<IActionResult> DeactivateDoctor(Guid id)
    {
        var doctor = _persistence.GetDoctorById(id);

        if (doctor is null || !doctor.IsActive)
        {
            throw new ValidationException("No se encuentra el medico o no esta activo");
            // return NotFound("No se encuentra el medico o no esta activo");
        }

        doctor.Deactivate();

        return NoContent();
    }
}
