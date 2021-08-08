CREATE PROCEDURE STP_UpdateUser
	@userIDParameter int,
	@firstNameParameter varchar(50),
	@firstLastNameParameter varchar(50),
	@secondLastNameParameter varchar(50),
	@cellphoneParameter int,
	@emailParameter varchar(50),
	@passwordParameter varchar(50),
	@saltParameter varchar(50)
AS
BEGIN
	DECLARE @CredentialID varchar(50);

	UPDATE [GeneralPurposeDB].[dbo].[Users]
	SET FirstName=@firstNameParameter,
		FistLastName=@secondLastNameParameter,
		SecondLastName=@secondLastNameParameter,
		Cellphone=@cellphoneParameter,
		Email=@emailParameter
	WHERE UserID=@userIDParameter;

	Update [GeneralPurposeDB].[dbo].[Credentials]
    SET [Password] = @passwordParameter,
        [Salt] = @saltParameter
    WHERE CredentialID = @CredentialID;

END
