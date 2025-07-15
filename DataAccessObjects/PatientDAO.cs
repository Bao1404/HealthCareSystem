using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class PatientDAO
    {
        private readonly HealthCareSystemContext _context;
        private static PatientDAO instance;
        private static readonly object _lock = new object();
        public static PatientDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new PatientDAO();
                        }
                    }
                }
                return instance;
            }
        }
        public PatientDAO()
        {
            _context = new HealthCareSystemContext();
        }
        public async Task CreatePatient(Patient patient)
        {
            try
            {
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
