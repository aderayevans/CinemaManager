select * from CINEMA_USER ur left join CINEMA_USER_TYPE ut on ur.typeuser_alias = ut.typeuser_alias where ur.typeuser_alias = 'ad';

alter table CINEMA_USER add user_fullname varchar(100)

UPDATE CINEMA_USER 
SET user_fullname = 'Quang'
SET username = 'admin';
             
insert into CINEMA_USER (username, typeuser_alias, password, user_fullname)
values ('nv01', 'em', 'f7ea56343549e740f7e0ab12e300eeb69fcfc9857c47458058e69a638c29759f', 'Nguyen Van A')

create table CINEMA
(
	cinemaid int not null identity(1,1),
	cinema_rownum int not null default(8),
	cinema_colnum int not null default(8),
	primary key (cinemaid)
)

create table CINEMA_MOVIE
(
	movieid int not null identity(1,1),
	moviename varchar(100) not null,
	duration int not null,
	primary key (movieid)
)
drop table CINEMA
drop table CINEMA_MOVIE
drop table CINEMA_SEAT
drop table CINEMA_TIME

create table CINEMA_SEAT
(
	row_index int not null,
	col_index int not null,
	cinemaid int not null,
	primary key (row_index, col_index, cinemaid)
)

create table CINEMA_TIME
(
	cinemaid int not null,
	movieid int not null,
	date_time DATE,
	time_start TIME (0) not null,

	primary key (cinemaid, movieid)
)

/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     6/2/2021 12:48:42 PM                         */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('CINEMA_SEAT') and o.name = 'FK_CINEMA_S_RELATIONS_CINEMA')
alter table CINEMA_SEAT
   drop constraint FK_CINEMA_S_RELATIONS_CINEMA
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('CINEMA_TIME') and o.name = 'FK_CINEMA_T_RELATIONS_CINEMA_M')
alter table CINEMA_TIME
   drop constraint FK_CINEMA_T_RELATIONS_CINEMA_M
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('CINEMA_TIME') and o.name = 'FK_CINEMA_T_RELATIONS_CINEMA')
alter table CINEMA_TIME
   drop constraint FK_CINEMA_T_RELATIONS_CINEMA
go

if exists (select 1
            from  sysobjects
           where  id = object_id('CINEMA')
            and   type = 'U')
   drop table CINEMA
go

if exists (select 1
            from  sysobjects
           where  id = object_id('CINEMA_MOVIE')
            and   type = 'U')
   drop table CINEMA_MOVIE
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('CINEMA_SEAT')
            and   name  = 'RELATIONSHIP_1_FK'
            and   indid > 0
            and   indid < 255)
   drop index CINEMA_SEAT.RELATIONSHIP_1_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('CINEMA_SEAT')
            and   type = 'U')
   drop table CINEMA_SEAT
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('CINEMA_TIME')
            and   name  = 'RELATIONSHIP_3_FK'
            and   indid > 0
            and   indid < 255)
   drop index CINEMA_TIME.RELATIONSHIP_3_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('CINEMA_TIME')
            and   name  = 'RELATIONSHIP_2_FK'
            and   indid > 0
            and   indid < 255)
   drop index CINEMA_TIME.RELATIONSHIP_2_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('CINEMA_TIME')
            and   type = 'U')
   drop table CINEMA_TIME
go

/*==============================================================*/
/* Table: CINEMA                                                */
/*==============================================================*/
create table CINEMA (
   CINEMAID             int                  not null identity(1,1),
   CINEMA_ROWNUM        int                  null,
   CINEMA_COLNUM        int                  null,
   constraint PK_CINEMA primary key nonclustered (CINEMAID)
)
go

/*==============================================================*/
/* Table: CINEMA_MOVIE                                          */
/*==============================================================*/
create table CINEMA_MOVIE (
   MOVIEID              int                  not null identity(1,1),
   MOVIENAME            varchar(100)         null,
   DURATION             int                  null,
   constraint PK_CINEMA_MOVIE primary key nonclustered (MOVIEID)
)
go

