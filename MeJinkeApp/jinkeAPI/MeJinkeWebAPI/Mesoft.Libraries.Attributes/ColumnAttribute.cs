using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesoft.Libraries.Attributes
{
    public class MeColumnAttribute
        : Attribute
    {
        private string _ColumnName = null;
        public MeColumnAttribute(string columnName)
        {
            this._ColumnName = columnName;
        }
        public string GetColumnName()
        {
            return _ColumnName;
        }
    }
}
