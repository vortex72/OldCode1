<%
'------------------------------------------------------------------

Sub RW(v)
' Execute the Response.Write function for "v"
  Response.Write(v)
End Sub

'------------------------------------------------------------------

Sub RWL(v)
' Execute the Response.Write function for "v" with a line feed
  Response.Write(v) & vbCrLf
End Sub

'------------------------------------------------------------------

Function ConvertToString(v)
' Convert parameter to string and check for nulls
  If IsNull(v) Then
    ConvertToString = ""
  Else
    ConvertToString = CStr(v)
  End If
End Function

'------------------------------------------------------------------

Function ConvertToStandardASCII(sValue, sReplacement)
  ' Replace any character in sValue with a character code > 127 with sReplacement character
  Dim i, rv, charValue
  rv = ""
  For i = 1 To Len(sValue)
    charValue = Asc(Mid(sValue, i, 1))
    If charValue <= 127 Then
      rv = rv & Chr(charValue)
    Else
      rv = rv & sReplacement
    End If
  Next
  ConvertToStandardASCII = rv
End Function

'------------------------------------------------------------------

Function NullsToZero(v)
' Convert input parameter to 0 if null
  If IsNull(v) Then
    NullsToZero = 0
  Else
    NullsToZero = v
  End If
End Function

'------------------------------------------------------------------

Function BlanksToZero(v)
' Convert input parameter to 0 if empty
  If Trim(v) = "" Then
    BlanksToZero = 0
  Else
    BlanksToZero = v
  End If
End Function

'------------------------------------------------------------------

Function ZerosToBlank(v)
' Convert input parameter to empty string if 0
  If CStr(v) = "0" Then
    ZerosToBlank = ""
  Else
    ZerosToBlank = v
  End If
End Function

'------------------------------------------------------------------

Function BlanksToNull(v)
' Convert input parameter to null if empty
  If Trim(v) = "" Then
    BlanksToNull = Null
  Else
    BlanksToNull = v
  End If
End Function

'------------------------------------------------------------------

Function NullsToNBSP(sText)
	If IsNull(sText) Or (Trim(sText) = "") Then
	  NullsToNBSP = "&nbsp;"
	Else
	  NullsToNBSP = sText
	End If
End Function

'------------------------------------------------------------------

Function BlanksToNBSP(v)
' Convert input parameter to HTML &nbsp; if empty
  If Trim(v) = "" Then
    BlanksToNBSP = "&nbsp;"
  Else
    BlanksToNBSP = v
  End If
End Function

'------------------------------------------------------------------

Function ConvertToInteger(v)
' Convert input parameter to integer or to 0 if empty
  If Len(v) > 0 Then
    If IsNumeric(v) Then
      ConvertToInteger = CInt(v)
    Else
      ConvertToInteger = 0
    End If
  Else
    ConvertToInteger = 0
  End If
End Function

'------------------------------------------------------------------

Function ConvertToLong(v)
' Convert input parameter to long integer or to 0 if empty
  If Len(v) > 0 Then
    If IsNumeric(v) Then
      ConvertToLong = CLng(v)
    Else
      ConvertToLong = 0
    End If
  Else
    ConvertToLong = 0
  End If
End Function

'------------------------------------------------------------------

Function ConvertToSingle(v)
' Convert input parameter to single point value or to 0 if empty
  If Len(v) > 0 Then
    If IsNumeric(v) Then
      ConvertToSingle = CSng(v)
    Else
      ConvertToSingle = 0
    End If
  Else
    ConvertToSingle = 0
  End If
End Function
'------------------------------------------------------------------

Function ZeroToBlank(v)
' Convert input parameter to empty string if 0
  If v = 0 Then
    ZeroToBlank = ""
  Else
    ZeroToBlank = v
  End If
End Function

'------------------------------------------------------------------

Function MinimumValue(iValue1, iValue2)
  ' Return the lowest of the two passed values
  If CInt(iValue1) < CInt(iValue2) Then
    MinimumValue = iValue1
  Else
    MinimumValue = iValue2
  End If
End Function

'------------------------------------------------------------------

Function BooleanCheckMark(v)
' Display a check-on image if true, otherwise display check-off
	If v = True Then
		BooleanCheckMark = ImageCheckOn
	Else
		BooleanCheckMark = ImageCheckOff
	End If
End Function

'------------------------------------------------------------------

Function BooleanCheckBox(v)
' Set a checkbox input field to "checked" if true
	If v = True Then
		BooleanCheckBox = " checked"
	Else
		BooleanCheckBox = ""
	End If
End Function

'------------------------------------------------------------------

