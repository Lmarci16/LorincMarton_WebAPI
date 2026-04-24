using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LorincMarton_WebAPI.Modells;

namespace LorincMarton_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerekController : ControllerBase
    {
        private readonly PartnerSzerzodesContext _context;

        public PartnerekController(PartnerSzerzodesContext context)
        {
            _context = context;
        }

        // GET: api/Partnerek
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Object>>> GetPartners()
        {
            return await _context.Partnerek.Select(t=>new {
            Id=t.Id,
            PartnerNev=t.PartnerNev,
            Email=t.Email,
            }).ToListAsync();
        }

        // GET: api/Partnerek/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Object>> GetPartner(int id)
        {
            var partner = await _context.Partnerek.Include(t => t.Szerzodes).FirstOrDefaultAsync(t => t.Id == id);

            if (partner == null)
            {
                return NotFound();
            }

            return new
            {
                Id=partner.Id,
                PartnerNev=partner.PartnerNev,
                Email=partner.Email,

            };
        }

        // PUT: api/Partnerek/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPartner(int id, Partner partner)
        {
            if (id != partner.Id)
            {
                return BadRequest();
            }

            _context.Entry(partner).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartnerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Partnerek
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Partner>> PostPartner(Partner partner)
        {
            _context.Partnerek.Add(partner);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPartner", new { id = partner.Id }, partner);
        }

        // DELETE: api/Partnerek/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePartner(int id)
        {
            var partner = await _context.Partnerek.FindAsync(id);
            if (partner == null)
            {
                return NotFound();
            }

            _context.Partnerek.Remove(partner);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PartnerExists(int id)
        {
            return _context.Partnerek.Any(e => e.Id == id);
        }
    }
}
