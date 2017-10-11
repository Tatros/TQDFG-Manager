using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using de.tum.i22.monitor.malware.applications;
using de.tum.i22.monitor.structures;

using NLog;


namespace QDFGGraphManager.GraphBuilder
{
    internal sealed class GEXFWriter
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void writeGraph(DFTGraph g, String outputFile, bool dynamic, long start, long end, Dictionary<string, string> processedIDs)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;

            StringBuilder stringBuilder = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings);

            xmlWriter.WriteStartDocument();

            //xmlWriter.WriteRaw("\r\n<gexf xmlns=\"http://www.gexf.net/1.2draft\" version=\"1.2\">\r\n");
            xmlWriter.WriteRaw("\r\n<gexf xmlns=\"http://www.gexf.net/1.2draft\" version=\"1.2\" xmlns:viz=\"http://www.gexf.net/1.2draft/viz\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.gexf.net/1.2draft http://www.gexf.net/1.2draft/gexf.xsd\">\r\n");

                xmlWriter.WriteStartElement("graph");
                if (dynamic)
                {
                    xmlWriter.WriteAttributeString("mode", "dynamic");
                    xmlWriter.WriteAttributeString("timeformat", "double");
                }
                else
                {
                    xmlWriter.WriteAttributeString("mode", "static");
                }

                xmlWriter.WriteAttributeString("defaultedgetype", "directed");
                    
                    xmlWriter.WriteStartElement("attributes");
                    xmlWriter.WriteAttributeString("class", "node");
                    xmlWriter.WriteAttributeString("mode", "static");

                        xmlWriter.WriteStartElement("attribute");
                        xmlWriter.WriteAttributeString("id", "0");
                        xmlWriter.WriteAttributeString("title", "inDegree");
                        xmlWriter.WriteAttributeString("type", "long");
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("attribute");
                        xmlWriter.WriteAttributeString("id", "1");
                        xmlWriter.WriteAttributeString("title", "outDegree");
                        xmlWriter.WriteAttributeString("type", "long");
                        xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    
                    if (dynamic)
                    {
                        xmlWriter.WriteStartElement("attributes");
                        xmlWriter.WriteAttributeString("class", "node");
                        xmlWriter.WriteAttributeString("mode", "dynamic");
                            xmlWriter.WriteStartElement("attribute");
                            xmlWriter.WriteAttributeString("id", "time_dynamic");
                            xmlWriter.WriteAttributeString("title", "time_dynamic");
                            xmlWriter.WriteAttributeString("type", "long");
                            xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndElement();
                    }   

                    xmlWriter.WriteStartElement("nodes");
                    foreach (DFTNode n in g.Vertices)
                    {
                        xmlWriter.WriteStartElement("node");
                        xmlWriter.WriteAttributeString("id", n.node_id.ToString());
                        xmlWriter.WriteAttributeString("label", n.nameFTR);
                        xmlWriter.WriteAttributeString("type", n._nodeType.ToString());

                        if (dynamic)
                        {
                            if (processedIDs.ContainsKey(n.nameFTR))
                            {
                                xmlWriter.WriteAttributeString("start", processedIDs[n.nameFTR]);
                            }
                            else
                            {
                                logger.Fatal("Key " + n.nameFTR + " not found in time table");
                            }

                        }

                            xmlWriter.WriteStartElement("attvalues");
                                xmlWriter.WriteStartElement("attvalue");
                                xmlWriter.WriteAttributeString("for", "0");
                                xmlWriter.WriteAttributeString("value", g.InDegree(n).ToString());
                                xmlWriter.WriteEndElement();

                                xmlWriter.WriteStartElement("attvalue");
                                xmlWriter.WriteAttributeString("for", "1");
                                xmlWriter.WriteAttributeString("value", g.OutDegree(n).ToString());
                                xmlWriter.WriteEndElement();

                                if (dynamic)
                                {
                                    if (processedIDs.ContainsKey(n.nameFTR))
                                    {
                                        xmlWriter.WriteStartElement("attvalue");
                                        xmlWriter.WriteAttributeString("for", "time_dynamic");
                                        xmlWriter.WriteAttributeString("value", processedIDs[n.nameFTR]);
                                        xmlWriter.WriteAttributeString("start", start.ToString());
                                        xmlWriter.WriteAttributeString("end", end.ToString());
                                        xmlWriter.WriteEndElement();
                                    }

                                    else
                                    {
                                        logger.Fatal("Key " + n.nameFTR + " not found in time table.");
                                    }
                                }
                            xmlWriter.WriteEndElement();

                            if (n._nodeType == DFTNodeType.SOCKET.ToString())
                            {
                                xmlWriter.WriteRaw("\r\n<viz:color r=\"0\" g=\"0\" b=\"255\"></viz:color>\r\n");
                            }
                                
                            else if (n.nameFTR.Contains("malware"))
                                xmlWriter.WriteRaw("\r\n<viz:color r=\"255\" g=\"0\" b=\"0\"></viz:color>\r\n");
                            else if (n.nameFTR.Contains("goodware"))
                                xmlWriter.WriteRaw("\r\n<viz:color r=\"0\" g=\"255\" b=\"0\"></viz:color>\r\n");
                            else
                                xmlWriter.WriteRaw("\r\n<viz:color r=\"153\" g=\"153\" b=\"153\"></viz:color>\r\n");

                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("edges");
                    long edgeID = 0;
                    foreach (DFTEdge e in g.Edges)
                    {
                        xmlWriter.WriteStartElement("edge");
                        xmlWriter.WriteAttributeString("id", edgeID.ToString());
                        xmlWriter.WriteAttributeString("source", e.Source.node_id.ToString());
                        xmlWriter.WriteAttributeString("target", e.Target.node_id.ToString());
                        xmlWriter.WriteAttributeString("label", e._name);
                        xmlWriter.WriteAttributeString("Weight", e._size.ToString());
                        xmlWriter.WriteAttributeString("RelativeWeight", e.relSize.ToString());
                        //xmlWriter.WriteAttributeString("start", e.getMinTimestamp().ToString());
                        //xmlWriter.WriteAttributeString("end", e.getMaxTimestamp().ToString());
                        xmlWriter.WriteEndElement();
                        edgeID++;
                    }
                    xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            xmlWriter.WriteRaw("\r\n</gexf>");
            xmlWriter.WriteEndDocument();

            xmlWriter.Flush();
            xmlWriter.Close();

            Utility.Graph.writeGraphToFile(stringBuilder.ToString(), outputFile);
        }
    }
}
