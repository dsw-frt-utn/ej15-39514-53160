using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Domain.Entities
{
    public class Doctor : BaseEntity
    {
        public string Name { get; init; }
        public string LicenseNumber { get; init; }
        public bool IsActive { get; private set; }

        //agrego la foreign key de especialidad
        public Guid? SpecialityId { get; set; }
        public Speciality? Speciality { get; private set; }// la asociacion entre doctor y especialidad es agregacion
        

        //constructor privado para no romper con mi contrato de constructor y porque mi orm lo necesita
        private Doctor() { }

        public Doctor(string name, string licenseNumber, bool isActive, Speciality? speciality, Guid? id = null) : base(id)
        {
            Name = name;
            LicenseNumber = licenseNumber;
            IsActive = true;
            Speciality = speciality;
        }
        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
