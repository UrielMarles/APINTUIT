DROP TABLE clientes;

CREATE TABLE clientes(
ID INT IDENTITY(1,1) PRIMARY KEY,
Nombres VARCHAR(100) not null,
Apellidos VARCHAR(100) not null,
FechaNacimiento DATE,
CUIT VARCHAR(50) not null,
Domicilio VARCHAR(100),
Telefono VARCHAR(50) not null,
Correo VARCHAR(50) not null
);

INSERT INTO clientes (Nombres, Apellidos, FechaNacimiento, CUIT, Domicilio, Telefono, Correo)
VALUES
('Juan', 'Pérez', '1990-05-15', '12345678901', 'Calle 123', '555-1234', 'juan.perez@example.com'),
('María', 'López', '1985-08-20', '98765432109', 'Avenida 456', '555-5678', 'maria.lopez@example.com');


SELECT * FROM clientes;