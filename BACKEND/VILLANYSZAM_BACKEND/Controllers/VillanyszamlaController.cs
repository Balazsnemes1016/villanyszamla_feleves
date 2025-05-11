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

            // Első körben kiszámoljuk a havi díjakat kedvezmény nélkül
            foreach (var ev in evek)
            {
                foreach (var fogyasztas in fogyasztasAdatok[ev])
                {
                    double dij = fogyasztas * input.UnitPrice + rendszerDij;
                    haviDijak[ev].Add(dij);
                }
            }

            // Éves díjak kiszámítása
            var evesDijak = new Dictionary<string, double>();
            foreach (var ev in evek)
            {
                evesDijak[ev] = haviDijak[ev].Sum();
            }

            // Kedvezmény meghatározása
            var kedvezmenyesEvek = new List<int>();
            for (int i = 0; i < evek.Count - 2; i++)
            {
                string ev1 = evek[i];
                string ev2 = evek[i + 1];
                string ev3 = evek[i + 2];

                if (evesDijak[ev1] > 350000 && evesDijak[ev2] > 350000)
                {
                    kedvezmenyesEvek.Add(int.Parse(ev3));

                    // Újraszámolás 13% kedvezménnyel
                    haviDijak[ev3].Clear();
                    foreach (var fogyasztas in fogyasztasAdatok[ev3])
                    {
                        double kedvDij = (fogyasztas * input.UnitPrice + rendszerDij) * 0.87;
                        haviDijak[ev3].Add(kedvDij);
                    }

                    // Újra kiszámoljuk az évi összegét
                    evesDijak[ev3] = haviDijak[ev3].Sum();
                }
            }
            
            return Ok(new SzamitasValasz
            {
                HaviDijak = haviDijak,
                EvesDijak = evesDijak,
                KedvezmenyesEvek = kedvezmenyesEvek
            });
        }
    }
}

