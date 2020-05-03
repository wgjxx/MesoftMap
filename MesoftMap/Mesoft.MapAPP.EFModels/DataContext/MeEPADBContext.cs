using MesoftMap.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MesoftMap.DataContext
{
    public class MeEPADBContext : DbContext
    {
        public MeEPADBContext(DbContextOptions<MeEPADBContext> options) : base(options) { }


        public DbSet<Operator> Operators { get; set; } 
        public DbSet<OperatorRight> OperatorRights { get; set; }
        /// <summary>
        /// 地图信息数据表
        /// </summary>
        public DbSet<MapInfo> MapInfos { get; set; }

        public DbSet<MapRegion> MapRegions { get; set; }
        public DbSet<RegionPathPoint> RegionPathPoints { get; set; }
        
    }
}
