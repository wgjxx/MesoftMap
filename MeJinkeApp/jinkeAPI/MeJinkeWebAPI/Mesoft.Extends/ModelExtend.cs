
//using Mesoft.Libraries.Attributes;
using Mesoft.Libraries.IDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mesoft.Extends
{
    public static class ModelExtend
    {
        public static string GetColumn<T>(this T p) where T : PropertyInfo
        {
            if (p.IsDefined(typeof(ColumnAttribute)))
            {
                ColumnAttribute attribute = (ColumnAttribute)p.GetCustomAttribute(typeof(ColumnAttribute), true);
                return attribute.Name;
            }

            return p.Name;
        }

    }
}
