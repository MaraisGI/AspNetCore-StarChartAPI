using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var result = _context.CelestialObjects.Where(c => c.Id == id).FirstOrDefault();
            if (result == null) return NotFound();
            result.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == id).ToList();
            return Ok(result);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var results = _context.CelestialObjects.Where(c => c.Name == name).ToList();
            if (results == null || results.Count == 0) return NotFound();
            foreach (CelestialObject co in results)
            {
                co.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == co.Id).ToList();
            }
            return Ok(results);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var results = _context.CelestialObjects.ToList();
            if (results == null || results.Count == 0) return NotFound();
            foreach (CelestialObject co in results)
            {
                co.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == co.Id).ToList();
            }
            return Ok(results);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject co)
        {
            _context.CelestialObjects.Add(co);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { co.Id }, co);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject co)
        {
            var cu = _context.CelestialObjects.Where(c => c.Id == id).FirstOrDefault();
            if (cu == null) return NotFound();
            cu.Name = co.Name;
            cu.OrbitalPeriod = co.OrbitalPeriod;
            cu.OrbitedObjectId = co.OrbitedObjectId;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var cp = _context.CelestialObjects.Where(c => c.Id == id).FirstOrDefault();
            if (cp == null) return NotFound();
            cp.Name = name;
            _context.Update(cp);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cd = _context.CelestialObjects.Where(c => c.Id == id || c.OrbitedObjectId == id).ToList();
            if (cd == null || cd.Count == 0) return NotFound();
            _context.CelestialObjects.RemoveRange(cd);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
