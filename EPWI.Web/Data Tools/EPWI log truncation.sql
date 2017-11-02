-- Activity log
SELECT 
	[Action]
	, [Controller]
	, [UserName]
	, [ActionDate]
	, [ActionData], 
	IDENTITY( int ) AS [ActivityID]
  INTO ##ALtemp
  FROM [dbo].[ActivityLog] al
  where al.ActionDate > DATEADD(day, -45, GETDATE()) -- change this to 30 days on prod launch

TRUNCATE TABLE dbo.ActivityLog;
DROP TABLE dbo.ActivityLog;
SELECT
	IDENTITY( int ) AS [ActivityID] 
	, [Action]
	, [Controller]
	, [UserName]
	, [ActionDate]
	, [ActionData]
	
  INTO dbo.ActivityLog
  FROM ##ALtemp
  DROP TABLE ##ALtemp;


-- Fax log
SELECT 
	[FaxNumber]
	, [RecipName]
	, [RecipCompany]
	, [Subject]
	, [Sender]
	, [ErrorDescription]
	, [DateSent] 
	, [FaxContent]
	, IDENTITY( int ) AS [LogID]
  INTO ##FLtemp
  FROM [dbo].[FaxLog] fl
  where fl.DateSent > DATEADD(day, -45, GETDATE()) -- change this to 60 days on prod launch

TRUNCATE TABLE dbo.FaxLog;
DROP TABLE dbo.FaxLog;
SELECT
	IDENTITY( int ) AS [LogID]
	, [FaxNumber]
	, [RecipName]
	, [RecipCompany]
	, [Subject]
	, [Sender]
	, [ErrorDescription]
	, [DateSent] 
	, [FaxContent]
	
  INTO dbo.FaxLog
  FROM ##FLtemp
  DROP TABLE ##FLtemp;


