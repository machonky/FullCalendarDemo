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
                start = bd.Id.FromDateIndex().ToString("yyyy-MM-dd"),
                allDay = true
            });

            return new JsonResult(calendarModels);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostSelectedEvents([FromBody] SelectedEventsData data)
        {
            var startDate = data.StartDate.ToDateTime(new TimeOnly());
            var endDate = data.EndDate.ToDateTime(new TimeOnly());
            var startDateIndex = startDate.DateTimeToDateIndex();
            var endDateIndex = endDate.DateTimeToDateIndex();

            var businessDayIndices = await _context.BusinessDays
                .Where(bd => bd.Id >= startDateIndex && bd.Id <= endDateIndex)
                .ToListAsync();

            var selectedDates = Enumerable.Range(0, (endDate - startDate).Days)
                .Select(offset => startDate.AddDays(offset))
                .ToList();

            foreach(var date in selectedDates)
            {
                var businessDayId = date.DateTimeToDateIndex();
                var existingBusinessDay = businessDayIndices.FirstOrDefault(bd => bd.Id == businessDayId);
                if (existingBusinessDay != null) 
                {
                    _context.BusinessDays.Remove(existingBusinessDay);
                }
                else
                {
                    var newBusinessDay = new BusinessDay
                    {
                        Id = businessDayId
                    };
                    _context.BusinessDays.Add(newBusinessDay);
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
