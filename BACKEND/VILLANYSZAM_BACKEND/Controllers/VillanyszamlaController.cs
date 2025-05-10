using Microsoft.AspNetCore.Mvc;
using VillanyszamlaBackend.Models;
using System.Collections.Generic;
using System.Globalization;

namespace VillanyszamlaBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VillanyszamlaController : ControllerBase
    {
        [HttpPost("szamitas")]
        public ActionResult<SzamitasValasz> Szamitas([FromBody] SzamitasInput input)
        {
            const double rendszerDij = 23.4;

           

