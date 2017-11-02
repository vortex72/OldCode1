using System;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Data;
using System.IO;
using System.Reflection;
using System.Data.OleDb;

namespace EPWI.Components.Utility
{
	/// <summary>
	/// Summary description for ExportUtils.
	/// </summary>
	public class AdoUtils
	{
		/// <summary>
		/// Return an ADO Formatted DataSet
		/// </summary>
		/// <param name="ds"></param>
		/// <returns></returns>
		public static string GetADORecordSetXml(DataSet ds)
		{
			// Transform the data
			string transformedDataSet = TransformToString(ds.GetXml());

			//Create an xmlwriter object, to write the ADO Recordset Format XML						
			System.IO.MemoryStream aMemStr = new System.IO.MemoryStream();			
			XmlTextWriter xwriter = new XmlTextWriter(aMemStr, System.Text.Encoding.Default);

			//Call this Sub to write the ADONamespaces to the XMLTextWriter
			WriteADONamespaces(ref xwriter);
			
			//Call this Sub to write the ADO Recordset Schema
			WriteSchemaElement(ds, ds.Tables[0].TableName, ref xwriter);

			//Call this Function to transform the Dataset xml to ADO Recordset XML
			//Pass the Transformed ADO REcordset XML to this Sub
			//to write in correct format.
			RewriteFullElements(ref xwriter, transformedDataSet);

			// Flush and close the writer
			xwriter.Flush();
			xwriter.Close();

			string strXml = System.Text.Encoding.UTF8.GetString(aMemStr.ToArray());
			return strXml;
		}

    /// <summary>
    /// Converts a ADO recordset persisted as XML to an ADO.Net DataSet
    /// </summary>
    /// <param name="ds">The dataset to fill</param>
    /// <param name="xml">The recordset xml</param>
    public static void FillDataSetWithAdoXml(DataSet ds, string xml)
    {
      var adoStream = new ADODB.Stream();
      adoStream.Open(Type.Missing, ADODB.ConnectModeEnum.adModeUnknown, ADODB.StreamOpenOptionsEnum.adOpenStreamUnspecified, null, null);
      adoStream.WriteText(xml, ADODB.StreamWriteEnum.stWriteLine);
      adoStream.Flush();
      adoStream.Position = 0;

      var rs = new ADODB.Recordset();
      rs.Open(adoStream, Type.Missing, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic, 0);
      var dataAdapter = new OleDbDataAdapter();

      dataAdapter.Fill(ds, rs, "Resultset");
    }
		
		/// <summary>
		/// The XSLT does not tranform with fullendelements. ADO Recordset 
		/// cannot read this. This method is used to convert the 
		/// elements to have fullendelements.
		/// </summary>
		/// <param name="wrt"></param>
		/// <param name="ADOXmlString"></param>
		private static void RewriteFullElements(ref XmlTextWriter wrt, string ADOXmlString)
		{
			// Get reader
			XmlTextReader rdr = new XmlTextReader(ADOXmlString, XmlNodeType.Document, null);
			MemoryStream outStream = new MemoryStream();

			rdr.MoveToContent();

			// 'if the ReadState is not EndofFile, read the XmlTextReader for nodes.
			while(rdr.ReadState != ReadState.EndOfFile)
			{	
				if(rdr.Name == "s:Schema")
				{
					wrt.WriteNode(rdr, false);
					wrt.Flush();
				}
				else if(rdr.Name == "z:row" && rdr.NodeType == XmlNodeType.Element)
				{
					wrt.WriteStartElement("z", "row", "#RowsetSchema");
					rdr.MoveToFirstAttribute();
					wrt.WriteAttributes(rdr,true);
					wrt.Flush();
				}
				else if(rdr.Name == "z:row" && rdr.NodeType == XmlNodeType.EndElement)
				{
					//	The following is the key statement that closes the z:row 
					//	element without generating a full end element
					wrt.WriteEndElement();
					wrt.Flush();
				}
				else if(rdr.Name == "rs:data" && rdr.NodeType == XmlNodeType.Element)
				{
					wrt.WriteStartElement("rs", "data", "urn:schemas-microsoft-com:rowset");
				}
				else if(rdr.Name == "rs:data" && rdr.NodeType == XmlNodeType.EndElement)
				{
					wrt.WriteEndElement();
					wrt.Flush();
				}
				
				rdr.Read();
			}
			wrt.WriteEndElement();
			wrt.Flush();
		}


