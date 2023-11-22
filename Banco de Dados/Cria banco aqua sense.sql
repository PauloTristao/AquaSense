

go
create database AquaSense2

go
use AquaSense2


------------------------------------------Criação da tabela Usuario---------------------------------------------------
go
create table Usuario
(
	id_usuario int primary key identity(1,1) not null,
	login_usuario varchar(100) not null,
	nome_pessoa varchar(100) not null,
	senha varchar(100) not null,
	imagem varbinary(max) null,
	adm bit default(0) null,

	UNIQUE (login_usuario),
)

------------------------------------------Criação da tabela Conj. Habitacional------------------------------------------------
go
create table Conjunto_Habitacional
(
	id_conjunto_habitacional int primary key identity(1,1) not null,
	nome varchar(100) not null,
	endereco varchar(200) not null,
	cnpj varchar(20) not null,
	id_usuario_adm int not null,
	
	foreign key (id_usuario_adm) references Usuario(id_usuario) on delete cascade,

	UNIQUE (id_usuario_adm)
)


------------------------------------------Criação da tabela Sensor--------------------------------------------------
go
create table Sensor
(
	id_sensor int primary key identity(1,1) not null,
	descricao varchar(100) not null,
	codigo_fiware varchar(20) NOT NULL,
)
------------------------------------------Criação da tabela Apartamento--------------------------------------------------
go
create table Apartamento
(
	id_apartamento int primary key identity(1,1) not null,
	descricao varchar(100) not null,
	id_conjunto_habitacional int not null,
	id_sensor int null,
	id_usuario int null,

	UNIQUE (id_sensor),

	foreign key (id_conjunto_habitacional) references Conjunto_Habitacional(id_conjunto_habitacional) on delete cascade,
)

--------------------------------------------SP'S USUARIO------------------------------------------------------------
go
Create or alter procedure spUpdate_Usuario
(
	@id int,
	@login_usuario varchar(max),
	@nome_pessoa varchar(max),
	@senha varchar(max),
	@adm bit,
	@imagem varbinary(max)
)
as
begin
	update Usuario set login_usuario = @login_usuario, nome_pessoa = @nome_pessoa,
					   senha = @senha, adm = @adm, imagem = @imagem 
					   where id_usuario = @id
end

go
create or alter procedure spInsert_Usuario
(
	@id int,
	@login_usuario varchar(max),
	@nome_pessoa varchar(max),
	@senha varchar(max),
	@adm bit,
	@imagem varbinary(max)
)
as
begin
	insert into Usuario (login_usuario, nome_pessoa, senha, adm, imagem) 
			            values (@login_usuario, @nome_pessoa, @senha, @adm, @imagem)
end

go
create or alter procedure spExclui_Usuario
(
	@id int
)
as
begin
 delete Usuario where id_usuario = @id
end
GO

CREATE OR ALTER PROCEDURE [dbo].[spConsultaAvancadaUsuarios]
(
    @login VARCHAR(MAX),
    @nomePessoa VARCHAR(MAX),
    @adm int
)
AS
BEGIN
    IF @adm = 2
    BEGIN
        -- Se @adm for 2, não aplique nenhum filtro para a coluna Adm
        SELECT * 
        FROM Usuario 
        WHERE 
            (@login = '' OR login_usuario LIKE '%' + @login + '%')
            AND (@nomePessoa = '' OR nome_pessoa LIKE '%' + @nomePessoa + '%');
    END
    ELSE
    BEGIN
        -- Se @adm for diferente de 2, aplique o filtro Adm = @adm
        SELECT * 
        FROM Usuario 
        WHERE 
            (@login = '' OR login_usuario LIKE '%' + @login + '%')
            AND (@nomePessoa = '' OR nome_pessoa LIKE '%' + @nomePessoa + '%')
            AND Adm = @adm;
    END
END

------------------------------------------------SP's Login---------------------------------------------------------
GO
create or Alter procedure spConsulta_Acesso
(
	@login_usuario varchar(max),
	@senha varchar(max)
)
as
begin
	select id_usuario, login_usuario, nome_pessoa, senha, imagem, adm from Usuario where login_usuario = @login_usuario and senha = @senha
