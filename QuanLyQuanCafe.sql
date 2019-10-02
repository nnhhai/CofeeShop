CREATE DATABASE QuanLyQuanCafe
GO

USE QuanLyQuanCafe
GO


-- Food
-- Table
-- FoodCategory
-- Account
-- Bill
-- BillInfo

CREATE TABLE TableFood
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Bàn chưa có tên',
	status NVARCHAR(100) NOT NULL DEFAULT N'Trống'	-- Trống || Có người
)
GO

CREATE TABLE Account
(
	UserName NVARCHAR(100) PRIMARY KEY,	
	DisplayName NVARCHAR(100) NOT NULL DEFAULT N'Kter',
	PassWord NVARCHAR(1000) NOT NULL DEFAULT 0,
	Type INT NOT NULL  DEFAULT 0 -- 1: admin && 0: staff
)
GO

CREATE TABLE FoodCategory
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên'
)
GO

CREATE TABLE Food
(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	idCategory INT NOT NULL,
	price FLOAT NOT NULL DEFAULT 0
	
	FOREIGN KEY (idCategory) REFERENCES dbo.FoodCategory(id)
)
GO

CREATE TABLE Bill
(
	id INT IDENTITY PRIMARY KEY,
	DateCheckIn DATE NOT NULL DEFAULT GETDATE(),
	DateCheckOut DATE,
	idTable INT NOT NULL,
	status INT NOT NULL DEFAULT 0 -- 1: đã thanh toán && 0: chưa thanh toán
	
	FOREIGN KEY (idTable) REFERENCES dbo.TableFood(id)
)
GO

CREATE TABLE BillInfo
(
	id INT IDENTITY PRIMARY KEY,
	idBill INT NOT NULL,
	idFood INT NOT NULL,
	count INT NOT NULL DEFAULT 0
	
	FOREIGN KEY (idBill) REFERENCES dbo.Bill(id),
	FOREIGN KEY (idFood) REFERENCES dbo.Food(id)
)
GO

INSERT INTO dbo.Account
        ( UserName ,
          DisplayName ,
          PassWord ,
          Type
        )
VALUES  ( N'K9' , -- UserName - nvarchar(100)
          N'RongK9' , -- DisplayName - nvarchar(100)
          N'1' , -- PassWord - nvarchar(1000)
          1  -- Type - int
        )
INSERT INTO dbo.Account
        ( UserName ,
          DisplayName ,
          PassWord ,
          Type
        )
VALUES  ( N'staff' , -- UserName - nvarchar(100)
          N'staff' , -- DisplayName - nvarchar(100)
          N'1' , -- PassWord - nvarchar(1000)
          0  -- Type - int
        )
GO

select * from dbo.TableFood
select * from dbo.Bill
select * from dbo.BillInfo
select * from dbo.Food
select * from dbo.FoodCategory
select * from dbo.Account
--update dbo.TableFood set status='Free' where id=4;
alter table dbo.TableFood drop column status
delete dbo.BillInfo
delete dbo.Bill
select bi.id, bi.idBill, b.idTable, b.status from dbo.Bill as b, dbo.BillInfo as bi where b.id=bi.idBill and b.status=1 and b.idTable=16
select * from dbo.Bill where idTable=2 and status=1
select * from dbo.BillInfo where idBill in (select id from dbo.Bill where idTable=2 and status=1)

--add bill
insert dbo.Bill (idTable, status) values (1, 1)
insert dbo.Bill (idTable, status) values (2, 1)
insert dbo.Bill (idTable, status) values (3, 1)
insert dbo.Bill (idTable, status) values (4, 1)

--add bill info
insert dbo.BillInfo (idBill, idFood, count) values (1, 2, 1)
insert dbo.BillInfo (idBill, idFood, count) values (1, 3, 1)
insert dbo.BillInfo (idBill, idFood, count) values (1, 7, 2)
insert dbo.BillInfo (idBill, idFood, count) values (2, 1, 2)
insert dbo.BillInfo (idBill, idFood, count) values (2, 2, 1)
insert dbo.BillInfo (idBill, idFood, count) values (3, 5, 1)
insert dbo.BillInfo (idBill, idFood, count) values (3, 9, 1)
insert dbo.BillInfo (idBill, idFood, count) values (4, 4, 3)
insert dbo.BillInfo (idBill, idFood, count) values (4, 1, 1)
insert dbo.BillInfo (idBill, idFood, count) values (4, 7, 4)
--thêm foodcategory
insert dbo.FoodCategory (name) values ('drink')
insert dbo.FoodCategory (name) values ('cake')

