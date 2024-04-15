using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FullCalendarDemo.Data;
using FullCalendarDemo.Models;

namespace FullCalendarDemo.Pages
{
    public class CalendarModel : PageModel
    {
        private readonly FullCalendarDemo.Data.BusinessDaysDbContext _context;
        private readonly ILogger<CalendarModel> _logger;

        public CalendarModel(FullCalendarDemo.Data.BusinessDaysDbContext context, ILogger<CalendarModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<BusinessDay> BusinessDays { get;set; } = default!;

        public async Task OnGetAsync()
        {
            //BusinessDays = await _context.BusinessDays.ToListAsync();
        }

        public async Task<IActionResult> OnGetBusinessDays()
        {
            var businessDays = await _context.BusinessDays.ToListAsync();
            var calendarModels = businessDays.Select(bd => new
            {
                id = bd.Id,
                title = "Business Day",
                start = new DateTime(bd.Id / 10000, (bd.Id / 100) % 100, bd.Id % 100).ToString("yyyy-MM-dd"),
                allDay = true
            });

            return new JsonResult(calendarModels);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostSelectedEvents([FromBody] SelectedEventsData data)
        {
            var startDate = data.StartDate;
            var endDate = data.EndDate;

            // Iterate over the date range and create BusinessDay entities
            for (var date = startDate; date < endDate; date = date.AddDays(1))
            {
                int businessDayId = int.Parse(date.ToString("yyyyMMdd"));

                // Check if the business day already exists in the database
                bool businessDayExists = await _context.BusinessDays.AnyAsync(bd => bd.Id == businessDayId);

                if (!businessDayExists)
                {
                    // Create a new BusinessDay entity and add it to the database
                    var businessDay = new BusinessDay
                    {
                        Id = businessDayId
                    };

                    _context.BusinessDays.Add(businessDay);
                }
            }

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }

        public class SelectedEventsData
        {
            public DateOnly StartDate { get; set; }
            public DateOnly EndDate { get; set; }
        }
    }
}
