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
	FROM Store
END

CREATE PROCEDURE Spr_Buscar_ProductPath_PorId
	@Id INT
AS
BEGIN
	SELECT
		Id
		,FullName			
		,Currency			
		,CurrencyPromotion	
		,MinPrice			
		,MinPricePromotion	
		,MaxPrice			
		,MaxPricePromotion	
		,UniquePrice		
		,UniquePricePromotion
		,UrlImage	
	FROM ProductPath
	WHERE Id = @Id		
END

CREATE PROCEDURE Spr_Buscar_Product_PorId
	@Id INT
AS
BEGIN
	SELECT
		Id
		,Name
		,Description
		,CategoryId
		,ImageSrc
		,BrandId
	FROM Product
	WHERE Id = @Id
END

CREATE PROCEDURE Spr_Listar_Product
AS
BEGIN
	SELECT
		Id
		,Name
		,Description
		,CategoryId
		,ImageSrc
		,BrandId
	FROM Product
END

ALTER PROCEDURE Spr_Insert_Product
		@Name VARCHAR(50)
		,@Description VARCHAR(4000)
		,@CategoryId INT
		,@BrandId INT
		,@ImageSrc VARCHAR(255) = ''
AS
BEGIN
	INSERT INTO Product
	(
		Name
		,Description
		,CategoryId
		,ImageSrc
		,BrandId
	)
	VALUES
	(
		@Name
		,@Description
		,@CategoryId
		,@ImageSrc
		,@BrandId
	)
	SELECT @@IDENTITY
END

ALTER PROCEDURE Spr_Alterar_Product
	@Id INT
	,@Name VARCHAR(50)
	,@Description VARCHAR(4000)
	,@CategoryId INT
	,@BrandId INT
	,@ImageSrc VARCHAR(255) = ''
AS
BEGIN
	UPDATE Product
	SET
		Name = @Name
		,Description = @Description
		,CategoryId = @CategoryId
		,BrandId = @BrandId
		,ImageSrc = @ImageSrc
	WHERE Id = @Id
END


CREATE PROCEDURE Spr_Listar_Brand
AS
BEGIN
	SELECT
		Id
		,Name
	FROM Brand
END

CREATE PROCEDURE Spr_Listar_Category
AS
BEGIN
	SELECT
		Id
		,Name
	FROM Category
END

SELECT * FROM Product

SELECT * FROM ProductStore

ALTER PROCEDURE Spr_Insert_ProductStore
	@ProductId INT
	,@StoreId INT
	,@ProductStoreId VARCHAR(30)
AS
BEGIN
	INSERT INTO ProductStore
	(
		ProductId
		,StoreId
		,ProductStoreId
	)
	VALUES
	(
		@ProductId
		,@StoreId
		,@ProductStoreId
	)
END

CREATE PROCEDURE Spr_Listar_ProductStore_PorProductId
	@ProductId INT
AS
BEGIN
	SELECT
		ProductId
		,StoreId
		,ProductStoreId
	FROM ProductStore
	WHERE ProductId = @ProductId
END

EXEC Spr_Listar_ProductStore_PorProductId 10

CREATE PROCEDURE Spr_Excluir_ProductStore
	@Id INT
AS
BEGIN
	DELETE FROM ProductStore WHERE ProductId = @Id
END

CREATE PROCEDURE Spr_Excluir_Product
	@Id INT
AS
BEGIN
	DELETE FROM Product WHERE Id = @Id
END


CREATE PROCEDURE Spr_Buscar_Brand_PorId
	@Id SMALLINT
AS
BEGIN
	SELECT 
		Id
		,Name
	FROM Brand
	WHERE Id = @Id
END

ALTER PROCEDURE Spr_Buscar_Category_PorId
	@Id SMALLINT
AS
BEGIN
	SELECT 
		Id
		,Name
	FROM Category
	WHERE Id = @Id
END

CREATE PROCEDURE Spr_Buscar_Store_PorId
	@Id INT
AS
BEGIN
	SELECT 
		Id
		,Name
		,UrlStore
		,UrlProduct
		,ProductPathId
	FROM Store
	WHERE Id = @Id
END


SELECT * FROM Product