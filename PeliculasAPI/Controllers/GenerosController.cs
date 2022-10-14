using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DTOs;
using PeliculasAPI.Entidades;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GenerosController:ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;

        public GenerosController(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get()
        {
            var entidades = await _context.Generos.ToListAsync();
            var dtos = mapper.Map<List<GeneroDTO>>(entidades);
            return dtos;
        }

        [HttpGet ("{id:int}", Name = "ObtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
            var entidad = await _context.Generos.FirstOrDefaultAsync(x=> x.Id == id);
            if(entidad == null)
            {
                return NotFound("No se encontro el genero");
            }
            var dtos = mapper.Map<GeneroDTO>(entidad);
            return dtos;
        }
        [HttpPost]
        public async Task<ActionResult>Post([FromBody] GeneroCreacionDTO generoCreacion) 
        { 
            var entidad = mapper.Map<Genero>(generoCreacion);
            _context.Add(entidad);
            await _context.SaveChangesAsync();
            var generoDto= mapper.Map<GeneroDTO>(entidad);

            return new CreatedAtRouteResult("ObtenerGenero", new { id = generoDto.Id }, generoDto);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put (int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var entidad = mapper.Map<Genero>(generoCreacionDTO);
            entidad.Id = id;
            _context.Entry(entidad).State= EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entidad = await _context.Generos.AnyAsync(x => x.Id == id);
            if (!entidad)
            {
                return NotFound();
            }
            _context.Remove(new Genero() { Id = id });
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
