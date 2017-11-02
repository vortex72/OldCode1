<%
'----- Site Wide Variables that do not change -----

Const PageTitle = "Engine and Performance Warehouse"

' Encryption Key used for encrypting password in saved cookie
' Obtained from KeyGenerator.asp
' Length = 50 - Ensure to paste as HTML and check for double quotes
Const EncryptionKey = "*@,<?RKZ,;Y1<)F]/2QST<RD:3H%(RB;,ARA;7VXZ6HE_V&C;]"

' The current script and the current query string the user is viewing
Dim ScriptName, QueryString
ScriptName = Request.ServerVariables("SCRIPT_NAME")
QueryString = Request.ServerVariables("QUERY_STRING")

' Suggestion Box and Email Referral Functionality
Const   MailUsername = "admin@epwi.net"
Const   MailPassword = "955Decatur"
Const	MailHost = "smtp.office365.com"
Const	MailFromAddress = "admin@epwi.net"
Const	MailFromName = "Engine and Performance Warehouse Web Site"
Const	MailReceipientAddress = "webmaster@epwi.net"
'Const	MailPort = 587
Const   MailUsesSSL = True
Const   MailAuthMethod = 1 'basic
Const	MailReceipientName = "Webmaster"
Const	MailSubject = "Information Request"
Const MailConfirmationText = "<p>&nbsp;</p><p align=center><b><i><font size=4>Thank you for your comments!</font></i></b></p>"

' Image Tags
Const ImageFirstPage = "<img src=""/images/firstpage.gif"" width=""16"" height=""16"" border=""0"" alt=""Goto First Page"">"
Const ImagePrevPage  = "<img src=""/images/prevpage.gif"" width=""16"" height=""16"" border=""0"" alt=""Goto Prev Page"">"
Const ImageNextPage  = "<img src=""/images/nextpage.gif"" width=""16"" height=""16"" border=""0"" alt=""Goto Next Page"">"
Const ImageLastPage  = "<img src=""/images/lastpage.gif"" width=""16"" height=""16"" border=""0"" alt=""Goto Last Page"">"
Const ImageSortArrows  = "<img src=""/images/sortarrows.gif"" width=""5"" height=""10"" border=""0"" alt=""Sort By This Colum"">"
Const ImageCheckOn = "<img src=""images/check-on.gif"" width=""13"" height=""13"" border=""0"" alt=""True"">"
Const ImageCheckOff = "<img src=""images/check-off.gif"" width=""13"" height=""13"" border=""0"" alt=""False"">"

' eSynthesys Access Control
Const eSynUserName = "epwi"
Const eSynPassword = "1234"
Const eSynURL = "http://208.177.9.42/esyn/"
Const eSynPacketName = "XMLContent="
Const Request_POST = 1 ' Used for ASP Tear

' Other Email Specific Functionality
Const CompanyName = "Engine and Performance Warehouse"
Const CompanyURL = "www.epwi.net"
Const CompanyPhone = "(800) 888-8970"

Dim PageWidth
PageWidth = 760

Dim Margin

Margin = 5

' ----- FONTS -----
Dim BodyFace, BodyColor, BodySize, BodyFont
Dim HeaderFace, HeaderColor, HeaderSize, HeaderFont

BodyFace = "Verdana, Arial, Helvetica, sans-serif"
BodyColor = "#000000"
BodySize = 2
BodyFont = "<FONT face=" & BodyFace & " color=" & BodyColor & " size=" & BodySize & ">"

HeaderFace = "Georgia, Times New Roman, Times, serif"
HeaderSize = 4
HeaderFont = "<FONT face=" & HeaderFace & " size=" & HeaderSize & ">"

' ----- COLORS -----
Dim DarkRed, LightRed, Grey, Black, White
Dim BackColor, TextColor, LinkColor, VLinkColor, ALinkColor

DarkRed = "#D2312A"
LightRed = "#F3CBC9"
Grey = "#DEDEDE"
Black = "#000000"
White = "#FFFFFF"

BackColor = White
TextColor = Black
LinkColor = DarkRed
VLinkColor = DarkRed
ALinkColor = Grey

dim BodyTag
BodyTag = "<body bgcolor='" & BackColor & "' text='" & TextColor & "' link='" & LinkColor & "' vlink='" & VLinkColor & "' alink='" & ALinkColor & "' leftmargin='0' topmargin='0'>"


' ----- Page Specific Variables -----
function SubNavTable(numcells,cells())
dim TableStr, CellWidth, i
	if numcells = 0 then numcells = 1
	CellWidth = 100 / numcells
	TableStr = "<table width='100%' border='0' cellspacing='0' cellpadding='0'>" & vbCrLf
	TableStr = TableStr & "<tr>" & vbCrLf
	for i = 1 to numcells
		TableStr = TableStr & "<td align='center' width='" & CellWidth & "%'><b>" 
		TableStr = TableStr & "<a href='" & cells(i,1) & "'><FONT face=" & BodyFace & " size=2>" & cells(i,0) & "</a>"
		TableStr = TableStr & "</b></td>"
		if i < numcells then
			TableStr = TableStr & "<td align='center' width='1'>|</font></td>"
		end if
	next
	TableStr = TableStr &  vbCrLf & "</tr>" & vbCrLf
	TableStr = TableStr & "</table>" & vbCrLf
	SubNavTable = TableStr
end function

'----- Application Names -----
' Redo this part so it gets info from DB

const ProductLit	= 1
const EngineKits	= 2
const PriceSheets	= 3
const JobOpenings	= 4
const AboutPage     = 5
const Locations		= 6
const Links			= 7
const Programs		= 8
const Flyers		= 9
const Specials		= 10
const Linecards		= 11

'----- Site Wide Variables
const strClientName = "epw"
const strPathDelimeter = "&nbsp;> "
%>
