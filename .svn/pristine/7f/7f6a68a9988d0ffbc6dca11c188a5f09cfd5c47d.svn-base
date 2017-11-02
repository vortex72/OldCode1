<%
Function LoginUser(strUserName, strPassword)
	'------------------------------------------------
	' Take a passed UserName and Password and verify
	' that info against the database.  Return a validation
	' code and set the server variables if valid.
	'------------------------------------------------
	
  Dim LoginResult, iUserID, sFirstName, sLastName, sPassword, iUserLevel, sEmail
  Dim sGroups, dExpireDate, bActiveAccount, iZipCode, bAccessAccountStatus
  Dim iCustomerID, sCustomerCompanyCode, sCompany, iPricingFactor, bAccessInvoiceCost, bAccessEliteCost
  LoginResult = 0
    
  ' Call the GetUserInfo SP
  Set cmd = Server.CreateObject("ADODB.Command")
  Set cmd.ActiveConnection = conn
  cmd.CommandType = adCmdStoredProc
  cmd.CommandText = "usp_GetUserInfo"
  cmd.Parameters.Append = cmd.CreateParameter("@sUser",adChar,adParamInput,20,strUserName)
  Dim rsUserInfo
  Set rsUserInfo = cmd.Execute
  Set cmd = Nothing

  ' If there are no records returned, set result to 1
  ' otherwise load the DB information
  If rsUserInfo.BOF And rsUserInfo.EOF Then
    LoginResult = 1
  Else
    iUserID = rsUserInfo("UserID")
    sFirstName = rsUserInfo("FirstName")
    sLastName = rsUserInfo("LastName")
    sPassword = rsUserInfo("Password")
    sEmail = rsUserInfo("Email")
    iUserLevel = rsUserInfo("UserLevel")
    iZipCode = rsUserInfo("ZipPostal")
    dExpireDate = rsUserInfo("ExpireDate")
    bActiveAccount = rsUserInfo("ActiveAccount")
    bAccessAccountStatus = rsUserInfo("AccessAccountStatus")
    
		If rsUserInfo("EPWIUserID") > 0 Then iCustomerID = rsUserInfo("EPWIUserID")
		If rsUserInfo("EPWIWhse") <> "" Then sCustomerCompanyCode = rsUserInfo("EPWIWhse")
		If rsUserInfo("Company") <> "" Then sCompany = rsUserInfo("Company")
		iPricingFactor = rsUserInfo("PricingFactor")
		bAccessInvoiceCost = rsUserInfo("AccessInvoiceCost")
		bAccessEliteCost = rsUserInfo("AccessEliteCost")
  End If
    
  ' Create a one-dim array with all of the groupnames
  sGroups = ""
  While Not rsUserInfo.EOF
    sGroups = sGroups & rsUserInfo("GroupName")
    rsUserInfo.MoveNext
    If Not rsUserInfo.EOF Then sGroups = sGroups & ","
  Wend
    
  'LoginResults:
  '0 = Valid User
  '1 = User Does Not Exist
  '2 = No Password Entered
  '3 = Invalid Password 
  '4 = Inactive Account
  '5 = Account Expired
  '9 = Database Error

  If LoginResult = 0 Then
    If strPassword = "" Then 
      LoginResult = 2
    ElseIf (strPassword <> "") And (sPassword <> strPassword) Then 
      LoginResult = 3
    ElseIf bActiveAccount <> True Then 
      LoginResult = 4
    ElseIf (dExpireDate <> "") And (dExpireDate < Date()) Then 
      LoginResult = 5
    End If
  End If
    
  ' If the user is valid, set server variables
  If LoginResult = 0 Then
    Session("UserID") = iUserID
    Session("UserLevel") = iUserLevel
    Session("UserGroup") = sGroups
    Session("UserName") = strUserName
    Session("UserFirstName") = sFirstName
    Session("UserLastName") = sLastName
		Session("Email") = sEmail
    Session("UserZipCode") = iZipCode
    Session("AccessAccountStatus") = bAccessAccountStatus
    Session("iCustomerID") = iCustomerID    
		Session("sCustomerCompanyCode") = sCustomerCompanyCode
		Session("CompanyName") = sCompany
    Session("PricingFactor") = iPricingFactor
		Session("AccessInvoiceCost") = bAccessInvoiceCost
		Session("AccessEliteCost") = bAccessEliteCost    
  End If

  ' Return result      
  LoginUser = LoginResult
