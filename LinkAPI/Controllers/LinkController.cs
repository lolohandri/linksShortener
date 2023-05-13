using LinkAPI.Context;
using LinkAPI.Dto.Link;
using LinkAPI.Interfaces;
using LinkAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Net;

namespace LinkAPI.Controllers
{
    [Route("api/links")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IUrlShortener _urlShortener;
        public LinkController(IUnitOfWork unitOfWork, IUrlShortener urlShortener)
        {
            _unitOfWork = unitOfWork;
            _urlShortener = urlShortener;
        }

        [HttpGet, AllowAnonymous]
        [EnableCors("default")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Link>))]
        public IActionResult GetAll()
        {
            var links = _unitOfWork.LinkRepository.GetAll();
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
            var link = _unitOfWork.LinkRepository.Get(id);
            if (link == null || !ModelState.IsValid)
            {
                return BadRequest("Item not found");
            }
            return Ok(link);
        }

        [HttpPost, Authorize]
        [EnableCors("default")]
        [ProducesResponseType(201, Type = typeof(Link))]
        public IActionResult Create([FromBody]LinkDto link)
        {
            //validating the input url
            if (!Uri.TryCreate(link.Url, UriKind.Absolute, out var inputLink))
            {
                return BadRequest("Invalid url has been provided");
            }

            if (_unitOfWork.LinkRepository.IsUrlExists(link.Url))
            {
                return BadRequest("Url already exists");
            }


            var shortUrl = new Link()
            {
                OriginLink = link.Url,
                ShortLink = _urlShortener.GetShortUrl(HttpContext),
                Date = DateTime.Now,
            };
            _unitOfWork.LinkRepository.Create(shortUrl);
            _unitOfWork.Save();

            
            return Ok(shortUrl);

        }

        [HttpDelete("{id:long}")]
        [EnableCors("default")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult Delete(long id)
        {
            var check = _unitOfWork.LinkRepository.Delete(id);
            _unitOfWork.Save();
            if (!check)
            {
                return BadRequest("Item not found");
            }
            return Ok("Succesfully deleted");
        }
    }
}
