using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Interfaces;
using AutoMapper;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(owners);
        }

        [HttpGet("{OwnerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]

        public IActionResult GetOwner(int id)
        {
            if (!_ownerRepository.OwnerExists(id))
            {
                return NotFound();
            }
            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owner);
        }

        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]

        public IActionResult GetPokemonByOwnerId(int ownerId)
        {
            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            var owner = _mapper.Map<List<OwnerDto>>(
                _ownerRepository.GetPokemonByOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(owner);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateOwner([FromBody] OwnerDto ownerCreate)
        {
            if (ownerCreate == null)
            {
                return BadRequest(ModelState);
            }
            var owner = _ownerRepository.GetOwners()
                .Where(x => x.FirstName.Trim().ToUpper() == ownerCreate.FirstName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (owner != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ownerMap = _mapper.Map<Owner>(ownerCreate);

            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

    }
}
