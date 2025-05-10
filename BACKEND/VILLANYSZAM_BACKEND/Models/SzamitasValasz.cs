using System.Collections.Generic;

namespace VillanyszamlaBackend.Models
{
    public class SzamitasValasz
    {
        public Dictionary<string, List<double>> HaviDijak { get; set; }
        public Dictionary<string, double> EvesDijak { get; set; }
        public List<int> KedvezmenyesEvek { get; set; }
    }
}
