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
    public class SzerzodesekController : ControllerBase
    {
        private readonly PartnerSzerzodesContext _context;

        public SzerzodesekController(PartnerSzerzodesContext context)
        {
            _context = context;
        }

        // GET: api/Szerzodesek
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Object>>> GetSzerzodesek()
        {
            return await _context.Szerzodesek.Select(t=>new {
                Id=t.Id,
                SzerzodesSzam=t.SzerzodesSzam,
                IgazgatoJovahagyta=t.IgazgatoJovahagyta,
                SzerzodesTargya=t.SzerzodesTargya,
                PartnerNev=t.Partner !=null ? t.Partner.PartnerNev:null
            }).ToListAsync();
        }

        // GET: api/Szerzodesek/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Object>> GetSzerzodes(int id)
        {
            var szerzodes = await _context.Szerzodesek.Include(t => t.Partner).FirstOrDefaultAsync(t => t.Id == id);

            if (szerzodes == null)
            {
                return NotFound();
            }

            return new 
            { 
                Id=szerzodes.Id,
                SzerzodesSzam=szerzodes.SzerzodesSzam,
                IgazgatoJovahagyta=szerzodes.IgazgatoJovahagyta,
                SzerzodesTargya=szerzodes.SzerzodesTargya,
                PartnerNev = szerzodes.Partner != null ? szerzodes.Partner.PartnerNev : null
            };
        }

        // PUT: api/Szerzodesek/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSzerzodes(int id, Szerzodes szerzodes)
        {
            if (id != szerzodes.Id)
            {
                return BadRequest();
            }

            _context.Entry(szerzodes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SzerzodesExists(id))
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

        // POST: api/Szerzodesek
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Szerzodes>> PostSzerzodes(Szerzodes szerzodes)
        {
            _context.Szerzodesek.Add(szerzodes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSzerzodes", new { id = szerzodes.Id }, szerzodes);
        }

        // DELETE: api/Szerzodesek/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSzerzodes(int id)
        {
            var szerzodes = await _context.Szerzodesek.FindAsync(id);
            if (szerzodes == null)
            {
                return NotFound();
            }

            _context.Szerzodesek.Remove(szerzodes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SzerzodesExists(int id)
        {
            return _context.Szerzodesek.Any(e => e.Id == id);
        }
    }
}
