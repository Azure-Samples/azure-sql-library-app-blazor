-- Copyright (c) Microsoft Corporation.
-- Licensed under the MIT License.
use master;
go

if not exists 
    (select name from sys.databases where name = N'library')
create database library;
go

use library;
go

drop table if exists dbo.books_authors;
drop table if exists dbo.books;
drop table if exists dbo.authors;
drop sequence if exists dbo.globalId;
go

create sequence dbo.globalId
as int
start with 1000000
increment by 1
go

create table dbo.books
(
    id int not null primary key default (next value for dbo.globalId),
    title nvarchar(1000) not null,
    [year] int null,
    [pages] int null
)
go

create table dbo.authors
(
    id int not null primary key default (next value for dbo.globalId),
    first_name nvarchar(100) not null,
    middle_name nvarchar(100) null,
    last_name nvarchar(100) not null
)
go

create table dbo.books_authors
(
    author_id int not null foreign key references dbo.authors(id),
    book_id int not null foreign key references dbo.books(id),
    primary key (
        author_id,
        book_id
    )
)
go

create nonclustered index ixnc1 on dbo.books_authors(book_id, author_id)
go

insert into dbo.authors 
values
  (1, 'Isaac', 'Yudovick', 'Asimov'),
  (2, 'Arthur', 'Charles', 'Clarke'),
  (3, 'Herbert', 'George', 'Wells'),
  (4, 'Jules', 'Gabriel', 'Verne'),
  (5, 'Philip', 'Kindred','Dick');
GO

insert into dbo.books
values
    (1000, 'Prelude to Foundation', 1988, 403),
    (1001, 'Forward the Foundation', 1993, 417),
    (1002, 'Foundation', 1951, 255),
    (1003, 'Foundation and Empire', 1952, 247),
    (1004, 'Second Foundation', 1953, 210),
    (1005, 'Foundation''s Edge', 1982, 367),
    (1006, 'Foundation and Earth', 1986, 356),
    (1007, 'Nemesis', 1989, 386),
    (1008, '2001: A Space Odyssey', 1968, 221),
    (1009, '2010: Odyssey Two', 1982, 291),
    (1010, '2061: Odyssey Three ', 1987, 256),
    (1011, '3001: The Final Odyssey ', 1997, 288),
    (1012, 'The Time Machine', 1895, 118),
    (1013, 'The Island of Doctor Moreau', 1896, 153),
    (1014, 'The Invisible Man', 1897, 151),
    (1015, 'The War of the Worlds', 1898, 192),
    (1016, 'Journey to the Center of the Earth', 1864, 183),
    (1017, 'Twenty Thousand Leagues Under the Sea', 1870, 187),
    (1018, 'Around the World in Eighty Days', 1873, 167),
    (1019, 'From the Earth to the Moon', 1865, 186),
    (1020, 'Do Androids Dream of Electric Sheep?', 1968, 244),
    (1021, 'Ubik', 1969, 224),
    (1022, 'The Man in the High Castle', 1962, 259),
    (1023, 'A Scanner Darkly', 1977, 224);
go

insert into dbo.books_authors
values
    (1, 1000),
    (1, 1001),
    (1, 1002),
    (1, 1003),
    (1, 1004),
    (1, 1005),
    (1, 1006),
    (1, 1007),
    (2, 1008),
    (2, 1009),
    (2, 1010),
    (2, 1011),
    (3, 1012),
    (3, 1013),
    (3, 1014),
    (3, 1015),
    (4, 1016),
    (4, 1017),
    (4, 1018),
    (4, 1019),
    (5, 1020),
    (5, 1021),
    (5, 1022),
    (5, 1023);
go

create or alter view dbo.vw_books_details
as
    with
        aggregated_authors
        as
        (
            select
                ba.book_id,
                string_agg(concat(a.first_name, ' ', (a.middle_name + ' '), a.last_name), ', ') as authors
            from
                dbo.books_authors ba
                inner join
                dbo.authors a on ba.author_id = a.id
            group by
        ba.book_id
        )
    select
        b.id,
        b.title,
        b.pages,
        b.[year],
        aa.authors
    from
        dbo.books b
        inner join
        aggregated_authors aa on b.id = aa.book_id
go

create or alter procedure dbo.stp_get_all_cowritten_books_by_author
    @author nvarchar(100),
    @searchType char(1) = 'c'
as

declare @authorSearchString nvarchar(110);

if @searchType = 'c' 
    set @authorSearchString = '%' + @author + '%' -- contains
else if @searchType = 's' 
    set @authorSearchString = @author + '%' -- startswith
else 
    throw 50000, '@searchType must be set to "c" or "s"', 16;

with
    aggregated_authors
    as
    (
        select
            ba.book_id,
            string_agg(concat(a.first_name, ' ', (a.middle_name + ' '), a.last_name), ', ') as authors,
            author_count = count(*)
        from
            dbo.books_authors ba
            inner join
            dbo.authors a on ba.author_id = a.id
        group by
        ba.book_id
    )
select
    b.id,
    b.title,
    b.pages,
    b.[year],
    aa.authors
from
    dbo.books b
    inner join
    aggregated_authors aa on b.id = aa.book_id
    inner join
    dbo.books_authors ba on b.id = ba.book_id
    inner join
    dbo.authors a on a.id = ba.author_id
where
    aa.author_count > 1
    and
    (
        concat(a.first_name, ' ', (a.middle_name + ' '), a.last_name) like @authorSearchString
    or
    concat(a.first_name, ' ', a.last_name) like @authorSearchString
);
go