Function BooleanSelected(v)
' Set a option input field to "selected" if true
	If v = True Then
		BooleanSelected = " selected"
	Else
		BooleanSelected = ""
	End If
End Function

'------------------------------------------------------------------

Function StartOfThisMonth()
'Returns String contaning the first day of the current month (mm/dd/yyyy)
  Dim iCurrentMonth
  Dim iCurrentYear
  Dim dCurrentDate
  dCurrentDate = Now()

  iCurrentMonth = Month(dCurrentDate)
  iCurrentYear = Year(dCurrentDate)  

  StartOfThisMonth = iCurrentMonth & "/1/" & iCurrentYear
End Function

'------------------------------------------------------------------

Function EndOfThisMonth()
'Returns String contaning the last day of the current month (mm/dd/yyyy)
  Dim iCurrentMonth, iNextMonth
  Dim iCurrentYear, iNextMonthYear
  Dim dCurrentDate
  dCurrentDate = Now()

  iCurrentMonth = Month(dCurrentDate)
  iNextMonth  = Month(DateAdd("m",1,dCurrentDate))
  iCurrentYear = Year(dCurrentDate)
  iNextMonthYear = Year(DateAdd("m",1,dCurrentDate))

  EndOfThisMonth = iCurrentMonth & "/" & Day(DateAdd("d",-1,iNextMonth & "/1/" & iNextMonthYear)) & "/" & iCurrentYear
End Function

'------------------------------------------------------------------

Function FirstDayOfMonth(dDate)
'Returns String contaning the first day of the passed month (mm/dd/yyyy)
  Dim iCurrentMonth
  Dim iCurrentYear

  iCurrentMonth = Month(dDate)
  iCurrentYear = Year(dDate)  

  FirstDayOfMonth = iCurrentMonth & "/1/" & iCurrentYear
End Function

'------------------------------------------------------------------

Function LastDayOfMonth(dDate)
'Returns String contaning the last day of the passed month (mm/dd/yyyy)
  Dim iCurrentMonth, iNextMonth
  Dim iCurrentYear, iNextMonthYear

  iCurrentMonth = Month(dDate)
  iNextMonth  = Month(DateAdd("m",1,dDate))
  iCurrentYear = Year(dDate)
  iNextMonthYear = Year(DateAdd("m",1,dDate))

  LastDayOfMonth = iCurrentMonth & "/" & Day(DateAdd("d",-1,iNextMonth & "/1/" & iNextMonthYear)) & "/" & iCurrentYear
End Function

'------------------------------------------------------------------

Function FormatMedDate(datevalue)
' Formats the date as 'Mar. 17, 2002'
  Dim ReturnValue
  ReturnValue = ""
  ReturnValue = ReturnValue & MonthName(Month(datevalue),True) & ". "
  ReturnValue = ReturnValue & Day(datevalue) & ", "
  ReturnValue = ReturnValue & Year(datevalue)
  FormatMedDate = ReturnValue
End Function

'----------------------------------------------------------------	

Function FormatPhone(sPhoneNumber)
  Dim ReturnValue
  ReturnValue = sPhoneNumber
	If (ReturnValue <> "") Then
	  If Left(ReturnValue,1) = "1" Then ReturnValue = Mid(ReturnValue,2) ' Remove the leading "1" if it exists
		FormatPhone = "(" & Mid(ReturnValue,1,3) & ")&nbsp;" & Mid(ReturnValue,4,3) & "-" & Mid(ReturnValue,7,4)
	Else
		FormatPhone = ""
	End If
End Function

'----------------------------------------------------------------	

Function FormatPhoneExt(sPhoneExt)
	If (sPhoneExt > 0) Then
		FormatPhoneExt = "&nbsp;&nbsp;&nbsp; Ext. " & sPhoneExt
	Else
		FormatPhoneExt = ""
	End If
End Function

'------------------------------------------------------------------

Function FormatCurrency(cCurrency)
  ' Format cCurrency as $0.00
  If IsNumeric(cCurrency) Then
    FormatCurrency = "$" & FormatNumber(cCurrency, 2, True, False, True)
  End If
End Function

'------------------------------------------------------------------

Function LeadingZeros(vData, iLength)
  ' Add leading zeros to vData if it is less than the length
  ' e.g.: LeadingZeros(1234, 5) = "01234"
  LeadingZeros = CStr(vData)
  Dim i
  For i = 1 To (iLength - Len(vData))
    LeadingZeros = "0" & LeadingZeros
  Next
End Function

'------------------------------------------------------------------

