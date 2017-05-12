using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace EngineSearch.Engines
{
    public class Google : Engine
    {
        public Google() {
            this.setBaseUrl("https://www.google.com/search?q=[TERM]");
        }

        /**
         * We have to implement the search method
         * */
        public override List<EngineSearch.Elements.ItemResult> search(String term) {
            List<EngineSearch.Elements.ItemResult> results = new List<EngineSearch.Elements.ItemResult>();

            String final_url = this.getBaseUrl();
            final_url = final_url.Replace("[TERM]", System.Uri.EscapeDataString(term));
            String engine_result = "";
            try
            {
                engine_result = this.getSearchHtml(final_url);

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(engine_result);
                IEnumerable<HtmlNode> allLinks = doc.GetElementbyId("ires").Descendants("div")
                                                            .Where(d =>
                                                               d.Attributes.Contains("class")
                                                               &&
                                                               d.Attributes["class"].Value.Contains("g")
                                                            );
                int cont = 0;
                foreach (HtmlNode link in allLinks)
                {
                    HtmlNode h3 = link.SelectSingleNode("h3");
                    HtmlNode a = h3.SelectSingleNode("a");
                    // Checks whether the link contains an HREF attribute
                    if (a.Attributes.Contains("href"))
                    {
                        EngineSearch.Elements.ItemResult ir = new EngineSearch.Elements.ItemResult();
                        ir.url = a.Attributes["href"].Value;
                        ir.title = a.InnerText;
                        ir.source = "Google";
                        ir.importance = cont++;
                        results.Add( ir );
                    }
                }


            }
            catch (SystemException err) {
                throw new System.Exception( "Could not complete the search for Google Engine" , err );
            }
            
            /**
             * Now that we have the actual HTML string, lets analize the information
             * */

            return results;
        }
    }
}
