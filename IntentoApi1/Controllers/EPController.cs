using IntentoApi1.Datos;
using IntentoApi1.Models;
using IntentoApi1.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace IntentoApi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EPController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult<IEnumerable<EPDto>> GetEPDto()
        {
            return Ok(EPStore.EPList);
        }

        [HttpGet("Id")]

        public EPDto GetEPDto(int Id)
        {
            return EPStore.EPList.FirstOrDefault(a => a.Id == Id);
        }

        [HttpGet("PruebaId2", Name = "GetEP")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<EPDto> GetEpDto(int Id)
        {
            if (Id == 0)
            {
                return BadRequest();
            }

            var Ep = EPStore.EPList.FirstOrDefault(a => a.Id == Id);

            if (Ep == null)
            {
                return NotFound();
            }

            return Ok(Ep);
        }

     [HttpPost]
     [ProducesResponseType(StatusCodes.Status201Created)]
     [ProducesResponseType(StatusCodes.Status400BadRequest)]
     [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<EPDto> CrearEPDto([FromBody] EPDto epdto)
        {

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(EPStore.EPList.FirstOrDefault(v => v.Nombre.ToLower() == epdto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "Ese nombre ya existe");
                return BadRequest(ModelState);
            }

            if (epdto == null)
            {
                return BadRequest(epdto);
            }
            if (epdto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            epdto.Id = EPStore.EPList.OrderByDescending(e => e.Id).FirstOrDefault().Id + 1;
            EPStore.EPList.Add(epdto);
            return CreatedAtRoute("GetEP", new { id = epdto.Id }, epdto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult DeleteEp(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var ep = EPStore.EPList.FirstOrDefault(v => v.Id == id);

            if (ep == null)
            {
                return NotFound();
            }
            EPStore.EPList.Remove(ep);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult UpdateEP(int id, [FromBody] EPDto epdto)
        {
            if (epdto == null)
            {
                return BadRequest();
            }
            var ep = EPStore.EPList.FirstOrDefault(v => v.Id == id);
            ep.Nombre = epdto.Nombre;
            ep.Ocupantes = epdto.Ocupantes;
            ep.MetrosCuadrados = epdto.MetrosCuadrados;
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult UpdateEP2(int id, JsonPatchDocument<EPDto> patchdto)
        {
            if (patchdto == null || id == 0)
            {
                return BadRequest();
            }
            var ep = EPStore.EPList.FirstOrDefault(v => v.Id == id);
            patchdto.ApplyTo(ep, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