--add food
insert dbo.Food (name, idCategory, price) values ('capuchino', 1, 60)
insert dbo.Food (name, idCategory, price) values ('espresso', 1, 50)
insert dbo.Food (name, idCategory, price) values ('macchiato', 1, 50)
insert dbo.Food (name, idCategory, price) values ('mocha', 1, 40)
insert dbo.Food (name, idCategory, price) values ('latte', 1, 40)
insert dbo.Food (name, idCategory, price) values ('aaaaa', 1, 40)

insert dbo.Food (name, idCategory, price) values ('pudding', 2, 60)
insert dbo.Food (name, idCategory, price) values ('butter', 2, 60)
insert dbo.Food (name, idCategory, price) values ('biscuit', 2, 40)
insert dbo.Food (name, idCategory, price) values ('carrot', 2, 50)
insert dbo.Food (name, idCategory, price) values ('apple', 2, 50)

--thêm bàn
DECLARE @i INT = 0

WHILE @i <= 10
BEGIN
	INSERT dbo.TableFood (name)VALUES  ( N'Table ' + CAST(@i AS nvarchar(100)))
	SET @i = @i + 1
END

alter proc USP_InsertBillInfo
@idBill int, @idFood int, @count int
as
begin
	declare @isExitBillInfo int
	declare @foodCount int = 1
	
	select @isExitBillInfo = id, @foodCount = count from dbo.BillInfo where idBill = @idBill and idFood = @idFood
	if (@isExitBillInfo > 0)
	begin
		declare @newCount int = @foodCount + @count
		if(@newCount > 0)
			update dbo.BillInfo set count = @newCount where idFood = @idFood
		else delete dbo.BillInfo where idBill = @idBill and idFood = @idFood
	end
	else
	begin
	if (@count > 0)
		insert dbo.BillInfo (idBill, idFood, count)
		values (@idBill, @idFood, @count)
	end
end
go

create proc USP_InsertBill 
@idTable int
as
begin
	insert dbo.Bill (idTable) values (@idTable)
end
go

create trigger UTG_UpdateBillInfo
on dbo.BillInfo for insert, update
as
begin
	declare @idBill int
	select @idBill = idBill from inserted

	declare @idTable int
	select @idTable = idTable from dbo.Bill where id = @idBill and status = 0
	update dbo.TableFood set status = 'Busy' where id = @idTable
end
go

create trigger UTG_UpdateBill
on dbo.Bill for update
as
begin
	declare @idBill int
	select @idBill = id from inserted

	declare @idTable int
	select @idTable = idTable from dbo.Bill where id = @idBill

	declare @count int = 0
	select @count = count(*) from dbo.Bill where idTable = @idTable and status = 0

	if(@count = 0)
		update dbo.TableFood set status = 'Free' where id = @idTable 
end
go

alter proc USP_UpdateAccount
@userName nvarchar(100), @passWord nvarchar(100), @newPass nvarchar(100)
as
begin
	declare @correctPass int = 0
	select @correctPass = count(*) from dbo.Account where UserName = @userName and PassWord = @passWord

	if(@correctPass = 1)
	begin
		if(@newpass != NULL or @newPass != '')
		begin
			update dbo.Account set PassWord = @newPass
		end
	end
end
go

alter trigger UTG_DeleteBillInfo
on dbo.BillInfo for delete
as
begin
	declare @idBill int
	select @idBill = idBill from deleted

	declare @idTable int
	select @idTable = idTable from dbo.Bill where id = @idBill

	declare @count int = 0
	select @count = count(*) from dbo.BillInfo as bi, dbo.Bill as b where bi.idBill = b.id and b.id = @idBill and b.status = 0
	if(@count = 0)
	update dbo.TableFood set status = 'Free' where id = @idTable
end
go

select MAX(id) from dbo.FoodCategory where id!=4
update dbo.Food set idCategory = (select MAX(id) from dbo.FoodCategory where id!=4) where idCategory =4
delete dbo.FoodCategory where id = 4

create trigger UTG_DeleteFoodCategory
on dbo.FoodCategory for delete
as
begin
	declare @idCategory int
	select @idCategory = id from deleted

	declare @newIdCategory int
	select @newIdCategory = MAX(id) from dbo.FoodCategory

	update dbo.Food set idCategory = @newIdCategory where idCategory = @idCategory
end
go

SELECT f.name, bi.count, f.price, f.price*bi.count AS totalPrice 
FROM dbo.BillInfo AS bi, dbo.Bill AS b, dbo.Food AS f WHERE bi.idBill = b.id AND bi.idFood = f.id AND b.idTable = 1

select b.id, t.name, b.total from dbo.Bill as b, dbo.TableFood as t where b.idTable = t.id and b.total > 0

select * from dbo.Account where UserName = 'admin'



