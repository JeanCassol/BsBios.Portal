CREATE USER BSBIOS IDENTIFIED BY fusion123
DEFAULT TABLESPACE users 
TEMPORARY TABLESPACE temp;

GRANT CONNECT TO BSBIOS;

GRANT CREATE TABLE TO BSBIOS;
GRANT CREATE VIEW TO BSBIOS;
GRANT CREATE PROCEDURE TO BSBIOS;
GRANT CREATE SEQUENCE TO BSBIOS;

GRANT UNLIMITED TABLESPACE TO BSBIOS;



