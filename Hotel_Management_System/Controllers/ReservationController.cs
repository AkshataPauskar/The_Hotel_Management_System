using Hotel_Management_System.Models;
using Hotel_Management_System.Pagination;
using Hotel_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hotel_Management_System.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationsService reservationService;
        private readonly DataPagination dataPagination;
        public ReservationController(IReservationsService reservationsService)
        {
            this.reservationService = reservationsService;
            dataPagination = new DataPagination();
        }

        // GET: api/<ReservationsController>
        [HttpGet]
        public ObjectResult GetAllReservations(int page = 1, int pageSize = 10)
        {
            try
            {
                if (page <= 0 || pageSize <= 0)
                {
                    return BadRequest("Page and pageSize must be greater than zero.");
                }

                var reservations = reservationService.GetAllReservations();

                var paginatedResponse = dataPagination.GetPaginatedResponse(reservations, page, pageSize);

                return Ok(paginatedResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/<ReservationsController>/5
        [HttpGet("{id}")]
        public ObjectResult GetReservationById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Reservation ID is required.");
                }

                var reservation = reservationService.GetReservationById(id);
                if (reservation == null)
                {
                    return NotFound($"Reservation with ID={id} not found.");
                }

                return Ok(reservation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST api/<ReservationsController>
        [HttpPost]
        public ActionResult<Reservation> AddNewReservation([FromBody] Reservation newReservationDetails)
        {
            if (newReservationDetails == null)
            {
                return BadRequest("Reservation details cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                reservationService.AddNewReservation(newReservationDetails);
                return CreatedAtAction(nameof(GetReservationById), new { id = newReservationDetails.Id }, newReservationDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        // PUT api/<ReservationsController>/5
        [HttpPut("{id}")]
        public ActionResult UpdateReservation(string id, [FromBody] Reservation reservationData)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Reservation ID is required.");
            }

            if (reservationData == null)
            {
                return BadRequest("Reservation data cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingReservation = reservationService.GetReservationById(id);
                if (existingReservation == null)
                {
                    return NotFound($"Reservation with ID = {id} not found.");
                }

                reservationService.UpdateReservationDetails(id, reservationData);

                return Ok($"Reservation with ID = {id} updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE api/<ReservationsController>/5
        [HttpDelete("{id}")]
        public ActionResult DeleteReservation(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Reservation ID is required.");
            }

            try
            {
                var reservation = reservationService.GetReservationById(id);
                if (reservation == null)
                {
                    return NotFound($"Reservation with ID = {id} not found.");
                }

                reservationService.DeleteReservation(id);
                return Ok($"Reservation with ID = {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
