Create DataBase MCTS
go
use MCTS
go
--Group nghe hoặc viết
create table GROUPS (
	ID int identity(1,1) primary key,
	Name nvarchar(50),
	AudioName nvarchar(max),
	Status bit,	
)
--Cấu trúc đề thi gồm 8 phần mõi phần thuộc 1 group
create table PART (
	ID int identity(1,1) primary key,
	Name nvarchar(50),
	IdGroup int,
	Descriptions nvarchar(max),
	Status bit,
	foreign key (IdGroup) references GROUPS (Id) ON DELETE CASCADE
)
--Trong đề có nhiều loại câu
create table TYPE_QUESTIONS
(
	ID int identity(1,1) primary key,
	Name nvarchar(50),
	Descriptions nvarchar(max),
	Status bit
)
-- 1 loại sẽ có nhiều nhóm câu hỏi
create table GROUP_TYPE_QUESTIONS
(
	ID int identity(1,1) primary key,
	Name nvarchar(50),
	Descriptions nvarchar(max),
	Images nvarchar(max),
	Status bit,
	IdTypeQuestion int,
	foreign key (IdTypeQuestion) references TYPE_QUESTIONS (Id) ON DELETE CASCADE
)
--Mõi Câu hỏi chỉ nằm trong 1 phần
Create table QUESTIONS
(
	ID int identity(1,1) primary key,
	Descriptions nvarchar(max),
	LevelOfDificult float,
	Distinctiveness float,
	Images nvarchar(max),
	IdPart int,
	IdGroupTypeQuestions int,
	Status bit,
	foreign key (IdPart) references PART (Id) ON DELETE CASCADE,
	foreign key (IdGroupTypeQuestions) references GROUP_TYPE_QUESTIONS (Id) ON DELETE CASCADE
)

-- mõi câu hỏi có nhiều đáp án
create table ANSWERS (
	ID int identity(1,1) primary key,
	Descriptions nvarchar(max),
	Status bit,
	JammingLevel float,
	IdQuestion int,
	foreign key (IdQuestion) references QUESTIONS (Id) ON DELETE CASCADE
)
--Người dùng---------------------------------
create table USERS (
	Id int identity(1,1) primary key,
	Name nvarchar(50),
	Passwords nvarchar(max),
	Descriptions nvarchar(max),
	Status bit
)

