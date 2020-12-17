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
    }
}
