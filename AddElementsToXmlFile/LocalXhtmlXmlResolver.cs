using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Web.Http;
using System.Xml.Schema;



//Error	1	Elements defined in a namespace cannot be explicitly declared as private, protected, or protected internal	C:\Users\Aribeiro\Documents\Visual Studio 2010\Projects\AutToc\AutToc\AutToc\LocalXhtmlXmlResolver.cs	19	11	AutToc

//resolve entities
namespace AddElementsToXmlFile
{


    public class XmlResolver : XmlUrlResolver
    {

        //private static bool isValid = true;

        private static readonly Dictionary<string, Uri> KnownUris = new Dictionary<string, Uri>
        {
            { "-//W3C//DTD XHTML 1.0 Strict//EN", new Uri("http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd") },
            { "-//W3C XHTML 1.0 Transitional//EN", new Uri("http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd") },
            { "-//W3C//DTD XHTML 1.0 Transitional//EN", new Uri("http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd") },
            { "-//W3C XHTML 1.0 Frameset//EN", new Uri("http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd") },
            { "-//W3C//DTD XHTML 1.1//EN", new Uri("http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd") },
            { "-//W3C//ENTITIES Symbols for XHTML//EN", new Uri("http://www.w3.org/MarkUp/DTD/xhtml-lat1.ent") },
            { "-//W3C//DTD HTML 4.01//EN", new Uri(" http://www.w3.org/TR/html4/strict.dtd")},
                    
        };


        public static void ValidationHandler(object sender, ValidationEventArgs args)
        {
            Console.WriteLine("***Validation error");
            Console.WriteLine("\tSeverity:{0}", args.Severity);
            Console.WriteLine("\tMessage  :{0}", args.Message);
        }

        private bool enableHttpCaching;
        private System.Net.ICredentials credentials;

        public XmlResolver(bool enableHttpCaching)
        {
            this.enableHttpCaching = enableHttpCaching;
        }

        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {


            //   Debug.WriteLineIf(!KnownUris.ContainsKey(relativeUri), "Could not find: " + relativeUri);

            return KnownUris.ContainsKey(relativeUri) ? KnownUris[relativeUri] : base.ResolveUri(baseUri, relativeUri);

        }


        private Uri ResolverUri()
        {
            throw new NotImplementedException("base.ResolveUri(baseUri, relativeUri");
        }

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if (absoluteUri == null)
            {
                throw new ArgumentNullException();
            }

            //resolve resources from cache (if possible)
            if (absoluteUri.Scheme == "http" && this.enableHttpCaching && (ofObjectToReturn == null || ofObjectToReturn == typeof(Stream)))
            {
                var request = System.Net.WebRequest.Create(absoluteUri);

                request.CachePolicy = new System.Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.Default);

                if (this.credentials != null)
                {
                    request.Credentials = this.credentials;
                }

                var response = request.GetResponse();

                return response.GetResponseStream();
            }
            //otherwise use the default behavior of the XmlUrlResolver class (resolve resources from source)
            return base.GetEntity(absoluteUri, role, ofObjectToReturn);
        }

    }

}

