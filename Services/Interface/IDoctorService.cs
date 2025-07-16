using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IDoctorService
    {
        public Task<Doctor> GetDoctorsByIdAsync(int id);
    }
}
