using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;

namespace Common.Utilities
{
    /**
     * The structure for the headers
     * */
    public struct RequestHeader {
        public string key, value;
    }

    /**
     Class Request
         */
    public class Request
    {
        /**
         * The URL we are going to read the information from
         * */
        private string url;

        /*
         The headers of the request
             */
        private List<RequestHeader> headers = null;

        public Request( String url ) {
            this.url = url;
            this.headers = new List<RequestHeader>();
        }


        public void addHeader( RequestHeader header ) {
            this.headers.Add(header);
        }

        public void addHeader(string key , string value) {
            RequestHeader r = new RequestHeader();
            r.key = key;
            r.value = value;
            this.headers.Add(r);
        }

        public string getPlain() {
            Uri uri = new Uri( this.url );
            WebClient wc = new WebClient();
            if ( this.headers.Count > 0 ) {
                foreach (RequestHeader header in this.headers) {
                    wc.Headers.Add( header.key, header.value );
                }
            }

            String result = "";
            try
            {
                result = wc.DownloadString(uri);
            }
            catch (Exception err)
            {
                String message = "The API URL could not be acceded. Please check your API KEY and access URL.";
                System.Console.WriteLine(message);
                throw new System.Exception( message , err );
                
            }
            return result;
        }

    }
}
