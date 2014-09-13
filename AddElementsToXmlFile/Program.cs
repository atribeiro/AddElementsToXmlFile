using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AddElementsToXmlFile
{
    class Program
    {

        public static string doc { get; set; }

        static void Main(string[] args)
        {

            string[] filePaths = Directory.GetFiles("filePath", "*.xml", SearchOption.AllDirectories);

            var pathDestination = "filePath";

            try
            {
                if (Directory.Exists(pathDestination))
                {
                    Console.WriteLine("That path exists already.");
                    
                }

                DirectoryInfo di = Directory.CreateDirectory(pathDestination);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(pathDestination));

            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e);
            }
                    
            foreach (var filePath in filePaths)
            {

                var output = File.ReadAllText(filePath);
               
                string newFile = pathDestination + Path.GetFileName(filePath);
                
                Console.WriteLine(newFile);
            
                XDocument document;

                    using (var stringReader = new StringReader(output))
                    {

                       var settings = new XmlReaderSettings
                            {

                                ProhibitDtd = false,
                                XmlResolver = new XmlResolver(false), //bool.Parse(ConfigurationManager.AppSettings["CacheDTDs"]
                                DtdProcessing = DtdProcessing.Parse
                            };

                        document = XDocument.Load(XmlReader.Create(stringReader, settings));
                        {

                            //add new Valuesitems to filename

                            string sourceID = GetSourceID(document);
                            
                            string filename = ElementValue(document);
                            var res22 = filename;
                            res22 = res22.Replace("[", "");
                            res22 = res22.Replace("]", " ");
                            String spaceLetDig = Regex.Replace(res22, "(([0-9])[A-Z])([0-9])", "$1 $2 $3", RegexOptions.IgnoreCase);
                          
                            res22 = sourceID + "#" + spaceLetDig.Replace(" ", "#");

                            Console.WriteLine(res22);

                            var uid = document.XPathSelectElements("/root//elem1").FirstOrDefault();
                            if (uid != null) uid.Value = (res22);

                            {
                                document.Save(filePath);

                            }

                            XDocument doc = XDocument.Load(filePath);

                            {
                                //add element empty elements
                                var elem = doc.Root.Element("elem");
                                elem.Element("nextElem").AddAfterSelf(new XElement("newElem", Path.GetFileNameWithoutExtension(filePath)));
                                elem.Element("newElem").AddAfterSelf(new XElement("date", ""));
                                elem.Element("date").AddFirst(new XElement("day", ""));
                                elem.Element("date").Element("day").AddAfterSelf(new XElement("month", ""));
                                elem.Element("date").Element("month").AddAfterSelf(new XElement("year", ""));
                                
                                var title = doc.XPathSelectElement("//title");
                                if (title == null)
                                {
                                  elem.Element("date").AddAfterSelf(new XElement("title", ""));
                                  elem.Element("title").AddAfterSelf(new XElement("series", ""));
                                }

                                if (title != null)
                                {
                                   elem.Element("title").AddAfterSelf(new XElement("series", ""));
                                }
                                elem.Element("series").AddAfterSelf(new XElement("docref", ""));
                                elem.Element("docref").AddAfterSelf(new XElement("justcitetitle", ""));
                                elem.Element("justcitetitle").AddAfterSelf(new XElement("docsum", ""));
                                elem.Element("docsum").AddAfterSelf(new XElement("pdflink", ""));
                                elem.Element("pdflink").AddAfterSelf(new XElement("displaycourt", ""));
                                elem.Element("displaycourt").AddAfterSelf(new XElement("did", ""));
                                elem.Element("fyear").AddAfterSelf(new XElement("courtid", ""));
                                elem.Add(new XElement("jurisdictionid", ""));
                                elem.Add(new XElement("uidyear", ""));
                                elem.Element("uidyear");elem.Add(new XElement("seriescode", ""));


                            }
                            doc.Save(newFile);
                        
                        }

                    }
                }
            }
        

       // select element, str "<p>[2014 BCDA 1234]</p>" return "BCDA"
        private static string GetSourceID(XDocument document)
        {
           
                var Element = document.XPathSelectElements("/root/Element");
           
                var str = (Element).First().Value;

                str = str.Replace("<p>", "");
                str = str.Replace("</p>", "");

                str = str.Replace("[", "");
                str = str.Replace("]", "");
                str = str.Replace("(", "");
                str = str.Replace(")", "");

                str = str.Replace("BCDA", "ABCD");
                

                string[] substrings = str.Split(' ');
                str = substrings[1];


                return str;
            }

        // select element, str "<p>[2014 BCDA 1234]</p>" retrieve value "2014 BCDA 1234"
        private static string ElementValue(XDocument document)
        {
            
            var Element = document.XPathSelectElements("/root/Element");


            var res = (Element).First().Value;

            res = res.Replace("\n", "");
            res = res.Replace("<p>", "");
            res = res.Replace("</p>", "");
            res = res.Replace("[", "");
            res = res.Replace("]", "");
            res = res.Replace("(", "");
            res = res.Replace(")", "");

            
            return res;
        }
    }

}
    

          




        



