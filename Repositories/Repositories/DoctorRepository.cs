using BusinessObjects;
using DataAccessObjects;
using Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        public async Task<Doctor> GetDoctorsByIdAsync(int id) => await DoctorDAO.Instance.GetDoctorByUserId(id);
        
    }
}