end
GO

------------------------------------------------SP's Conjunto Habitacional---------------------------------------------------------

go
Create or alter procedure spUpdate_Conjunto_Habitacional
(
	@id int,
	@nome varchar(max),
	@endereco varchar(max),
	@cnpj varchar(max),
	@id_usuario_adm int
)
as
begin
	update Conjunto_Habitacional set nome = @Nome, id_usuario_adm = @id_usuario_adm,
									 endereco = @endereco, cnpj = @cnpj
								 where id_conjunto_habitacional = @id
end

go
create or alter procedure spInsert_ConjuntoHabitacional
(
	@id int,
	@nome varchar(max),
	@endereco varchar(max),
	@cnpj varchar(max),
	@id_usuario_adm int
)
as
begin
	insert into Conjunto_Habitacional(nome, endereco, cnpj, id_usuario_adm) 
			            values (@nome, @endereco, @cnpj, @id_usuario_adm)
end

go
create or alter procedure spConsulta_SensoresDisponiveis
(
	@id_usuario_adm varchar(max)
)
as
begin
 select * from Conjunto_Habitacional where id_usuario_adm = @id_usuario_adm
end
GO

--go
--create or alter procedure spConsulta_PortifolioPorUsuario
--(
--   @id_usuario int
--)
--as
--begin
-- select * from ConjuntoHabitacional where id_usuario = @id_usuario
--end
--GO


------------------------------------------------------SP's GENERICAS-----------------------------------------------

go
create or alter procedure spListagem
(
 @tabela varchar(max),
 @ordem varchar(max))
as
begin
 exec('select * from ' + @tabela +
 ' order by ' + @ordem)
end
GO

create or alter procedure spConsulta
(
 @id int ,
 @tabela varchar(max)
)
as
begin
 declare @sql varchar(max);
 set @sql = 'select * from ' + @tabela +
 ' where id_' + @tabela + ' = ' + cast(@id as varchar(max))
 exec(@sql)
 end
GO

CREATE OR ALTER procedure spDelete
(
 @id int ,
 @tabela varchar(max)
)
as
begin
 declare @sql varchar(max);
 set @sql = ' delete ' + @tabela +
 ' where id_' + @tabela + ' = ' + cast(@id as varchar(max))
 exec(@sql)
end
GO

------------------------------------------------------SP's Sensor-----------------------------------------------

GO
create or alter procedure spInsert_Sensor
 @id int,
 @descricao varchar(max),
 @codigo_fiware varchar(max)
as
begin
 insert into Sensor (descricao, codigo_fiware)
 values (@descricao, @codigo_fiware)
end
GO

create or alter procedure spUpdate_Sensor
(
 @id int,
 @descricao varchar(max),
 @codigo_fiware varchar(max)
)
as
begin
 update Sensor set descricao = @descricao, codigo_fiware = @codigo_fiware where id_Sensor = @id
end
GO

------------------------------------------------------SP's Apartamento-----------------------------------------------
GO
CREATE OR ALTER procedure [dbo].[spConsulta_ApartamentoPorConjuntoHabitacional]
(
   @id_conjunto_habitacional int
)
as
begin
 select * from Apartamento where id_conjunto_habitacional = @id_conjunto_habitacional
end

GO
create or alter procedure spInsert_Apartamento
(
	@id int,
	@descricao varchar(max),
	@id_conjunto_habitacional int,
	@id_sensor int,
	@id_usuario int
)
as
begin
	insert into Apartamento (descricao, id_conjunto_habitacional, id_sensor, id_usuario)
	values (@descricao, @id_conjunto_habitacional, @id_sensor, @id_usuario)
end
GO

go
Create or alter procedure spUpdate_Apartamento
(
	@id int,
	@descricao varchar(max),
	@id_conjunto_habitacional int,
	@id_sensor int,
	@id_usuario int
)
as
begin
	update Apartamento set descricao = @descricao, id_conjunto_habitacional = @id_conjunto_habitacional,
									   id_sensor = @id_sensor, id_usuario = @id_usuario
						where id_Apartamento = @id

end
