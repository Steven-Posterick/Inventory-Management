-- Table: public.Product
-- DROP TABLE public."Product";

CREATE TABLE IF NOT EXISTS public."Product"
(
    "Id" integer NOT NULL,
    "Description" character varying(50) COLLATE pg_catalog."default",
    "Price" numeric(18,2),
    "Cost" numeric(18,2),
    CONSTRAINT "Product_pkey" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE public."Product"
    OWNER to postgres;

-- Table: public.Receipt_Record
-- DROP TABLE public."Receipt_Record";

CREATE TABLE IF NOT EXISTS public."Receipt_Record"
(
    "Id" integer NOT NULL,
    "Price" numeric(18,2),
    CONSTRAINT "Receipt_Record_pkey" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE public."Receipt_Record"
    OWNER to postgres;
    
-- Table: public.Received_Record
-- DROP TABLE public."Received_Record";

CREATE TABLE IF NOT EXISTS public."Received_Record"
(
    "Id" integer NOT NULL,
    "Cost" numeric(18,2),
    CONSTRAINT "Received_Record_pkey" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE public."Received_Record"
    OWNER to postgres;
    

-- Table: public.Allocation
-- DROP TABLE public."Allocation";

CREATE TABLE IF NOT EXISTS public."Allocation"
(
    "ReceiptId" integer NOT NULL,
    "ReceivedId" integer NOT NULL,
    "Allocated_Quantity" integer,
    CONSTRAINT "Allocation_pkey" PRIMARY KEY ("ReceiptId", "ReceivedId"),
    CONSTRAINT "ReceiptId_fkey" FOREIGN KEY ("ReceiptId")
        REFERENCES public."Receipt_Record" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID,
    CONSTRAINT "ReceivedId_fkey" FOREIGN KEY ("ReceivedId")
        REFERENCES public."Received_Record" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
)

TABLESPACE pg_default;

ALTER TABLE public."Allocation"
    OWNER to postgres;

-- Table: public.Record
-- DROP TABLE public."Record";

CREATE TABLE IF NOT EXISTS public."Record"
(
    "RecordId" integer NOT NULL,
    "ProductId" integer,
    "Quantity" integer,
    "Date" date,
    CONSTRAINT "Record_pkey" PRIMARY KEY ("RecordId"),
    CONSTRAINT "ProductId" FOREIGN KEY ("ProductId")
        REFERENCES public."Product" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT "Receipt_RecordId_fkey" FOREIGN KEY ("RecordId")
        REFERENCES public."Receipt_Record" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID,
    CONSTRAINT "Received_RecordId_fkey" FOREIGN KEY ("RecordId")
        REFERENCES public."Received_Record" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
)

TABLESPACE pg_default;

ALTER TABLE public."Record"
    OWNER to postgres;