		/// <summary>
		/// Write out the ADO Namespaces
		/// </summary>
		/// <param name="writer"></param>
		private static void WriteADONamespaces(ref XmlTextWriter writer)
		{
			//		'The following is to specify the encoding of the xml file
			writer.WriteProcessingInstruction("xml", "version='1.0' encoding='ISO-8859-1'");

			//
			//		'The following is the ado recordset format
			//		'<xml xmlns:s='uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882' 
			//		'        xmlns:dt='uuid:C2F41010-65B3-11d1-A29F-00AA00C14882'
			//		'        xmlns:rs='urn:schemas-microsoft-com:rowset' 
			//		'        xmlns:z='#RowsetSchema'>
			//		'    </xml>
			//
			writer.WriteStartElement("", "xml", "");
			writer.WriteAttributeString("xmlns", "s",  null, "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882");
			writer.WriteAttributeString("xmlns", "dt", null, "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882");
			writer.WriteAttributeString("xmlns", "rs", null, "urn:schemas-microsoft-com:rowset");
			writer.WriteAttributeString("xmlns", "z",  null, "#RowsetSchema");
			writer.Flush();
		}


		/// <summary>
		/// Write out the schema elements
		/// </summary>
		/// <param name="ds"></param>
		/// <param name="dbname"></param>
		/// <param name="writer"></param>
		private static void WriteSchemaElement(DataSet ds, string dbname, ref XmlTextWriter writer)
		{
			//		'ADO Recordset format for defining the schema
			//		' <s:Schema id='RowsetSchema'>
			//		'            <s:ElementType name='row' content='eltOnly' rs:updatable='true'>
			//		'            </s:ElementType>
			//		'        </s:Schema>
			//
			//		'write element schema
			writer.WriteStartElement("s", "Schema", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882");
			writer.WriteAttributeString("id", "RowsetSchema");
			
			//		'write element ElementTyoe
			writer.WriteStartElement("s", "ElementType", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882");
			//
			//		'write the attributes for ElementType
			writer.WriteAttributeString("name", "", "row");
			writer.WriteAttributeString("content", "", "eltOnly");
			writer.WriteAttributeString("rs", "updatable", "urn:schemas-microsoft-com:rowset", "true");

			WriteSchema(ds, dbname, ref writer);

			//			'write the end element for ElementType
			writer.WriteFullEndElement();
			
			//		'write the end element for Schema 
			writer.WriteFullEndElement();
			writer.Flush();
		}

		
		/// <summary>
		/// Write out a schema element
		/// </summary>
		/// <param name="ds"></param>
		/// <param name="dbname"></param>
		/// <param name="writer"></param>
		private static void WriteSchema(DataSet ds, string dbname, ref XmlTextWriter writer)
		{
			Int32 i = 1;
            
			foreach(DataColumn dc in ds.Tables[0].Columns)
			{
				dc.ColumnMapping = MappingType.Attribute;

				writer.WriteStartElement("s", "AttributeType", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882");
				
				//		'write all the attributes 
				writer.WriteAttributeString("name", "", XmlConvert.EncodeName(dc.ColumnName));
				writer.WriteAttributeString("rs", "number", "urn:schemas-microsoft-com:rowset", i.ToString());

				writer.WriteStartElement("s", "datatype", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882");
				//		'write attributes
				switch (dc.DataType.ToString())
				{
					case("System.String") :
					{
						// <s:datatype dt:type="string" dt:maxlength="255" rs:maybenull="false" />
						writer.WriteAttributeString("dt", "type", "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882", "string");
						writer.WriteAttributeString("dt", "maxlength", "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882", "255");
						writer.WriteAttributeString("rs", "maybenull", "urn:schemas-microsoft-com:rowset", dc.AllowDBNull.ToString());
						break;
					}
					case("System.Int16") :
					case("System.Int32") :
					{
						//<s:datatype dt:type="number" rs:dbtype="numeric" dt:maxLength="19" rs:scale="0" rs:precision="9" rs:fixedlength="true" rs:maybenull="false"/>
						writer.WriteAttributeString("dt", "type", "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882", "number");
						writer.WriteAttributeString("rs", "dbtype", "urn:schemas-microsoft-com:rowset", "numeric");
						writer.WriteAttributeString("dt", "maxlength", "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882", "19");
						writer.WriteAttributeString("rs", "scale", "urn:schemas-microsoft-com:rowset", "0");
						writer.WriteAttributeString("rs", "precision", "urn:schemas-microsoft-com:rowset", "9");
						writer.WriteAttributeString("rs", "fixedlength", "urn:schemas-microsoft-com:rowset", "true");
						writer.WriteAttributeString("rs", "maybenull", "urn:schemas-microsoft-com:rowset", dc.AllowDBNull.ToString());
						break;
					}
					case("System.DateTime") :
					{
						//<s:datatype dt:type="dateTime" rs:dbtype="variantdate" dt:maxLength="16" rs:fixedlength="true" />
						writer.WriteAttributeString("dt", "type", "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882", "dateTime");
						writer.WriteAttributeString("rs", "dbtype", "urn:schemas-microsoft-com:rowset", "variantdate");
						writer.WriteAttributeString("dt", "maxlength", "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882", "16");
						writer.WriteAttributeString("rs", "fixedlength", "urn:schemas-microsoft-com:rowset", dc.AllowDBNull.ToString());
						break;
					}
				}
				//		'write end element for datatype
				writer.WriteEndElement();

				//		'end element for AttributeType
				writer.WriteEndElement();
				writer.Flush();
				i++;
			}

			writer.WriteStartElement("s", "AttributeType", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882");				
			//		'write all the attributes 
			writer.WriteAttributeString("name", "", "TotalCount");
			writer.WriteAttributeString("rs", "number", "urn:schemas-microsoft-com:rowset", i.ToString());
			// 'write child element

			writer.WriteStartElement("s", "datatype", "uuid:BDC6E3F0-6DA3-11d1-A2A3-00AA00C14882");
			//		'write attributes
			writer.WriteAttributeString("dt", "type", "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882", "number");
			writer.WriteAttributeString("rs", "dbtype", "urn:schemas-microsoft-com:rowset", "numeric");
			writer.WriteAttributeString("dt", "maxlength", "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882", "19");
			writer.WriteAttributeString("rs", "scale", "urn:schemas-microsoft-com:rowset", "0");
			writer.WriteAttributeString("rs", "precision", "urn:schemas-microsoft-com:rowset", "9");
			writer.WriteAttributeString("rs", "fixedlength", "urn:schemas-microsoft-com:rowset", "true");
			writer.WriteAttributeString("rs", "maybenull", "urn:schemas-microsoft-com:rowset", "false");
			//		'write end element for datatype
			writer.WriteEndElement();

			//		'end element for AttributeType
			writer.WriteEndElement();
			writer.Flush();

		}
	

		/// <summary>
		/// Get the string ADO Recordset Type
		/// </summary>
		/// <param name="dtype"></param>
		/// <returns></returns>
		private static string GetDatatype(string dtype)
		{
			switch(dtype)
			{
				case "System.Int32":
				case "System.Int16":
					return "int";
				case "System.DateTime":
					return "dateTime";
				default:
					return "string";
			}
		}

		public static string TransformToString(string xmlData)
		{
			//Create a new XslTransform object.
			XslTransform xslt = new XslTransform();

			var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EPWI.Components.Utility.ADORecordSet.xsl");

			var reader = new XmlTextReader(stream);
			xslt.Load(reader);

			// Read the string into the xmltext reader
			XmlTextReader xr = new XmlTextReader(new StringReader(xmlData));

			//Create a new XPathDocument and load the XML data to be transformed.
			XPathDocument mydata = new XPathDocument(xr);

			System.IO.MemoryStream aMemStr = new System.IO.MemoryStream();

			//Create an XmlTextWriter which outputs to the console.
			XmlWriter writer = new XmlTextWriter(aMemStr, null);

			//Transform the data and send the output to the console.
			xslt.Transform(mydata, null, writer, null);

			writer.Close();

			string strXml = System.Text.Encoding.UTF8.GetString(aMemStr.ToArray());

			return strXml;
		}
	}
}
