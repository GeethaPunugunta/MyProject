using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UrlContentParser.Helpers
{
    public static class Extensions
    {//
        public static Uri ToUri(this string s)
        {
            return new UriBuilder(s).Uri;
        }
    }
}


















