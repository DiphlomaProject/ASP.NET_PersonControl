select * from AspNetUsers where id in (select USERID from AspNetUserRoles where RoleId in (select Id from AspNetRoles) );
select AspNetUsers.Email, AspNetUserRoles.UserId, AspNetUserRoles.RoleId, AspNetRoles.Name from AspNetRoles, AspNetUsers, AspNetUserRoles
 where AspNetUsers.Id in (AspNetUserRoles.UserId) and AspNetUserRoles.RoleId = AspNetRoles.Id order by AspNetUsers.Email;
 select * from AspNetUsers;
  select * from AspNetRoles;
  select * from AspNetUserRoles;
  select * from AspNetUsers;
 alter table AspNetUsers add Country nvarchar (256), City nvarchar(256), Address nvarchar(256); 

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