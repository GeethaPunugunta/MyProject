using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using HtmlAgilityPack;

namespace UrlContentParser.Services
{
    /// <summary>
    /// Summary description for ImageGalleryService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ImageGalleryService : WebService
    {
        /// <summary>
        /// Returns list of image urls
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [WebMethod(Description = "GetImagesList")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]  
        public List<string> GetImagesList(string input)
        {
            try
            {
                using (var client = new WebClient())
                {
                    var source = client.DownloadString(input);
                    var document = new HtmlDocument();
                    document.LoadHtml(source);

                    //Read all image source tags from the html documents
                    var imageSources = (from a in document.DocumentNode.Descendants("img")
                                        where a.Attributes["src"] != null
                                        select new
                                        {
                                            attr = a.Attributes["src"]
                                        }).ToList();

                    var imageList = new List<string>();

                    //regex to valid if img source has url information
                    var urlValidation = new Regex("^([a-z]+://|//)");

                    foreach (var link in imageSources)
                    {

                        imageList.Add((urlValidation.IsMatch(link.attr.Value))
                            ? link.attr.Value
                            : string.Format("{0}{1}", input, link.attr.Value));

                    }

                    return imageList;

                }

            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
         
        }
    }
}
