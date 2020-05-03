using Mesoft.Helpers.Interfaces;
using Mesoft.MapAPP.Interface;
using MesoftMap.DataContext;
using MesoftMap.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mesoft.MapAPP.Services
{
    public class MapService : IMapService
    {
        private MeEPADBContext _db;
        public MapService(MeEPADBContext context)
        {
            _db = context;
        }
        public MeEPADBContext _DB { get => _db; }

        public WebApiReturnCode AddRegion(MapRegion region)
        {
            if (region == null)
            {
                return new WebApiReturnCode { CodeId = -1, ErrMessage = "region对象不能为空！" };
            }
            if (region.ID > 0)
            {
                var item = _DB.MapRegions.Find(region.ID);
                if (null != item)
                {
                    if (_db.MapRegions.Any(item => item.ID != region.ID && item.RegionName.Equals(region.RegionName)))
                        return new WebApiReturnCode { CodeId = -1, ErrMessage = "已存在相同的轮廓名称，请换一个新名称！" };
                    else
                    {
                        var oldPoints = _DB.RegionPathPoints.Where(item => item.RegionID == region.ID).ToList();
                        _DB.RegionPathPoints.RemoveRange(oldPoints);
                        _DB.SaveChanges();

                        item.RegionName = region.RegionName;
                        item.PolygonStrokeDashStyle = item.PolygonStrokeDashStyle;
                        _DB.Entry(item).State = EntityState.Modified;
                        
                        
                        _DB.SaveChanges();
                        return new WebApiReturnCode { CodeId = region.ID, ErrMessage = "ok" };
                    }
                }

            }

            if (_db.MapRegions.Any(item => item.RegionName.Equals(region.RegionName)))
            {
                return new WebApiReturnCode { CodeId = -1, ErrMessage = "已存在相同的轮廓名称，请换一个新名称！" };
            }
            region.ID = 0;
            _DB.MapRegions.Add(region);
            int iResult = _DB.SaveChanges();
            if (iResult > 0)
            {
               
                iResult = iResult + _db.SaveChanges();
            }
            return new WebApiReturnCode { CodeId = region.ID, ErrMessage = "ok" };
        }
        public WebApiReturnCode AddRegionPoints(List<RegionPathPoint> points)
        {
            if (points == null)
            {
                return new WebApiReturnCode { CodeId = -1, ErrMessage = "points不能为空" };
            }
            int iResult = 0;
            for(int i=0; i<points.Count; i++)
            {
                RegionPathPoint point = new RegionPathPoint { Lat = points[i].Lat, Lng = points[i].Lng, RegionID = points[i].RegionID };
                _DB.RegionPathPoints.Add(point);
                iResult+=_DB.SaveChanges();
            }
            //_DB.RegionPathPoints.AddRange(points);
            //int iResult = _DB.SaveChanges();
            return new WebApiReturnCode { CodeId = iResult, ErrMessage = "ok" };
        }
        public WebApiReturnCode AddRegion(MapRegion region, List<RegionPathPoint> points)
        {
            if (region == null)
            {
                return new WebApiReturnCode { CodeId = -1, ErrMessage = "region对象不能为空！" };
            }
            if (region.ID > 0)
            {
                var item = _DB.MapRegions.Find(region.ID);
                if (null != item)
                {
                    if (_db.MapRegions.Any(item => item.ID != region.ID && item.RegionName.Equals(region.RegionName)))
                        return new WebApiReturnCode { CodeId = -1, ErrMessage = "已存在相同的轮廓名称，请换一个新名称！" };
                    else
                    {
                        if (!region.RegionName.Equals(item.RegionName))
                        {
                            item.RegionName = region.RegionName;
                            item.PolygonStrokeDashStyle = item.PolygonStrokeDashStyle;
                            _DB.Entry(item).State = EntityState.Modified;
                        }
                        
                        var oldPoints = _DB.RegionPathPoints.Where(item => item.RegionID == region.ID).ToList();
                        if (oldPoints.Count > points.Count)
                        {
                            _DB.RegionPathPoints.RemoveRange(oldPoints.GetRange(points.Count, oldPoints.Count - points.Count));
                            oldPoints = oldPoints.GetRange(0, points.Count);
                            
                        }
                        for( int i=0; i < oldPoints.Count; i++)
                        {
                            var point = oldPoints[i];
                            
                            point.Lat = points[i].Lat;
                            point.Lng = points[i].Lng;
                            _DB.Entry(point).State = EntityState.Modified;
                        }
                        _DB.SaveChanges();
                        if (points.Count > oldPoints.Count)
                        {
                            points = points.GetRange(oldPoints.Count, points.Count - oldPoints.Count);
                            points.ForEach(item => item.RegionID = region.ID);
                            AddRegionPoints(points);
                        }                        
                        return new WebApiReturnCode { CodeId = region.ID, ErrMessage = "ok" };
                    }
                }

            }

            if (_db.MapRegions.Any(item => item.RegionName.Equals(region.RegionName)))
            {
                return new WebApiReturnCode { CodeId = -1, ErrMessage = "已存在相同的轮廓名称，请换一个新名称！" };
            }
            region.ID = 0;
            _DB.MapRegions.Add(region);
            int iResult = _DB.SaveChanges();
            if (iResult > 0)
            {
                points.ForEach(item => item.RegionID = region.ID);
                AddRegionPoints(points);
            }
            return new WebApiReturnCode { CodeId = region.ID, ErrMessage = "ok" };
        }

        

        public IEnumerable<MapInfo> GetAllMapInfoes()
        {
            var items = this._DB.Set<MapInfo>().ToList();
            return items;
        }
        /// <summary>
        /// 目前只获取到允许显示的区域
        /// </summary>
        /// <returns></returns>
        public IEnumerable<V_MapRegion> GetAllRegions()
        {
            var regions = _db.MapRegions.Where(item => item.CanBeDisplay).ToList();
            List<V_MapRegion> items = new List<V_MapRegion>();
            foreach (var region in regions)
            {
                var points = _db.RegionPathPoints.Where(i => i.RegionID == region.ID).ToList();
                items.Add(new V_MapRegion
                {
                    RegionName = region.RegionName,
                    FillColor=region.FillColor,
                    StrokeColor=region.StrokeColor,
                    Points = points
                });

            }
            return items;

        }

        public WebApiReturnCode GetRegion(int id)
        {
            throw new NotImplementedException();
        }

        public WebApiReturnCode RemoveRegion(int id)
        {
            var item = _db.MapRegions.Find(id);
            if (item == null)
            {
                return new WebApiReturnCode { CodeId = -1, ErrMessage = "未找到记录", Data = item };
            }
            if (!item.CanBeDelete())
            {
                return new WebApiReturnCode { CodeId = -1, ErrMessage = "该记录不允许删除！", Data = item };
            }
            _db.Remove(item);
            int iResult = _db.SaveChanges();
            return new WebApiReturnCode { CodeId = iResult, ErrMessage = "ok", Data = item };
        }

        public WebApiReturnCode SaveRegionColor(int regionID, string fillColor, string strokeColor)
        {
            var item = _DB.MapRegions.Find(regionID);
            if (null == item)
            {
                return new WebApiReturnCode { CodeId = -1, ErrMessage = "未找到区域，可能该区域已被删除！" };
                

            }
            item.FillColor = fillColor;
            item.StrokeColor = strokeColor;
            _DB.Entry(item).State = EntityState.Modified;
            _DB.SaveChanges();
            return new WebApiReturnCode { CodeId = item.ID, ErrMessage = "ok" };
        }
    }
}
