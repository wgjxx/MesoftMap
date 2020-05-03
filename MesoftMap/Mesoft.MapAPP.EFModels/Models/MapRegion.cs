using Mesoft.EF.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MesoftMap.Models
{
    /// <summary>
    /// 保存地图上轮廓信息（主要是轮廓名字以及点集）
    /// </summary>
    [Table("Map_Regions")]
    public class MapRegion : BaseModel
    {
        public MapRegion()
        {
            Rectime = DateTime.Now;
            CanBeDisplay = true;
        }
        public bool CanBeDelete()
        {
            return !this.IsSystem;
        }
        public override string GetTableName()
        {
            return "Map_Regions";            
        }        
        public virtual string RegionName { get; set; }
        public string FillColor { get; set; }   //区域填充颜色
        public string StrokeColor { get; set; }  //区域轮廓颜色
        public virtual string PolygonStrokeDashStyle { get; set; }
        public bool CanBeDisplay { get; set; }
        public DateTime Rectime { get; set; }
        public bool IsSystem { get; set; }
    }
    [Table("Map_RegionPathPoints")]
    public class RegionPathPoint : BaseModel
    {
        public override string GetTableName()
        {
            return "Map_RegionPathPoints";
        }
        /// <summary>
        /// 关联的区域表ID
        /// </summary>
        public int RegionID { get; set; }
        public virtual double Lat { get; set; }   //纬度
        public virtual double Lng { get; set; }   //经度
        
    }
    /// <summary>
    /// 用于向客户端返回使用
    /// </summary>
    public class V_MapRegion
    {
        public int RegionID { get; set; }
        public string RegionName { get; set; }
        public string FillColor { get; set; }
        public string StrokeColor { get; set; }  //区域轮廓颜色
        public List<RegionPathPoint> Points { get; set; }
    }
}
