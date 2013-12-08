----COMANDOS PARA EXECUTAR NO SERVIDOR ONDE SERÁ FEITO BACKUP

--comandos para executar com usuario sys no sqlplus
grant create any directory to bsbios;
grant EXP_FULL_DATABASE to bsbios;
grant IMP_FULL_DATABASE to bsbios;

--comandos para executar com o usuário que vai fazer o backup do seu esquema no sqlplus
create or replace directory dmp_dir as 'c:\tmp';

--comandos para executar no prompt de comando
expdp bsbios/fusion123 schemas=bsbios directory=dmp_dir dumpfile=bsbios_20131119.dmp logfile=bsbios_20131119.log


--=====================================================================
--COMANDOS PARA EXECUTAR NO SERVIDOR ONDE SERÁ FEITO RESTORE

--comandos para executar com usuário sys no sqlplus
conn sys/fusion123 as sysdba;
drop user bsbios CASCADE;
create user bsbios identified by fusion123 
DEFAULT TABLESPACE users 
TEMPORARY TABLESPACE temp;
grant resource, connect, create view, create table, create sequence, create any directory, unlimited tablespace, EXP_FULL_DATABASE, IMP_FULL_DATABASE to bsbios;

--comandos para executar com o usuário que vai fazer o restore
create or replace directory dmp_dir as 'c:\tmp';


--comando para executar no prompt de comando
impdp bsbios/fusion123 schemas=bsbios directory=dmp_dir dumpfile=bsbios_20131119.dmp logfile=bsbios_20131119.log




