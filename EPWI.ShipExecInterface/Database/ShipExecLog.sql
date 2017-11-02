create table ShipExecLog (
	LogId int Primary key identity(1,1),
	LogDate datetime not null default getdate(),
	RequestType varchar(10),
	RequestId int,
	SuccessFlag bit,
	RequestBody varchar(max),
	ResponseBody varchar(max),
	ErrorMessage varchar(max)
);