Function PlainTextToHTML(sText)
	Dim ReturnValue
	ReturnValue = Replace(sText,Chr(13),"<br>")
	PlainTextToHTML = ReturnValue
End Function

'------------------------------------------------------------------

Function HTMLtoPlainText(sText)
	Dim ReturnValue
	ReturnValue = Replace(sText,"<br>",Chr(13))
	HTMLtoPlainText = ReturnValue
End Function

'------------------------------------------------------------------

Function EnCrypt(strPlainText)
  Dim g_key, strChar, iKeyChar, iStringChar, i, iCryptChar, strEncrypted
  g_Key = mid(EncryptionKey,1,Len(strPlainText))
  For i = 1 To Len(strPlainText)
     iKeyChar = Asc(mid(g_Key,i,1))
     iStringChar = Asc(mid(strPlainText,i,1))
     iCryptChar = iKeyChar Xor iStringChar
     strEncrypted =  strEncrypted & Chr(iCryptChar)
  Next
  EnCrypt = strEncrypted
End Function

'------------------------------------------------------------------

Function DeCrypt(strEncryptedText)
Dim g_key, strChar, iKeyChar, iStringChar, i, iDeCryptChar, strDecrypted
  g_Key = mid(EncryptionKey,1,Len(strEncryptedText))
  For i = 1 To Len(strEncryptedText)
     iKeyChar = (Asc(mid(g_Key,i,1)))
     iStringChar = Asc(mid(strEncryptedText,i,1))
     iDeCryptChar = iKeyChar Xor iStringChar
     strDecrypted =  strDecrypted & Chr(iDeCryptChar)
  Next
  DeCrypt = strDecrypted
End Function

'------------------------------------------------------------------

Function ConvertPostDataToString()
  ' Convert the contents of the post data to a string value
  ' Used mostly for logging purposes
  On Error Resume Next
  Dim ReturnValue
  Dim FormItem
  For Each FormItem In Request.Form
    ReturnValue = ReturnValue & FormItem & "=" & Request.Form(FormItem) & "&"
  Next
  ConvertPostDataToString = CStr(ReturnValue)
End Function

'------------------------------------------------------------------

Function LogAccess(sFields, sValues)
' Add entry to tbl_accesslog
  On Error Resume Next
  Dim sSQL
  sSQL = "INSERT INTO tbl_accesslog (" & sFields & ") VALUES (" & sValues & ")"
  Dim cmdAddAccessLog
  Set cmdAddAccessLog = Server.CreateObject("ADODB.Command")
  cmdAddAccessLog.ActiveConnection = conn
  cmdAddAccessLog.CommandText = sSQL
  cmdAddAccessLog.Execute
  Set cmdAddAccessLog = Nothing
End Function

'------------------------------------------------------------------

Sub LogError(objErr, strCodeSection, strNotes, bRedirect, strErrorDesc)

	'------------------------------------
	'Log error to file and to database
	'------------------------------------
  Dim strRemoteAddr, strAppPath, strQueryString
  strRemoteAddr = Request.ServerVariables("REMOTE_ADDR")
  strAppPath = Request.ServerVariables("SCRIPT_NAME")
  strQueryString = Request.ServerVariables("QUERY_STRING")
  ' File Logging
	Const ForReading = 1, ForWriting = 2, ForAppending = 8
	Const TristateUseDefault = -2
	Const TristateTrue = -1
	Const TristateFalse = 0
	Const LogFile = "log/errorlog.txt"
	Dim objFS, objTextS
	Set objFS = Server.CreateObject("Scripting.FileSystemObject")
	If objFS.FileExists(Server.MapPath(LogFile)) = True Then
		Set objTextS = objFS.OpenTextFile(Server.MapPath(LogFile), ForAppending, False, TristateFalse)
	Else
		Set objTextS = objFS.CreateTextFile(Server.MapPath(LogFile), False, False)
	End If
	objTextS.WriteLine "Date/Time: " & Now()
	objTextS.WriteLine "Error Number: " & objErr.Number
  objTextS.WriteLine "Error Description: " & objErr.Description
	objTextS.WriteLine "Remote Address: " & strRemoteAddr
  objTextS.WriteLine "Code Section: " & strCodeSection 
  objTextS.WriteLine "Notes: " & strNotes 
  objTextS.WriteLine "Script Name: " & strAppPath
	objTextS.WriteLine "Query String: " & strQueryString
	objTextS.WriteLine "-------------------------------------------------------"
	objTextS.close

  ' Database Logging
  Set cmd = Server.CreateObject("ADODB.Command")
  Set cmd.ActiveConnection = conn
  cmd.CommandType = adCmdStoredProc
  cmd.CommandText = "usp_LogError"
  cmd.Parameters.Append = cmd.CreateParameter("iErrorNums",adInteger,adParamInput,4,objErr.Number)
  cmd.Parameters.Append = cmd.CreateParameter("sErrorDesc",adVarChar,adParamInput,250,objErr.Description)
  cmd.Parameters.Append = cmd.CreateParameter("sRemoteAddr",adChar,adParamInput,15,strRemoteAddr)
  cmd.Parameters.Append = cmd.CreateParameter("sCodeSection",adChar,adParamInput,50,strCodeSection)
  cmd.Parameters.Append = cmd.CreateParameter("sNotes",adVarChar,adParamInput,500,strNotes)
  cmd.Parameters.Append = cmd.CreateParameter("sScriptName",adChar,adParamInput,25,strAppPath)
  cmd.Parameters.Append = cmd.CreateParameter("sQueryString",adVarChar,adParamInput,250,strQueryString)
  cmd.Execute
  Set cmd = Nothing

  If bRedirect Then
    Response.Clear
    Response.Redirect "error.asp?error=" & Server.URLEncode(strErrorDesc)
    Response.End
  End If
