INSERT INTO ProductPath
(	FullName			
	,Currency			
	,CurrencyPromotion	
	,MinPrice			
	,MinPricePromotion	
	,MaxPrice			
	,MaxPricePromotion	
	,UniquePrice			
	,UniquePricePromotion
	,UrlImage			
)
VALUES
(
	'//*[@id="j-product-detail-bd"]/div[1]/div/h1/'
	,'//*[@id="j-product-detail-bd"]/div[1]/div/div[3]/div[2]/div/div[1]/div/div/span[1]/'
	,'//*[@id="j-product-detail-bd"]/div[1]/div/div[3]/div[2]/div/div[2]/div/div/span[1]/'
	,'//*[@id="j-sku-price"]/span[1]/'
	,'//*[@id="j-sku-discount-price"]/span[1]/'
	,'//*[@id="j-sku-price"]/span[2]/'
	,'//*[@id="j-sku-discount-price"]/span[2]/'
	,'//*[@id="j-sku-price"]/'
	,'//*[@id="j-sku-discount-price"]/'
	,'//*[@id="magnifier"]/div/a/img/'
)
GO

INSERT INTO Store
(
	Name
	,UrlStore
	,UrlProduct
	,ProductPathId
)
VALUES
(
	'Aliexpress'
	,'https://pt.aliexpress.com/'
	,'https://pt.aliexpress.com/item/xxx/'
	,1
)

INSERT INTO ProductPath
(	FullName			
	,Currency			
	,CurrencyPromotion	
	,MinPrice			
	,MinPricePromotion	
	,MaxPrice			
	,MaxPricePromotion	
	,UniquePrice			
	,UniquePricePromotion
	,UrlImage			
)
VALUES
(
	'/html/body/div[6]/div[2]/div[2]/h1/'
	,'//*[@id="pinfoItemSalePrice"]/dd/span[1]/i[1]/'
	,'//*[@id="pinfoItemSalePrice"]/dd/span[1]/i[1]/'
	,'//*[@id="pinfoItemSalePrice"]/dd/span[1]/i[2]/'
	,'//*[@id="pinfoItemSalePrice"]/dd/span[1]/i[2]/'
	,'//*[@id="pinfoItemSalePrice"]/dd/span[1]/i[4]/'
	,'//*[@id="pinfoItemSalePrice"]/dd/span[1]/i[4]/'
	,''
	,''
	,''
)
GO

INSERT INTO Store
(
	Name
	,UrlStore
	,UrlProduct
	,ProductPathId
)
VALUES
(
	'DealExtreme'
	,'https://www.dx.com/pt/'
	,'https://www.dx.com/pt/p/xxx-'
	,2
)


SELECT 
	A.*, 
	B.* 
FROM Store AS A
	INNER JOIN ProductPath AS B
	ON A.ProductPathId = B.Id