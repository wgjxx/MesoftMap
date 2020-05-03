using Mesoft.Helpers.Interfaces;
using MesoftMap.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mesoft.MapAPP.Interface
{
    public interface IMapService
    {
        
        IEnumerable<MapInfo> GetAllMapInfoes();

        WebApiReturnCode AddRegion(MapRegion region, List<RegionPathPoint> points);
        WebApiReturnCode AddRegion(MapRegion region);
        WebApiReturnCode RemoveRegion(int id);
        IEnumerable<V_MapRegion> GetAllRegions();
        WebApiReturnCode GetRegion(int id);
        WebApiReturnCode AddRegionPoints(List<RegionPathPoint> points);
        WebApiReturnCode SaveRegionColor(int regionID, string fillColor, string strokeColor);
    }
}
