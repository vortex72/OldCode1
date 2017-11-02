<%
'------------------------------------------------------------------

Function SendWDDXPacket_ReturnXMLString(sWDDXPacket, sURL)
  ' Use ASP Tear to send a WDDX XML Packet to a remote server
  ' and retreive the response as a string
  ' Inputs:
  '   sWDDXPacket = WDDX XML Packet as String
  '   sURL = Remote Server Address
  ' Outputs:
  '   SendWDDXPacket_ReturnXMLString as String

  Dim objAspTear
  Const Request_POST = 1

  ' Data required to send
  Dim strDataURL, strUsername, strPassword
  strDataURL = sURL
  strUsername = "epwi"
  strPassword = "1234"

  Set objAspTear = Server.CreateObject("SOFTWING.AspTear")
  objAspTear.ConnectionTimeout = 4000 ' 4 seconds

  ' Receive packet from eSynthesys 
  SendWDDXPacket_ReturnXMLString = objAspTear.Retrieve(strDataURL, Request_POST, "XMLContent=" & sWDDXPacket, strUsername, strPassword)
  Set objAspTear = Nothing

End Function
'------------------------------------------------------------------

Function SendXMLPacket_ReturnXMLString(sXMLPacket, sURL)
  ' Use ASP Tear to send a Standard XML Packet to a remote server
  ' and retreive the response as a string
  ' Inputs:
  '   sXMLPacket = XML Packet as String
  '   sURL = Remote Server Address
  ' Outputs:
  '   SendXMLPacket_ReturnXMLString as String

  Dim objAspTear
  Const Request_POST = 1

  ' Data required to send
  Dim strDataURL, strUsername, strPassword
  strDataURL = sURL
  strUsername = "epwi"
  strPassword = "1234"

  Set objAspTear = Server.CreateObject("SOFTWING.AspTear")
  objAspTear.ConnectionTimeout = 4000 ' 4 seconds

  ' Receive packet from eSynthesys 
  SendXMLPacket_ReturnXMLString = objAspTear.Retrieve(strDataURL, Request_POST, "XMLContent=" & sXMLPacket, strUsername, strPassword)
  Set objAspTear = Nothing

End Function

'------------------------------------------------------------------

Function SendWDDXPacket_ReturnADORS(sWDDXPacket, sURL)
  ' Call SendWDDXPacket_ReturnXMLString to send a WDDX XML Packet to a remote server
  ' and retreive the response as a string
  ' Inputs:
  '   sWDDXPacket = WDDX XML Packet as String
  '   sURL = Remote Server Address
  ' Outputs:
  '   SendWDDXPacket_ReturnADORS as ADODB.Recordset
  
  Dim sXMLReceivePacket
  sXMLReceivePacket = SendWDDXPacket_ReturnXMLString(sWDDXPacket, sURL)

  If Len(Trim(sXMLReceivePacket)) > 0 Then 
    Set SendWDDXPacket_ReturnADORS = ConvertXMLStringToADORS(sXMLReceivePacket)
  Else
    Set SendWDDXPacket_ReturnADORS = Server.CreateObject("ADODB.Recordset")
  End If

End Function

'------------------------------------------------------------------

Function SendXMLPacket_ReturnADORS(sWDDXPacket, sURL)
  ' Call SendXMLPacket_ReturnXMLString to send a standard XML Packet to a remote server
  ' and retreive the response as a string
  ' Inputs:
  '   sWDDXPacket = XML Packet as String
  '   sURL = Remote Server Address
  ' Outputs:
  '   SendXMLPacket_ReturnADORS as ADODB.Recordset
  
  Dim sXMLReceivePacket
  sXMLReceivePacket = SendXMLPacket_ReturnXMLString(sWDDXPacket, sURL)

  If Len(Trim(sXMLReceivePacket)) > 0 Then 
    Set SendXMLPacket_ReturnADORS = ConvertXMLStringToADORS(sXMLReceivePacket)
  Else
    Set SendXMLPacket_ReturnADORS = Server.CreateObject("ADODB.Recordset")
  End If

End Function

'------------------------------------------------------------------

Function ConvertXMLStringToADORS(sXMLString)
  ' Convert an String containing XML Data in ADO-XML format
  ' to an ADO Recorset object
  ' Inputs:
  '   sXMLString = ADO-XML Packet as String
  ' Outputs:
  '   ConvertXMLStringToADORS as ADODB.Recordset

  ' Create a stream object hold the received XML  
  Dim ADOXMLStream
  Set ADOXMLStream = Server.CreateObject("ADODB.Stream")

  ' Load the Stream with the value of the String
  ADOXMLStream.Open
  ADOXMLStream.WriteText = sXMLString

  ' Move back to the beginning of the stream
  ADOXMLStream.Position = 0

  ' Create the recordset
  Dim rsObj
  Set rsObj = Server.CreateObject("ADODB.Recordset")
  
  ' Convert the Stream to the RS
  rsObj.Open ADOXMLStream
  
  ' Close the Stream
  ADOXMLStream.Close
  
  Set ConvertXMLStringToADORS = rsObj
'  rsObj.Close

End Function

%>