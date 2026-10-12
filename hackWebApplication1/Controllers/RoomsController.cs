
using hackWebApplication1.Data;
using hackWebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace hackWebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/rooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms(
            [FromQuery] bool availableOnly = false)
        {
            var query = _context.Rooms.AsQueryable();

            if (availableOnly)
            {
                query = query.Where(r => r.OccupiedBeds < r.Capacity);
            }

            return Ok(await query.ToListAsync());
        }

        // GET: api/rooms/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
                return NotFound();

            return Ok(room);
        }

        // POST: api/rooms
        [HttpPost]
        public async Task<ActionResult<Room>> CreateRoom(Room room)
        {
            if (room.OccupiedBeds > room.Capacity)
                ModelState.AddModelError(
                    nameof(room.OccupiedBeds),
                    "Occupied beds cannot exceed capacity.");

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetRoom),
                    new { id = room.Id },
                    room);
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is SqlException sqlEx
                      && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                return Conflict(new
                {
                    message = "This room number already exists."
                });
            }
        }

        // PUT: api/rooms/1
        [HttpPut("{id}")]
        public async Task<ActionResult<Room>> UpdateRoom(int id, Room updatedRoom)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
                return NotFound();

            if (updatedRoom.OccupiedBeds > updatedRoom.Capacity)
                ModelState.AddModelError(
                    nameof(updatedRoom.OccupiedBeds),
                    "Occupied beds cannot exceed capacity.");

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            room.RoomNumber = updatedRoom.RoomNumber;
            room.RoomType = updatedRoom.RoomType;
            room.Capacity = updatedRoom.Capacity;
            room.OccupiedBeds = updatedRoom.OccupiedBeds;
            room.MonthlyFee = updatedRoom.MonthlyFee;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(room);
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is SqlException sqlEx
                      && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                return Conflict(new
                {
                    message = "This room number already exists."
                });
            }
        }

        // DELETE: api/rooms/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
                return NotFound();

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}