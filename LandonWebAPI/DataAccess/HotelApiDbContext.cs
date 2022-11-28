using LandonWebAPI.Models.DbEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LandonWebAPI.DataAccess;

public class HotelApiDbContext : IdentityDbContext<UserEntity, UserRoleEntity, Guid>
{
    public HotelApiDbContext()
    {
    }

    public HotelApiDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<RoomEntity> Rooms { get; set; }

    public DbSet<BookingEntity> Bookings { get; set; }
}