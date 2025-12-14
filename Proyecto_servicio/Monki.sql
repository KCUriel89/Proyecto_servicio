CREATE DATABASE Moneki;
GO
USE Moneki;
GO

-- TABLA: Usuarios 

CREATE TABLE Usuarios (
    ID_Usuario INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario NVARCHAR(100) NOT NULL,
    ApellidoPaterno NVARCHAR(100) NOT NULL,
    ApellidoMaterno NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    TipoUsuario NVARCHAR(50) NOT NULL,
    DireccionUsuario NVARCHAR(300),
    Telefono NVARCHAR(20)
);

INSERT INTO Usuarios (NombreUsuario, ApellidoPaterno, ApellidoMaterno, Contrasena, Email, TipoUsuario, DireccionUsuario, Telefono)
VALUES
('Juan', 'Martínez', 'Rojas', 'Pass123!', 'juan.martinez@example.com', 'Administrador', 'Calle Reforma 123, CDMX', '5544332211'),
('Laura', 'Gómez', 'Sánchez', 'User456!', 'laura.gomez@example.com', 'Operador', 'Av. Insurgentes Sur 800, CDMX', '5588997766'),
('Roberto', 'Vega', 'Hurtado', 'Secure789!', 'roberto.vega@example.com', 'Supervisor', 'Col. Roma Norte 45, CDMX', '5511223344');

select * from Usuarios
GO

 --  TABLA: ADMINISTRADORES

CREATE TABLE Administradores (
    ID_Administrador INT IDENTITY(1,1) PRIMARY KEY,
    NombreAdministrador NVARCHAR(100) NOT NULL,
    Password NVARCHAR(200) NOT NULL
);

INSERT INTO Administradores (NombreAdministrador, Contrasena)
VALUES ('SuperAdmin', 'admin123');

select * from Administradores
GO

-- Tabla: Trabajadores

CREATE TABLE Trabajadores (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    ApellidoPaterno NVARCHAR(100) NOT NULL,
    ApellidoMaterno NVARCHAR(100) NOT NULL,
    Cargo NVARCHAR(100) NOT NULL,
    Departamento NVARCHAR(100) NOT NULL
);

INSERT INTO Trabajadores (Nombre, ApellidoPaterno, ApellidoMaterno, Cargo, Departamento)
VALUES
('Carlos', 'Ramírez', 'López', 'Analista', 'TI'),
('María', 'Hernández', 'García', 'Supervisora', 'Recursos Humanos'),
('Jorge', 'Pérez', 'Santos', 'Técnico', 'Operaciones');

Select * From Trabajadores
GO

-- Tabla: Tramites

CREATE TABLE Tramites (
    ID_Tramite INT IDENTITY(1,1) PRIMARY KEY,
    Tipo NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(MAX) NULL,
    FechaSolicitud DATETIME NOT NULL DEFAULT GETDATE(),
    Estado NVARCHAR(50) NOT NULL,
    ID_Trabajador INT NOT NULL,
    FOREIGN KEY (ID_Trabajador) REFERENCES Trabajadores(ID)
);

INSERT INTO Tramites (Tipo, Descripcion, FechaSolicitud, Estado, ID_Trabajador)
VALUES
('Renovación de Identificación', 'Renovación anual del gafete de acceso', GETDATE(), 'En proceso', 1),
('Alta de Personal', 'Trámite inicial de registro biométrico', GETDATE(), 'Completado', 2),
('Actualización de Datos', 'Actualización de fotografía y datos personales', GETDATE(), 'Pendiente', 3);

select * from Tramites
GO

-- Tabla: Documentos

CREATE TABLE Documentos (
    ID_Documento INT IDENTITY(1,1) PRIMARY KEY,
    NombreArchivo NVARCHAR(255) NOT NULL,
    TipoArchivo NVARCHAR(50) NOT NULL,
    RutaArchivo NVARCHAR(500) NOT NULL,
    ID_Tramite INT NOT NULL,
    FOREIGN KEY (ID_Tramite) REFERENCES Tramites(ID_Tramite)
);

INSERT INTO Documentos (NombreArchivo, TipoArchivo, RutaArchivo, ID_Tramite)
VALUES
('gafete_carlos.pdf', 'PDF', 'C:\\Docs\\Tramites\\gafete_carlos.pdf', 1),
('formato_alta_maria.docx', 'DOCX', 'C:\\Docs\\Tramites\\formato_alta_maria.docx', 2),
('actualizacion_jorge.jpg', 'JPG', 'C:\\Docs\\Tramites\\actualizacion_jorge.jpg', 3);

select * from Documentos
GO

-- Tabla: Biometricos

CREATE TABLE Biometricos (
    ID_Biometrico INT IDENTITY(1,1) PRIMARY KEY,
    ID_Trabajador INT NOT NULL,
    ID_Tramite INT NULL,
    TipoDato NVARCHAR(50) NOT NULL,  -- Huella, Rostro, no se me ocurre otro ejemplo
    Datos VARBINARY(MAX) NOT NULL,   -- Plantilla biométrica ._.
    FechaCaptura DATETIME NOT NULL DEFAULT GETDATE(),

    FOREIGN KEY (ID_Trabajador) REFERENCES Trabajadores(ID),
    FOREIGN KEY (ID_Tramite) REFERENCES Tramites(ID_Tramite)
);

INSERT INTO Biometricos (ID_Trabajador, ID_Tramite, TipoDato, Datos, FechaCaptura)
VALUES
(1, 1, 'Huella', 0xABCD1234, GETDATE()),
(2, 2, 'Rostro', 0xFFEE7788, GETDATE()),
(3, NULL, 'Iris', 0x11223344, GETDATE());  -- Biométrico capturado sin trámite activo (el puro dato sin tramite pues)

select * from Biometricos
GO

SELECT 
    t.ID AS ID_Trabajador,
    t.Nombre AS NombreTrabajador,
    tr.ID_Tramite,
    tr.Tipo AS TipoTramite,
    d.NombreArchivo AS Documento,
    b.TipoDato AS TipoBiometrico,
    b.FechaCaptura
FROM Trabajadores t
LEFT JOIN Tramites tr ON tr.ID_Trabajador = t.ID
LEFT JOIN Documentos d ON d.ID_Tramite = tr.ID_Tramite
LEFT JOIN Biometricos b ON b.ID_Trabajador = t.ID;