End Sub

'------------------------------------------------------------------

Sub LogEdit(strUserName, intAppID, intCatID, intItemID, strEditType, strEditSection, strNotes)

  If Not IsNumeric(intAppID) Then intAppID = 0
  If Not IsNumeric(intCatID) Then intCatID = 0
  If Not IsNumeric(intItemID) Then intItemID = 0
  
  Set cmd = Server.CreateObject("ADODB.Command")
  Set cmd.ActiveConnection = conn
  cmd.CommandType = adCmdStoredProc
  cmd.CommandText = "usp_LogEdit"
  cmd.Parameters.Append = cmd.CreateParameter("@sUserName",adChar,adParamInput,20,strUserName)
  cmd.Parameters.Append = cmd.CreateParameter("@iAppID",adInteger,adParamInput,4,CInt(intAppID))
  cmd.Parameters.Append = cmd.CreateParameter("@iCatID",adInteger,adParamInput,4,CInt(intCatID))
  cmd.Parameters.Append = cmd.CreateParameter("@iItemID",adInteger,adParamInput,4,CInt(intItemID))
  cmd.Parameters.Append = cmd.CreateParameter("@sEditType",adChar,adParamInput,20,strEditType)
  cmd.Parameters.Append = cmd.CreateParameter("@sEditSection",adChar,adParamInput,20,strEditSection)
  cmd.Parameters.Append = cmd.CreateParameter("@sNotes",adVarChar,adParamInput,250,strNotes)
  cmd.Execute
  Set cmd = Nothing
End Sub

'------------------------------------------------------------------

Sub CheckDBConnection()
	'------------------------------------------------
	'Ensure that a connection to the database exists
	'------------------------------------------------
    On Error Resume Next
    
    ' Open recordset 
    Set cmd = Server.CreateObject("ADODB.Command")
    Set cmd.ActiveConnection = conn
    cmd.CommandType = adCmdStoredProc
    cmd.CommandText = "usp_CheckDBConn"
    Dim rsDBConn
    Set rsDBConn = cmd.Execute
    Set rsDBConn = Nothing
    Set cmd = Nothing

    ' Error Handler - Before calling external procedures
	  If Err.number <> 0 Then
      LogError Err,"CheckDBConn","",true,""
    End If
    On Error Goto 0
    
End Sub

'------------------------------------------------------------------

Function ReadFromTemplate(strTemplateLocation)
	'------------------------------------
	'Get HTML code from template
	'------------------------------------
	On Error Resume Next
	Dim objFile, strFileName, InStream, strFileContent
	Set objFile = Server.CreateObject("Scripting.FileSystemObject")
	strFileName = Server.MapPath(strTemplateLocation)
	Set InStream = objFile.OpenTextFile (strFileName, 1, False, False)
	If Err.number = 0 Then
		While Not InStream.AtEndOfStream
			strFileContent = strFileContent & InStream.ReadLine & vbcrlf
		Wend
		Set Instream = Nothing
		ReadFromTemplate = strFileContent
	Else
		Response.Write "Error: Unable To load template file " & strTemplateLocation
		Response.End
	End If
	On Error GoTo 0
End Function

'------------------------------------------------------------------

