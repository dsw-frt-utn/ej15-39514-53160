using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Data
{
    public class PersistenceEf: IPersistence
    {
        private readonly Dsw2026Ej15DbContext _context;

        public PersistenceEf(Dsw2026Ej15DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctors() => await _context.Doctors.Include(d => d.Speciality).Where(d => d.IsActive).ToListAsync();
        public async Task<Doctor?> GetDoctorById(Guid id) => await _context.Doctors.Include(d => d.Speciality).FirstOrDefaultAsync(d => d.Id == id);
        public async Task<Speciality?> GetSpecialityById(Guid id) => await _context.Specialities.FirstOrDefaultAsync(s => s.Id == id);
        public async Task SaveDoctor(Doctor doctor)
        {
            //add vincula o agrega el objeto al contexto, pero del contexto debo bajarlo e impactarlo en la base de datos
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateDoctor(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
        }
    }
}
