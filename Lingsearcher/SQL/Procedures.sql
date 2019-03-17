ALTER PROCEDURE Spr_Buscar_UserSystem_PorUserApplicationId
	@UserApplicationId NVARCHAR(128)
AS
BEGIN
	SELECT
		Id
		,UserApplicationId
		,FgActive
		,AddressId
	FROM UserSystem
	WHERE UserApplicationId = @UserApplicationId
END

ALTER PROCEDURE Spr_Buscar_Address_PorId
	@Id BIGINT
AS
BEGIN
	SELECT
		Id
		,Street
		,[State]
		,PostalCode
		,Country
		,City
		,Neighbourhood
	FROM [Address]
	WHERE Id = @Id
END

ALTER PROCEDURE Spr_Insert_Address
		@Street VARCHAR(50)
		,@State CHAR(2)
		,@PostalCode VARCHAR(30)
		,@Country VARCHAR(50)
		,@City VARCHAR(50)
		,@Neighbourhood VARCHAR(50)
AS
BEGIN
	INSERT INTO [Address]
	(
		Street 
		,[State]
		,PostalCode 
		,Country 
		,City
		,Neighbourhood
	)VALUES
	(
		@Street 
		,@State
		,@PostalCode 
		,@Country 
		,@City
		,@Neighbourhood
	);

	SELECT @@IDENTITY;
END

ALTER PROCEDURE Spr_Insert_UserSystem
	@UserApplicationId NVARCHAR(128)
	,@FgActive BIT
	,@AddressId BIGINT
AS
BEGIN
	INSERT INTO UserSystem
	(
		UserApplicationId
		,FgActive
		,AddressId
	)VALUES
	(
		@UserApplicationId
		,@FgActive
		,@AddressId
	)
END

ALTER PROCEDURE Spr_Alterar_UserSystem
	@Id BIGINT
	,@UserApplicationId NVARCHAR(128)
	,@FgActive BIT
	,@AddressId BIGINT
AS
BEGIN
	UPDATE UserSystem
	SET
		UserApplicationId = @UserApplicationId
		,FgActive = @FgActive
		,AddressId = @AddressId
	WHERE Id = @Id
END
	
CREATE PROCEDURE Spr_Alterar_Address
	@Id BIGINT
	,@Street VARCHAR(50)
	,@State CHAR(2)
	,@PostalCode VARCHAR(30)
	,@Country VARCHAR(50)
	,@City VARCHAR(50)
	,@Neighbourhood VARCHAR(50)
AS
BEGIN
	UPDATE [Address]
	SET
		Street = @Street
		,[State] = @State
		,PostalCode = @PostalCode
		,Country = @Country
		,City = @City
		,Neighbourhood = @Neighbourhood
	WHERE Id = @Id
END

CREATE PROCEDURE Spr_Listar_Store
AS
BEGIN
	SELECT
		Id
		,Name
		,UrlStore
		,UrlProduct
		,ProductPathId
END