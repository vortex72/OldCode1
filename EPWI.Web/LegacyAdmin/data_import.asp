<!-- 
    METADATA 
    TYPE="typelib" 
    UUID="CD000000-8B95-11D1-82DB-00C04FB1625D"  
    NAME="CDO for Windows 2000 Library" 
-->  
<!-- #include file="std_includes\pageinit.asp" -->
<!-- #include file="std_includes\std_functions.asp" -->
<!-- #include file="data_import_maps.asp" -->
<% 
Server.ScriptTimeout = 180000
'If Session("UserName") = "" Then CheckLoginCookie()
'CheckUserAccess "group","admin","","login.asp?redirect=true" 

Dim AdminConn
Set AdminConn = Server.CreateObject("ADODB.Connection")
AdminConn.ConnectionString = "Server=tcp:epwi.database.windows.net,1433;Database=epwi.prod;User ID=epwi-admin@epwi;Password=3Pw12016-HP;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Provider=SQLOLEDB.1;Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;Use Encryption for Data=False;Tag with column collation when possible=False;"
AdminConn.Open
%>
<html>
<head>
<!-- #include file="page_layout\head_content.asp" -->
</head>
<body>
<!-- #include file="page_layout\body_start.asp" -->
<%
Dim bDebug
bDebug = False

' Get the name of the table to retreive records from the query string
Dim sDataToRetreive
sDataToRetreive = Request.QueryString("datatable")

' Library and Table on AS/400 to Retreive Records From
Dim sAS400Table, sWebTable
sAS400Table = "EPWCOMN." & sDataToRetreive

' Total Record Counter
Dim iTotalRecordsProcessed
iTotalRecordsProcessed = 0

Dim iRecordCountMultiplier, iRecordsToRetreive, iStartRecord, iEndRecord
iRecordCountMultiplier = 0
iRecordsToRetreive = 10000

Dim sSQL

