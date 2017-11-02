<%
'------------------------------------------------------------------

Function AS400DateToWebDate(iDate)
  ' Convert a date in YYYYMMDD format to MM/DD/YYYY format
  AS400DateToWebDate = CInt(Mid(iDate,5,2)) & "/" & CInt(Right(iDate,2)) & "/" & Left(iDate,4)
End Function

'------------------------------------------------------------------

Function WebDateToAS400Date(iDate)
' Convert a date in MM/DD/YYYY format to YYYYMMDD format
  If IsDate(iDate) Then
    WebDateToAS400Date = Year(iDate) & LeadingZeros(Month(iDate), 2) & LeadingZeros(Day(iDate),2)
  End If
End Function

'------------------------------------------------------------------

Function DisablePage(PageName)
  RWL "<center>"
  RWL "<p>&nbsp;</p>"
  RWL "<p style=""font-size:14pt; font-weight:bold; color:#990000;"">We apologize, but " & PageName & " functionality is currently unavailable.</p>"
  RWL "<p>We exepect this section to be available shortly,<br>"
  RWL "and apologize for any inconvienience that this delay has caused.</p>"
  RWL "<p>Please contact your <a href=""locations.asp""><b>nearest EPWI location</b></a> for more information.</p>"
  RWL "</center>"
  Response.End
End Function

'------------------------------------------------------------------

Function WarehouseAbbr(Whse)
  Select Case Whse
    Case whseALB : WarehouseAbbr = "ALB"
    Case whseANC : WarehouseAbbr = "ANC"
    Case whseDAL : WarehouseAbbr = "DAL"
    Case whseDEN : WarehouseAbbr = "DEN"
    Case whseHOU : WarehouseAbbr = "HOU"
    Case whseOAK : WarehouseAbbr = "OAK"
    Case whseOKC : WarehouseAbbr = "OKC"
    Case whsePDX : WarehouseAbbr = "PDX"
    Case whsePHX : WarehouseAbbr = "PHX"
    Case whseSAN : WarehouseAbbr = "SAN"
    Case whseTAC : WarehouseAbbr = "TAC"
    Case whseLA1 : WarehouseAbbr = "LA1"
  End Select
End Function

'------------------------------------------------------------------

Function WarehouseCode(WhseAbbr)
  Select Case Trim(UCase(WhseAbbr))
    Case "ALB" : WarehouseCode = whseALB
    Case "ANC" : WarehouseCode = whseANC
    Case "DAL" : WarehouseCode = whseDAL
    Case "DEN" : WarehouseCode = whseDEN
    Case "HOU" : WarehouseCode = whseHOU
    Case "OAK" : WarehouseCode = whseOAK
    Case "OKC" : WarehouseCode = whseOKC
    Case "PDX" : WarehouseCode = whsePDX
    Case "PHX" : WarehouseCode = whsePHX
    Case "SAN" : WarehouseCode = whseSAN
    Case "TAC" : WarehouseCode = whseTAC
    Case "LA1" : WarehouseCode = whseLA1
  End Select
End Function
%>