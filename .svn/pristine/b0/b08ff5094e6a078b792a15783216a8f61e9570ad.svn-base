<%
'LogDebug ' Uncomment to enable extended debuging

Function LogDebug()
  ' Add entry to tbl_debuglog
  On Error Resume Next
  Dim sSQL
  sSQL = "INSERT INTO tbl_debuglog " 
  sSQL = sSQL & "(UserName, IPAddress, Page, Browser, PostData, QueryStringData, SessionData, HTTPHeader) VALUES ("
  sSQL = sSQL & "'" & Session("UserName") & "', "
  sSQL = sSQL & "'" & Request.ServerVariables("REMOTE_ADDR") & "', "
  sSQL = sSQL & "'" & ScriptName & "', "
  sSQL = sSQL & "'" & Request.ServerVariables("HTTP_USER_AGENT") & "', "
  sSQL = sSQL & "'" & ConvertPostDataToString & "', "
  sSQL = sSQL & "'" & QueryString & "', "
  sSQL = sSQL & "'" & GetSessionItemString & "', "
  sSQL = sSQL & "'" & Request.ServerVariables("ALL_HTTP") & "') "
  Dim cmdAddAccessLog
  Set cmdAddAccessLog = Server.CreateObject("ADODB.Command")
  cmdAddAccessLog.ActiveConnection = conn
  cmdAddAccessLog.CommandText = sSQL
  cmdAddAccessLog.Execute
  Set cmdAddAccessLog = Nothing
End Function

'-------------------------------------------------

Function DisplayDebugText(vData)
  ' Display vData for HTML presentation if user is an administrator
  If AdminUser Then
    Response.Write Server.HTMLEncode(vData) & "<br>"
  End If
End Function

'-------------------------------------------------

Function GetSessionItemString()
  Dim ReturnValue
  Dim sessitem
  ReturnValue = ReturnValue &  "SessionID:" & Session.SessionID & vbCrLf
  ReturnValue = ReturnValue &  "Count:" & Session.Contents.Count & vbCrLf
  For Each sessitem in Session.Contents
    If IsObject(Session.Contents(sessitem)) Then
      ReturnValue = ReturnValue & sessitem & ":Session object cannot be displayed." & vbCrLf
    Else
      If IsArray(Session.Contents(sessitem)) Then
         ReturnValue = ReturnValue &  "Array named " & Session.Content(sessitem) & vbCrLf
         For each objArray in Session.Contents(sessitem)
             ReturnValue = ReturnValue & vbCrLf
             Session.Contents(sessitem)(objArray) & vbCrLf
         Next
             ReturnValue = ReturnValue & vbCrLf
      Else
             ReturnValue = ReturnValue & sessitem & ":" & Session.Contents(sessitem) & vbCrLf
       End If
    End If
  Next 
  GetSessionItemString = ReturnValue
End Function

'-------------------------------------------------

Dim StepNumber
StepNumber = 0

Sub DisplayStep
  StepNumber = StepNumber + 1
  Response.Write("| Step #" & StepNumber & " |")
  Response.Flush
End Sub

'-------------------------------------------------

Sub DebugAlert(text)
	%>
	<script language=JavaScript>
	<!-- Hide	
		alert("<%= text %>");
	// -->
	</script>
	<%
End Sub

%>
