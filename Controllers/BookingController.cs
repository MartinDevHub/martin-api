using Booking_API.Data;
using Marcoff_API.Data;
using Marcoff_API.Models;
using Marcoff_API.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Marcoff_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly ApplicationDbContext _db;
        public BookingController(ILogger<BookingController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<BookingDto>> GetBookings()
        {
            _logger.LogInformation("Obtain Bookings");
            return Ok(_db.Bookings.ToList());
        }

        [HttpGet("id", Name = "GetBooking") ]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BookingDto> GetBooking(int id)
          
            {
            if(id == 0) { return BadRequest(); };

            //var booking = BookingStore.BookingDtoList.FirstOrDefault(b => b.Id == id);

            var booking = _db.Bookings.FirstOrDefault(b => b.Id == id);

            if (booking == null) {
                _logger.LogInformation("Invalid: check Id");
                return NotFound(); };

            return Ok(booking);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<BookingDto> PostBokings([FromBody] BookingDto booking)
        {
            
            //Required validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Personalized validation: if the name already exists, returns a personalized validation.
            if(_db.Bookings.FirstOrDefault(b=>b.Name.ToLower() == booking.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Name Exists", "Name already exists");
                return BadRequest(ModelState);
            }


            if (booking == null )
            {
               return BadRequest();
            }
        
            if (booking.Id > 0) {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Booking modelo = new()
            {
                Name = booking.Name,
                Detail = booking.Detail,
                Fee = booking.Fee,
                BedsOcuppied = booking.BedsOcuppied,
            };

            _db.Bookings.Add(modelo);
            _db.SaveChanges();

            return CreatedAtRoute("GetBooking", new { id = booking.Id }, booking);
        }

        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteBooking(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var booking = _db.Bookings.FirstOrDefault(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            _db.Bookings.Remove(booking);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBooking(int id, [FromBody] BookingDto bookingDto)
        {
           

            if (bookingDto == null || id!=bookingDto.Id)
            {
                return BadRequest();
            }

            //var booking = BookingStore.BookingDtoList.FirstOrDefault(b => b.Id == id);

            //booking.Name = bookingDto.Name;

            Booking modelo = new()
            {
                Id = bookingDto.Id,
                Name = bookingDto.Name,
                Detail = bookingDto.Detail,
                Fee = bookingDto.Fee,
                BedsOcuppied = bookingDto.BedsOcuppied,
            };

            _db.Bookings.Update(modelo);
            _db.SaveChanges();

            return NoContent();
            
        }

        [HttpPatch("{id:int}")]
        public IActionResult UpdatePartialBooking(int id, JsonPatchDocument<BookingDto> bookingPatchDto)
        {


            if (bookingPatchDto == null || id == 0)
            {
                return BadRequest();
            }

            //var booking = BookingStore.BookingDtoList.FirstOrDefault(b => b.Id == id);
            var booking = _db.Bookings.AsNoTracking().FirstOrDefault(b => b.Id == id);

            BookingDto bookingDto = new()
            {
                Id = booking.Id,
                Name = booking.Name,
                Detail = booking.Detail,
                Fee = booking.Fee,
                BedsOcuppied = booking.BedsOcuppied,
            };

            if (booking == null)
            {
                return BadRequest();
            }

            bookingPatchDto.ApplyTo(bookingDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Booking modelo = new()
            {
                Id = bookingDto.Id,
                Name = bookingDto.Name,
                Detail = bookingDto.Detail,
                Fee = bookingDto.Fee,
                BedsOcuppied = bookingDto.BedsOcuppied,
            };
            _db.Bookings.Update(modelo);
            _db.SaveChanges();
            return NoContent();

        }

    }
}
