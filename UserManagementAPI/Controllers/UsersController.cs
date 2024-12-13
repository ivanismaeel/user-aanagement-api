using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static List<User> Users = new List<User>();

        // GET: api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            try
            {
                return Ok(Users);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            try
            {
                var user = Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/users
        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                user.Id = Users.Any() ? Users.Max(u => u.Id) + 1 : 1;
                Users.Add(user);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }
                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.Email = updatedUser.Email;
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                var user = Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }
                Users.Remove(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