Function GetCodeSections(sHTMLCode, sBeginCode, sLoopCode, sEndCode)
	'------------------------------------------------
	' Take HTML code and parse out three sections for 
	' beginning code, code that loops, and end code.
	' Will not accept embedded or multiple loops.
	'------------------------------------------------
	'Set the standard loop and end loop qualifier
	Dim strBeginLoopCode, strEndLoopCode
	strBeginLoopCode = "<!--#Loop#//-->"
	strEndLoopCode = "<!--#EndLoop#//-->"
	If InStr(sHTMLCode,strBeginLoopCode) = 0 Then
		' Determine if there is looping code available
		Response.Write "ERROR: No Looping Code Found"
		Response.End
	Else
		' Split the HTML code, first at the begin loop, then at the end loop
		' Temp arrays are used for storage, return three strings
		Dim aryTemp1, aryTemp2
		aryTemp1 = Split(sHTMLCode, strBeginLoopCode)
		sBeginCode = aryTemp1(0)
		aryTemp2 = Split(aryTemp1(1), strEndLoopCode)
		sLoopCode = aryTemp2(0)
		sEndCode = aryTemp2(1)
	End If
End Function

'------------------------------------------------------------------

Function GetLoopCode(sLoopCode, objRecordSet, aLinkValues)
	'------------------------------------------------
	' Take Loop Code and replace field varables "Field" with 
	' appropriate value from the recordset.  If the field is
	' referenced as "LinkField" then append a link tag before
	' and after the field value and relate the link value to
	' the values (passed in order) to the array aLinkValues.
	'------------------------------------------------
	
	' Set the standard loop and end loop qualifiers
	Const strFieldBeginText = "{#-"
	Const strFieldEndText = ")-#}"

  ' Possible Values	
	' Field(fieldname)
	' LinkField(fieldname)
	' LinkValue(fieldname)
	' NextRecord(endcode) ' DOES NOT WORK! LINKS ARE NOT PASSED PROPERLY!
	' - endcode is that code that should be displayed if the end of the recordset 
	'   is reached inbetween the loop tags
	
	' strFieldType = The name of the current field that is being processed
	Dim strFieldType

	' Set the position locators, a boolean value if the field is a link
	' and the number of link fields found
	' iFieldPosCount is the start of either the StdField, LinkField or LinkValue declaration
	' iValuePosCount is the location of the parentheses after iFieldPosCount
	Dim iPosCount, iFieldPosCount, iValuePosCount, iLinkCount
	iLinkCount = 0
	
	' Set the field name as found in the code and the value from the DB
	Dim strFieldName, strFieldValue
	
	' Set the two variables for holding the code
	' StartCode is the initial code that decreases as ModifiedCode is generated 
	Dim strStartCode, strModCode
	strStartCode = sLoopCode ' Assign Start Code to passed sLoopCode
	strModCode = ""

	' Repeat while there are still field variables in code
	Do While (InStr(strStartCode, strFieldBeginText) > 0)
		strFieldValue = ""
		
		' Move the location pointer to the first field variable by
		' locating the position(s) of the field or linkfield codes		
		iFieldPosCount = InStr(strStartCode, strFieldBeginText)
		
		' Find field type by taking the position of the strFieldBeginText (plus it's length)
		' and subtracting that from the position of the parentheses and find the text between
		iValuePosCount = InStr(strStartCode, "(")
		strFieldType = Mid(strStartCode, iFieldPosCount + Len(strFieldBeginText), iValuePosCount - (iFieldPosCount + Len(strFieldBeginText)))
		iPosCount = iFieldPosCount
      
		' Move the text before the first field code to the ModifiedCode and
		' remove that text from the StartingCode
		strModCode = strModCode & Left(strStartCode, iPosCount - 1)
		strStartCode = Mid(strStartCode, iValuePosCount+1)

		' Remove the field variable code and determine field name and value from DB
		' Add error checking to ensure that field is found in RS
		iPosCount = InStr(strStartCode, strFieldEndText)
		strFieldName = Left(strStartCode, iPosCount-1)

    ' If the field type is a field (as opposed to a command)
    ' get the field value from the recordset
    If strFieldType <> "NextRecord" Then
		  On Error Resume Next
		  	strFieldValue = Trim(objRecordSet(strFieldName))
		  	If Err.number <> 0 Then
		  		strFieldValue = "ERROR: Invalid Field Name: " & strFieldName
		  	End If
		  On Error Goto 0
		End If

		' Remove all remaining variable code from StartCode
		strStartCode = Mid(strStartCode, iPosCount + Len(strFieldEndText))
		
    ' Define values as they pertain to each field type
    ' Increase the link count if needed
	  ' Field = Standard Value From DB
	  ' LinkField = Complete Link tag with the field value as the linked item 
	  '   (<a href="[aLinkValues(i)]">[FieldValue]</a>)
	  '   Only good for text fields, no images
	  '   Values are passed and accessed in order of appearance from the array aLinkValues
	  ' LinkValue = Value that is accessable from the template to create custom links
	  '   Values are passed and accessed as above
    Select Case strFieldType
      Case "Field"
   			strModCode = strModCode & strFieldValue
      Case "LinkField"
   			strModCode = strModCode & "<a href=""" & aLinkValues(iLinkCount) & """>" & strFieldValue & "</a>"
        iLinkCount = iLinkCount + 1
      Case "LinkValue"
        strModCode = strModCode & aLinkValues(iLinkCount)
        iLinkCount = iLinkCount + 1
      Case "NextRecord"
        objRecordSet.MoveNext
        iLinkCount = 0
        If objRecordset.EOF Then 
          strStartCode = strFieldName ' Add remaining code'
          Exit Do ' Get out if we are at the end
        End If
    End Select
	Loop
	
	'Return the modified code and any remaining start code
	GetLoopCode = strModCode & strStartCode
End Function

'------------------------------------------------------------------

Sub CreateImageRolloverScripts()
	'------------------------------------------------
	' If the site uses images for the app menu,
	' display JS rollover functions 
	'------------------------------------------------
  Dim ReturnValue
  ReturnValue = ""
  ReturnValue = ReturnValue & "<script language=""JavaScript"">" & vbCrLf
  ReturnValue = ReturnValue & "<!--" & vbCrLf
  ReturnValue = ReturnValue & "function swapImgRestore() {" & vbCrLf
  ReturnValue = ReturnValue & "  var i,x,a=document.sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;" & vbCrLf
  ReturnValue = ReturnValue & "}" & vbCrLf & vbCrLf
  ReturnValue = ReturnValue & "function preloadImages() {" & vbCrLf
  ReturnValue = ReturnValue & "  var d=document; if(d.images){ if(!d.p) d.p=new Array();" & vbCrLf
  ReturnValue = ReturnValue & "    var i,j=d.p.length,a=preloadImages.arguments; for(i=0; i<a.length; i++)" & vbCrLf
  ReturnValue = ReturnValue & "    if (a[i].indexOf(""#"")!=0){ d.p[j]=new Image; d.p[j++].src=a[i];}}" & vbCrLf
  ReturnValue = ReturnValue & "}" & vbCrLf & vbCrLf
  ReturnValue = ReturnValue & "function findObj(n, d) {" & vbCrLf
  ReturnValue = ReturnValue & "  var p,i,x;  if(!d) d=document; if((p=n.indexOf(""?""))>0&&parent.frames.length) {" & vbCrLf
  ReturnValue = ReturnValue & "    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}" & vbCrLf
  ReturnValue = ReturnValue & "  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];" & vbCrLf
  ReturnValue = ReturnValue & "  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=findObj(n,d.layers[i].document); return x;" & vbCrLf
  ReturnValue = ReturnValue & "}" & vbCrLf & vbCrLf
  ReturnValue = ReturnValue & "function swapImage() {" & vbCrLf
  ReturnValue = ReturnValue & "  var i,j=0,x,a=swapImage.arguments; document.sr=new Array; for(i=0;i<(a.length-2);i+=3)" & vbCrLf
  ReturnValue = ReturnValue & "   if ((x=findObj(a[i]))!=null){document.sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}" & vbCrLf
  ReturnValue = ReturnValue & "}" & vbCrLf
  ReturnValue = ReturnValue & "//-->" & vbCrLf
  ReturnValue = ReturnValue & "</script>" & vbCrLf
  Response.Write ReturnValue
End Sub

'------------------------------------------------------------------

Function AdminUser()
  ' Return a value of true if the term "admin" is in the Session("UserGroup") variable
  AdminUser = False
  If (InStr(Session("UserGroup"),"admin")>0) Then AdminUser = True
End Function

'------------------------------------------------------------------

Function CheckGroupMembership(sGroupName)
  ' Return a value of true if the sGroupName term is in the Session("UserGroup") variable
  CheckGroupMembership = False
  If (InStr(LCase(Session("UserGroup")),LCase(sGroupName))>0) Then CheckGroupMembership = True
End Function

'------------------------------------------------------------------

Sub DisplayCalendarCode()
  'Displays the JS and IFrame tags used to display the date picker calendar.
  'Requires that the calendar files be loaded under the "/components/cal/" folder
  'Use DisplayCalendarLink to display the image link where needed
  Dim strUserAgent
  strUserAgent = Request.ServerVariables("HTTP_USER_AGENT")
  If (Instr(1, strUserAgent, "MSIE", 1) <> 0) Then
    Response.Write "<IFRAME STYLE=""display:none;position:absolute;width:148;height:194;z-index=100"" ID=""CalFrame"" MARGINHEIGHT=0 MARGINWIDTH=0 NORESIZE FRAMEBORDER=0 SCROLLING=NO SRC=""/components/cal/calendar.htm""></IFRAME>" & vbCrLf
    Response.Write "<SCRIPT LANGUAGE=""javascript"" SRC=""/components/cal/calendar.js""></SCRIPT>" & vbCrLf
    Response.Write "<SCRIPT FOR=document EVENT=""onclick()"">" & vbCrLf
    Response.Write "<!--" & vbCrLf
    Response.Write "document.all.CalFrame.style.display=""none"";" & vbCrLf
    Response.Write "//-->" & vbCrLf
    Response.Write "</SCRIPT>" & vbCrLf
  End If
End Sub

'------------------------------------------------------------------

Sub DisplayCalendarFields(sFormName, sSubmitName, sSubmitValue, sTitleText, Width, bVerticalOrient, sOnChange)
  ' Displays the calendar fields and selection boxes (images).  
  ' DisplayCalendarCode must be called before calling this function.
  ' sFormName = The name of the form
  ' sSubmitName = The "name" value for the submit button.  If blank, the button won't be shown.
  ' sSubmitValue = The displayed "value" for the submit button
  ' sTitleText = The text to display at the top (or left) of the calendar. If blank, it won't be shown.
  ' bVerticalOrient = Determines whether to display the table vertically or horizontally.
  Dim iColSpan
  If bVerticalOrient Then
    iColspan = 2
  Else
    iColspan = 1
  End If
  
  Response.Write "<table border=""0"" cellpadding=""0"" cellspacing=""0"" bordercolor=""#000000"" width=""" & Width & """>"
  Response.Write "<tr>"
  If sTitleText <> "" Then
    Response.Write "<td colspan=""" & iColSpan & """ class=""FieldTitle"" style=""border:solid 1px black; text-align:center;"" nowrap>"
    Response.Write sTitleText
    Response.Write "</td>"
    If bVerticalOrient Then
      Response.Write "</tr>"
      Response.Write "<tr>"
    Else
      Response.Write "<td><img src=""images/spacer.gif"" width=""15"" height=""1""></td>"
    End If
  End If
  Response.Write "<td align=""right"" class=""FieldData"" nowrap>Start:</td>"
  Response.Write "<td class=""FieldData"" nowrap>"
  Response.Write "<input type=""text"" class=""inputbox"" name=""startdate"" size=""10"" maxlength=""10"" value=""" & dStartDate & """ onChange=""" & sOnChange & """>"
  DisplayCalendarLink 1, sFormName, "startdate", FormatDateTime(DateAdd("yyyy", -1, now()),2), FormatDateTime(DateAdd("yyyy", 1, now()),2)
  Response.Write "</td>"
  If bVerticalOrient Then
    Response.Write "</tr>"
    Response.Write "<tr>"
  Else
    Response.Write "<td><img src=""images/spacer.gif"" width=""15"" height=""1""></td>"
  End If
  Response.Write "<td align=""right"" class=""FieldData"" nowrap>End:</td>"
  Response.Write "<td class=""FieldData"" nowrap>"
  Response.Write "<input type=""text"" class=""inputbox"" name=""enddate"" size=""10"" maxlength=""10"" value=""" & dEndDate & """ onChange=""" & sOnChange & """>"
  DisplayCalendarLink 2, sFormName, "enddate", FormatDateTime(DateAdd("yyyy", -1, now()),2), FormatDateTime(DateAdd("yyyy", 1, now()),2)
  Response.Write "</td>"
  If sSubmitName <> "" Then
    If bVerticalOrient Then 
      Response.Write "</tr>"
      Response.Write "<tr>"
    Else
      Response.Write "<td><img src=""images/spacer.gif"" width=""15"" height=""1""></td>"
    End If
    Response.Write "<td colspan=""" & iColSpan & """ align=""center"" class=""FieldData"" nowrap>"
    Response.Write "<input type=""submit"" name=""" & sSubmitName & """ value=""" & sSubmitValue & """>"
    Response.Write "</td>"
  End If
  Response.Write "</tr>"
  Response.Write "</table>"        
End Sub

'------------------------------------------------------------------

Sub DisplayCalendarLink(iInstance, sFormName, sFormField, dStartDate, dEndDate)
  ' Displays calendar image link.  Requires the DisplayCalendarCode to be called beforehand
  ' iInstance is the the count of how many times the calendar image appears on a page
  ' dStartDate and dEndDate controll how many years/months/days to allow for selection.
  Response.Write "<a href=""javascript:ShowCalendar(document." & sFormName & ".dateimg" & iInstance & ",document." & sFormName & "." & sFormField & ",null, '" & dStartDate & "', '" & dEndDate & "')"" onclick=""event.cancelBubble=true;"">"
  Response.Write "<img align=""top"" border=""0"" height=""21"" id=""dateimg" & iInstance & """ src=""/components/cal/calendar.gif"" style=""POSITION: relative"" width=""34""></a>"
