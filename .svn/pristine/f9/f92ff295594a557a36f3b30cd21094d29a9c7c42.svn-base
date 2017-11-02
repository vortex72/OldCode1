<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" />
  <xsl:template match="/">
    <xsl:apply-templates select="faxcover"/>
  </xsl:template>
	<xsl:template match="faxcover">
	  <html>
    <head>
    <title>Untitled Document</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1"/>
		<base href="http://www.epwi.net"/>
		<style type="text/css">
		  body, p, td {font-family:Arial, Helvetica, sans-serif;}
		</style>
		</head>
    <body>
		<style type="text/css">
		  body, p, td {font-family:Arial, Helvetica, sans-serif;}
		</style>
		<p>&#160;</p>
		<table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td>
	        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="50%"><img src="http://www.epwi.net/images/invoice_std_logo.gif" width="106" height="106"/></td>
              <td width="50%" align="right"><img src="http://www.epwi.net/images/invoice_net_logo.gif" width="228" height="100"/></td>
            </tr>
          </table>
          <p align="center">
            <font size="5" face="Courier New, Courier, mono"><b>Engine &amp; Performance Warehouse South, Inc.</b></font><br/>
            <font size="2">4930 Cash Road &#8226; Dallas, TX 75247 &#8226; 214-637-3301 &#8226; FAX: 866-234-4838</font>
          </p>
          <p>
            <font size="7"><b>Fax</b></font>
          </p>
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr> 
              <td width="50%">
                <b>To:</b>&#160;
                <xsl:value-of select="to_name"/>
                <xsl:choose>
						    	<xsl:when test="string(to_company)">
						    		<br/><xsl:value-of select="to_company"/>
						    	</xsl:when>
						    </xsl:choose>
              </td>
              <td width="50%">
                <b>From:</b>&#160;
                  <xsl:value-of select="from_name"/>
                  <xsl:choose>
						    	<xsl:when test="string(from_company)">
						    		<br/><xsl:value-of select="from_company"/>
						    	</xsl:when>
						    </xsl:choose>
              </td>
            </tr>
            <tr> 
              <td colspan="2"><hr/></td>
            </tr>
            <tr> 
              <td><b>Fax:</b>&#160;<xsl:value-of select="to_fax"/></td>
              <td><b>Pages:</b>&#160;<xsl:value-of select="pages"/></td>
            </tr>
            <tr> 
              <td colspan="2"><hr/></td>
            </tr>
            <tr> 
              <td><b>Phone:</b>&#160;<xsl:value-of select="to_phone"/></td>
              <td><b>Date:</b>&#160;<xsl:value-of select="date"/></td>
            </tr>
            <tr> 
              <td colspan="2"><hr/></td>
            </tr>
            <tr> 
              <td colspan="2"><b>Subject:</b>&#160;<xsl:value-of select="subject"/></td>
            </tr>
          </table>
          <hr/>
          <p>
            <b>Comments:</b><br/>
            <xsl:value-of select="body"/>
          </p>
          <p>This fax contains confidential information</p>
        </td>
      </tr>
    </table>
    </body>
    </html>
	</xsl:template>
</xsl:stylesheet>



