using System;
using System.Web.Mvc;
using EPWI.Components.Utility;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Configuration;
using EPWI.Components.Models;

namespace EPWI.Web.Controllers
{
  [Authorize(Roles = "EMPLOYEE")]
  public class EmailController : LoggingController
  {
    [ValidateInput(false)]
    public ActionResult SendContentEmail(string fromName, string fromAddress, string toAddress, string subject, string notes, string data)
    {
      data = formatContent(data);

      var bytes = Encoding.ASCII.GetBytes(data);
      var message = new MailMessage(fromAddress, toAddress, subject, notes);
      message.From = new MailAddress(fromAddress, fromName);
      message.Attachments.Add(new Attachment(new MemoryStream(bytes), "attachment.htm"));
      MailUtility.SendEmail(message);
      return new EmptyResult();
    }

    [ValidateInput(false)]
    public ActionResult SendContentFax(string fromName, string recipientName, string recipientCompany, string toFaxNumber, string subject, bool includeCoverSheet, string notes, string data)
    {
      var parsedFaxNumber = toFaxNumber.Replace(")", string.Empty).Replace("(", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty);
      if (!parsedFaxNumber.StartsWith("1"))
      {
        parsedFaxNumber = "1" + parsedFaxNumber;
      }

      data = formatContent(data);

      var message = new MailMessage();
      message.From = new MailAddress(CustomerData.CompanyCode.GetValueOrDefault('N') == 'N' ? ConfigurationManager.AppSettings["northFromAddress"] : ConfigurationManager.AppSettings["southFromAddress"]);
      message.To.Add(new MailAddress(parsedFaxNumber + ConfigurationManager.AppSettings["faxSuffix"]));
      
      if (includeCoverSheet)
      {
        var coverSheetBytes = generateCoverSheet(recipientCompany, recipientName, toFaxNumber, fromName, subject, notes, CustomerData.CompanyCode.GetValueOrDefault('N'));
        message.Attachments.Add(new Attachment(new MemoryStream(coverSheetBytes), "coversheet.html"));
      }

      var bytes = Encoding.ASCII.GetBytes(data);
      message.Attachments.Add(new Attachment(new MemoryStream(bytes), "attachment.html"));
      message.IsBodyHtml = false;

      var logEntry = new FaxLog();

      try
      {
        MailUtility.SendEmail(message);
      }
      catch (System.Exception ex)
      {
        logEntry.ErrorDescription = ex.Message.Left(255);
        throw;
      }
      finally
      {
        var rep = new LogRepository();
        logEntry.FaxNumber = parsedFaxNumber.Left(15);
        logEntry.RecipName = recipientName.Left(50);
        logEntry.RecipCompany = recipientCompany.Left(50);
        logEntry.Subject = subject.Left(50);
        logEntry.Sender = fromName.Left(50);
        logEntry.FaxContent = data;
        logEntry.DateSent = DateTime.Now;
        rep.AddFaxLogEntry(logEntry);
        rep.Save();
      }

      return new EmptyResult();
    }

    /// <summary>
    /// Corrects URLs and wraps content with html tags
    /// </summary>
    /// <param name="data">The content to format</param>
    /// <returns>The formatted content</returns>
    private string formatContent(string data)
    {
      
      var sb = new StringBuilder();
      var siteUrl = getSiteBasePath();

      // replace image paths with the full URL
      data = data.Replace($"{getApplicationPath()}/Content/", $"{siteUrl}/Content/");
      //wrap the content with html markup
      //sb.Append("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=7\" /><!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">");
      sb.Append("<html><head>");
      sb.Append("<title>Engine and Performance Warehouse</title>");
      sb.AppendFormat("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}/Content/epwi.css\" />", siteUrl);
      sb.AppendFormat("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}/Content/print.css\" />", siteUrl);
      sb.AppendFormat("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}/Content/accountstatus.css\" />", siteUrl);
      // this is required for eFax
      sb.Append("<base href=\"http://www.epwi.net\" />");
      sb.Append("</head><body>");
      sb.Append(data);
      sb.Append("</body></html>");

      return sb.ToString();
    }

    private byte[] generateCoverSheet(string toCompany, string toName, string toFax, string fromName, string subject, string body, char companyCode)
    {
      StringBuilder xml = new StringBuilder();
      var ms = new MemoryStream();
      var xslt = new XslCompiledTransform();
      string coversheetPath = Server.MapPath(companyCode == 'N' ? "~/Content/faxcover_north.xsl" : "~/Content/faxcover_south.xsl");
      xslt.Load(coversheetPath);

      var writer = XmlWriter.Create(xml, xslt.OutputSettings);
      writer.WriteStartDocument();
      writer.WriteStartElement("faxcover");
      writer.WriteElementString("to_company", toCompany);
      writer.WriteElementString("to_name", toName);
      writer.WriteElementString("to_fax", toFax);
      writer.WriteElementString("to_phone", string.Empty);
      writer.WriteElementString("from_company", "Engine & Performance Warehouse");
      writer.WriteElementString("pages", string.Empty);
      writer.WriteElementString("date", DateTime.Now.ToString() + " MST");
      writer.WriteElementString("subject", subject);
      writer.WriteElementString("body", body);
      writer.WriteEndElement();
      writer.WriteEndDocument();
      writer.Flush();
      writer.Close();

      var doc = new XmlDocument();
      doc.Load(new StringReader(xml.ToString()));
      
      xslt.Transform(doc, null, ms);

      return ms.ToArray();
    }

    private string getSiteBasePath()
    {
      return $"http://{Request.Url.Host}{getApplicationPath()}";
    }

    private string getApplicationPath()
    {
      return Request.ApplicationPath == "/" ? string.Empty : Request.ApplicationPath;
    }
  }
}
