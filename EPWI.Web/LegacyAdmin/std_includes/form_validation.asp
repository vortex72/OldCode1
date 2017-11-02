<%
'---------------------------------------------------------------------------
' Purpose:  Form Validation
' Field Types: 
'		Text -- jsTextValidation(FieldName, FieldLabel, Minimum, Maximum, Required)
'		Password -- jsPasswordValidation(Password1, Password2, Minimum, Maximum, Required)
'		E-mail -- sEmailValidation(FieldName, FieldLabel, Minimum, Maximum, Required)
'		Number -- jsAllNumbersValidation(FieldName, FieldLabel, Minimum, Maximum, Required)
'	  
'	NOTE:  if there are no minimum or maximum value, pass NULL as the parameter
'		EXAMPLE:	
'			Place an include file reference to this file in the document
'			Place the Sub-Routine code on the form page
'			Run one function per form field you wish validated.
'			e.g.:
'				jsFormValidationBegin
'				jsTextValidation "txtName", "Name", 3, 50, True
'				jsPasswordValidation "txtPass1", "txtPass2", 8, 16, True
'				jsAllNumbersValidation "txtZip", "Zip Code", 5, 9, False
'				jsTextValidation "txtComments", "Comments", NULL, NULL, False
'				jsFormValidationEnd
'
'---------------------------------------------------------------------------

'---------------------------------------------------------------------------
' Purpose:  Display a line of text (text & vbCRLF)
'---------------------------------------------------------------------------
Sub Display( Text )
  Response.Write(Text & vbCrLf)
End Sub

'---------------------------------------------------------------------------
' Purpose:  Write Beginning of script tag for Form Validator Script
'---------------------------------------------------------------------------
Sub jsFormValidationBegin
  jsFormValidationInit
  Display "<script Language=""JavaScript"">"
  Display "<!--"
  Display ""
  Display "function validateForm(formObj)"
  Display "{"
  Display ""
End Sub

'----------------------------------------------------------------
' Purpose:  Write End of script tag for Form Validator Script
'----------------------------------------------------------------
Sub jsFormValidationEnd
  Display "  return (true);"
  Display ""
  Display "}"
  Display "//-->"
  Display "</script>"
End Sub

'----------------------------------------------------------------
' Purpose: Validate Text value.
' Inputs :
' sName  : name of the input
' sLable : label to display, if label = "" then use sName instead
' iMin, iMax: Minimum & Maximum characters allowed respectively
' bRequired : True/False, is required value?
' Return : Write the Text Box Validation script to the client browser
'----------------------------------------------------------------
Sub jsTextValidation(sName, sLable, iMin, iMax, bRequired)
  Display "  // Validation script for field '" & sName & "' (" & sLable & ")"
  If bRequired Then
    'Display "  alert(formObj." & sName & ".value);"
    Display "  if (formObj." & sName & ".value == """" || isWhitespace(formObj." & sName & ".value) )"
    Display "  {"
    Display "    alert(""Please enter a value for the \""" & sLable & "\"" field."");"
    Display "    formObj." & sName & ".focus();"
    Display "    return (false);"
    Display "  }"
    Display ""
  Else
    Display "  if (formObj." & sName & ".value != """")"
    Display "  {"
    Display "  // if user types the value then check the value"
  End If

	If Len(iMin) > 0 Then 
		Display "  if (formObj." & sName & ".value.length < "&iMin&")"
		Display "  {"
		Display "    alert(""Please enter at least "&iMin&" characters in the \""" & sLable & "\"" field."");"
		Display "    formObj." & sName & ".focus();"
		Display "    return (false);"
		Display "  }"
		Display ""
	End If

	If Len(iMax) > 0 Then
  Display "  if (formObj." & sName & ".value.length > "&iMax&")"
  Display "  {"
  Display "    alert(""Please enter at most "&iMax&" characters in the \""" & sLable & "\"" field."");"
  Display "    formObj." & sName & ".focus();"
  Display "    return (false);"
  Display "  }"
  Display ""
	End If

  If Not bRequired Then Display "  }"

End Sub

