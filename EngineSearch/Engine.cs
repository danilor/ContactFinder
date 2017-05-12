using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace EngineSearch
{
    abstract public class Engine
    {
        protected String base_url = "";


        protected Engine setBaseUrl( String base_url ) {
            this.base_url = base_url;
            return this;
        }

        protected String getBaseUrl()
        {
            return this.base_url;
        }

        protected String getSearchHtml( String url ) {
            Common.Utilities.Request req = new Common.Utilities.Request(url);
            
            String result = "";
            try
            {
               result =  req.getPlain();
            }
            catch (Exception err)
            {
                String message = "The API URL could not be acceded. Please check your API KEY and access URL";
                System.Console.WriteLine(message);
                throw new System.Exception( message );
            }

            return result;
        }

        /**
         *  THis will return a list of result items.
         */
        abstract public List<EngineSearch.Elements.ItemResult> search(String term);
    }
}
