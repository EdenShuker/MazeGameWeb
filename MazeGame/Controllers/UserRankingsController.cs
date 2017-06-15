using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MazeGame.Models;

namespace MazeGame.Controllers
{
    public class UserRankingsController : ApiController
    {
        private UsersInfoContext db = new UsersInfoContext();

        // GET: api/UserRankings
        public IQueryable<UserRankings> GetUserRankings()
        {
            return db.UserRankings;
        }

        // GET: api/UserRankings/5
        [ResponseType(typeof(UserRankings))]
        public async Task<IHttpActionResult> GetUserRankings(int id)
        {
            UserRankings userRankings = await db.UserRankings.FindAsync(id);
            if (userRankings == null)
            {
                return NotFound();
            }

            return Ok(userRankings);
        }

        // PUT: api/UserRankings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUserRankings(int id, UserRankings userRankings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userRankings.Id)
            {
                return BadRequest();
            }

            db.Entry(userRankings).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRankingsExists(id))
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

        // POST: api/UserRankings
        [ResponseType(typeof(UserRankings))]
        public async Task<IHttpActionResult> PostUserRankings(UserRankings userRankings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserRankings.Add(userRankings);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = userRankings.Id }, userRankings);
        }

        // DELETE: api/UserRankings/5
        [ResponseType(typeof(UserRankings))]
        public async Task<IHttpActionResult> DeleteUserRankings(int id)
        {
            UserRankings userRankings = await db.UserRankings.FindAsync(id);
            if (userRankings == null)
            {
                return NotFound();
            }

            db.UserRankings.Remove(userRankings);
            await db.SaveChangesAsync();

            return Ok(userRankings);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserRankingsExists(int id)
        {
            return db.UserRankings.Count(e => e.Id == id) > 0;
        }
    }
}