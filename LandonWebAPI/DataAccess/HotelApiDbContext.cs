using LandonWebAPI.Models.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace LandonWebAPI.DataAccess;

public class HotelApiDbContext : DbContext
{
    public HotelApiDbContext(DbContextOptions options) : base(options)
    { }

    public DbSet<RoomEntity> Rooms { get; set; }

    public DbSet<BookingEntity> Bookings { get; set; }
}