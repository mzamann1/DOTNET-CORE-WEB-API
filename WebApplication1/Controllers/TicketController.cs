using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TicketController : ControllerBase
    {
        private TicketContext _context;

        public TicketController(TicketContext context)
        {
            _context = context;

            if (_context.TicketItems.Count() == 0) //if no item in database
            {
                _context.TicketItems.Add(new TicketItem { Concert="Beyonce"}); //create new one 
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<TicketItem> GetAll()
        {
            return _context.TicketItems.AsNoTracking().ToList();
        }

        //   api/[Controller]/{id}
        [HttpGet("{id}",Name = "GetTicket")]
        public IActionResult GetById(long id)
        {
            var ticket = _context.TicketItems.FirstOrDefault(t => t.Id == id); //searching field matching with id

            if (ticket ==null)
            {
                return NotFound(); //404
                
            }
            return new ObjectResult(ticket);//200
        }

        [HttpPost] //      route /[controller]/create
        public IActionResult Create([FromBody]TicketItem ticket)
        {
            if (ticket==null)
            {
                return BadRequest();
            }
            _context.TicketItems.Add(ticket);
            _context.SaveChanges();

            //return "/GetTicket/" + ticket.id
            return CreatedAtRoute("GetTicket", new { id = ticket.Id },ticket);
        }
    }
}