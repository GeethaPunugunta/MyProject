using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HtmlAgilityPack;
using UrlContentParser.Helpers;

namespace UrlContentParser
{
    public partial class Home : System.Web.UI.Page
    {

        #region events
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Text = "";
            if (!IsPostBack)
            {
                txtBoxUrl.Text = string.Empty;

            }

        }

        protected void btnGetData_OnClick(object sender, EventArgs e)
        {
            try
            {
                //Add http to URL if missing
                var url = txtBoxUrl.Text.ToUri();

                //Invoke a webservice call on document ready to render images.
                Page.ClientScript.RegisterStartupScript(GetType(), "startup",
                                                              string.Format(
                                                                  "<script type=text/javascript> $(document).ready(function () {{ GetImagesList('{0}', 0);  }});</script>",
                                                                  url.AbsoluteUri));
                
                using (var client = new WebClient())
                {
                    var source = client.DownloadString(url.AbsoluteUri);
                    var document = new HtmlDocument();
                    document.LoadHtml(source);

                    //get document contents as string
                    //exclude sciprt, style and head tags.
                    var input = document.DocumentNode.Descendants().Where(x =>
                                x.NodeType == HtmlNodeType.Text &&
                                x.ParentNode.Name != "script" &&
                                x.ParentNode.Name != "style" &&
                                x.ParentNode.Name != "head")
                                .Aggregate(string.Empty, (current, node) => current + node.InnerText);

                    BuildDataObjects(input);
                }
            }
            catch (Exception ex)
            {
                //TODO:Add exception logging code here.
                lblErrorMessage.Text = ex.Message;
            }

        

        }
        #endregion


        #region Helper Methods
        /// <summary>
        /// Formats the input string and displays information in Chart 
        /// with Top 10 word occurences and Table with list of words
        /// </summary>
        /// <param name="input"></param>
        private void BuildDataObjects(string input)
        {
            try
            {
                var wordsList = StringSplitter(input);

                //group list of all words returned with number of occurences
                //order the list in descending order and take the top 10 records.
                //convert information into dictonary with "Word" as the key and "number of occurrences" as count
                var wordDictionary = (from word in wordsList
                                      group word by word into g
                                      select new
                                      {
                                          Word = g.Key,
                                          Count = g.Count()
                                      }).OrderByDescending(a => a.Count).Take(10).ToDictionary(a => a.Word, a => a.Count);


                lbltotCount.Text = string.Format("Total Number of Words={0}", wordsList.Count());

                BuildChart(wordDictionary);
                BuildTable(wordDictionary);
            }
            catch (Exception ex)
            {
               lblErrorMessage.Text = ex.Message;
            }
            
           

        }

        private void BuildTable(Dictionary<string, int> wordDictionary)
        {
            GVWords.DataSource = wordDictionary;
            GVWords.DataBind();
          
        }

        /// <summary>
        /// Returns list of words 
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private static List<string> StringSplitter(string inputString)
        {

                //Remove HTML tags from input string
                inputString = Regex.Replace(inputString, @"<[^>]+>|&nbsp;", "");

                var words = new List<string>();
                var wordBuilder = new StringBuilder();

                //reads each character from the string
                foreach (var inputChar in inputString)
                {
                    //Check if the Character is a letter
                    if (Char.IsLetter(inputChar))
                    {
                        //Append letter to string builder
                        wordBuilder.Append(inputChar);
                    }
                    else 
                    {
                        //If charchter is not a letter, add stringbuilder value to list and clear string builder.
                        words.Add(wordBuilder.ToString());
                        wordBuilder.Clear();
                    }

                }

                //exclude whitespaces and empty string 
                //convert the list to ToLower() and group
                return words.Where(s => !string.IsNullOrWhiteSpace(s)).Select(x => x.ToLower()).ToList();
            
           

            
        }

        /// <summary>
        /// Formats the input string and displays information in Chart 
        /// with Top 10 word occurences and Table with list of words
        /// </summary>
        /// <param name="input"></param>
        private void BuildChart(Dictionary<string, int> wordDictionary)
        {
            foreach (var word in wordDictionary)
            {
                chtWordCounts.Series[0].Points.AddXY(word.Key, Convert.ToInt32(word.Value));
                chtWordCounts.Series[0].XValueMember = "Word";
            }
        }

#endregion
    }
}