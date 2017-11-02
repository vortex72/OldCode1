SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

IF (OBJECT_ID('usp_ShipExecLog_Insert', 'P') IS NULL)
	EXECUTE('CREATE PROCEDURE [dbo].[usp_ShipExecLog_Insert] AS BEGIN RETURN NULL END;')
GO

ALTER PROCEDURE dbo.usp_ShipExecLog_Insert
(
	@RequestType varchar(10),@RequestId int,@SuccessFlag bit,@Requestbody varchar(max)
	,@ResponseBody varchar(max) = null,@ErrorMessage varchar(max)
)
AS
/*
	============================================================
	Procedure:		usp_ShipExecLog_Insert
	Description:	Adds a record ot ShipExecLog
	============================================================
	Revision History
	============================================================
	Rev	Date		Author					Description
	============================================================
	1	02/27/2017  TB			            Created
	============================================================
*/
BEGIN

	insert into ShipExecLog(
		RequestType,
		RequestId,
		SuccessFlag,
		RequestBody,
		ResponseBody,
		ErrorMessage)
	values(
		@RequestType,
		@RequestId,
		@SuccessFlag,
		@RequestBody,
		@ResponseBody,
		@ErrorMessage
		)

end