'----------------------------------------------------------------
' Purpose: Validate Both Password field, usually used for new registration.
' Inputs :
' sPassword1 : name of the first password input box
' sPassword2 : name of the 2nd   password input box
' iMin, iMax: Minimum & Maximum characters allowed respectively
' bRequired : True/False, is required value?
' Return : Write the Password Validation script to the client browser
'----------------------------------------------------------------
Sub jsPasswordValidation(sPassword1, sPassword2, iMin, iMax, bRequired)
  Display "  // Passsword field validation"

  jsTextValidation sPassword1, "Password", iMin, iMax, bRequired
  jsTextValidation sPassword2, "Verify Password", iMin, iMax, bRequired

  Display "  if (formObj."&sPassword1&".value != formObj."&sPassword2&".value)"
  Display "  {"
  Display "    alert(""Both \""Password\"" field must has the same value."");"
  Display "    formObj."&sPassword2&".value = """";"
  Display "    formObj."&sPassword2&".focus();"
  Display "    return (false);"
  Display "  }"
  Display ""
End Sub

'----------------------------------------------------------------
' Purpose: Validate e-mail input, usually used for new registration.
' Inputs :
' sName  : name of the input, usually: "email"
' sLable : label to display, if label = "" then use sName instead
' iMin, iMax: Minimum & Maximum characters allowed respectively
' bRequired : True/False, is required value?
' Return : Write the e-mail Validation script to the client browser
'----------------------------------------------------------------
Sub jsEmailValidation(sName, sLable, iMin, iMax, bRequired)
  Display "  // Email field validation"
  jsTextValidation sName, sLable, iMin, iMax, bRequired
  
  If Not bRequired Then
    Display "  if (formObj." & sName & ".value != """")"
    Display "  {"
    Display "  // if user types the value then Check the value"
  End If

  Display "  if (formObj." & sName & ".value.indexOf(""@"",1) == -1)"
  Display "  {"
  Display "    alert(""Not a valid e-mail address."");"
  Display "    formObj." & sName & ".focus();"
  Display "    return (false);"
  Display "  }"
  Display ""
  Display "  if (formObj." & sName & ".value.indexOf(""."",formObj." & sName & ".value.indexOf(""@"")+1) == -1)"
  Display "  {"
  Display "    alert(""Not a valid e-mail address."");"
  Display "    formObj." & sName & ".focus();"
  Display "    return (false);"
  Display "  }"
  Display ""
  
  If not bRequired Then
    Display "  // End checking e-mail value"
    Display "  }"
    Display ""
  End If
End Sub

'----------------------------------------------------------------
' Purpose: Validate Number (checks for length of input, not values)
' Inputs :
' sName  : name of the input
' sLable : label to display, if label = "" then use sName instead
' iMin, iMax: Minimum & Maximum characters allowed respectively
' bRequired : True/False, is required value?
' Return : Write the Text Box Validation script to the client browser
'----------------------------------------------------------------
Sub jsAllNumbersValidation(sName, sLable, iMin, iMax, bRequired)
  Display " // Validation script for field '" & sName & "' (" & sLable & ")"
  If bRequired Then
	 Display " if (formObj." & sName & ".value == """")"
    Display "  {"
    Display "    alert(""Please enter a value for the \""" & sLable & "\"" field."");"
    Display "    formObj." & sName & ".focus();"
    Display "    return (false);"
    Display "  }"
    Display ""
  Else
    Display "  if (formObj." & sName & ".value != """")"
    Display "  {"
    Display "  // if user type the value then Check the value"
  End If
  Display "  // check for numeric value"
  Display " 	var parsed = true;"
  Display " 	var validchars = ""1234567890"";"
  Display " 	var testString = formObj." & sName & ".value;"
  Display "	for(var i = 0; i < testString.length; i++){"
  Display "	  var letter = testString.charAt(i);"
  Display "		if(validchars.indexOf(letter) == -1){"
  Display "     alert(""Please enter a numerical value for the \""" & sLable & "\"" field."");"
  Display "     formObj." & sName & ".focus();"
  Display "     return (false);"
  Display "	  }"
  Display "	}"
        
	If Len(iMin) > 0 then 
    Display "  if (formObj." & sName & ".value.length < "&iMin&")"
    Display "  {"
    Display "    alert(""Please enter at least "&iMin&" characters in the \""" & sLable & "\"" field."");"
    Display "    formObj." & sName & ".focus();"
    Display "    return (false);"
    Display "  }"
    Display ""
	End If
	If Len(iMax) > 0 then
    Display "  if (formObj." & sName & ".value.length > "&iMax&")"
    Display "  {"
    Display "    alert(""Please enter at most "&iMax&" characters in the \""" & sLable & "\"" field."");"
    Display "    formObj." & sName & ".focus();"
    Display "    return (false);"
    Display "  }"
    Display ""
	End If

  If Not bRequired Then Display "  }"

