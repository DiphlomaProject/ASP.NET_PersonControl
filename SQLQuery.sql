select * from AspNetUsers where id in (select USERID from AspNetUserRoles where RoleId in (select Id from AspNetRoles) );
select AspNetUsers.Email, AspNetUserRoles.UserId, AspNetUserRoles.RoleId, AspNetRoles.Name from AspNetRoles, AspNetUsers, AspNetUserRoles
 where AspNetUsers.Id in (AspNetUserRoles.UserId) and AspNetUserRoles.RoleId = AspNetRoles.Id order by AspNetUsers.Email;
 select * from AspNetUsers;
  select * from AspNetRoles;
  select * from AspNetUserRoles;
  select * from AspNetUsers;
 alter table AspNetUsers add Country nvarchar (256), City nvarchar(256), Address nvarchar(256); 
DELETE FROM AspNetUserRoles;
update AspNetUsers set  UserName = '80500451040j@gmail.com' where Id = 'd072c6f4-e7e6-4ab9-a488-0b9efb14fec9';

 insert into AspNetRoles (Id, Name) values ( (select COUNT(Id) + 1 from AspNetRoles), 'Admin' );
 insert into AspNetUserRoles(userid, roleid) values ( 'ce2f2694-a66e-453a-8555-be3d1191b19d', 1 );
 ALTER TABLE AspNetUserRoles ADD Id int NOT NULL IDENTITY(1, 1); 
 delete from AspNetUsers where id = '3';

 alter table AspNetUsers Drop ADDRESS, COUNTRY, CITY;
 drop table AspNetUserRoles;
 drop table AspNetRoles;
 drop table AspNetUserClaims;
 drop table AspNetUserLogins;
 drop table AspNetUsers;
 drop table __MigrationHistory;
 select * from [dbo].[__MigrationHistory];

 -- notifications 
 drop table Notifications;
 create table Notifications (
	Id int identity not null primary key,
	isReaded bit not null,
	notifName nvarchar(128) not null,
	notifDescription nvarchar(256) not null,
	notifData DateTime not null  
 );

 -- enable broker
 alter database [ASP.NET_PersonsControl] set Enable_Broker with rollback immediate;

<<<<<<< HEAD
 insert into Notifications ( isReaded, notifName, notifDescription, notifData) values ( 0, 'Title2', 'Desc2!!!', GETDATE());
=======
 insert into Notifications ( isReaded, notifName, notifDescription, notifData) values ( 0, 'Title', 'Desc!!!', GETDATE());
>>>>>>> ed0a23d8c2fcb416b735a5b387cfac2be1d5435f
