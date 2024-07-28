using Hotel_Management_System.Models;
using Hotel_Management_System.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hotel_Management_System.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [Route("/LoginValidation")]
        [HttpPost]
        public ObjectResult Login([FromBody] Login loginDetails)
        {
            if (loginDetails == null)
            {
                return BadRequest("Login details are required.");
            }

            var user = userService.ValidateUser(loginDetails);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok("Login successful");
        }

        // GET: api/<UserController>
        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            try
            {
                var users = userService.GetUsers();
                if (users == null || users.Count == 0)
                {
                    return NotFound("No users found.");
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID cannot be null or empty.");
            }

            try
            {
                var user = userService.GetUser(id);
                if (user == null)
                {
                    return NotFound($"User with ID = {id} not found.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST api/<UserController>
        [HttpPost]
        public ActionResult<User> CreateNewUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User cannot be null.");
            }

            try
            {
                user.CreatedOn = DateTime.Now;
                userService.Create(user);
                return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public ActionResult UpdateUser(string id, [FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User cannot be null.");
            }

            try
            {
                var existingUser = userService.GetUser(id);

                if (existingUser == null)
                {
                    return NotFound($"User with Id = {id} not found.");
                }

                userService.Update(id, user);
                return Ok($"User with Id ={id} updated");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(string id)
        {
            var user = userService.GetUser(id);

            if (user == null)
            {
                return NotFound($"User with Id = {id} not found");
            }
            userService.Delete(id);
            return Ok($"User with Id ={id} deleted");
        }

    }
}
