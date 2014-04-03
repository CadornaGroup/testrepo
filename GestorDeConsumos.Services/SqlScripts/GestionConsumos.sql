USE master
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'Consumos')
    DROP DATABASE [Consumos]
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'Consumos')
    CREATE DATABASE [Consumos]
GO

USE [Consumos]
GO

CREATE TABLE [Equipos] (
    imei nvarchar(17) PRIMARY KEY
)

CREATE TABLE [Usuario] (
    idTarjetaAF		 nvarchar(14) PRIMARY KEY, 
    nombre			 nvarchar(100), 
    cantDesayunos	 int, 
    cantAlmuerzos 	 int, 
    ctaCteHabilitada bit
)

CREATE TABLE [Consumo] (
    id				 int identity(1,1) primary key,
    fecha			 datetime, 
    imei			 nvarchar(17),
    idTarjetaAF		 nvarchar(14),
    tipo			 nvarchar(20),
    importe			 smallmoney, 
)

-- datos de prueba
INSERT INTO Equipos VALUES('352001234567890')
INSERT INTO Usuario VALUES('7c7e5007', 'Gentili, Eduardo', 1, 1, 1)
GO

-- Mock procedure
CREATE PROCEDURE [dbo].[registrarConsumo]
   @IMEI varchar(15),
   @Tarjeta varchar(14),
   @TipoDeConsumo varchar(2), -- DS, AL o CC
   @ImporteCuentaCorriente decimal(10, 2)
AS
    IF @TipoDeConsumo <> 'DS' AND @TipoDeConsumo <> 'AL' AND @TipoDeConsumo <> 'CC'
    BEGIN
        SELECT 'ERROR' AS Resultado, N'El tipo de consumo especificado no es válido' AS Mensaje, '' AS NombreUsuario;
        RETURN
    END
    
    IF NOT EXISTS (SELECT * FROM Equipos WHERE imei = @IMEI)
    BEGIN
        SELECT 'ERROR' AS Resultado, N'El imei proporcionado no es válido' AS Mensaje, '' AS NombreUsuario;
        RETURN
    END

    IF NOT EXISTS (SELECT * FROM Usuario WHERE idTarjetaAF = @Tarjeta)
    BEGIN
        SELECT 'ERROR' AS Resultado, N'El id de tarjeta proporcionado no corresponde a un usuario válido' AS Mensaje, '' AS NombreUsuario;
        RETURN
    END    

    DECLARE @cantConsumosFecha int
    SET @cantConsumosFecha = (
        SELECT COUNT(*) 
            FROM Consumo 
            WHERE imei = @IMEI AND
                  idTarjetaAF = @Tarjeta AND 
                  tipo = @TipoDeConsumo AND
                  CONVERT (DATE, fecha) = CONVERT(DATE, getDate())
    )
    
    -- Devolver resultado
    DECLARE @nombreUsuario nvarchar(100);
    SET @nombreUsuario = (SELECT nombre FROM Usuario WHERE idTarjetaAF = @Tarjeta);

    IF ((@TipoDeConsumo = 'DS' OR @TipoDeConsumo = 'AL') AND @cantConsumosFecha >= 1)
        SELECT 'ERROR' AS Resultado, N'El usuario no tiene consumos disponibles' AS Mensaje, @nombreUsuario AS NombreUsuario;
    ELSE BEGIN
        -- Insertar el consumo
        INSERT INTO Consumo(fecha, imei, idTarjetaAF, tipo, importe) 
            VALUES (getDate(), @IMEI, @Tarjeta, @TipoDeConsumo, @ImporteCuentaCorriente);
                    
        SELECT 'OK' AS Resultado, 'Consumo registrado con éxito' AS Mensaje, @nombreUsuario AS NombreUsuario;
        RETURN
    END
GO


-- TESTS
/*
declare @d datetime = getdate()
exec registrarConsumo 
    @imei ='a', 
    @idTarjetaAF = 'a', 
    @fecha = @d, 
    @tipoConsumo = 'tipo invalido'
GO

declare @d datetime = getdate()
exec registrarConsumo 
    @imei ='a', 
    @idTarjetaAF = 'a', 
    @fecha = @d, 
    @tipoConsumo = 'AL'
go	

declare @d datetime = getdate()
exec registrarConsumo 
    @imei ='c', 
    @idTarjetaAF = 'a', 
    @fecha = @d, 
    @tipoConsumo = 'AL'
go	

declare @d datetime = getdate()
exec registrarConsumo 
    @imei ='352001234567890', 
    @idTarjetaAF = '7c7e5007', 
    @fecha = @d, 
    @tipoConsumo = 'AL'
go	

declare @d datetime = dateadd(n, 1, getdate())
exec registrarConsumo 
    @imei ='352001234567890', 
    @idTarjetaAF = '7c7e5007', 
    @fecha = @d, 
    @tipoConsumo = 'AL'
go	

*/
