<%
Dim strSQLConn
'strSQLConn = "Server=tcp:epwi.database.windows.net,1433;Database=epwi.prod;User ID=epwi-admin@epwi;Password=xxx;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Provider=SQLOLEDB.1;Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;Use Encryption for Data=False;Tag with column collation when possible=False;"
strSQLConn = "Server=TBOTT7\sql2014;Database=epwi.prod;User ID=epwuser;Password=Test123!;Tag with column collation when possible=False;Provider=SQLOLEDB.1;Use Procedure for Prepare=1;Auto Translate=True;Packet Size=4096;"

Dim conn
Set conn = Server.CreateObject("ADODB.Connection")
'conn.ConnectionString = Application("objConn_ConnectionString")
conn.ConnectionString = strSQLConn
conn.Open

Dim cmd

' ----- USAGE -----
'	 Set cmd = Server.CreateObject("ADODB.Command")
'	 Set cmd.ActiveConnection = conn
'    cmd.CommandType = adCmdStoredProc
'    cmd.CommandText = "usp_[StoredProcName]"
'    cmd.Parameters.Append = cmd.CreateParameter("[ParamName]",[ParamType],adParamInput,[ParamLength],[ParamValue])
'    Dim rsAppSettings
'    Set rsAppSettings = cmd.Execute
'	 Set cmd = Nothing

'-----------------------------------------------------

%>