End Sub

'----------------------------------------------------------------
' Purpose: Validate Number (checks for value of input, not length)
' Inputs :
' sName  : name of the input
' sLable : label to display, if label = "" then use sName instead
' iMin, iMax: Minimum & Maximum value allowed respectively
' bRequired : True/False, is required value?
' Return : Write the Text Box Validation script to the client browser
'----------------------------------------------------------------
Sub jsNumberValidation(sName, sLable, iMin, iMax, bRequired)
  Display " // Validation script for field '" & sName & "' (" & sLable & ")"
  If bRequired Then
	  Display " if (formObj." & sName & ".value == """")"
    Display "  {"
    Display "    alert(""Please enter a value for the \""" & sLable & "\"" field."");"
    Display "    formObj." & sName & ".focus();"
    Display "    return (false);"
    Display "  }"
    Display ""
  Else
    Display "  if (formObj." & sName & ".value != """")"
    Display "  {"
    Display "  // if user type the value then Check the value"
  End If
  Display "  var bIsNegative = false;"
  Display "  if (formObj." & sName & ".value.substr(0,1) == ""-""){" ' Find out if the value is negative
  Display "    bIsNegative = true;"
  Display "    formObj." & sName & ".value = formObj." & sName & ".value.replace(""-"","""")" ' Remove the "-" sign to validate number
  Display "  }"  
  Display "  // check for numeric value"
  Display " 	var parsed = true;"
  Display " 	var validchars = ""1234567890"";"
  Display " 	var testString = formObj." & sName & ".value;"
  Display "	for(var i = 0; i < testString.length; i++){"
  Display "	  var letter = testString.charAt(i);"
  Display "		if(validchars.indexOf(letter) == -1){"
  Display "     alert(""Please enter a numerical value for the \""" & sLable & "\"" field."");"
  Display "     formObj." & sName & ".focus();"
  Display "     return (false);"
  Display "	  }"
  Display "	}"
  Display "  if (bIsNegative == true){"
  Display "   formObj." & sName & ".value = ""-"" + formObj." & sName & ".value;" ' Re-append the "-" sign to validate mins and maxs
  Display "  }"        
        
	If Len(iMin) > 0 then 
    Display "  if (formObj." & sName & ".value < "&iMin&")"
    Display "  {"
    Display "    alert(""Please enter a value greater than or equal to "&iMin&" in the \""" & sLable & "\"" field."");"
    Display "    formObj." & sName & ".focus();"
    Display "    return (false);"
    Display "  }"
    Display ""
	End If
	If Len(iMax) > 0 then
    Display "  if (formObj." & sName & ".value > "&iMax&")"
    Display "  {"
    Display "    alert(""Please enter a value less than or equal to "&iMax&" in the \""" & sLable & "\"" field."");"
    Display "    formObj." & sName & ".focus();"
    Display "    return (false);"
    Display "  }"
    Display ""
	End If

  If Not bRequired Then Display "  }"

End Sub

'----------------------------------------------------------------
' Purpose: Validate Phone Number.  
'          Ensures that only 7 or 10 numbers are input.
'          Ignores all other characters
' Inputs :
' sName  : name of the input
' sLable : label to display, if label = "" then use sName instead
' bRequired : True/False, is required value?
' bForce10 : Require only 10 digit numbers, not 7
' Return : Write the Text Box Validation script to the client browser
'----------------------------------------------------------------
Sub jsPhoneNumberValidation(sName, sLable, bRequired, bForceTen)
  Display " // Validation script for field '" & sName & "' (" & sLable & ")"
  If bRequired Then
	  Display "if(formObj." & sName & ".value == """")"
    Display "  {"
    Display "    alert(""Please enter a value for the \""" & sLable & "\"" field."");"
    Display "    formObj." & sName & ".focus();"
    Display "    return (false);"
    Display "  }"
    Display ""
  Else
    Display "  if (formObj." & sName & ".value != """")"
    Display "  {"
    Display "  // if user type the value then Check the value"
  End If
  Display "  // check for numeric value"
  Display " var parsed = true;"
  Display " var validchars = ""1234567890"";"
  Display " var testString = formObj." & sName & ".value;"
  Display " var cleanPhone = new String("""");"
  Display "	for(var i = 0; i < testString.length; i++){"
  Display "	  var letter = testString.charAt(i);"
  Display "		if(validchars.indexOf(letter) != -1){"
  Display "     cleanPhone += testString.charAt(i);"
  Display "   }"
  Display "	}"
  If bForceTen Then
    Display " if (cleanPhone.length != 10)"
    Display " {"
    Display "   alert(""Please enter only 10 digits in the \""" & sLable & "\"" field."");"
    Display "   formObj." & sName & ".focus();"
    Display "   return (false);"
    Display " }"
  Else
    Display " if (cleanPhone.length != 7 && cleanPhone.length != 10)"
    Display " {"
    Display "   alert(""Please enter 7 or 10 digits in the \""" & sLable & "\"" field."");"
    Display "   formObj." & sName & ".focus();"
    Display "   return (false);"
    Display " }"
  End If
  Display " else {"
  Display "   if (cleanPhone.length == 7) {"
  Display "     formObj." & sName & ".value = cleanPhone.substr(0,3) + ""-"" + cleanPhone.substr(3,4)"
  Display "   }"
  Display "   else {"
  Display "     formObj." & sName & ".value = ""("" + cleanPhone.substr(0,3) + "") "" + cleanPhone.substr(3,3) + ""-"" +cleanPhone.substr(6,4)"
  Display "   }"
  Display " }"
  Display ""
  If not bRequired Then Display "  }"
