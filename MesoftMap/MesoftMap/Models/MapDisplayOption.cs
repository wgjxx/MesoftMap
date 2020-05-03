using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MesoftMap.Models
{
    public class MapDisplayOption
    {
        public bool IsDisplayRegions { get; set; }
        
        public bool DisplayLatLng { get; set; }
        public bool IsDisplayAllMarkers { get; set; }
        public bool IsSetBoundary { get; set; }
        public bool CanSetRegionColor { get; set; }
        public string Position_TopOfMapClassDisplayOption { get; set; }
    }
}
