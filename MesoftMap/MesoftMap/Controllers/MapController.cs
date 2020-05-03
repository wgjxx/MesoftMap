using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mesoft.Helpers.Interfaces;
using Mesoft.MapAPP.Interface;
using MesoftMap.Models;
using MesoftMap.Utility.WebHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MesoftMap.Controllers
{
    [Route("api/[controller]")]
    public class MapController : Controller
    {
        private readonly ILogger<MapController> _logger;
        IMapService _MapService;
        private IWebHostEnvironment hostingEnv;
        MapDisplayOption _MapDisplayOption;
        public MapController(IWebHostEnvironment env, ILogger<MapController> logger, IMapService mapService, IOptions<MapDisplayOption> mapDisplayOption)
        {
            this._logger = logger;
            this._MapService = mapService;
            this.hostingEnv = env;
            this._MapDisplayOption = mapDisplayOption.Value;
        }
        [HttpGet("[action]")]
        public MapDisplayOption GetMapDisplayOption()
        {
            return this._MapDisplayOption;
        }

        [HttpGet("[action]")]
        public WebApiReturnCode GetAllMapinfoes()
        {
            string imgFilePath = hostingEnv.WebRootPath + @"\images";

            var items = this._MapService.GetAllMapInfoes();
            var data = (from item in items
                        select new
                        {
                            item.ID,
                            item.Industry,
                            IsTwinkle = null != item.IsTwinkle ? item.IsTwinkle.Value : false,
                            isHaveImage = System.IO.File.Exists($"{imgFilePath}\\{item.MapClassID}\\{item.ID}.jpg"),
                            item.Lat,
                            item.Lon,
                            item.MapClassID,
                            item.MarkColor,
                            item.MarkShape,
                            item.MarkSize,
                            item.Memo,
                            item.MonitorMemo,
                            item.Name,
                            item.Address,
                            item.Region,
                            item.MainMaterial,
                            item.MaxDischarged, item.AddDischarged, item.SubDischarged, item.SumDischarged,
                            item.DangerKind, item.DangerNo, item.DangerSum

                        }).ToList();
            return new WebApiReturnCode { CodeId = items.Count(), ErrMessage = "ok", Data = data };
        }
        [HttpGet("[action]")]
        public WebApiReturnCode GetAllRegions()
        {
            var items = this._MapService.GetAllRegions();
            return new WebApiReturnCode { CodeId = items.Count(), ErrMessage = "ok", Data = items };
        }
        [HttpPost("[action]")]
        public WebApiReturnCode AddRegion([FromBody] MapRegion region)
        {
            //string jsonstring = CommonHelper.GetRequestBodyAsync(HttpContext.Request).Result;
            // = CommonHelper.DeserializeJsonToObject<V_MapRegion>(jsonstring, out string msg);

            //MapRegion region = new MapRegion() {ID = itemview.RegionID, RegionName = itemview.RegionName };
            //List<RegionPathPoint> points = itemview.Points;
            var ret = this._MapService.AddRegion(region);
            return ret;
        }
        [HttpPost("[action]")]
        public WebApiReturnCode UpdateRegionColor([FromBody] RegionColor rgColor)
        {
            WebApiReturnCode ret = this._MapService.SaveRegionColor(rgColor.RegionID, rgColor.FillColor, rgColor.StrokeColor);
            return ret;
        }
        [HttpPost("[action]")]
        public WebApiReturnCode AddRegionPoints([FromBody]  List<RegionPathPoint> points, int regionID)
        {
            //string jsonstring = CommonHelper.GetRequestBodyAsync(HttpContext.Request).Result;
            // = CommonHelper.DeserializeJsonToObject<V_MapRegion>(jsonstring, out string msg);
            points.ForEach(item => item.RegionID = regionID);
            var ret = this._MapService.AddRegionPoints(points);
            return ret;
        }
        [HttpPost("[action]")]
        public WebApiReturnCode AddRegionWithPoints([FromBody] V_MapRegion itemview)
        {
            MapRegion region = new MapRegion() { ID = itemview.RegionID, RegionName = itemview.RegionName };
            List<RegionPathPoint> points = itemview.Points;
            var ret = this._MapService.AddRegion(region, points);
            return ret;
        }

        [HttpGet("[action]")]
        public WebApiReturnCode DeleteRegion(int id)
        {
            return this._MapService.RemoveRegion(id);

        }
    }
}