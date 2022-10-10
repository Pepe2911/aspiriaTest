using ApiAspiria.Models;
using ApiAspiria.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ApiAspiria.Controllers
{
    [EnableCors("EnableCors")]
    [ApiController]
    public class JuguetesController : ControllerBase
    {
        private readonly IJugueteRepository _jugueteRepository;

        public JuguetesController(IJugueteRepository jugueteRepository)
        {
            _jugueteRepository = jugueteRepository;
        }
        [Route("api/Juguetes/GetJuguetes")]
        [HttpGet]
        public async Task<IActionResult> GetJuguetes()
        {
            var juguetes = await _jugueteRepository.Get();
            return Ok(juguetes);
        }
        [Route("api/Juguetes/SaveToy")]
        [HttpPost]
        public async Task<IActionResult> SaveToy(Juguete data)
        {
            if(data.ID == 0)
            {
               await _jugueteRepository.CreateAndSave(data);
            }
            else
            {
                await _jugueteRepository.UpdateAndSave(data);
            }
            return Ok();
        }
        [Route("api/Juguetes/DeleteToy")]
        [HttpPost]
        public async Task<IActionResult> DeleteToy(Juguete data)
        {
            if (data.ID == 0)
            {
                return BadRequest();
            }
            else
            {
                await _jugueteRepository.DeleteAndSave(data);
            }
            return Ok();
        }

    }
}
