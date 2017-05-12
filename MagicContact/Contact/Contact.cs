using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicContact.Contact
{
    /**
     * This class represent the CONTACT information we are harvesting
     * */
    public class Contact
    {
        public String   requestId, fullName, givenName, familyName, email,
                        normalizedLocation, deducedLocation, country_name, 
                        country_code, continent, location_general;
        public float    likelihood, location_likelihood;
        public List<ContactPhoto> photos = new List<ContactPhoto>();
        public List<ContactWebsite> websites = new List<ContactWebsite>();
        public List<ContactSocialProfile> social_profiles= new List<ContactSocialProfile>();

        public List<EngineSearch.Elements.ItemResult> engine_results;

        public Contact addPhoto( String origin , String url ) {
            ContactPhoto photo = new ContactPhoto();
            photo.type      =   "photo";
            photo.typeId    =   origin;
            photo.typeName  =   origin;
            photo.url       =   url;
            this.photos.Add( photo );
            return this;
        }

        public Contact addWebsite( String origin , String url) {
            ContactWebsite web = new ContactWebsite();
            web.type        =   "web";
            web.typeId      =   origin;
            web.typeName    =   origin;
            web.url         =   url;
            this.websites.Add( web );
            return this;
        }

        public Contact addSocialProfile( String origin , String url) {
            ContactSocialProfile sp = new ContactSocialProfile();
            sp.type = "social";
            sp.typeId = origin;
            sp.typeName = origin;
            sp.url = url;
            this.social_profiles.Add( sp );
            return this;
        }

        /**
         * Adding a single search engine result to the list.
         * Each engine result has its "source" origin, so it will be easy to differenciate once they are being
         * print.
         * */
        public Contact addEngineResult(EngineSearch.Elements.ItemResult engine) {
            this.engine_results.Add( engine );
            return this;
        }

        public List<EngineSearch.Elements.ItemResult> getEngineResults() {
            /**
             * This first step is to order the list by importance, no matter of the engine they come from
             * */
            this.engine_results = this.engine_results.OrderBy(o=>o.importance).ToList();
            return this.engine_results;
        }
    }
}
