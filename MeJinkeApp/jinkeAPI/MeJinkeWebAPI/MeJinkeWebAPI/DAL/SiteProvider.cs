using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// SiteProvider 的摘要说明
/// </summary>
/// 
namespace MeSoftOA.DAL
{

    public abstract class SiteProvider
    {

        public static JinkeSiteProvider JinkeProvider
        {
            get { return JinkeSiteProvider.Instance; }
        }

        public static CSSiteProvider CSProvider
        {
            get { return CSSiteProvider.Instance; }
        }
    }

}