End Function

'------------------------------------------------------------------

Function CheckLoginCookie()

' --- TEMPORARY DEBUGGING CODE (8/7/2009) -----------
  Dim CookieValues, strKey, strSubKey
  For Each strKey In Request.Cookies
    CookieValues = CookieValues + strKey & "=" & Request.Cookies(strKey) & "|"
  Next
  LogAccess "UserName,IPAddress,Type,Notes", "'" & sUser & "','" & Request.ServerVariables("REMOTE_ADDR") & "','CookieValues','" & CookieValues & "'"      
' --- END TEMPORARY DEBUGGING CODE ------------------

' Returns true if the user was able to login via the cookie  
  Dim sUser, sPass
  CheckLoginCookie = False
  If Request.Cookies("UserInfo")("UserName") <> ""  Then
    sUser = Request.Cookies("UserInfo")("UserName")
    sPass = Request.Cookies("UserInfo")("Password")
    sPass = DeCrypt(sPass)
    
    ' Addition 3/12/2009 - FogBugz Case #262 - Remember selected view in session and cookie
    If Request.Cookies("UserInfo")("SelectedView") <> 2 Then
      Session("SelectedView") = 1
    Else
      Session("SelectedView") = 2
    End If    
    
  	If LoginUser(sUser, sPass) = 0 Then
      LogAccess "UserName,IPAddress,Type,Notes", "'" & sUser & "','" & Request.ServerVariables("REMOTE_ADDR") & "','Auto Login','" & Request.ServerVariables("HTTP_USER_AGENT") & "'"      
      CheckLoginCookie = True
		End If
  End If
End Function

'------------------------------------------------------------------

Sub LogOutUser()
' --- TEMPORARY DEBUGGING CODE (9/2/2009) -----------
LogAccess "UserName,IPAddress,Type,Notes", "'" & sUser & "','" & Request.ServerVariables("REMOTE_ADDR") & "','Logout',''"      
' --- END TEMPORARY DEBUGGING CODE ------------------

  Response.Cookies("UserInfo")("UserName") = ""
  Response.Cookies("UserInfo")("Password") = ""
  Response.Cookies("UserInfo").Expires = "January 1, 1980"
  Session("UserID") = ""
  Session("UserLevel") = ""
  Session("UserGroup") = ""
  Session("UserName") = ""
  Session("UserFirstName") = ""
  Session("UserLastName") = ""
  Session("UserZipCode") = ""
  Session.Abandon
End Sub

'------------------------------------------------------------------

Class LostPasswordUser
  Public strFirstName
  Public strLastName
  Public strUserName
  Public strEmail
  Public strPass
  Public InvalidUser

  Public Function GetLostPasswordUserInfo(email)
    Dim rsUsers, strSQL
    strSQL = "SELECT * FROM tbl_users WHERE email='" & email & "'"
    Set rsUsers = Server.CreateObject("ADODB.Recordset")
    rsUsers.Open strSQL,conn,adOpenForwardOnly,adLockReadOnly   
    If Not (rsUsers.BOF And rsUsers.EOF) Then
      strEmail = rsUsers("Email")
      strPass = rsUsers("Password")
      strUserName = rsUsers("UserName")
      strFirstName = rsUsers("FirstName")
      strLastName = rsUsers("LastName")
      InvalidUser = False
    Else
      InvalidUser = True
    End If
    rsUsers.Close
    Set rsUsers = Nothing
  End Function

  Sub MailLostPassEmail
  	Dim strURL
  	Dim objMail, i, strBody
  	strBody = "You have requested to receive your user name and password to the " & CompanyName & " website (" & CompanyURL & ")." & vbCrLf
  	strBody = strBody & "Your Username is: " & strUserName & vbCrLf
  	strBody = strBody & "Your Password is: " & strPass & vbCrLf & vbCrLf
  	strBody = strBody & "If you request further assistance, please contact" & vbCrLf
  	strBody = strBody & CompanyName & " directly at " & CompanyPhone & "."
  	Set objMail = Server.CreateObject("Persits.MailSender")
    objMail.Username = MailUsername
    objMail.Password = MailPassword
  	objMail.Host = MailHost
    'objMail.Port = MailPort
    objMail.SSL = MailUsesSSL
  	objMail.From = MailFromAddress
  	objMail.FromName = MailFromName
  	objMail.AddAddress strEmail, strFirstName & " " & strLastName
  	objMail.Subject = CompanyURL & " Password Request"
  	objMail.Body = strBody
  	On Error Resume Next
  	objMail.SendToQueue
  	Set objMail = Nothing
  	If Err <> 0 Then
  	   Response.Write "Error encountered: " & Err.Description & "<p>"
  	   Response.Write "Please contact UES directly for more information"
  	Else
      Response.Write "<table width=""400"" border=""0"" cellspacing=""0"" cellpadding=""0"" align=""center"">" & vbCrLf
      Response.Write "  <tr>" & vbCrLf
      Response.Write "    <td colspan=""2"" class=""AppTitle""><p>&nbsp;</p><center>Lost Password Request Form</center></td>" & vbCrLf
      Response.Write "  </tr>" & vbCrLf
      Response.Write "  <tr>" & vbCrLf
      Response.Write "    <td colspan=""2"" align=""center"">" & vbCrLf
      Response.Write "      <p><font face=""Arial, Helvetica"">You password has been mailed to<br><b>" & strEmail & "</b>.<br>" & vbCrLf
      Response.Write "      If you need further assistance, please contact<br>" & CompanyName & "<br>directly at " & CompanyPhone & ".<br><i>Thank you!</i></p><p>&nbsp;</p>" & vbCrLf
      Response.Write "    </td>" & vbCrLf
      Response.Write "  </tr>" & vbCrLf
      Response.Write "</table>" & vbCrLf
  	End If
  End Sub		
