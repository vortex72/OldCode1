<%@ Language=VBScript %>
<%
Option Explicit
Response.Expires = 0 
%>
<!-- #include file="adovbs.inc" -->
<!-- #include file="dbconn.inc" -->
<!-- #include file="variables.asp" -->
<!-- #include file="std_functions.asp" -->
<!-- #include file="login_functions.asp" -->
<!-- #include file="form_validation.asp" -->
<!-- #include file="xml_functions.asp" -->
<!-- #include file="epwi_functions.asp" -->
<!-- #include file="debug.asp"-->
<%
Select Case LCase(ScriptName)
  Case "/accountstatus.asp", "/flyers.asp", "/invoicedetail.asp", _
      "/invoicelist.asp", "/kitcatalog.asp", "/linecards.asp", "/pricesheets.asp", _
      "/specials.asp", "/statements.asp", "/stockstatus.asp", "/userinfo.asp"
        LogAccess "UserName,Type,Page", "'" & Session("UserName") & "','View','" & ScriptName & "'"  
End Select
%>