End Sub

'------------------------------------------------------------------

Sub DumpRStoTable(objRS)
  RWL "<table width=""100%"" border=""1"" cellspacing=""0"" cellpadding=""2"">"
  RWL "<tr>"
  Dim Field
  For Each Field In objRS.Fields
    RWL "<th>" & Field.Name & "</th>"
  Next
  RWL "</tr>"
  While Not objRS.EOF
    RWL "<tr>"
    For Each Field In objRS.Fields
      RWL "<td>" & Field.Value & "</td>"
    Next  
    RWL "</tr>"
    objRS.MoveNext
  Wend
  RWL "</table>"
End Sub

'------------------------------------------------------------------

Sub DisplayRSTable(objRS)
  ' Display the recordset as a sortable HTML table
  ' Any additional parameters must be passed in as a query string value
  ' Don't use query values that start with "rs_" - these belong to this function
  
  ' Generate the querystring to append to the field links
  ' by removing any existing query values that are part of this function
  Dim QueryValue, strQueryValues
  For Each QueryValue In Request.QueryString
    If Left(QueryValue,3) <> "rs_" Then
      strQueryValues = strQueryValues & QueryValue & "=" & Request.QueryString(QueryValue) & "&"
    End If
  Next

  ' Determine the sort order
  Dim sSortValue, sSortOrder
  sSortValue = Trim(Request.QueryString("rs_sortvalue"))
  sSortOrder = Trim(Request.QueryString("rs_sortorder"))
  If sSortOrder = "asc" Then sSortOrder = "desc" Else sSortOrder = "asc"
  If sSortValue <> "" Then objRS.Sort = sSortvalue & " " & sSortOrder

  ' Display the table      
  RWL "<table width=""100%"" border=""1"" cellspacing=""0"" cellpadding=""2"">"
  RWL "<tr>"
  Dim Field
  For Each Field In objRS.Fields
    RWL "<th><a href=""" & Request.ServerVariables("SCRIPT_NAME") & "?rs_sortvalue=[" & Server.URLEncode(Field.Name) & "]&rs_sortorder=" & sSortOrder & "&" & strQueryValues & """>" & Field.Name & "</a></th>"
  Next
  RWL "</tr>"
  While Not objRS.EOF
    RWL "<tr>"
    For Each Field In objRS.Fields
      RWL "<td>" & NullsToNBSP(Field.Value) & "</td>"
    Next  
    RWL "</tr>"
    objRS.MoveNext
  Wend
  RWL "</table>"
End Sub

'------------------------------------------------------------------

Sub SendHiddenFormFields(sFieldQualifier)
  ' Write all form fields receieved back to the form as hidden values
  ' The FieldQualifier is used to select only fields that begin with a certain string
  ' So that the other fields (such as submit) are not re-written
  Dim Field, ReturnValue, FieldValue
	For Each Field In Request.Form
	  If LCase(Left(Field,4)) = sFieldQualifier Then
		  ReturnValue = "<input type=""hidden"" " & "name=""" & Field & """ value="""
		  FieldValue = Request.Form(Field)
		  ReturnValue = ReturnValue + CStr(FieldValue) & """>" & vbCrLf
		  Response.Write ReturnValue
		End If
	Next
End Sub

'------------------------------------------------------------------
	
Sub ReadFormVariables(sFieldQualifier)
  ' Assign all received form fields to VB variables of the same name
  ' The FieldQualifier is used to select only fields that begin with a certain string
  ' So that the other fields (such as submit) are not re-written
  Dim Field, VarString 
	For Each Field In Request.Form
	  If LCase(Left(Field,4)) = sFieldQualifier Then
	    VarString = Right(Field,Len(Field) - Len(sFieldQualifier)) & " = Request.Form(""" & Field & """)" 
	    Execute(VarString)
	  End If
	Next
End Sub

'------------------------------------------------------------------

%>