-------------Đề thi---------------------------------------------------------
create table EXAMS (
	ID int identity(1,1) primary key,
	Name nvarchar(max),
	Descriptions nvarchar(max),
	CreateAt nvarchar(50),
	CreateBy int,
	ApproveBy int,
	TimeLimit nvarchar(50),
	Reliability float,
	LevelOfDificult float, 
	ExamDate nvarchar(100),
	StartTime nvarchar(50),
	EndTime nvarchar(50),
	Status bit,
	foreign key (CreateBy) references USERS (Id) ON DELETE CASCADE,
	foreign key (ApproveBy) references USERS (Id)
)
------------------------------------------------------------------------
-- Câu hỏi của đề
create table QUESTION_OF_EXAMS (
	ID int identity(1,1) primary key,
	IdExam int,
	IdQuestion int,
	foreign key (IdExam) references EXAMS (Id) ON DELETE CASCADE,
	foreign key (IdQuestion) references QUESTIONS (Id) ON DELETE CASCADE
)
--------------------Ca thi------------------------------------------------
-- Một đề thi chia làm nhiều ca thi
create table EXAM_CODES (
	ID int identity(1,1) primary key,
	Code nvarchar(50),
	IdExam int,
	foreign key (IdExam) references EXAMS (Id) ON DELETE CASCADE
)
-- Thí sinh
create table CANDIDATES (
	ID int identity(1,1) primary key,
	Name nvarchar(50),
	DateOfBirth date,
	Phone nvarchar(20),
	Address nvarchar(max),
	Email nvarchar(max),	
	Image nvarchar(max),
	Status bit,
)
-- Một thí sinh có nhiều kết quả thi , 1 đề thi do nhiều thí sinh thi
create table RESULT (
	ID int identity(1,1) primary key,
	TotalScore int,
	IdExamCode int,
	IdCandidate int,
	foreign key (IdExamCode) references EXAM_CODES (Id) ON DELETE CASCADE,
	foreign key (IdCandidate) references CANDIDATES (Id) ON DELETE CASCADE
)
CREATE TABLE PermissionGroup 
(
    ID int IDENTITY PRIMARY KEY,
    Name NVARCHAR(max),
    Code NVARCHAR,
    Status NVARCHAR(50)
)
CREATE TABLE Permission
(
     Id int IDENTITY PRIMARY KEY,
     Name NVARCHAR(max),
     IdPermissionGroup int,
     Status bit,
     FOREIGN KEY (IdPermissionGroup) REFERENCES PermissionGroup (Id) ON DELETE CASCADE
)
CREATE TABLE Permission_Detail
(
    ID int IDENTITY PRIMARY KEY,
    Name NVARCHAR(50),
    IdPermission int,
    Status NVARCHAR(50),
    FOREIGN KEY (IdPermission) REFERENCES Permission(ID) ON DELETE CASCADE,
)
CREATE TABLE Roles
(
    ID int IDENTITY PRIMARY KEY,
    Name NVARCHAR(50)
)
CREATE TABLE Roles_Detail
(
   ID int IDENTITY PRIMARY KEY,
   IdRoles int,
   IdPermissionDetail int,
   FOREIGN KEY (IdRoles) REFERENCES Roles (Id) ON DELETE CASCADE,
   FOREIGN KEY (IdPermissionDetail) REFERENCES Permission_Detail(Id) ON DELETE CASCADE
)
Create TABLE User_Role_Detail
(
    ID int IDENTITY PRIMARY KEY,
    IdUser int,
    IdRoles int,
    FOREIGN KEY (IdUser) REFERENCES USERS ON DELETE CASCADE,
    FOREIGN KEY(IdRoles) REFERENCES Roles ON DELETE CASCADE
)
create TABLE USERS_Permission_Detail
(
    ID int IDENTITY PRIMARY KEY,
    IdUser int,
    IdPermissionDetail int,
    FOREIGN KEY (IdUser) REFERENCES USERS ON DELETE CASCADE,
    FOREIGN KEY(IdPermissionDetail) REFERENCES Permission_Detail(Id) ON DELETE CASCADE
)


-----------------------------------------------------------------------------------------------Data-----------------------------------------------------------------------------------------------
insert into GROUPS
values ('Listen','','1'),
	   ('Reading,','','1')
insert into PART --(TB_TYPE)
values  ('Type 1',1,'Picture Description','1'),
		('Type 2',1,'You will hear a question or statement and three responses spoken in English. They will not be printed in your test book and will be spoken only one time. Select the best response to the question or statement and mark the letter (A), (B), or (C) on your answer sheet. ','1'),
		('Type 3',1,'You will hear some conversations between two or more people. You will be asked to answer three questions about what the speakers say in each conversation. Select the best response to each question and mark the letter (A), (B), (C), or (D) on your answer sheet. The conversations will not be printed in your test book and will be spoken only one time. ','1'),
		('Type 4',1,'You will hear some talks given by a single speaker. You will be asked to answer three questions about what the speaker says in each talk. Select the best response to each question and mark the letter (A), (B), (C), or (D) on your answer sheet. The talks will not be printed in your test book and will be spoken only one time.','1'),
		('Type 5',2,'A word or phrase is missing in each of the sentences below. Four answer choices are given below each sentence. Select the best answer to complete the sentence. Then mark the letter (A), (B), (C), or (D) on your answer sheet. ','1'),
		('Type 6',2,'Read the texts that follow. A word, phrase, or sentence is missing in parts of each text. Four answer choices for each question are given below the text. Select the best answer to complete the text. Then mark the letter (A), (B), (C), or (D) on your answer sheet. ','1'),
		('Type 7',2,'Read the texts that follow. A word, phrase, or sentence is missing in parts of each text. Four answer choices for each question are given below the text. Select the best answer to complete the text. Then mark the letter (A), (B), (C), or (D) on your answer sheet. ','1'),
		('Type 8',2,'In this part you will read a selection of texts, such as magazine and newspaper articles, e-mails, and instant messages. Each text or set of texts is followed by several questions. Select the best answer for each question and mark the letter (A), (B), (C), or (D) on your answer sheet. ','1')
