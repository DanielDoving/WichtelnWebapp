DROP TABLE Comment, Wish, Account;
--
-- Table structure User
--
create table Account(
	"ACCOUNT_ID"	int identity(1,1),
	"EMAIL"			varchar(50),
	"USERNAME"		varchar(20),
	"NAME"			varchar(20),
	"SURNAME"		varchar(20),
	"PASSWORD"		varchar(256)
	primary key (ACCOUNT_ID)
);

--
-- Table structure Wish
--
create table Wish(
	"WISH_ID"		int identity(1,1),
	"FK_ACCOUNT_ID"	int foreign key 
					references Account(ACCOUNT_ID),
	"ITEM_TITLE"	varchar(256),
	"ITEM_DESCRIPTION"	text,
	"GRANTED"		bit default 0,
	"TIMESTAMP"		datetime DEFAULT(GETDATE()),
	primary key (WISH_ID)
);

--
-- Table structure Comment
--
create table Comment(
	"COMMENT_ID"	int identity(1,1),
	"FK_COMMENTER_ID"	int foreign key 
					references Account(ACCOUNT_ID),
	"FK_ACCOUNT_ID"	int foreign key 
					references Account(ACCOUNT_ID),
	"COMMENT"		text
	primary key (COMMENT_ID)
);
