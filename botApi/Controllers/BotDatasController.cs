using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using botApi.Models;

namespace botApi.Controllers
{
    public class BotDatasController : ApiController
    {
        private BotEntities db = new BotEntities();

        // GET: api/BotDatas
        public IQueryable<BotData> GetBotData()
        {
            return db.BotData;
        }

        // GET: api/BotDatas/5
        [ResponseType(typeof(BotData))]
        public IHttpActionResult GetBotData(int id)
        {
            BotData botData = db.BotData.Find(id);
            if (botData == null)
            {
                return NotFound();
            }

            return Ok(botData);
        }

        // PUT: api/BotDatas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBotData(int id, BotData botData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != botData.BotSeq)
            {
                return BadRequest();
            }

            db.Entry(botData).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BotDataExists(id))
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

        // POST: api/BotDatas
        [ResponseType(typeof(BotData))]
        public IHttpActionResult PostBotData(BotData botData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BotData.Add(botData);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = botData.BotSeq }, botData);
        }

        // DELETE: api/BotDatas/5
        [ResponseType(typeof(BotData))]
        public IHttpActionResult DeleteBotData(int id)
        {
            BotData botData = db.BotData.Find(id);
            if (botData == null)
            {
                return NotFound();
            }

            db.BotData.Remove(botData);
            db.SaveChanges();

            return Ok(botData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BotDataExists(int id)
        {
            return db.BotData.Count(e => e.BotSeq == id) > 0;
        }
    }
}