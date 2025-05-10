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

            var lines = input.Matrix.Split('\n', '\r')
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Trim())
                .ToList();

            var evek = lines[0].Split(',').Select(ev => ev.Trim()).ToList();
            var haviDijak = new Dictionary<string, List<double>>();
            var fogyasztasAdatok = new Dictionary<string, List<double>>();

            foreach (var ev in evek)
            {
                haviDijak[ev] = new List<double>();
                fogyasztasAdatok[ev] = new List<double>();
            }

            // Bemeneti fogyasztási adatok szétosztása
            for (int i = 1; i < lines.Count; i++)
            {
                var sor = lines[i].Split(',').Select(s => double.Parse(s.Trim(), CultureInfo.InvariantCulture)).ToList();
                for (int j = 0; j < sor.Count; j++)
                {
                    string ev = evek[j];
                    double fogyasztas = sor[j];
                    fogyasztasAdatok[ev].Add(fogyasztas);
                }
            }

            
