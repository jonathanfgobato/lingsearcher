
CREATE TABLE [Address]
(
	Id BIGINT IDENTITY,
	Street VARCHAR(50) NOT NULL,
	[State] CHAR(2) NOT NULL,
	PostalCode VARCHAR(30) NOT NULL, 
	Country VARCHAR(50) NOT NULL,
	City VARCHAR(50) NOT NULL,
	Neighbourhood VARCHAR(50) NOT NULL,
	--FgActive BIT,
	CONSTRAINT Pk_AddressId PRIMARY KEY(Id)
)
GO

CREATE TABLE UserSystem
(
	Id BIGINT IDENTITY NOT NULL,
	UserApplicationId NVARCHAR(128) NOT NULL,
	FgActive BIT,
	AddressId BIGINT NOT NULL,
	CONSTRAINT PK_UserSystemId PRIMARY KEY (Id)
	,CONSTRAINT FK_AspNetUsersId FOREIGN KEY(UserApplicationId) REFERENCES AspNetUsers(Id)
	,CONSTRAINT FK_AddressId FOREIGN KEY(AddressId) REFERENCES [Address](Id)
)
GO


CREATE TABLE [LogException](  
    [Id] BIGINT IDENTITY(1,1) NOT NULL,  
    [Date] [datetime] NOT NULL,  
    [Thread] [varchar](255) NOT NULL,  
    [Level] [varchar](50) NOT NULL,  
    [Logger] [varchar](255) NOT NULL,  
    [Message] [varchar](4000) NOT NULL,  
    [Exception] [varchar](2000) NULL,  
 CONSTRAINT PK_LogException PRIMARY KEY(Id)   
)

CREATE TABLE ProductPath
(
	Id						INT IDENTITY(1,1) NOT NULL			
	,FullName				VARCHAR(100) NOT NULL
	,Currency				VARCHAR(100) NOT NULL
	,CurrencyPromotion		VARCHAR(100) NOT NULL
	,MinPrice				VARCHAR(100) NOT NULL
	,MinPricePromotion		VARCHAR(100) NOT NULL
	,MaxPrice				VARCHAR(100) NOT NULL
	,MaxPricePromotion		VARCHAR(100) NOT NULL
	,UniquePrice			VARCHAR(100) NOT NULL
	,UniquePricePromotion	VARCHAR(100) NOT NULL
	,UrlImage				VARCHAR(100) NOT NULL
	CONSTRAINT PK_ProductPath PRIMARY KEY(Id)
)

CREATE TABLE Store
(
	Id INT IDENTITY(1,1) NOT NULL,
	Name VARCHAR(50) NOT NULL,
	UrlStore VARCHAR(50) NOT NULL,
	UrlProduct VARCHAR(50) NOT NULL,
	ProductPathId INT NOT NULL,
	CONSTRAINT PK_Store_Id PRIMARY KEY(Id),
	CONSTRAINT FK_Store_ProductPathId_ProductPath FOREIGN KEY(ProductPathId) REFERENCES ProductPath(Id)
)

CREATE TABLE Category
(
	Id INT IDENTITY(1,1) NOT NULL,
	Name VARCHAR(50) NOT NULL,
	CONSTRAINT PK_Category_Id PRIMARY KEY(Id)
)

CREATE TABLE Brand
(
	Id SMALLINT IDENTITY(1,1) NOT NULL,
	Name VARCHAR(50) NOT NULL,
	CONSTRAINT PK_Brand_Id PRIMARY KEY(Id)
)

CREATE TABLE Product
(
	Id INT IDENTITY(1,1) NOT NULL,
	Name VARCHAR(50) NOT NULL,
	Description VARCHAR(4000),
	CategoryId INT NOT NULL,
	ImageSrc VARCHAR(255) NOT NULL,
	BrandId SMALLINT NOT NULL,
	CONSTRAINT PK_Product_Id PRIMARY KEY (Id),
	CONSTRAINT FK_Product_CategoryId FOREIGN KEY (CategoryId) REFERENCES Category(Id),
	CONSTRAINT FK_Brand_BrandId FOREIGN KEY (BrandId) REFERENCES Brand(Id)
)

CREATE TABLE ProductStore
(
	ProductId INT NOT NULL,
	StoreId INT NOT NULL,
	ProductStoreId VARCHAR(30) NOT NULL,
	CONSTRAINT PK_ProductStore_ProductId_StoreId PRIMARY KEY(ProductId, StoreId),
	CONSTRAINT FK_ProductStore_ProductId FOREIGN KEY (ProductId) REFERENCES Product(Id),
	CONSTRAINT FK_ProductStore_StoreId FOREIGN KEY (StoreId) REFERENCES Store(Id),
)


SELECT * FROM ProductStore

SELECT * FROM Product