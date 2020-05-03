using Mesoft.EF.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MesoftMap.Models
{
    [Table("C_Maps")]
    public class MapInfo : BaseModel
    {
        public override string GetTableName()
        {
            return "C_Maps";            
        }

        public virtual string City { get; set; }
        public virtual string Region { get; set; }
        public virtual string Name { get; set; }
        public virtual string EnterpriseID { get; set; }
        public virtual string Address { get; set; }
        public virtual double Lon { get; set; }
        public virtual double Lat { get; set; }
        public virtual string Industry { get; set; }
        public virtual string licence { get; set; }
        public virtual string DangerNo { get; set; }
        public virtual double? DangerSum { get; set; }
        public string DangerKind { get; set; }
        public virtual string MonitorMemo { get; set; }
        public virtual string Memo { get; set; }

        public virtual int MapClassID { get; set; }
        public virtual double? MarkColor { get; set; }
        public virtual string MarkShape { get; set; }
        public virtual double? MarkSize { get; set; }
        public virtual bool? IsTwinkle { get; set; }


        public virtual string MainMaterial { get; set; }
        public virtual double? MaxDischarged { get; set; }
        public virtual double? AddDischarged { get; set; }
        public virtual double? SubDischarged { get; set; }
        public virtual double? SumDischarged { get; set; }
    }
}
