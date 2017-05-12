using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MagicContact.Contact;
using System.Net;
using System.Web;
using System.Xml;

namespace MagicContact
{
    public class FullContact
    {
        public String apikey { get; }
        public String apiurl { get; }
        public String lastSearch { get; set; }
        private Contact.Contact contact = null;
        private String defaultFormat = "xml";
        private String saveHtmlFormat = "html";
        private String saveDirectoryName = "saved";
        private String apikeyheaderkey = "X-FullContact-APIKey";

        /*
         * Constructor.
         * It will read the URL and API KEY from the Configuration file.
         */
        public FullContact() {
            this.apikey = ConfigurationManager.AppSettings["api_key"];
            this.apiurl = ConfigurationManager.AppSettings["api_url_email"];
        }
        /**
         *  GET for contact
         * */
        public Contact.Contact getContact() {
            return this.contact;
        }

        /**
         * Lets find any information related to the email
         * */
        public bool lookByEmail( String email ){
            //prepare the URL
            String aux = this.apiurl;
            try
            {
                aux = aux.Replace("!FORMAT!", this.defaultFormat);
                aux = aux.Replace("!EMAIL!", System.Uri.EscapeDataString(email));
            }
            catch (Exception err) {
                Console.WriteLine(err.Message);
            }


            Common.Utilities.Request req = new Common.Utilities.Request(aux);
            req.addHeader( this.apikeyheaderkey , this.apikey);

           
            try{
                String result = req.getPlain() ;
                this.parseXMLString( result , email );
            }catch (Exception err) {
                String message = "The API URL could not be acceded. Please check your API KEY and access URL";
                System.Console.WriteLine( message );
                //throw new System.Exception( message );
                return false;
            }
            return true;
        }

        /**
         * It will make an aditional request to get the HTML of the contact
         * */
        public bool saveHTML(String email)
        {
            //prepare the URL
            String aux = this.apiurl;
            try
            {
                aux = aux.Replace("!FORMAT!", this.saveHtmlFormat);
                aux = aux.Replace("!EMAIL!", System.Uri.EscapeDataString(email));
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }

            Common.Utilities.Request req = new Common.Utilities.Request(aux);
            req.addHeader(this.apikeyheaderkey, this.apikey);
            String result = "";
            try
            {
                result = req.getPlain();
            }
            catch (Exception err)
            {
                String message = "The API URL could not be acceded. Please check your API KEY and access URL";
                System.Console.WriteLine(message);
                //throw new System.Exception( message );
                return false;
            }

            String newFileName = this.createFileStructureName( email );
            try
            {
                if ( !Common.Utilities.File.createFolder(this.saveDirectoryName) ) {
                    return false;
                }
                if ( !Common.Utilities.File.saveNewFileSimple(System.IO.Path.Combine(this.saveDirectoryName, newFileName + ".html"), result) ) {
                    return false;
                }
            }
            catch (Exception err) {
                String message = "The directory or the file could not be created. Please check permissions.";
                System.Console.WriteLine(message);
                //throw new System.Exception( message );
                return false;
            }
            return true;
        }

        private String createFileStructureName( String email) {
            email = email.Replace("@", "[AT]");
            email = email.Replace(".", "[DOT]");
            return email;
        }


        private void parseXMLString( String xmlstring , String email ) {

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlstring);

            XmlNode  status = doc.DocumentElement.SelectSingleNode("status");

            int status_integer = 0;

            if (!Int32.TryParse(status.InnerText, out status_integer))
            {
                String message = "Could not read or parse the Status";
                System.Console.WriteLine( message );
                throw new Exception( message );
            }
            /**
             * 200 indicates that the parse was successfull
             * */
            if ( status_integer != 200 ) {
                String message = "Status is different from 200 [" + status_integer.ToString() + "]";
                System.Console.WriteLine( message );
                throw new Exception( message );
            }

            /**
             * If it arrives here it means that we can store the information
             * http://stackoverflow.com/questions/642293/how-do-i-read-and-parse-an-xml-file-in-c
             * */
            this.contact = new Contact.Contact();
            try
            {
                // Lets get the basic contact information
                this.contact.email  = email;
                this.contact.likelihood 
                    = float.Parse(doc.DocumentElement.SelectSingleNode("likelihood").InnerText);
                this.contact.requestId 
                    = (doc.DocumentElement.SelectSingleNode("requestId").InnerText);
                this.contact.familyName 
                    = (doc.DocumentElement.SelectSingleNode("contactInfo").SelectSingleNode("familyName").InnerText );
                this.contact.fullName 
                    = (doc.DocumentElement.SelectSingleNode("contactInfo").SelectSingleNode("fullName").InnerText);
                this.contact.givenName 
                    = (doc.DocumentElement.SelectSingleNode("contactInfo").SelectSingleNode("givenName").InnerText);

                // Here we are going to get the social profiles information. We get all the nodes inside social profiles
                XmlNodeList socialNodes = doc.DocumentElement.SelectSingleNode("socialProfiles").SelectNodes("socialProfile");
                if(socialNodes.Count > 0) for (int i = 0; i < socialNodes.Count ; i++) {
                        XmlNode socialProfile = socialNodes.Item(i);
                        this.contact.addSocialProfile(socialProfile.SelectSingleNode("type").InnerText.ToString(), socialProfile.SelectSingleNode("url").InnerText.ToString() );
                }

                // Here we are going to get the photos information. We get all the nodes inside social profiles
                XmlNodeList photosNodes = doc.DocumentElement.SelectSingleNode("photos").SelectNodes("photo");
                if (photosNodes.Count > 0) for (int i = 0; i < photosNodes.Count; i++)
                    {
                        XmlNode photo = photosNodes.Item(i);
                        this.contact.addPhoto(photo.SelectSingleNode("type").InnerText.ToString(), photo.SelectSingleNode("url").InnerText.ToString());
                    }


                
                EngineSearch.Engines.Google GoogleEngine = new EngineSearch.Engines.Google();
                List<EngineSearch.Elements.ItemResult> google_results = GoogleEngine.search( email );

                foreach ( EngineSearch.Elements.ItemResult item in google_results ) {
                    this.contact.addEngineResult(item);
                }

            }
            catch (Exception err) {
                String message = "An error occurred parsing the contact information";
                System.Console.WriteLine( message );
            }
            

        }

    }
}
