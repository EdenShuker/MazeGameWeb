using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MazeGame.Models;

namespace MazeGame.Controllers
{
    public class UsersController : ApiController
    {
        private UsersInfoContext db = new UsersInfoContext();

        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users.OrderByDescending(user => user.Wins - user.Losses);
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(string id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(string id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Name)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        private static string Hash(string str)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] buffer = Encoding.ASCII.GetBytes(str);
            byte[] hashed = sha1.ComputeHash(buffer);
            return Convert.ToBase64String(hashed);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);

            user.Password = Hash(user.Password);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = user.Name }, user);
        }

        // POST: api/Users
        [HttpPost]
        [Route("api/Users/login")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Login(User user)
        {
            User record = await db.Users.FindAsync(user.Name);
            if (record == null)
            {
                return BadRequest();
            }

            if (record.Password != Hash(user.Password))
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/Users/win")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Win(User user)
        {
            User record = await db.Users.FindAsync(user.Name);
            if (record == null)
            {
                return NotFound();
            }

            db.Entry(record).Entity.Wins += 1;
            db.Entry(record).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("api/Users/lose")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Lose(User user)
        {
            User record = await db.Users.FindAsync(user.Name);
            if (record == null)
            {
                return NotFound();
            }

            db.Entry(record).Entity.Losses += 1;
            db.Entry(record).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string id)
        {
            return db.Users.Count(e => e.Name == id) > 0;
        }
    }
}