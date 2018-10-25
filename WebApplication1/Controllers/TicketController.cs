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

        /* route  api/[Controller]/{id}
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

/*	route /api/[controller]/create								*/
        [HttpPost] 
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

	
	/* route /api/[controller]/Update/id									*/
        [HttpPut("{id}")]
        public IActionResult Update(long id,[FromBody] TicketItem ticket)
        {
            if (ticket == null || ticket.Id != id)
            {
                return BadRequest();
            }

            var _ticket = _context.TicketItems.FirstOrDefault(t => t.Id == id);

            if (_ticket ==null)
            {
                return NotFound();
            }

            _ticket.Concert = ticket.Concert;
            _ticket.Available = ticket.Available;
            _ticket.Artist = ticket.Artist;
            _context.TicketItems.Update(_ticket);

            _context.SaveChanges();
            return new NoContentResult();

        }


	/* route /api/[controller]/Delete/id									*/
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var ticket = _context.TicketItems.FirstOrDefault(t => t.Id == id);


            if  (ticket == null)
            {
                return NotFound();
            }

            _context.TicketItems.Remove(ticket);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}