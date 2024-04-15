using FullCalendarDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace FullCalendarDemo.Data;

public class BusinessDaysDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<BusinessDay> BusinessDays { get; set; }
}
