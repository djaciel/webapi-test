using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using webapi_test.Data.Models;

namespace webapi_test.API.Controllers
{
    public class CustomerController : ApiController
    {
        private Entities db = new Entities();

        public IQueryable<Customer> GetCustomer()
        {
            return db.Customer;
        }

        public async Task<IHttpActionResult> GetCustomer(int id)
        {
            Customer cust = await db.Customer.FindAsync(id);
            if (cust == null)
            {
                return NotFound();
            }

            return Ok(cust);
        }

        public IHttpActionResult GetCustomerByName(string name)
        {
            Customer cust = db.Customer.FirstOrDefault(x => x.Name == name);

            if (cust == null)
            {
                return NotFound();
            }

            return Ok(cust);
        }

        public async Task<IHttpActionResult> PostCustomer(Customer cust)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customer.Add(cust);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = cust.IdCustomer }, cust);
        }

        public async Task<IHttpActionResult> PutCustomer(int id, Customer cust)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cust.IdCustomer)
            {
                return BadRequest();
            }

            db.Entry(cust).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExist(id))
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

        public async Task<IHttpActionResult> DeleteCustomer(int id)
        {
            Customer cust = await db.Customer.FindAsync(id);
            if (cust == null)
            {
                return NotFound();
            }

            db.Customer.Remove(cust);
            await db.SaveChangesAsync();

            return Ok(cust);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExist(int id)
        {
            return db.Customer.Count(e => e.IdCustomer == id) > 0;
        }
    }
}