Select Case sDataToRetreive

  Case "INVMSPF"
    sWebTable = "tbl_kitcat_INVMSPC"
    InitiateImport
    While iEndRecord < 750000
      On Error Resume Next
      iStartRecord = iRecordCountMultiplier * iRecordsToRetreive
      iEndRecord = iStartRecord + iRecordsToRetreive - 1
      sSQL = "SELECT DISTINCT * FROM " & sAS400Table & " WHERE NI >= " & iStartRecord & " AND NI <= " & iEndRecord
      RetreiveRecords sSQL
      ImportData aINVMSPFMap, True, "Processing NI Codes " & LeadingZeros(iStartRecord,6) & " to " & LeadingZeros(iEndRecord,6)
      iRecordCountMultiplier = iRecordCountMultiplier + 1
      If Err.number > 0 Then
        RWL "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b style=""color:#FF0000;"">ERROR:</b> " & err.Description
        RWL "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;SQL = " & sSQL
      End If
      On Error Goto 0
      RWL "<br>"
    Wend

  Case "INVMS1PF"
    sWebTable = "tbl_kitcat_INVMS1PC"
    InitiateImport
    While iEndRecord < 750000
      On Error Resume Next
      iStartRecord = iRecordCountMultiplier * iRecordsToRetreive
      iEndRecord = iStartRecord + iRecordsToRetreive - 1
      sSQL = "SELECT DISTINCT * FROM " & sAS400Table & " WHERE NI >= " & iStartRecord & " AND NI <= " & iEndRecord
      RetreiveRecords sSQL
      ImportData aINVMS1PFMap, True, "Processing NI Codes " & LeadingZeros(iStartRecord,6) & " to " & LeadingZeros(iEndRecord,6)
      iRecordCountMultiplier = iRecordCountMultiplier + 1
      If Err.number > 0 Then
        RWL "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b style=""color:#FF0000;"">ERROR:</b> " & err.Description
        RWL "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;SQL = " & sSQL
      End If
      On Error Goto 0
      RWL "<br>"
    Wend  
    
  Case "KITHPF"
    sWebTable = "tbl_kitcat_KITHPC"
    InitiateImport
    RetreiveRecords "SELECT DISTINCT * FROM " & sAS400Table
    ImportData aKITHPFMap, False, "Processing All Records"

  Case "KITPPF"
    sWebTable = "tbl_kitcat_KITPPC"
    InitiateImport
    RetreiveRecords "SELECT DISTINCT * FROM " & sAS400Table
    ImportData aKITPPFMap, False, "Processing All Records"

  Case "KITPNPF"
    sWebTable = "tbl_kitcat_KITPNPC"
    InitiateImport
    RetreiveRecords "SELECT DISTINCT * FROM " & sAS400Table
    ImportData aKITPNPFMap, False, "Processing All Records"

  Case "SIZEUPC"
    sWebTable = "tbl_kitcat_SIZEUPC"
    InitiateImport
    RetreiveRecords "SELECT DISTINCT * FROM " & sAS400Table
    ImportData aSIZEUPCMap, False, "Processing All Records"

  Case "ILDESCPF"
    sWebTable = "tbl_kitcat_ILDESCPF"
    InitiateImport
    RetreiveRecords "SELECT DISTINCT * FROM " & sAS400Table & " WHERE LINE <> ''"
    ImportData aILDESCPFMap, False, "Processing All Records"
    RWL "<hr>"
    RWL "Updating displayable link<br>"
    AdminConn.Execute "UPDATE tbl_kitcat_ILDESCPF SET Displayable = 0 WHERE (LINE < 'a%')"

  Case "KZENGPF"
    sWebTable = "tbl_kitcat_KZENGPF"
    InitiateImport
    RetreiveRecords "SELECT DISTINCT * FROM " & sAS400Table
    ImportData aKZENGPFMap, False, "Processing All Records"

  Case "KZKTNPF"
    ' For some reason, this function has a wierd error where duplicate records get returned erroniously
    ' 2006-12-30 - This error was fixed by adding a new KitNoteID primary key field... this should correct the problem
    sWebTable = "tbl_kitcat_KZKTNPF"
    InitiateImport ' Comment out to manually add records beyond 'A'
    Dim iChar
    For iChar = Asc("A") To Asc("Z") ' Change to another letter to start after 'A'
      ' The CATGPC field is not inherantly in the KZKTNPF, so it has to be joined by slecting the first category available for any note line 
      RetreiveRecords "SELECT DISTINCT EPWCOMN.KZKTNPF.KXEG, EPWCOMN.KZKTNPF.KZKN#, (SELECT MIN(KCATG) AS CATGPC FROM EPWCOMN.KZPGCPF WHERE KZLI# = EPWCOMN.KZKNRPF.KZLI#) AS CATGPC, EPWCOMN.KZKTNPF.KZNT1, EPWCOMN.KZKTNPF.KZNT2, EPWCOMN.KZKTNPF.KZNT3, EPWCOMN.KZKTNPF.KZNT4 FROM {OJ EPWCOMN.KZKTNPF LEFT OUTER JOIN EPWCOMN.KZKNRPF ON EPWCOMN.KZKTNPF.KXEG = EPWCOMN.KZKNRPF.KXEG AND EPWCOMN.KZKTNPF.KZKN# = EPWCOMN.KZKNRPF.KZKN# LEFT OUTER JOIN EPWCOMN.KZPGCPF ON EPWCOMN.KZKNRPF.KZLI# = EPWCOMN.KZPGCPF.KZLI#} WHERE EPWCOMN.KZKTNPF.KXEG LIKE '" & Chr(iChar) & "%'"
      ImportData aKZKTNPFMap, True, "Processing Kits Starting with '" & Chr(iChar) & "'"
      RWL "<br>"
    Next
    
    
  Case "KZRELPF"
    sWebTable = "tbl_kitcat_KZRELPC"
    InitiateImport
    RetreiveRecords "SELECT DISTINCT * FROM " & sAS400Table
    ImportData aKZRELPFMap, False, "Processing All Records"

  Case "OLCENPF"
    sWebTable = "tbl_kitcat_OLCENPC"
    InitiateImport
    RetreiveRecords "SELECT DISTINCT * FROM " & sAS400Table
    ImportData aOLCENPFMap, False, "Processing All Records"

  Case "OLCMKPF"
    sWebTable = "tbl_kitcat_OLCMKPF"
    InitiateImport
    RetreiveRecords "SELECT DISTINCT * FROM " & sAS400Table
    ImportData aOLCMKPFMap, False, "Processing All Records"
    ' Remember to set the StandardKit Field - Perhaps not needed

  Case "OLCECPF"
    sWebTable = "tbl_kitcat_OLCECPC"
    InitiateImport
    RetreiveRecords "SELECT DISTINCT * FROM " & sAS400Table & " WHERE OECOMM <> ''"
    ImportData aOLCECPFMap, False, "Processing All Records"

  Case "OLCPTNPF"
    sWebTable = "tbl_kitcat_OLCPTNPC"
    InitiateImport
    While iEndRecord < 500000
      On Error Resume Next
      iStartRecord = iRecordCountMultiplier * iRecordsToRetreive
      iEndRecord = iStartRecord + iRecordsToRetreive - 1
      sSQL = "SELECT DISTINCT * FROM " & sAS400Table & " WHERE OPNI >= " & iStartRecord & " AND OPNI <= " & iEndRecord
      RetreiveRecords sSQL
      ImportData aOLCPTNPFMap, True, "Processing NI Codes " & LeadingZeros(iStartRecord,6) & " to " & LeadingZeros(iEndRecord,6)
      iRecordCountMultiplier = iRecordCountMultiplier + 1
      If Err.number > 0 Then
        RWL "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b style=""color:#FF0000;"">ERROR:</b> " & err.Description
        RWL "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;SQL = " & sSQL
      End If
      On Error Goto 0
      RWL "<br>"
    Wend
    
  Case "AIKITHPF"
    sWebTable = "tbl_kitcat_AIKITHPC"
    InitiateImport
    RetreiveRecords "SELECT DISTINCT * FROM " & sAS400Table
    ImportData aAIKITHPFMap, False, "Processing All Records"

  Case "AIKITPPF"
    sWebTable = "tbl_kitcat_AIKITPPC"
    InitiateImport
    RetreiveRecords "SELECT DISTINCT * FROM " & sAS400Table
    ImportData aAIKITPPFMap, False, "Processing All Records"

  Case "ZSHPVPF"
    sWebTable = "tbl_kitcat_ZSHPVPF"
    InitiateImport
    RetreiveRecords "SELECT ZSWHSE, ZSSEQ, ZSVC, ZSVIA FROM EPWCOMN.ZSHPVPF"
    ImportData aZSHPVPFMap, False, "Processing All Records"
    
  Case ""
    RWL "Import Data For:<br>"
    RWL "&nbsp;-&nbsp;<a href=""" & ScriptName & "?datatable=ILDESCPF"">ILDESCPF</a><br>"
    RWL "&nbsp;-&nbsp;<a href=""" & ScriptName & "?datatable=KITHPF"">KITHPF</a><br>"
    RWL "&nbsp;-&nbsp;<a href=""" & ScriptName & "?datatable=KITPPF"">KITPPF</a><br>"
    RWL "&nbsp;-&nbsp;<a href=""" & ScriptName & "?datatable=KITPNPF"">KITPNPF</a><br>"
    RWL "&nbsp;-&nbsp;<a href=""" & ScriptName & "?datatable=KZENGPF"">KZENGPF</a><br>"
    RWL "&nbsp;-&nbsp;<a href=""" & ScriptName & "?datatable=KZKTNPF"">KZKTNPF</a><br>"
    RWL "&nbsp;-&nbsp;<a href=""" & ScriptName & "?datatable=KZRELPF"">KZRELPF</a><br>"
    RWL "&nbsp;-&nbsp;<a href=""" & ScriptName & "?datatable=OLCENPF"">OLCENPF</a><br>"
    RWL "&nbsp;-&nbsp;<a href=""" & ScriptName & "?datatable=OLCMKPF"">OLCMKPF</a><br>"
    'RWL "&nbsp;-&nbsp;<a href=""" & ScriptName & "?datatable=OLCECPF"">OLCECPF</a><br>"
    'RWL "&nbsp;-&nbsp;<a href=""" & ScriptName & "?datatable=OLCPTNPF"">OLCPTNPF</a> (Warning, this table takes over 15 minutes to import)<br>"
    RWL "&nbsp;-&nbsp;<a href=""" & ScriptName & "?datatable=SIZEUPC"">SIZEUPC</a> (Needs to be rebuilt on the AS400)<br>"
    RWL "&nbsp;-&nbsp;<a href=""" & ScriptName & "?datatable=INVMSPF"">INVMSPF</a> (Warning, this table takes over 1 hour to import)<br>"
    RWL "&nbsp;-&nbsp;<a href=""" & ScriptName & "?datatable=INVMS1PF"">INVMS1PF</a><br>"
    RWL "&nbsp;-&nbsp;<a href=""" & ScriptName & "?datatable=AIKITHPF"">AIKITHPF - ACES</a><br>"
    RWL "&nbsp;-&nbsp;<a href=""" & ScriptName & "?datatable=AIKITPPF"">AIKITPPF - ACES</a><br>"
    RWL "&nbsp;-&nbsp;<a href=""" & ScriptName & "?datatable=ZSHPVPF"">ZSHPVPF</a><br>"
    Response.End
    
  Case Else
    RWL "ERROR: Invalid Table Name"
    RWL "<a href=""" & ScriptName & """>RETURN TO DATA IMPORT MENU</a><br>"
    Response.End

End Select

RWL "<hr>"
RWL "Data import complete: " & iTotalRecordsProcessed & " total records processed<br>"
RWL "&nbsp;<br>"
RWL "<a href=""" & ScriptName & """>RETURN TO DATA IMPORT MENU</a><br>"
AdminConn.Close

' Send email notification    
Dim cdoConfig
Set cdoConfig = CreateObject("CDO.Configuration")  
With cdoConfig.Fields  
    .Item("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2
    .Item("http://schemas.microsoft.com/cdo/configuration/smtpserver") = "smtp.office365.com"
    '.Item("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = 587
    .Item("http://schemas.microsoft.com/cdo/configuration/smtpusessl") = True
    .Item("http://schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout") = 240
    .Item("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate") = 1
    .Item("http://schemas.microsoft.com/cdo/configuration/sendusername") = "admin@epwi.net"
    .Item("http://schemas.microsoft.com/cdo/configuration/sendpassword") = "955Decatur"
    .Update
End With 
Dim cdoMessage
Set cdoMessage = CreateObject("CDO.Message")  
With cdoMessage 
    Set .Configuration = cdoConfig 
    .From = "admin@epwi.net"
    .To = "davidvw@epwi.net" 
    '.To = "bflejterski@hattonpoint.com" 
    .Subject = "AS/400 Data Import for " & sDataToRetreive & " Is Complete" 
    .TextBody = "The import of " & iTotalRecordsProcessed & " records for the " & sDataToRetreive & " table has completed successfully"
    .Send 
End With 


Set cdoMessage = Nothing  
Set cdoConfig = Nothing  

%>
<!-- #include file="page_layout\body_end.asp" -->
</body>
</html>
<%

'---------------------------------------------------------

Sub InitiateImport()
  AdminConn.CommandTimeout = 600

  'Dim sDateValue
  'sDateValue = CStr(Year(Now()) & "-" & Month(Now()) & "-" & Day(Now()) & "_" & Hour(Now()) & "-" & Minute(Now()))

  RWL "Initiating data retreival from " & sAS400Table & " to " & sWebTable & " at: " & Now() & "<br>"
  Response.Flush

  RWL "Deleting old backup tables<br>"
  Response.Flush
  AdminConn.Execute "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" & sWebTable & "_BACKUP]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [dbo].[" & sWebTable & "_BACKUP]"

  'RWL "Backing up current data to " & sWebTable & "_BACKUP_" & sDateValue & "<br>"
  'Response.Flush
  'AdminConn.Execute "SELECT * INTO [dbo." & sWebTable & "_BACKUP_" & sDateValue & "] FROM " & sWebTable

  RWL "Purging data from " & sWebTable & "<br>"
  Response.Flush
  AdminConn.Execute "DELETE FROM " & sWebTable
  
  RWL "<hr>"
    
End Sub

'---------------------------------------------------------

Function RetreiveRecords(sSQL)
  ' Retreive records based on provided SQL from AS/400 and dump to a tab delimited text file
  ' in the \rawdata folder named for the web table name

  Dim objXMLHTTP
  Set objXMLHTTP = Server.CreateObject("WinHTTP.WinHTTPRequest.5.1")
  objXMLHTTP.open "POST", eSynURL & "/data_export.asp?sql=" & Server.URLEncode(sSQL), false
  objXMLHTTP.SetRequestHeader "Content-Type", "application/x-www-form-urlencoded;"
  objXMLHTTP.send
  Dim strRecords
  strRecords = objXMLHTTP.ResponseText
  strRecords = Replace(strRecords,Chr(10), Chr(13) & Chr(10))
  Set objXMLHTTP = Nothing

  ' Save the text file
  Dim objFS, objFile
  Set objFS = Server.CreateObject("Scripting.FileSystemObject")
  Set objFile = objFS.CreateTextFile(Server.MapPath("rawdata\" & sWebTable & ".txt"), True, False)
  objFile.Write strRecords
  objFile.Close
  Set objFile = Nothing
  Set objFS = Nothing
  
  strRecords = ""

End Function

'---------------------------------------------------------

Sub ImportData(aFieldArray, bCondensedResults, sNotes)

  RWL "<table style='table-layout:fixed'>"
  Dim objRS
  Set objRS = Server.CreateObject("ADODB.Recordset")
  objRS.ActiveConnection = AdminConn
  objRS.CursorLocation = adUseServer
  objRS.CursorType = adOpenKeyset
  objRS.LockType = adLockOptimistic

  RWL sNotes & ": "
  Response.Flush

  Dim objFS, objFile
  Set objFS = Server.CreateObject("Scripting.FileSystemObject")
  Set objFile = objFS.OpenTextFile(Server.MapPath("rawdata\" & sWebTable & ".txt"), 1, False)
  
  Dim sFieldNames
  sFieldNames = objFile.ReadLine

  Dim aFieldNames
  aFieldNames = Split(sFieldNames, vbTab)

  Dim FieldCount
  FieldCount = UBound(aFieldNames) - 1

  objRS.Source = adCmdTable
  objRS.Open sWebTable

  Dim FieldName
  Dim aFieldValues

  Dim RecordCount
  RecordCount = 0

  Dim iField
  While objFile.AtEndOfStream = False
    objRS.AddNew
    sFieldNames = objFile.ReadLine
    aFieldValues = Split(sFieldNames, vbTab)
    For iField = 0 To FieldCount
      FieldName = MapFieldName(aFieldArray, aFieldNames(iField))
      If FieldName <> "N/A" Then
        If bDebug Then
          RWL "<br>" & aFieldNames(iField) & " > " & MapFieldName(aFieldArray, aFieldNames(iField))
          RWL " | " & MapFieldName(aFieldArray, aFieldNames(iField)) & " = " & Trim(aFieldValues(iField))
        End If
        On Error Resume Next
        objRS(MapFieldName(aFieldArray, aFieldNames(iField))) = BlanksToNull(Trim(aFieldValues(iField)))
        If Err.number > 0 Then
          RWL "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b style=""color:#FF0000;"">ERROR:</b> " & err.Description
          RWL "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Field Name = " & aFieldNames(iField) & " | Field Value = " & aFieldValues(iField)
        End If
        On Error Goto 0
      End If
    Next
    objRS.Update
    
    If bCondensedResults Then
      If RecordCount Mod 10 = 0 Then 
        RW "."
        Response.Flush
      End If
    Else
      If RecordCount Mod 100 = 0 Then 
        RW "<br>" & LeadingZeros(RecordCount,6) & " to " & LeadingZeros(RecordCount + 99,6) & ": "
      End If
      RW "."
      Response.Flush
    End If
    
    RecordCount = RecordCount + 1
  Wend
  If bCondensedResults Then RWL " " & RecordCount & " records processed"
  
  iTotalRecordsProcessed = iTotalRecordsProcessed + RecordCount

  objRS.Close
  Set objFS = Nothing
  Set objRS = Nothing
  RWL "</table>"
End Sub

'---------------------------------------------------------

Function MapFieldName(aFieldArray,OrigField)
  MapFieldName = ""
  Dim j
  For j = 0 To UBound(aFieldArray,1)
    If aFieldArray(j,0) = OrigField Then MapFieldName = aFieldArray(j,1)
  Next
  If MapFieldName = "" Then MapFieldName = OrigField
End Function

%>

<!-- #include file="std_includes\pagerelease.asp" -->
