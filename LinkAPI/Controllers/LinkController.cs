using System.Security.Claims;
using LinkAPI.Dto.Link;
using LinkAPI.Interfaces;
using LinkAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace LinkAPI.Controllers
{
    [Route("links")]
    [ApiController]
    public class LinkController(IUnitOfWork unitOfWork, IUrlShortener urlShortener) : ControllerBase
    {
        [HttpGet, AllowAnonymous]
        [EnableCors("default")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Link>))]
        public IActionResult GetAll()
        {
            var links = unitOfWork.LinkRepository.GetAll();
            
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            
            return Ok(links);
        }

        [HttpGet("{id:long}")]
        [EnableCors("default")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Link>))]
        public IActionResult Get(long id)
        {
            var link = unitOfWork.LinkRepository.Get(id);
            
            if (link == null)
            {
                return NotFound( new { success = false, message = "Item not found" } );
            }
            
            return Ok(link);
        }

        [HttpPost, Authorize]
        [EnableCors("default")]
        [ProducesResponseType(201, Type = typeof(Link))]
        public IActionResult Create([FromBody]LinkDto link)
        {
            if (!Uri.TryCreate(link.Url, UriKind.Absolute, out _))
            {
                return BadRequest(new { success = false, message = "An invalid url has been provided" });
            }

            if (unitOfWork.LinkRepository.IsUrlExists(link.Url))
            {
                return BadRequest(new{ success = false, message = "Url already exists" });
            }

            var currentUser = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
            
            if (string.IsNullOrEmpty(currentUser))
            {
                return Unauthorized();
            }

            var shortUrl = new Link()
            {
                OriginLink = link.Url,
                ShortLink = urlShortener.GetShortUrl(HttpContext),
                Date = DateTime.Now,
                CreatedBy = currentUser
            };
            
            unitOfWork.LinkRepository.Create(shortUrl);
            
            unitOfWork.Save();
            
            return Ok(shortUrl);
        }

        [HttpDelete("{id:long}")]
        [EnableCors("default")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult Delete(long id)
        {
            var isDeleted = unitOfWork.LinkRepository.Delete(id);
            
            unitOfWork.Save();
            
            if (!isDeleted)
            {
                return BadRequest(new{ success = false, message = "Item not found" });
            }
            
            return Ok(new{ success = true, message = "Successfully deleted" });
        }

        [HttpGet("{shortUrlCode}")]
        [EnableCors("default")]
        [ProducesResponseType(200, Type = typeof(void))]
        public IActionResult RedirectByShortRepresentation([FromRoute] string shortUrlCode)
        {
            var url = unitOfWork.LinkRepository.GetAll().FirstOrDefault(link => link.ShortLink.Equals(shortUrlCode));

            if (url == null)
            {
                return NotFound(new { success = false, message = "Item not found" });
            }

            var originalUrl = url.OriginLink;

            return Redirect(originalUrl);
        }
    }
}
