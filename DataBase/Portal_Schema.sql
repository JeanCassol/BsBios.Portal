CREATE TABLE BSBIOS.USUARIO
  (
  Id NUMBER NOT NULL,
  Nome VARCHAR2 (80) NOT NULL,
  Login VARCHAR2 (20) NOT NULL,
  Senha VARCHAR2 (24) NOT NULL,
  Email VARCHAR2 (100) NOT NULL,
  Perfil INT NOT NULL
 )
/
COMMENT ON COLUMN BSBIOS.USUARIO.Id IS 'Chave da Tabela'
/
COMMENT ON COLUMN BSBIOS.USUARIO.Email IS 'E-mail utilizado para entrar em contato com o usu�rio'
/
COMMENT ON COLUMN BSBIOS.USUARIO.Perfil IS '1 - Comprador; 2 = Fornecedor'
/
ALTER TABLE BSBIOS.USUARIO ADD CONSTRAINT PK_USUARIO
  PRIMARY KEY (
  ID
)
 ENABLE 
 VALIDATE 
/
CREATE SEQUENCE BSBIOS.USUARIO_ID_SEQUENCE
 START WITH  1
 INCREMENT BY  1
 MINVALUE  1
/

INSERT INTO USUARIO
(Id, Nome, Login, Senha, Email)
SELECT usuario_id_sequence.NEXTVAL, 'Administrador Fusion', 'admfusion', 'nMt6vfHriwbmCFAim+R8qw==','mauro.leal@fusionconsultoria.com.br'
FROM DUAL
/
CREATE TABLE BSBIOS.PRODUTO
(
    Id NUMBER NOT NULL,
    CodigoSap VARCHAR2(18) NOT NULL,
    Descricao VARCHAR2(40) NOT NULL
)
/
ALTER TABLE BSBIOS.PRODUTO ADD CONSTRAINT PK_PRODUTO
  PRIMARY KEY (
  ID
)
 ENABLE 
 VALIDATE 
/
CREATE SEQUENCE BSBIOS.PRODUTO_ID_SEQUENCE
 START WITH  1
 INCREMENT BY  1
 MINVALUE  1
/
CREATE TABLE BSBIOS.FORNECEDOR
(
    Id NUMBER NOT NULL,
    CodigoSap VARCHAR2(10) NOT NULL,
    Nome VARCHAR2(35) NOT NULL
)
/
ALTER TABLE BSBIOS.FORNECEDOR ADD CONSTRAINT PK_FORNECEDOR
  PRIMARY KEY (
  ID
)
 ENABLE 
 VALIDATE 
/
CREATE SEQUENCE BSBIOS.FORNECEDOR_ID_SEQUENCE
 START WITH  1
 INCREMENT BY  1
 MINVALUE  1
/

--TABELA PRODUTOFORNECEDOR (INICIO)
CREATE TABLE BSBIOS.PRODUTOFORNECEDOR
(
    IdProduto NUMBER,
    IdFornecedor NUMBER
)
/
ALTER TABLE BSBIOS.PRODUTOFORNECEDOR ADD CONSTRAINT PK_PRODUTOFORNECEDOR
PRIMARY KEY (IdProduto, IdFornecedor)
ENABLE
VALIDATE
/
ALTER TABLE BSBIOS.PRODUTOFORNECEDOR ADD CONSTRAINT FK_PRODUTOFORNECEDOR_PRODUTO
foreign KEY (IdProduto) REFERENCES BSBIOS.PRODUTO(Id)
/
ALTER TABLE BSBIOS.PRODUTOFORNECEDOR ADD CONSTRAINT FK_PRODUTOFORNECEDOR_FORNEC
foreign KEY (IdFornecedor) REFERENCES BSBIOS.fornecedor(Id)
/

--TABLEA PRODUTOFORNECEDOR (FIM)

--TABELA CONDI��O DE PAGAMENTO (INICIO)
CREATE TABLE BSBIOS.CONDICAOPAGAMENTO
(
    Id NUMBER NOT NULL,
    CodigoSap VARCHAR2(4) NOT NULL,
    Descricao VARCHAR2(50) NOT NULL
)
/
ALTER TABLE BSBIOS.CONDICAOPAGAMENTO ADD CONSTRAINT PK_CONDICAOPAGAMENTO
  PRIMARY KEY (
  ID
)
 ENABLE 
 VALIDATE 
/
CREATE SEQUENCE BSBIOS.CONDICAOPAGAMENTO_ID_SEQUENCE
 START WITH  1
 INCREMENT BY  1
 MINVALUE  1
/
--TABELA CONDI��O DE PAGAMENTO (FIM)

--TABELA IVA (INICIO)
CREATE TABLE BSBIOS.IVA
(
    Id NUMBER NOT NULL,
    CodigoSap VARCHAR2(2) NOT NULL,
    Descricao VARCHAR2(50) NOT NULL
)
/
ALTER TABLE BSBIOS.IVA ADD CONSTRAINT PK_IVA
  PRIMARY KEY (
  ID
)
 ENABLE 
 VALIDATE 
/
CREATE SEQUENCE BSBIOS.IVA_ID_SEQUENCE
 START WITH  1
 INCREMENT BY  1
 MINVALUE  1
/
--TABELA IVA (FIM)


