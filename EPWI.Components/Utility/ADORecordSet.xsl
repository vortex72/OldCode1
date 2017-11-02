<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
xmlns:rs="urn:schemas-microsoft-com:rowset"
xmlns:z="#RowsetSchema">
<xsl:output method="xml" indent="yes"/>
<xsl:template match="*">
<rs:data>	
	<xsl:for-each select="*">
	<z:row><xsl:for-each select="*"><xsl:attribute name="{name()}"><xsl:value-of select="."/></xsl:attribute></xsl:for-each><xsl:attribute name="TotalCount">1</xsl:attribute></z:row>
	</xsl:for-each>
</rs:data>
</xsl:template>
</xsl:stylesheet>