<%
Sub DisplayEMailContactForm(strTemplateFileName)
	'------------------------------------------------
	' 
	'------------------------------------------------
  ' Requires the Persists APSMail 4.4+ Component (free)
  ' http://www.aspemail.com/download.html
  Dim strHTMLCode, strBeginCode, strLoopCode, strEndCode
  If Request.Form.Count > 0 Then
  	Dim strURL
  	Dim objMail, i, strBody
  	For i = 1 To Request.Form.Count
  		strBody = strBody & Request.Form.Key(i) & ": " & Request.Form.Item(i) & vbCrLf
  	Next
  	Set objMail = Server.CreateObject("Persits.MailSender")
    objMail.Username = MailUsername
    objMail.Password = MailPassword
  	objMail.Host = MailHost
  	objMail.From = MailFromAddress
  	objMail.FromName = MailFromName
  	objMail.AddAddress MailReceipientAddress, MailReceipientName
  	objMail.Subject = MailSubject
  	objMail.Body = strBody
  	On Error Resume Next
  	objMail.SendToQueue
  	Set objMail = Nothing
  	If Err <> 0 Then
  	   Response.Write "Error encountered: " & Err.Description & "<p>"
  	   Response.Write "Please contact " & CompanyName & " directly for more information"
  	   Response.End
  	Else
  		Response.Write MailConfirmationText
  	End If
  Else
    strHTMLCode = ReadFromTemplate(strSectionTemplateDir & "\email_forms\" & strTemplateFileName)
    strHTMLCode = Replace(strHTMLCode, "{#-FormAction-#}", Request.ServerVariables("SCRIPT_NAME") & "?" & Request.ServerVariables("QUERY_STRING"))
    Response.Write strHTMLCode
  End If
End Sub

'------------------------------------------------------------------

Sub DisplayEMailReferral(strTemplateFileName)
	'------------------------------------------------
	' 
	'------------------------------------------------
  ' Requires the Persists APSMail 4.4+ Component (free)
  ' http://www.aspemail.com/download.html
  Dim strHTMLCode, strBeginCode, strLoopCode, strEndCode
  If Request.Form.Count > 0 Then
  	Dim strURL
  	Dim objMail, i, strBody
  	Dim AddressList, Address
  	Set objMail = Server.CreateObject("Persits.MailSender")
    objMail.Username = MailUsername
    objMail.Password = MailPassword
  	objMail.Host = MailHost
    'objMail.Port = MailPort
    objMail.SSL = MailUsesSSL
  	objMail.From = Request.Form("SenderAddress")
  	objMail.FromName = Request.Form("SenderName")
  	AddressList = Split(Request.Form("RecipientAddresses"),",")
  	For Each Address In AddressList
  	  objMail.AddAddress Trim(Address)
  	Next
  	objMail.AddCC Trim(Request.Form("SenderAddress")), Trim(Request.Form("SenderName"))
  	objMail.AddBcc MailReceipientAddress, MailReceipientName
  	objMail.Subject = Request.Form("Subject")

    strBody = strBody & "This message was sent to you by " & Request.Form("SenderName") & "." & vbCrLf & vbCrLf
    strBody = strBody & Request.Form("Comments") & vbCrLf & vbCrLf
    strBody = strBody & "Be sure to visit " & CompanyName & " at " & CompanyURL & "!"& vbCrLf & vbCrLf
    strBody = strBody & "---------------------------------------------" & vbCrLf & vbCrLf
    'strBody = strBody & "This e-mail message was generated by Blue Ribbon Technologies" & vbCrLf
    'strBody = strBody & "http://www.blueribbontech.com" & vbCrLf

  	objMail.Body = strBody

  	On Error Resume Next
  	objMail.SendToQueue
  	Set objMail = Nothing
  	If Err <> 0 Then
  	   Response.Write "Error encountered: " & Err.Description & "<p>"
  	   Response.Write "Please contact " & CompanyName & " directly for more information"
  	   Response.End
  	Else
  		Response.Write MailConfirmationText
  	End If
  Else
    strHTMLCode = ReadFromTemplate(strSectionTemplateDir & "\email_forms\" & strTemplateFileName)
    strHTMLCode = Replace(strHTMLCode, "{#-FormAction-#}", Request.ServerVariables("SCRIPT_NAME") & "?" & Request.ServerVariables("QUERY_STRING"))
    Response.Write strHTMLCode
  End If
End Sub

'------------------------------------------------------------------

Sub SendMail(subject, body, bIsHTML)
	'------------------------------------------------
	' Standardized Email Send Routine that accepts a subject and body
	'------------------------------------------------
  ' Requires the Persists APSMail 4.4+ Component (free)
  ' http://www.aspemail.com/download.html

	Dim objMail
	Set objMail = Server.CreateObject("Persits.MailSender")
    objMail.Username = MailUsername
    objMail.Password = MailPassword
  	objMail.Host = MailHost
    'objMail.Port = MailPort
    objMail.SSL = MailUsesSSL
    objMail.From = MailFromAddress
    objMail.FromName = MailFromName
    objMail.AddAddress MailReceipientAddress, MailReceipientName
    objMail.Subject = subject
    objMail.Body = body
    objMail.IsHTML = bIsHTML
  
	On Error Resume Next
	objMail.SendToQueue
	Set objMail = Nothing
	If Err <> 0 Then
  	   Response.Write "Error encountered: " & Err.Description & "<p>"
  	   Response.Write "Please contact " & CompanyName & " directly for more information"
  	   Response.End
	End If
End Sub

%>