/*==============================================================*/
/* Table: CINEMA_SEAT                                           */
/*==============================================================*/
create table CINEMA_SEAT (
   CINEMAID             int                  not null,
   ROW_INDEX            int                  not null,
   COL_INDEX            int                  not null,
   constraint PK_CINEMA_SEAT primary key nonclustered (CINEMAID, ROW_INDEX, COL_INDEX)
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_1_FK                                     */
/*==============================================================*/
create index RELATIONSHIP_1_FK on CINEMA_SEAT (
CINEMAID ASC
)
go

/*==============================================================*/
/* Table: CINEMA_TIME                                           */
/*==============================================================*/
create table CINEMA_TIME (
   MOVIEID              int                  not null,
   CINEMAID             int                  not null,
   TIME_START           time(0)             not null,
   CINEMA_DATE          date             not null,
   constraint PK_CINEMA_TIME primary key nonclustered (MOVIEID, CINEMAID, TIME_START, CINEMA_DATE)
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_2_FK                                     */
/*==============================================================*/
create index RELATIONSHIP_2_FK on CINEMA_TIME (
MOVIEID ASC
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_3_FK                                     */
/*==============================================================*/
create index RELATIONSHIP_3_FK on CINEMA_TIME (
CINEMAID ASC
)
go

alter table CINEMA_SEAT
   add constraint FK_CINEMA_S_RELATIONS_CINEMA foreign key (CINEMAID)
      references CINEMA (cinemaID)
go

alter table CINEMA_TIME
   add constraint FK_CINEMA_T_RELATIONS_CINEMA_M foreign key (MOVIEID)
      references CINEMA_MOVIE (MOVIEID)
go

alter table CINEMA_TIME
   add constraint FK_CINEMA_T_RELATIONS_CINEMA foreign key (CINEMAID)
      references CINEMA (CINEMAID)
go


/**/

insert into CINEMA (CINEMA_ROWNUM, cinema_colnum) values (8, 8)

insert into cinema_time (movieid, cinemaid, TIME_START, CINEMA_DATE) values (1, 1, '17:00', CONVERT(DATETIME,'11-12-2021', 105))



select * from cinema_time where cinema_date = CONVERT(DATETIME,'2021-06-02', 105)

UPDATE CINEMA_TIME SET movieid = 1, cinemaid = 1, time_start = '02:00:00', cinema_date = ' 12-12-2021'
                                WHERE movieid = 1 and cinemaid = 1  and time_start = '17:00:00' and cinema_date = CONVERT(DATETIME,'02-06-2021', 105);

/*==============================================================*/
/* Table: TICKET                                                */
/*==============================================================*/
create table TICKET (
   TICKETID             int             not null identity(1,1),
   CIN_cinemaid      int                  not null,
   row_index         int                  not null,
   col_index         int                  not null,
   movieid          int                  not null,
   cinemaid          int                  not null,
   time_start         time(0)             not null,
   cinema_date         date             not null,
   constraint PK_TICKET primary key nonclustered (TICKETID)
)
go
drop table ticket

/*==============================================================*/
/* Index: RELATIONSHIP_5_FK                                     */
/*==============================================================*/
create index RELATIONSHIP_5_FK on TICKET (
movieid ASC,
cinemaid ASC,
time_start ASC,
cinema_date ASC
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_7_FK                                     */
/*==============================================================*/
create index RELATIONSHIP_7_FK on TICKET (
CIN_cinemaid ASC,
row_index ASC,
col_index ASC
)
go

alter table CINEMA_SEAT
   add constraint FK_CINEMA_S_RELATIONS_CINEMA foreign key (cinemaID)
      references CINEMA (cinemaID)
go

alter table CINEMA_SEAT
   add constraint FK_CINEMA_S_RELATIONS_TICKET foreign key (TICKETID)
      references TICKET (TICKETID)
go



alter table CINEMA_TIME
   add constraint FK_CINEMA_T_RELATIONS_CINEMA_M foreign key (MOVIEID)
      references CINEMA_MOVIE (MOVIEID)
go

alter table CINEMA_TIME
   add constraint FK_CINEMA_T_RELATIONS_CINEMA foreign key (cinemaID)
      references CINEMA (cinemaID)
go

alter table TICKET
   add constraint FK_TICKET_RELATIONS_CINEMA_T foreign key (movieid, cinemaid, time_start, cinema_date)
      references CINEMA_TIME (MOVIEID, cinemaID, TIME_START, CINEMA_DATE)
go

alter table TICKET
   add constraint FK_TICKET_RELATIONS_CINEMA_S foreign key (CIN_cinemaid, row_index, col_index)
      references CINEMA_SEAT (cinemaID, ROW_INDEX, COL_INDEX)
go

select * from ticket

select * from CINEMA_TIME ct left join CINEMA_MOVIE cm on ct.MOVIEID = cm.MOVIEID where ct.movieid = 1

alter table cinema_movie add price int;
alter table cinema_seat add state bit;
select * from cinema_movie

select * from CINEMA_TIME ct 
left join CINEMA_MOVIE cm on ct.MOVIEID = cm.MOVIEID 
left join CINEMA_SEAT cs on ct.CINEMAID = cs.CINEMAID 
where ct.movieid = 1

select * from CINEMA_SEAT

update CINEMA_SEAT set time_start = "19:00"
delete CINEMA_SEAT

alter table cinema_seat add time_start time(0);
alter table cinema_seat add cinema_date date;
alter table cinema_seat add movieid int;

alter table cinema_seat
   add constraint FK_cinema_seat_RELATIONS_CINEMA_t foreign key (movieid, cinemaid, time_start, cinema_date)
      references CINEMA_time (movieid, cinemaid, time_start, cinema_date)

alter table cinema_seat
   drop constraint FK_cinema_seat_RELATIONS_CINEMA_t

alter table cinema_seat
   drop column state

select * from cinema_time

select count (*) from ticket where row_index = 

select * from CINEMA_TIME ct left join CINEMA_MOVIE cm on ct.MOVIEID = cm.MOVIEID 
left join CINEMA_SEAT cs on ct.CINEMAID = cs.CINEMAID

select * from cinema_user
select * from cinema
select * from cinema_movie
select * from cinema_time
select * from ticket

delete from cinema_time where movieid = 1 and cinemaid = 10 and time_start = '17:30:00' and cinema_date = CONVERT(DATETIME,'1900-01-01', 105)
                      
insert into TICKET (cin_cinemaid, row_index, col_index, movieid, cinemaid, time_start, cinema_date)
values (1 , 1 ,4 ,1 ,1 ,'15:00:00',CONVERT(DATETIME,'2021-12-11', 105))

select * from cinema_time

declare @movieid varchar(50);
declare @cinemaid varchar(50);
declare @time_start varchar(50);
declare @cinema_date varchar(50);
set @movieid = 5
set @cinemaid = 1
set @time_start = '21:30:00'
set @cinema_date = '5-6-2021'
insert into CINEMA_TIME (movieid, cinemaid, time_start, cinema_date)
values (@movieid,@cinemaid,@time_start,CONVERT(DATETIME,@cinema_date, 105))

select * from cinema_time

insert into TICKET (cin_cinemaid, row_index, col_index, movieid, cinemaid, time_start, cinema_date)
values (1, 3 ,3 ,1 ,1 ,'20:00:00',CONVERT(DATETIME,'03-06-2021', 105))

select * from ticket

SELECT COUNT(*) FROM ticket where 
cin_cinemaid = 1 and
row_index = 0 and
col_index = 0 and
movieid = 1 and
cinemaid = 1 and
time_start = '20:00:00' and
cinema_date = CONVERT(DATETIME,'03-06-2021', 105)