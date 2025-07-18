using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthCareSystem.Models;

public class StatisticsController : Controller
{
    private readonly HealthCareSystemContext _context;

    public StatisticsController(HealthCareSystemContext context)
    {
        _context = context;
    }

    // GET: /Statistics/Today
    public async Task<IActionResult> Today()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        var hourlyData = await _context.Appointments
            .Where(a => a.AppointmentDateTime >= today && a.AppointmentDateTime < tomorrow)
            .GroupBy(a => a.AppointmentDateTime.Hour)
            .Select(g => new
            {
                Hour = g.Key,
                Count = g.Count()
            })
            .OrderBy(g => g.Hour)
            .ToListAsync();


        ViewBag.HourlyLabels = hourlyData.Select(x => $"{x.Hour}:00").ToList();
        ViewBag.HourlyCounts = hourlyData.Select(x => x.Count).ToList();


        return View();
    }

}