End Sub

'----------------------------------------------------------------
' Purpose: Verify that at least 1 radio or check box 
'          in a group has been selected
' Inputs :
' sName  : name of the input
' sLable : label to display
' bRequired : True/False, is required value?
' Return : Write the Text Box Validation script to the client browser
'----------------------------------------------------------------
Sub jsRadioOrCheckboxValidation(sName, sLable)
  Display "  // Validation script for the '" & sName & "' (" & sLable & ") field"
  Display "  var bSelectionMade = false;"
  Display "  for(var i = 0; i < formObj." & sName & ".length; i++){"
  Display "    if(formObj." & sName & "(i).checked == true){"
  Display "      bSelectionMade = true;"
  Display "    }"
  Display "  }"
  Display "  if (bSelectionMade == false){"
  Display "    alert(""Please make a selection for the \""" & sLable & "\"" field."");"
  Display "    return (false);"
  Display "  }"
End Sub


'----------------------------------------------------------------
' Purpose: Verify that the value entered is within the acceptable
'          range and that the proper amount of decimals are input
' Inputs :
' sName  : name of the input
' sLable : label to display
' iMin, iMax : Minimum and Maximum values allowed (dollars only)
'              Use one dollar less or more if cents can affect the value
' iDecimals : Decimal Points allowed
' NOTE: Use blank strings "" to not check validation for one of the values
' Return : Write the Text Box Validation script to the client browser
'----------------------------------------------------------------
Sub jsCurrencyValidation(sName, sLable, iMin, iMax, iDecimals, bRequired)
  Display " // Validation script for field '" & sName & "' (" & sLable & ")"
  If bRequired Then
	  Display "if(formObj." & sName & ".value == """")"
    Display "  {"
    Display "    alert(""Please enter a value for the \""" & sLable & "\"" field."");"
    Display "    formObj." & sName & ".focus();"
    Display "    return (false);"
    Display "  }"
    Display ""
  Else
    Display "  if (formObj." & sName & ".value != """")"
    Display "  {"
    Display "  // if user type the value then Check the value"
  End If
  Display "  var sCurrency = formObj." & sName & ".value;"
  Display "  sCurrency = sCurrency.replace(""$"","""")" ' Remove dollar sign
  Display "  sCurrency = sCurrency.replace(/ /g,"""")" ' Remove spaces for the whole string
  Display "  formObj." & sName & ".value = sCurrency;"
  Display "  var bIsNegative = false;"
  Display "  if (sCurrency.substr(0,1) == ""-""){" ' Find out if the value is negative
  Display "    bIsNegative = true;"
  Display "    sCurrency = sCurrency.replace(""-"","""")" ' Remove the "-" sign to validate number
  Display "  }"
  Display "  var iDecimalIndex = sCurrency.indexOf(""."");"
  Display "  var iDollars = 0;"
  Display "  var iCents = 0;"
  Display "  if (iDecimalIndex >= 0){"' Determine if cents were input
  Display "    iDollars = sCurrency.substr(0,iDecimalIndex);" ' Get just the dollars
  Display "    iCents = sCurrency.substr(iDecimalIndex + 1 ,sCurrency.length - iDollars.length - 1);" ' Get just the cents
  Display "  } "
  Display "  else {"
  Display "    iDollars = sCurrency"
  Display "  }"
  Display "  if ((AllNumbers(iDollars) != true) || (AllNumbers(iCents) != true)){" ' Find out if the dollars and cents are all numbers
  Display "    alert(""The value for the \""" & sLable & "\"" field does not appear to be valid."");"
  Display "    formObj." & sName & ".focus();"
  Display "    return (false);"
  Display "  }"
  Display "  if (bIsNegative == true){"
  Display "   iDollars = ""-"" + iDollars;" ' Re-append the "-" sign to validate mins and maxs
  Display "  }"

	If iMin <> "" Then
    Display "  if (iDollars < " & iMin & "){"
    Display "    alert(""The value for the \""" & sLable & "\"" field must be greater than " & iMin & "."");"
    Display "    formObj." & sName & ".focus();"
    Display "    return (false);"
    Display "  }"
  End If

	If iMax <> "" Then
    Display "  if (iDollars > " & iMax & "){"
    Display "    alert(""The value for the \""" & sLable & "\"" field must be less than " & iMax & "."");"
    Display "    formObj." & sName & ".focus();"
    Display "    return (false);"
    Display "  }"
  End If

	If iDecimals <> "" Then
    Display "  if (iCents.length > " & iDecimals & "){"
    Display "    alert(""The value for the \""" & sLable & "\"" field cannot have more than " & iDecimals & " decimals."");"
    Display "    formObj." & sName & ".focus();"
    Display "    return (false);"
    Display "  }"
  End If
  
  Display "  "
  Display "  "
  Display "  "
  Display "  "
  Display "  "
  Display "  "
  Display "  "
  Display "  "
  Display "  "
  If not bRequired Then Display "  }"
End Sub

'----------------------------------------------------------------
' Purpose: Verify that at least 1 radio or check box 
'          in a group has been selected
' Inputs :
' sName  : name of the input
' sLable : label to display
' Return : Write the Text Box Validation script to the client browser
'----------------------------------------------------------------
Sub jsDateValidation(sName, sLable, bRequired)
  Display " // Validation script for field '" & sName & "' (" & sLable & ")"
  If bRequired Then
	  Display "if(formObj." & sName & ".value == """")"
    Display "  {"
    Display "    alert(""Please enter a value for the \""" & sLable & "\"" field."");"
    Display "    formObj." & sName & ".focus();"
    Display "    return (false);"
    Display "  }"
    Display ""
  Else
    Display "  if (formObj." & sName & ".value != """")"
    Display "  {"
    Display "  // if user type the value then Check the value"
  End If
  Display "  var sDate = formObj." & sName & ".value;"
  Display "  var iCurIndex = 0;"
  Display "  var iNewIndex = 0;"
  Display "  if (sDate.indexOf(""/"") == -1){"
  Display "    alert(""Please enter a valid date 'MM/DD/YYY' for the \""" & sLable & "\"" field."");"
  Display "    formObj." & sName & ".focus();"
  Display "    return (false);"
  Display "  }"
  Display "  else {"
  Display "    iNewIndex = sDate.indexOf(""/"");"
  Display "    var sMonth = sDate.substr(0,iNewIndex);"
  Display "    iCurIndex = iNewIndex + 1;"
  Display "    iNewIndex = sDate.indexOf(""/"",iCurIndex);"
  Display "    var sDay = sDate.substr(iCurIndex,iNewIndex - iCurIndex);"
  Display "    iCurIndex = iNewIndex + 1;"
  Display "    iNewIndex = sDate.length;"
  Display "    var sYear = sDate.substr(iCurIndex,iNewIndex - iCurIndex);"
  Display "    if ((sMonth < 1) || (sMonth > 12) || (sDay < 1) || (sDay > 31) || (sYear < 1949) || (sYear > 2099)){"
  Display "      alert(""The date does not appear valid for the \""" & sLable & "\"" field."");"
  Display "      formObj." & sName & ".focus();"
  Display "      return (false);"
  Display "    }"
  Display "  }"
  If not bRequired Then Display "  }"
End Sub


Sub jsFormValidationInit
  'The function "isWhitespace"
  'Returns true if string s is empty or 
  'whitespace characters only.
  'Search through string's characters one by one
  'until we find a non-whitespace character.
  'When we do, return false; if we don't, return true.
  %>

  <SCRIPT LANGUAGE=javascript>
  <!--
  function isWhitespace (s)
  {
  	var whitespace = " \t\n\r";
  	var i;
      // Is s empty?
      if (isEmpty(s)) return true;
      for (i = 0; i < s.length; i++)
      {   
          // Check that current character isn't whitespace.
          var c = s.charAt(i);
          if (whitespace.indexOf(c) == -1) return false;
      }
      // All characters are whitespace.
      return true;
  }

  function isEmpty(s)
  {
  	// Check whether string s is empty.
  	return ((s == null) || (s.length == 0));
  }

  function AllNumbers(theString)
  {
  	// Check whether string s all numbers.
  	var parsed = true;
  	var validchars = "1234567890";
  	var testString = theString;
  	for(var i = 0; i < testString.length; i++){
  		var letter = testString.charAt(i);
  		if(validchars.indexOf(letter) != -1)
  			continue;
  		parsed = false;
  		break;
  	}
  	if(parsed){
  		return true;
  	}else{
  		return false;
  	}	
  }
  //-->
  </script>

  <%
  '// isFloat (STRING s [, BOOLEAN emptyOK])
  '// True if string s is an unsigned floating point (real) number. 
  '// Also returns true for unsigned integers. If you wish
  '// to distinguish between integers and floating point numbers,
  '// first call isInteger, then call isFloat.
  '// Does not accept exponential notation.
  '// For explanation of optional argument emptyOK,
  '// see comments of function isInteger.
  %>
  <SCRIPT LANGUAGE=javascript>
  <!--

  function isFloat(s)
  {   
  	var i;
    var seenDecimalPoint = false;
  	var defaultEmptyOK = true;
  	var decimalPointDelimiter = ".";
      if (isEmpty(s)) 
         if (isFloat.arguments.length == 1) return defaultEmptyOK;
         else return (isFloat.arguments[1] == true);
      if (s == decimalPointDelimiter) return false;
      // If non-numeric character, return false, else return true.
      for (i = 0; i < s.length; i++)
      {   
          // Check that current character is number.
          var c = s.charAt(i);
          if ((c == decimalPointDelimiter) && !seenDecimalPoint) seenDecimalPoint = true;
          else if (!isDigit(c)) return false;
      }
      // All characters are numbers.
      return true;
  }


  // Returns true if character c is a digit (0 .. 9).
  function isDigit (c) {
  	return ((c >= "0") && (c <= "9"))
  }

  function isInteger (s) {
    var i;
  	var defaultEmptyOK = true;
      if (isEmpty(s)) 
         if (isInteger.arguments.length == 1) return defaultEmptyOK;
         else return (isInteger.arguments[1] == true);
      // If non-numeric character, return false, else return true.
      for (i = 0; i < s.length; i++)
      {   
          // Check that current character is number.
          var c = s.charAt(i);
          if (!isDigit(c)) return false;
      }
      // All characters are numbers.
      return true;
  }

  //-->
  </SCRIPT>
<%  
End Sub
%>