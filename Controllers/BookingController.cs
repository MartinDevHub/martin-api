using AutoMapper;
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
        private readonly IMapper _mapper;
        public BookingController(ILogger<BookingController> logger, ApplicationDbContext db, IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookings()
        {
            _logger.LogInformation("Obtain Bookings");
            IEnumerable<Booking> bookingList = await _db.Bookings.ToListAsync();
             return Ok(_mapper.Map<IEnumerable<BookingDto>>(bookingList));
        }

        [HttpGet("id", Name = "GetBooking") ]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingDto>> GetBooking(int id)
          
            {
            if(id == 0) { return BadRequest(); };

            //var booking = BookingStore.BookingDtoList.FirstOrDefault(b => b.Id == id);

            var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null) {
                _logger.LogInformation("Invalid: check Id");
                return NotFound(); };

            return Ok(_mapper.Map<BookingDto>(booking));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task <ActionResult<BookingDto>> PostBokings([FromBody] BookingCreateDto createBookingDto)
        {
            
            //Required validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Personalized validation: if the name already exists, returns a personalized validation.
            if(await _db.Bookings.FirstOrDefaultAsync(b=>b.Name.ToLower() == createBookingDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Name Exists", "Name already exists");
                return BadRequest(ModelState);
            }


            if (createBookingDto == null )
            {
               return BadRequest(createBookingDto);
            }

            Booking modelo = _mapper.Map<Booking>(createBookingDto);

          await _db.Bookings.AddAsync(modelo);
          await _db.SaveChangesAsync();

            return CreatedAtRoute("GetBooking", new { id = modelo.Id }, modelo);
        }

        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task <IActionResult> DeleteBooking(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            _db.Bookings.Remove(booking);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task <IActionResult> UpdateBooking(int id, [FromBody] BookingUpdateDto updateBookingDto)
        {
           

            if (updateBookingDto == null || id!= updateBookingDto.Id)
            {
                return BadRequest();
            }

            Booking model = _mapper.Map<Booking>(updateBookingDto);

            _db.Bookings.Update(model);
            await _db.SaveChangesAsync();

            return NoContent();
            
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdatePartialBooking(int id, JsonPatchDocument<BookingUpdateDto> bookingPatchDto)
        {


            if (bookingPatchDto == null || id == 0)
            {
                return BadRequest();
            }

            var booking = await _db.Bookings.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);

            BookingUpdateDto bookingDto = _mapper.Map<BookingUpdateDto>(booking);

            if (booking == null)
            {
                return BadRequest();
            }

            bookingPatchDto.ApplyTo(bookingDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Booking modelo = _mapper.Map<Booking>(bookingDto);


            _db.Bookings.Update(modelo);
            await _db.SaveChangesAsync();
            return NoContent();

        }

    }
}
