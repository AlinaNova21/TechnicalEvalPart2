using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Http.Description;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalEvalPart2;
using TechnicalEvalPart2.DTOS;
using TechnicalEvalPart2.Models;

namespace TechnicalEvalPart2.Controllers
{
    [Route("api/[controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly AddressBookContext _context;

        public ContactsController(AddressBookContext context)
        {
            _context = context;
        }

        private static ContactDTO ContactToDTO(Contact contact)
        {
            return new ContactDTO
            {
                ContactId = contact.ContactId,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Street = contact.Street,
                City = contact.City,
                State = contact.State,
                Zip = contact.Zip,
                HomePhone = contact.HomePhone,
                MobilePhone = contact.MobilePhone,
                WorkPhone = contact.WorkPhone,
                Email = contact.Email
            };
        }
        private static Contact ContactFromDTO(ContactDTO contactDto)
        {
            return new Contact
            {
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                Street = contactDto.Street,
                City = contactDto.City,
                State = contactDto.State,
                Zip = contactDto.Zip,
                HomePhone = contactDto.HomePhone,
                MobilePhone = contactDto.MobilePhone,
                WorkPhone = contactDto.WorkPhone,
                Email = contactDto.Email
            };
        }

        private static readonly Expression<Func<Contact, ContactDTO>> AsContactDto = x => ContactToDTO(x);

        // GET: api/Contacts
        [HttpGet]
        public IQueryable<ContactDTO> GetContacts(string searchString)
        {
            IQueryable<Contact> contacts = _context.Contacts;
            if (!String.IsNullOrEmpty(searchString))
            {
                contacts = contacts.Where(c => c.FirstName.ToUpper().Contains(searchString.ToUpper()) || c.LastName.ToUpper().Contains(searchString.ToUpper()));
            }
            return contacts.Select(AsContactDto);
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        [ResponseType(typeof(ContactDTO))]
        public async Task<ActionResult<ContactDTO>> GetContact(int id)
        {
            ContactDTO contact = await _context.Contacts
                   .Where(c => c.ContactId == id)
                   .Select(AsContactDto)
                   .FirstOrDefaultAsync();

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(int id, Contact contact)
        {
            if (id != contact.ContactId)
            {
                return BadRequest();
            }

            _context.Entry(contact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
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

        // POST: api/Contacts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ContactDTO>> PostContact(ContactDTO contactDTO)
        {
            Contact contact = ContactFromDTO(contactDTO);

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContact", new { id = contact.ContactId }, ContactToDTO(contact));
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteContact(int id)
        {
            Contact contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.ContactId == id);
        }
    }
}
