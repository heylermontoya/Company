

-- Crear la tabla de Roles 
CREATE TABLE Roles (
    RoleId SERIAL PRIMARY KEY,
    RoleName VARCHAR(50) NOT NULL UNIQUE CHECK (RoleName IN ('Admin', 'Supervisor', 'Cashier', 'Viewer')), 
    CanCreateTransaction BOOLEAN NOT NULL DEFAULT FALSE,
	CanDeleteTransaction BOOLEAN NOT NULL DEFAULT false	
);

-- Crear la tabla de Users
CREATE TABLE Users (
    UserId SERIAL PRIMARY KEY,
    UserName VARCHAR(100) NOT NULL UNIQUE,
    Email VARCHAR(150) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL
);

-- Crear la tabla intermedia UsersInRoles
CREATE TABLE UsersInRoles (
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId) ON DELETE CASCADE
);

-- Crear la tabla de Products
CREATE TABLE Products (
    ProductId SERIAL PRIMARY KEY,
    ProductName VARCHAR(100) NOT NULL,
    Inventory INT NOT NULL CHECK (Inventory >= 0),
    Price NUMERIC(10, 2) NOT NULL CHECK (Price >= 0),
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE -- Nuevo campo para borrado lógico
);

-- Crear la tabla de Transactions
CREATE TABLE Transactions (
    TransactionId SERIAL PRIMARY KEY,
    ProductId INT NOT NULL,
    UserId INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    IsDeleted BOOLEAN NOT NULL DEFAULT false, -- Nuevo campo para borrado lógico
    TransactionDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE SET NULL
);

insert into public.Roles
(RoleName,CanCreateTransaction,CanDeleteTransaction)
values
('Admin',true,true), ('Supervisor',false,true), ('Cashier',true,false), ('Viewer',false,false)
