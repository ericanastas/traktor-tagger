<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="xml" indent="yes"/>


  
    <xsl:template match="/">

      <table>

        <tr>
          <td>
            Directory
          </td>
          <td>
            File
          </td>
          <td>
            Loudness
          </td>
        </tr>
      
      <xsl:for-each select="/NML/COLLECTION/ENTRY">
        <tr>
          <td>
            <xsl:value-of select="LOCATION/@DIR"/>

          </td>
          <td>
          
            <xsl:value-of select="LOCATION/@FILE"/>
          </td>
          <td>
            <xsl:value-of select="LOUDNESS/@ANALYZED_DB"/>
          </td>
        </tr>
      </xsl:for-each>

    </table>
    </xsl:template>
</xsl:stylesheet>