End Class

'------------------------------------------------------------------

Sub RedirectUser(strRedirectPage)

  If strRedirectPage <> "" Then
    If Not (InStr(strRedirectPage,"default") > 0) Then ' If redirecting to default, refresh all frames
      Response.Redirect(strRedirectPage)
    Else
    	Response.Write "<SCRIPT LANGUAGE=""JavaScript"">" & vbCrLf
    	Response.Write "<!-- Begin" & vbCrLf
  '    If InStr(strRedirectPage,"default.asp") > 0 Then ' If redirecting to default.asp, refresh all frames
        Response.Write "window.top.location.href = """ & strRedirectPage & """"& vbCrLf
  '    Else
  '      Response.Write "window.self.location.href = """ & strRedirectPage & """"& vbCrLf
  '    End If
    	Response.Write "//  End -->" & vbCrLf
    	Response.Write "</script>" & vbCrLf
    	Response.Write "<p>&nbsp;</p>" & vbCrLf
    	Response.Write "<div align=""center"">" & vbCrLf
    	Response.Write "Your browser is about to be refreshed,<br>" & vbCrLf
    	Response.Write "or you may click <a href=""" & strRedirectPage & """ target=_top><b>here</b></a> to manually reload." & vbCrLf
    	Response.Write "</div>" & vbCrLf
    	Response.End
    End If
  Else
    Response.Redirect("default.asp")
  End If
End Sub		 

'------------------------------------------------------------------

Function CheckUserAccess(AccessType,Value,AllowRedirect,DenyRedirect)
  ' Return a true value if the users value is appropriate for the access type
  ' Access Types: "group", "level", "username"
  ' Make sure to CheckLoginCookie() before calling this function
  CheckUserAccess = False
  Select Case LCase(AccessType)
    Case "group"
      If (InStr(Session("UserGroup"),Value)>0) Then CheckUserAccess = True
    Case "level"
      If Session("UserLevel") >= Value Then CheckUserAccess = True
    Case "username"
      If Session("UserLevel") = Value Then CheckUserAccess = True
  End Select

  Dim strCurrentPage
  strCurrentPage = Request.ServerVariables("SCRIPT_NAME") & "?" & Request.ServerVariables("QUERY_STRING")
  Session("PreviousPage") = strCurrentPage

  ' If a value is passed for DenyRedirect, the user is redirected if they don't have access
  If DenyRedirect <> "" Then
    If CheckUserAccess = False Then
      RedirectUser(DenyRedirect)
      Exit Function
    End If
  End If
  
  ' If a value is passed for AllowRedirect, the user is redirected if they do have access
  If AllowRedirect <> "" Then
    If CheckUserAccess = True Then
      RedirectUser(AllowRedirect)
      Exit Function
    End If
  End If
  
End Function


%>

