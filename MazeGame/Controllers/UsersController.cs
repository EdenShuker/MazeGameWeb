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

        /// <summary>
        /// Returns list of users.
        /// </summary>
        /// <returns> liat of users ordered by rank </returns>
        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users.OrderByDescending(user => user.Wins - user.Losses);
        }


        /// <summary>
        /// Return a user with the given id.
        /// </summary>
        /// <param name="id"> name of user </param>
        /// <returns> User with the given id </returns>
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

        /// <summary>
        /// Add the user with the given id to database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns> none </returns>
        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(string id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // invalid request
            if (id != user.Name)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                // save changes to db.
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


        /// <summary>
        /// Operate hashing function on string.
        /// </summary>
        /// <param name="str"> string </param>
        /// <returns>hashed string</returns>
        private static string Hash(string str)
        {
            // create hashing function
            SHA1 sha1 = SHA1.Create();
            byte[] buffer = Encoding.ASCII.GetBytes(str);
            byte[] hashed = sha1.ComputeHash(buffer);
            return Convert.ToBase64String(hashed);
        }


        /// <summary>
        /// Add user to database.
        /// </summary>
        /// <param name="user"> user to add </param>
        /// <returns> the user added </returns>
        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // add user to database
            db.Users.Add(user);
            // hash the password
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


        /// <summary>
        /// check if user exist.
        /// </summary>
        /// <param name="user"></param>
        /// <returns> throw an error if user doesn't exist </returns>
        // POST: api/Users
        [HttpPost]
        [Route("api/Users/login")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Login(User user)
        {
            // gat user record
            User record = await db.Users.FindAsync(user.Name);
            if (record == null)
            {
                // user name doesn't exist
                return BadRequest();
            }
            // passwords don't matches.
            if (record.Password != Hash(user.Password))
            {
                return BadRequest();
            }

            return Ok();
        }

        
        /// <summary>
        /// Update number of wins of user.
        /// </summary>
        /// <param name="user"> winning user </param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Users/win")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Win(User user)
        {
            // get the user 
            User record = await db.Users.FindAsync(user.Name);
            if (record == null)
            {
                return NotFound();
            }
            // increase number of wins 
            db.Entry(record).Entity.Wins += 1;
            db.Entry(record).State = EntityState.Modified;
            // save changes
            await db.SaveChangesAsync();

            return Ok();
        }


        /// <summary>
        /// Update number of losses.
        /// </summary>
        /// <param name="user"> losing user</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Users/lose")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Lose(User user)
        {
            // get the user
            User record = await db.Users.FindAsync(user.Name);
            if (record == null)
            {
                return NotFound();
            }

            // update number of losses
            db.Entry(record).Entity.Losses += 1;
            db.Entry(record).State = EntityState.Modified;
            // save changes
            await db.SaveChangesAsync();

            return Ok();
        }


        /// <summary>
        /// Delete the user with the given id.
        /// </summary>
        /// <param name="id"> id of user to delete</param>
        /// <returns> the user deleted </returns>
        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            // get the user
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            // delete user from db.
            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Check if user exist.
        /// </summary>
        /// <param name="id"> id of user </param>
        /// <returns> true if exist, false otherwise </returns>
        private bool UserExists(string id)
        {
            return db.Users.Count(e => e.Name == id) > 0;
        }
    }
}