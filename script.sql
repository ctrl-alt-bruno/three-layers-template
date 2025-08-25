CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Suppliers" (
    "Id" uuid NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Document" character varying(100) NOT NULL,
    "SupplierType" integer NOT NULL,
    "Active" boolean NOT NULL,
    CONSTRAINT "PK_Suppliers" PRIMARY KEY ("Id")
);

CREATE TABLE "Addresses" (
    "Id" uuid NOT NULL,
    "Street" character varying(100) NOT NULL,
    "Number" character varying(100) NOT NULL,
    "Complement" character varying(100),
    "PostalCode" character varying(100) NOT NULL,
    "Region" character varying(100) NOT NULL,
    "City" character varying(100) NOT NULL,
    "State" character varying(100) NOT NULL,
    "SupplierId" uuid NOT NULL,
    CONSTRAINT "PK_Addresses" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Addresses_Suppliers_SupplierId" FOREIGN KEY ("SupplierId") REFERENCES "Suppliers" ("Id") ON DELETE SET NULL
);

CREATE TABLE "Products" (
    "Id" uuid NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Description" character varying(100),
    "Value" numeric(18,2) NOT NULL,
    "CreationDate" timestamp with time zone NOT NULL,
    "Active" boolean NOT NULL,
    "SupplierId" uuid NOT NULL,
    CONSTRAINT "PK_Products" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Products_Suppliers_SupplierId" FOREIGN KEY ("SupplierId") REFERENCES "Suppliers" ("Id") ON DELETE SET NULL
);

CREATE UNIQUE INDEX "IX_Addresses_SupplierId" ON "Addresses" ("SupplierId");

CREATE INDEX "IX_Products_SupplierId" ON "Products" ("SupplierId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250720020208_Initial', '8.0.8');

COMMIT;

