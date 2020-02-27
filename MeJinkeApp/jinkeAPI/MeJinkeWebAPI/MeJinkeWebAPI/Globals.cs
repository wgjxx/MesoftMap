using MeJinkeWebAPI.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MeJinkeWebAPI
{
    public static class Globals
    {
        public readonly static MeSection Settings = (MeSection)WebConfigurationManager.GetSection("jinkeSetting");
    }

    
}