insert into TYPE_QUESTIONS
values  ('Listen Full','7=>31','1'),
		('Listen Group','32=>61','1'),
		('listen Group Image','62=>67','1'),
		('Single Question','101=>130','1'),
		('Group Question','131 => 146','1')
insert into GROUP_TYPE_QUESTIONS
values	('Group1_Part6','(3 September)-Five years ago, Brian Trang signed a five-year lease to open his restaurant, 
Trang''s Bistro, at 30 Luray Place. Mr. Trang admits that the first two years of operation were quite ----- r . "We offer spicy food from Vietnam''s central region," he explains. "We didn''t do well at first---- : the cuisine is based on unfamiliar herbs and hot flavors. It took a while to catch on with customers." But Mr. Trang was confident the food would gain in popularity, and he was correct. 
----- . Mr. Trang has just signed another five-year lease, and he is planning ------- the space . ----. next year.','','1',5),
		('NoneGroup','','','1',4),
		('Group2_Part7','','Application_Form.jpg','1',5)
insert into QUESTIONS
values	('At which event is the announcement being made? ','','','',4,2,1),
		('Who most likely is the speaker? ','','','',4,2,1),
		('','','','',6,1,1),
		('','','','',6,1,1),
		('Why did Ms. Constantini fill out the form?','','','',7,3,1),
		(' What instructions are included?','','','',7,3,1)
		
insert into ANSWERS
values  ('A book fair ','true','',1),
		('A product launch ','false','',1),
		('A technology conference ','false','',1),
		('A charity fundraiser ','false','',1),
		('An investment banker ','false','',2),
		('A city official','false','',2),
		('A food scientist','true','',2),
		('A restaurant manager','false','',2),
		('Competitive','false','',3),
		('Potential','false','',3),
		('Challenging','true','',3),
		('Rewarding','false','',3),
		('Renovate','false','',4),
		('Being renovated','false','',4),
		('Renovates','true','',4),
		('To renovate','false','',4),
		('To authorize a charge to her credit card','false','',5),
		('To be assigned to a new company division','false','',5),
		('To request a document renewal','true','',5),
		('To report lost equipment','false','',5),
		('Where to send the form','false','',6),
		('How to complete the form','false','',6),
		('When to submit the application','true','',6),
		('What documentation to attach','false','',6)

insert into USERS
values  ('ngocph','123','','1'),
		('KhangNX','321','','1')

insert into EXAMS
values ('Toeic Tuần 3 tháng 7','','28/04/2020',1,2,'120','','','23/07/2020','13:00','15:00','1'),
		('Toeic Tuần 4 tháng 8','','28/05/2020',1,2,'120','','','12/08/2020','13:00','15:00','1')

insert into QUESTION_OF_EXAMS
values	(1,1),
		(1,2),
		(1,3),
		(1,4),
		(1,5),
		(1,6)

set dateformat dmy
insert into CANDIDATES
values	('Pham Huu Ngoc', '05/05/1995', '0909090909', '140 Le Trong Tan', 'ngoc@example.com', '',''),
		('Nguyen Xuan Khang', '07/10/1995', '0808080808', '140 Phan Van Hon', 'khang@example.com', '',''),
		('Nguyen Van Lan', '15/05/1995', '09493456430', '30/12 Tan Ky Tan Quy', 'lan@example.com', '','')

insert into EXAM_CODES
values	('453',1),
		('324',1),
		('421',1)

insert into RESULT
values	('',1,1),
		('',2,2),
		('',1,3)
select * from CANDIDATES