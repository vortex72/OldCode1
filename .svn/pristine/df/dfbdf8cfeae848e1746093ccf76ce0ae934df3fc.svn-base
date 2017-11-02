<%
Function AddXMLNode(objXMLDOMDocument, ParentNode, NodeName, NodeValue)
  ' Adds a new node to objXMLDOMDocument to the ParentNode,
  ' Named NodeName with a value of NodeValue
	Dim objNode
	Set objNode = objXMLDOMDocument.createNode(1, LCase(NodeName), "")
	If Len(NodeValue) <> 0 Then
		objNode.text = Trim(NodeValue)
	End If
	ParentNode.appendChild objNode
	Set AddXMLNode = objNode
End Function

'------------------------------------------------------------------

Function TransformXMLfromXSL(XML, XSL) 
  ' Transform XML into HTML using supplied XSL style sheet
  ' XML and XSL can be XML Objects, XML Document Strings or XML Virtual Filenames
	Dim objXML
	Dim objXSL
	On Error Resume Next
	Set objXML = GetXMLDoc(XML)
	Set objXSL = GetXMLDoc(XSL)		
	If objXML.parseError <> 0 Then Response.Write reportParseError(objXML.parseError) : Response.End
	If objXSL.parseError <> 0 Then Response.Write reportParseError(objXSL.parseError) : Response.End
	TransformXMLfromXSL = objXML.transformNode(objXSL)
	On Error Goto 0
End Function

'------------------------------------------------------------------

Function GetXMLDoc(XML)
  ' Returns the XML Object from the XML variant 
  ' regardless if the XML variant is any of the following:
  ' - A Current XML Object
  ' - A virtual file name of an XML file
  ' - An XML document string
	
	Dim objXML
	If IsObject(XML) Then ' The variant is already an object
		Set objXML = XML 
	Else ' Create the XML object
		Set objXML = Server.CreateObject("Msxml2.DOMDocument.4.0")
		If InStr(XML,"<") > 0 Then ' The variant is a string because < is not valid in a filename
			objXML.LoadXML XML
		Else ' The variant is a virutal file name
			objXML.load(Server.MapPath(XML))
		End If
	End If
	Set GetXMLDoc = objXML
End Function

'------------------------------------------------------------------

Function ReportParseError(error)
  ' Return HTML formatted text describing error detail
  
  Dim Spaces : Spaces = "" 
  Dim i : i = 1
  While i < error.linepos ' Create a string of spaces to line position
    Spaces = Spaces & " "
    i = i + 1
  Wend
  Dim ReturnValue
  ReturnValue = "<font face=Verdana size=2><font size=4>XML Error loading '" & error.url & "'</font>" & vbCrLf
  ReturnValue = ReturnValue & "<p><b>" + error.reason + "</b></p>" & vbCrLf
  If error.line > 0 Then
    ReturnValue = ReturnValue & "<font size=3><xmp>at line " & error.line & ", character " & error.linepos & vbCrLf
    ReturnValue = ReturnValue & error.srcText & vbCrLf
    ReturnValue = ReturnValue & Spaces & "^" & "</xmp></font>"
  End If
  ReportParseError = ReturnValue
End Function

'------------------------------------------------------------------

Function SendXMLPacket_ReturnXMLString(sSendXMLPacket, sURL)
  ' Use ASP Tear to send a Standard XML Packet to eSynthesys
  ' and retreive the response as a string
  ' Inputs:
  '   sSendXMLPacket = XML Packet as String
  '   sURL = Remote Server Address
  ' Outputs:
  '   SendXMLPacket_ReturnXMLString as String

  ' For debugging purposes, uncomment the next line
  'DisplayDebugText(sSendXMLPacket)

  Dim objAspTear
  Set objAspTear = Server.CreateObject("SOFTWING.AspTear")
  objAspTear.ConnectionTimeout = 5000 ' 5 seconds
  On Error Resume Next
  SendXMLPacket_ReturnXMLString = objAspTear.Retrieve(sURL, Request_POST, eSynPacketName & sSendXMLPacket, eSynUserName, eSynPassword)
  If Err.number > 0 Then
    SendXMLPacket_ReturnXMLString = Err.Description
  End If
  On Error Goto 0
  Set objAspTear = Nothing

End Function

'------------------------------------------------------------------

Function SendXMLPacket_ReturnADORS(sSendXMLPacket, sURL)
  ' Call SendXMLPacket_ReturnXMLString to send a standard XML Packet to eSynthesys
  ' and retreive the response as a string
  ' Inputs:
  '   sSendXMLPacket = XML Packet as String
  '   sURL = Remote Server Address
  ' Outputs:
  '   SendXMLPacket_ReturnADORS as ADODB.Recordset
  '
  ' If an error occures, then return back a recordset with two fields
  '   ErrorOccured = True
  '   ErrorDescription = Text about the error 
  
  Dim sXMLReceivePacket
  sXMLReceivePacket = SendXMLPacket_ReturnXMLString(sSendXMLPacket, sURL)

  If Left(Trim(sXMLReceivePacket),4) = "<xml" Then ' Packet appears to be an XML document
    Set SendXMLPacket_ReturnADORS = ConvertXMLStringToADORS(sXMLReceivePacket)
  Else
    ' Generate recordset containing error information
    Dim rsError
    Set rsError = Server.CreateObject("ADODB.Recordset")
    rsError.Fields.Append "ErrorOccured", adBoolean
    rsError.Fields.Append "ErrorDescription", adVarChar, 4000
    rsError.Open
    rsError.AddNew
    rsError("ErrorOccured") = True
    If Len(Trim(sXMLReceivePacket)) > 0 Then
      rsError("ErrorDescription") = Left(sXMLReceivePacket,4000)
    Else
      rsError("ErrorDescription") = "No Error Information Returned"
    End If
    rsError.Update
    rsError.MoveFirst
    Set SendXMLPacket_ReturnADORS = rsError
    Set rsError = Nothing
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
  ' Leave rsObj Open to Parse

End Function

%>