﻿using Newtonsoft.Json.Linq;

namespace iMES.Bi.API
{
    class APIHelp
    {
    }


    public static class APIExtensions
    {
        public static string Request(this JObject JData, string strPro, string strDefault = null)
        {
            return JData[strPro] != null ? JData[strPro].ToString() : strDefault;

        }
    }
}
