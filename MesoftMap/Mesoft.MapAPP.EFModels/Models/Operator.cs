using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MesoftMap.Models
{
    [Table("A_Operator")]
    public class Operator
    {
        [Key]
        public string ID { get; set; }
        public string Name { get; set; }
        public int DepartmentID { get; set; }
        public int? GroupID { get; set; }
        public string Password { get; set; }
        public bool IsForbid { get; set; }
        public int? AuditGroupID { get; set; }
        public string Memo { get; set; }
        public DateTime? RecTime { get; set; }
        public string Opera { get; set; }
    }

    [Table("A_Right")]
    public class OperatorRight
    {
        [Key]
        public int ID { get; set; }
        public int GroupID { get; set; }
        
        public int ClassID { get; set; }
        public int RightType { get; set; }
        public bool IsBrowse { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public bool IsDelete { get; set; }
        public bool IsPrint { get; set; }
        public bool IsCheck { get; set; }

        public bool IsDesign { get; set; }
        public bool IsOther { get; set; }
    }
}
