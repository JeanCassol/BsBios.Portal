﻿--INÍCIO DAS MODIFICAÇÕES DO MERGE DE SUPRIMENTOS PARA INTEGRAR COM O FRETE

--PODE SER QUE JÁ EXISTA A SEQUENCIA
CREATE SEQUENCE REQUISICAOCOMPRA_ID_SEQUENCE INCREMENT BY 1 START WITH 1;

ALTER TABLE PROCESSOCOTACAO ADD (JUSTIFICATIVA CLOB );

--ALTERACAO NA TABELA REQUISICAOCOMPRA (INICIO)
ALTER TABLE REQUISICAOCOMPRA 
ADD (CODIGOGRUPODECOMPRA VARCHAR2(3 CHAR) );

ALTER TABLE REQUISICAOCOMPRA ADD MRP NUMBER(1,0);
ALTER TABLE REQUISICAOCOMPRA
ADD CONSTRAINT CHK_REQUISICAOCOMPRA_MRP CHECK 
(MRP BETWEEN 0 AND 1)
ENABLE;
UPDATE REQUISICAOCOMPRA SET MRP = 0;
ALTER TABLE REQUISICAOCOMPRA MODIFY MRP NUMBER(1,0) NOT NULL;

ALTER TABLE REQUISICAOCOMPRA 
ADD (GerouProcessoDeCotacao NUMBER(1,0));

UPDATE REQUISICAOCOMPRA SET GerouProcessoDeCotacao = 0
WHERE GerouProcessoDeCotacao IS NULL;
COMMIT;

--ALTERACAO NA TABELA REQUISICAOCOMPRA (FIM)

--CRIACAO DA TABELA PROCESSODECOTACAOITEM (INICIO)
CREATE TABLE PROCESSOCOTACAOITEM 
(
  ID NUMBER NOT NULL 
, IDPROCESSOCOTACAO NUMBER NOT NULL
, CODIGOPRODUTO VARCHAR2(18 BYTE) NOT NULL
, QUANTIDADE NUMBER(13, 3)  NOT NULL
, CODIGOUNIDADEMEDIDA VARCHAR2(3 CHAR) NOT NULL
, CONSTRAINT PK_PROCESSODECOTACAOITEM PRIMARY KEY 
  (
    ID 
  )
  ENABLE 
);

ALTER TABLE PROCESSOCOTACAOITEM
ADD CONSTRAINT ITEM_PROCESSOCOTACAO FOREIGN KEY
(
  IDPROCESSOCOTACAO 
)
REFERENCES PROCESSOCOTACAO
(
  ID 
)
ENABLE;

ALTER TABLE PROCESSOCOTACAOITEM
ADD CONSTRAINT ITEM_PRODUTO FOREIGN KEY
(
  CODIGOPRODUTO 
)
REFERENCES PRODUTO
(
  CODIGO 
)
ENABLE;

ALTER TABLE PROCESSOCOTACAOITEM
ADD CONSTRAINT ITEM_UNIDADEMEDIDA FOREIGN KEY
(
  CODIGOUNIDADEMEDIDA 
)
REFERENCES UNIDADEMEDIDA
(
  CODIGOINTERNO 
)
ENABLE;

CREATE SEQUENCE PROCESSOCOTACAOITEM_ID_SEQ INCREMENT BY 1 START WITH 1 MINVALUE 1;

--CRIACAO DA TABELA PROCESSODECOTACAOITEM (FIM)

--CRIACAO DA TABELA PROCESSODECOTACAOITEMFRETE (INICIO)

CREATE TABLE PROCESSOCOTACAOITEMFRETE 
(
	ID NUMBER NOT NULL,
	CADENCIA NUMBER(13,3),
	VALORPREVISTO NUMBER(13,2),
	VALORFECHADO NUMBER(13,2),
	TIPODEPRECO NUMBER(1,0) NULL,
	VALORMAXIMO NUMBER(13,2) NULL,
	CONSTRAINT PK_PROCESSOCOTACAOITEMFRETE PRIMARY KEY 
	(
	ID 
	)
	ENABLE 
);

ALTER TABLE PROCESSOCOTACAOITEMFRETE
ADD CONSTRAINT PROCESSOITEMFRETE_ITEM FOREIGN KEY
(
  ID 
)
REFERENCES PROCESSOCOTACAOITEM
(
  ID 
)
ENABLE;
--CRIACAO DA TABELA PROCESSOCOTACAOITEMFRETE (FIM)

--CRIACAO DA TABELA PROCESSODOTACAOITEMMATERIAL (INICIO)
CREATE TABLE PROCESSOCOTACAOITEMMATERIAL 
(
  ID NUMBER NOT NULL 
, IDREQUISICAOCOMPRA NUMBER NOT NULL 
, CONSTRAINT PK_PROCESSOCOTACAOITEMMATERIAL PRIMARY KEY 
  (
    ID 
  )
  ENABLE 
);

ALTER TABLE PROCESSOCOTACAOITEMMATERIAL
ADD CONSTRAINT FK_PROCESSOITEMMATERIAL_ITEM FOREIGN KEY
(
  ID 
)
REFERENCES PROCESSOCOTACAOITEM
(
  ID 
)
ENABLE;

ALTER TABLE PROCESSOCOTACAOITEMMATERIAL
ADD CONSTRAINT FK_PROCESSOITEMMAT_REQUISICAO FOREIGN KEY
(
  IDREQUISICAOCOMPRA 
)
REFERENCES REQUISICAOCOMPRA
(
  ID 
)
ENABLE;
--CRIACAO DA TABELA PROCESSOCOTACAOITEMMATERIAL (FIM)

--CRIACAO DE CHAVE ESTRANGEIRA NA TABELA DE REQUISICAO DE COMPRA (INICIO)
/*ALTER TABLE REQUISICAOCOMPRA
ADD CONSTRAINT FK_REQCOMPRA_PROCESSOITEM FOREIGN KEY
(
  IDPROCESSOCOTACAOITEM 
)
REFERENCES PROCESSOCOTACAOITEM
(
  ID 
)
ENABLE;*/
--CRIACAO DE CHAVE ESTRANGEIRA NA TABELA DE REQUISICAO DE COMPRA (FIM)


--IMPORTACAO DOS DADOS DA TABELA DE PROCESSO DE COTACAO PARA A TABELA DE ITENS (INICIO)
INSERT INTO PROCESSOCOTACAOITEM
(id,idprocessocotacao, codigoproduto, quantidade, codigounidademedida)
select processocotacaoitem_id_seq.nextval,pc.id, codigoproduto, quantidade, codigounidademedida
from processocotacao pc;

INSERT INTO PROCESSOCOTACAOITEMFRETE
(ID, CADENCIA, VALORPREVISTO, VALORFECHADO, TIPODEPRECO, VALORMAXIMO)
select pci.id, CADENCIA, VALORPREVISTO, VALORFECHADO, TIPODEPRECO, VALORMAXIMO
from processocotacaofrete pcf inner join processocotacaoitem pci on pcf.id = pci.idprocessocotacao;

INSERT INTO PROCESSOCOTACAOITEMMATERIAL
(id, idrequisicaocompra)
select item.id, pcm.idrequisicaocompra
from processocotacaoitem item inner join processocotacaomaterial pcm
on item.idprocessocotacao = pcm.id;

/*update requisicaocompra set idprocessocotacaoitem = 
(
  select id
  from processocotacaoitemmaterial item
  where requisicaocompra.id = item.idrequisicaocompra
)
where idprocessocotacaoitem is null;*/

commit;
--IMPORTACAO DOS DADOS DA TABELA DE PROCESSO DE COTACAO PARA A TABELA DE ITENS (FIM)

--EXCLUSAO DAS COLUNAS IMPORTADAS (INICIO)
ALTER TABLE PROCESSOCOTACAOMATERIAL DROP COLUMN IDREQUISICAOCOMPRA;
ALTER TABLE PROCESSOCOTACAO DROP COLUMN CODIGOPRODUTO;
ALTER TABLE PROCESSOCOTACAO DROP COLUMN CODIGOUNIDADEMEDIDA;
ALTER TABLE PROCESSOCOTACAO DROP COLUMN QUANTIDADE;


ALTER TABLE PROCESSOCOTACAOFRETE DROP COLUMN CADENCIA;
ALTER TABLE PROCESSOCOTACAOFRETE DROP COLUMN VALORPREVISTO;
ALTER TABLE PROCESSOCOTACAOFRETE DROP COLUMN VALORFECHADO;
ALTER TABLE PROCESSOCOTACAOFRETE DROP COLUMN TIPODEPRECO;
ALTER TABLE PROCESSOCOTACAOFRETE DROP COLUMN VALORMAXIMO;

--EXCLUSAO DAS COLUNAS IMPORTADAS (FIM)


--CRIACAO DA TABELA COTACAOITEM (INICIO)
CREATE SEQUENCE COTACAOITEM_ID_SEQUENCE INCREMENT BY 1 START WITH 1 MINVALUE 1;
CREATE TABLE COTACAOITEM 
(
  ID NUMBER NOT NULL 
, IDCOTACAO NUMBER NOT NULL 
, IDPROCESSOCOTACAOITEM NUMBER NOT NULL 
, QUANTIDADEDISPONIVEL NUMBER(13, 3) NOT NULL 
, VALORLIQUIDO NUMBER(13, 2) NOT NULL 
, VALORCOMIMPOSTOS NUMBER(13, 2) NOT NULL 
, OBSERVACOES CLOB 
, SELECIONADA NUMBER(1, 0) NOT NULL 
, QUANTIDADEADQUIRIDA NUMBER(13, 3) NULL 
, CONSTRAINT PK_COTACAOITEM PRIMARY KEY 
  (
    ID
  )
  ENABLE 
);

ALTER TABLE COTACAOITEM
ADD CONSTRAINT FK_COTACAOITEM_COTACAO FOREIGN KEY
(
  IDCOTACAO 
)
REFERENCES COTACAO
(
  ID 
)
ENABLE;

ALTER TABLE COTACAOITEM
ADD CONSTRAINT FK_COTACAOITEM_PROCESSOITEM FOREIGN KEY
(
  IDPROCESSOCOTACAOITEM 
)
REFERENCES PROCESSOCOTACAOITEM
(
  ID 
)
ENABLE;

ALTER TABLE COTACAOITEM
ADD CONSTRAINT CHK_COTACAOITEM_SELECIONADA CHECK 
(SELECIONADA BETWEEN 0 AND 1)
ENABLE;

--CRIACAO DA TABELA COTACAOITEM (FIM)

--CRIACAO DA TABELA COTACAOMATERIALITEM (INICIO)
CREATE TABLE COTACAOMATERIALITEM 
(
  ID NUMBER NOT NULL 
, CODIGOIVA VARCHAR2(2 BYTE) 
, MVA NUMBER(13, 2) 
, PRAZOENTREGA DATE NOT NULL 
, CONSTRAINT PK_COTACAOMATERIALITEM PRIMARY KEY 
  (
    ID
  )
  ENABLE 
);

ALTER TABLE COTACAOMATERIALITEM
ADD CONSTRAINT FK_COTACAOMATERIALITEM_COTACAO FOREIGN KEY
(
  ID
)
REFERENCES COTACAOITEM
(
  ID 
)
ENABLE;

ALTER TABLE COTACAOMATERIALITEM
ADD CONSTRAINT FK_COTACAOMATERIALITEM_IVA FOREIGN KEY
(
  CODIGOIVA 
)
REFERENCES IVA
(
  CODIGO 
)
ENABLE;

--CRIACAO DA TABELA COTACAOMATERIALITEM (FIM)

--CRIACAO DA TABELA COTACAOFRETEITEM (INICIO)
CREATE TABLE COTACAOFRETEITEM
(
	ID NUMBER NOT NULL,
	CADENCIA NUMBER(13,3),  
	CONSTRAINT PK_COTACAOFRETEITEM PRIMARY KEY 
	  (
		ID
	  )
	  ENABLE 
);

ALTER TABLE COTACAOFRETEITEM
ADD CONSTRAINT FK_COTACAOFRETEITEM_COTACAO FOREIGN KEY
(
  ID 
)
REFERENCES COTACAOITEM
(
  ID 
)
ENABLE;

--CRIACAO DA TABELA COACAOFRETEITEM (FIM)

--CRIACAO DA TABELA COTACAOITEMIMPOSTO (INICIO)
CREATE TABLE COTACAOITEMIMPOSTO 
(
  IDCOTACAOITEM NUMBER NOT NULL
, TIPOIMPOSTO NUMBER NOT NULL 
, ALIQUOTA NUMBER(4, 2) NOT NULL 
, VALOR NUMBER(13, 2) NOT NULL 
, CONSTRAINT PK_COTACAOITEMIMPOSTO PRIMARY KEY 
  (
    IDCOTACAOITEM    , 
    TIPOIMPOSTO 
  )
  ENABLE 
) ;

ALTER TABLE COTACAOITEMIMPOSTO
ADD CONSTRAINT FK_COTACAOITEMIMPOSTO_COTACAO FOREIGN KEY
(
  IDCOTACAOITEM
)
REFERENCES COTACAOITEM
(
  ID 
)
ENABLE;

--CRIACAO DA TABELA COTACAOITEMIMPOSTO (FIM)

--CRIA REGISTROS NAS TABELAS DE ITEM DE COTACAO DE FRETE
INSERT INTO COTACAOITEM
(id,idcotacao, idprocessocotacaoitem, quantidadedisponivel, valorliquido, valorcomimpostos, observacoes, selecionada, quantidadeadquirida)
select  cotacaoitem_id_sequence.nextval,cotacao.id, item.id, cotacao.quantidadedisponivel, cotacao.valorliquido, cotacao.valorcomimpostos, cotacao.observacoes,
cotacao.selecionada, cotacao.quantidadeadquirida
from fornecedorparticipante fp inner join processocotacaoitem item
on fp.idprocessocotacao = item.idprocessocotacao
inner join cotacao 
on fp.idcotacao = cotacao.id
inner join cotacaofrete
on cotacao.id = cotacaofrete.id;

INSERT INTO COTACAOFRETEITEM
(id, cadencia)
select cotacaoitem.id, cotacaofrete.cadencia
from fornecedorparticipante fp inner join processocotacaoitem item
on fp.idprocessocotacao = item.idprocessocotacao
inner join cotacao 
on fp.idcotacao = cotacao.id
inner join cotacaofrete 
on cotacao.id = cotacaofrete.id
inner join cotacaoitem on
cotacao.id = cotacaoitem.idcotacao
inner join cotacaofrete
on cotacao.id = cotacaofrete.id;


--CRIA REGISTROS NAS TABELAS DE ITEM DE COTACAO DE MATERIAL
INSERT INTO COTACAOITEM
(id, idcotacao, idprocessocotacaoitem, quantidadedisponivel, valorliquido, valorcomimpostos, observacoes, selecionada, quantidadeadquirida)
select cotacaoitem_id_sequence.nextval, cotacao.id, item.id, cotacao.quantidadedisponivel, cotacao.valorliquido, cotacao.valorcomimpostos, cotacao.observacoes,
cotacao.selecionada, cotacao.quantidadeadquirida
from fornecedorparticipante fp inner join processocotacaoitem item
on fp.idprocessocotacao = item.idprocessocotacao
inner join cotacao 
on fp.idcotacao = cotacao.id
inner join cotacaomaterial
on cotacao.id = cotacaomaterial.id;

insert into cotacaomaterialitem
(id, codigoiva, mva, prazoentrega)
select cotacaoitem.id,cotacaomaterial.codigoiva, cotacaomaterial.mva, cotacaomaterial.prazoentrega
from fornecedorparticipante fp inner join processocotacaoitem item
on fp.idprocessocotacao = item.idprocessocotacao
inner join cotacao 
on fp.idcotacao = cotacao.id
inner join cotacaomaterial
on cotacao.id = cotacaomaterial.id
inner join cotacaoitem on
cotacao.id = cotacaoitem.idcotacao;

--CRIA REGISTROS NA TABELA DE IMPOSTOS
insert into cotacaoitemimposto
(idcotacaoitem, tipoimposto, aliquota, valor)
select cotacaoitem.id, tipoimposto, aliquota, valor
from cotacaoimposto inner join cotacaoitem
on cotacaoimposto.idcotacao = cotacaoitem.idcotacao;

COMMIT;

--EXCLUIR COLUNAS QUE FORAM MOVIDAS DA TABELA COTACAO
ALTER TABLE COTACAO DROP COLUMN SELECIONADA;
ALTER TABLE COTACAO DROP COLUMN VALORLIQUIDO;
ALTER TABLE COTACAO DROP COLUMN QUANTIDADEADQUIRIDA;
ALTER TABLE COTACAO DROP COLUMN QUANTIDADEDISPONIVEL;
ALTER TABLE COTACAO DROP COLUMN VALORCOMIMPOSTOS;
ALTER TABLE COTACAO DROP COLUMN OBSERVACOES;

--EXCLUIR COLUNAS QUE FORAM MOVIDAS DA TABELA COTACAOMATERIAL
ALTER TABLE COTACAOMATERIAL DROP COLUMN MVA;
ALTER TABLE COTACAOMATERIAL DROP COLUMN PRAZOENTREGA;
ALTER TABLE COTACAOMATERIAL DROP COLUMN CODIGOIVA;

--EXCLUIR COLUNAS QUE FORAM MOVIDAS DA TABELA COTACAFRETE
ALTER TABLE COTACAOFRETE DROP COLUMN CADENCIA;

--CRIAR COLUNA DE COMPRADOR NO PROCESSO DE COTACAO (INICIO)
ALTER TABLE PROCESSOCOTACAO 
ADD (LOGINCOMPRADOR VARCHAR2(12 BYTE) );

ALTER TABLE PROCESSOCOTACAO
ADD CONSTRAINT FK_PROCESSOCOTACAO_COMPRADOR FOREIGN KEY
(
  LOGINCOMPRADOR 
)
REFERENCES USUARIO
(
  LOGIN 
)
ENABLE;
--CRIAR COLUNA DE COMPRADOR NO PROCESSO DE COTACAO (FIM)

--RENOMEACAO DA COLUNA VALORLIQUIDO PARA PRECO (INICIO)
ALTER TABLE COTACAOITEM RENAME COLUMN VALORLIQUIDO TO PRECO;
--RENOMEACAO DA COLUNA VALORLIQUIDO PARA PRECO (FIM)

--CRIACAO DA COLUNA VALOR LIQUIDO INICIAL NO ITEM DA COTACAO (INICIO)
ALTER TABLE COTACAOITEM 
ADD (PRECOINICIAL NUMBER(13, 2) );

UPDATE COTACAOITEM SET PRECOINICIAL = PRECO
WHERE PRECOINICIAL IS NULL;
COMMIT;

ALTER TABLE COTACAOITEM  
MODIFY (PRECOINICIAL NOT NULL);
--CRIACAO DA COLUNA VALOR LIQUIDO INICIAL NO ITEM DA COTACAO (FIM)

--CRIACAO DA COLUNA CUSTO NO ITEM DE COTACAO (INICIO)
ALTER TABLE COTACAOITEM 
ADD (CUSTO NUMBER(13, 2) );
UPDATE COTACAOITEM SET CUSTO = VALORCOMIMPOSTOS WHERE CUSTO IS NULL;
COMMIT;
ALTER TABLE COTACAOITEM MODIFY CUSTO NUMBER(13,2) NOT NULL;
--CRIACAO DA COLUNA CUSTO NO ITEM DE COTACAO (FIM)

--CRIACAO DA COLUNA Status na tabela RequisicaoCompra (INICIO)
ALTER TABLE REQUISICAOCOMPRA ADD Status NUMBER(1,0);
ALTER TABLE REQUISICAOCOMPRA ADD CONSTRAINT CHK_REQUISICAOCOMPRA_STATUS CHECK
(STATUS BETWEEN 1 AND 2);
UPDATE REQUISICAOCOMPRA SET STATUS = 1;
COMMIT;
ALTER TABLE REQUISICAOCOMPRA MODIFY Status NUMBER(1,0) NOT NULL;
--CRIACAO DA COLUNA Status na tabela RequisicaoCompra (FIM)

--CRIACA DA COLUNA DataFechamento na tabela ProcessoCotacao (INICIO)
ALTER TABLE PROCESSOCOTACAO ADD DATAFECHAMENTO DATE;
UPDATE PROCESSOCOTACAO SET DATAFECHAMENTO = DATALIMITERETORNO
WHERE DATAFECHAMENTO IS NULL;
COMMIT;
--CRIACAO DA COLUNA DataFechamento na tabela ProcessoCotacao (FIM)

--CRIACAO DA TABELA HISTORICODEPRECO (INICIO)
CREATE TABLE HISTORICODEPRECO 
(
  ID NUMBER NOT NULL,
  IDCOTACAOITEM NUMBER(13, 2) NOT NULL 
, DATAHORA DATE NOT NULL 
, VALOR NUMBER(13, 2) NOT NULL,
CONSTRAINT PK_HISTORICODEPRECO PRIMARY KEY (ID) ENABLE,
CONSTRAINT FK_HISTORICOPRECO_COTACAOITEM FOREIGN KEY (IDCOTACAOITEM) REFERENCES COTACAOITEM (ID) ENABLE
);

CREATE SEQUENCE HISTORICODEPRECO_ID_SEQUENCE INCREMENT BY 1 START WITH 1;

--INSERE HISTORICO PARA O PRECO INICIAL
INSERT INTO HISTORICODEPRECO
(ID, IDCOTACAOITEM, DATAHORA, VALOR)
SELECT HISTORICODEPRECO_ID_SEQUENCE.NEXTVAL, CI.ID,SYSDATE,PRECOINICIAL
FROM COTACAOITEM CI INNER JOIN COTACAOMATERIALITEM CIM
ON CI.ID = CIM.ID;

--INSERE HISTOICO PARA O PRECO FINAL, CASO SEJA DIFERENTE DO PRECO INICIAL
INSERT INTO HISTORICODEPRECO
(ID, IDCOTACAOITEM, DATAHORA, VALOR)
SELECT HISTORICODEPRECO_ID_SEQUENCE.NEXTVAL, CI.ID,SYSDATE,PRECO
FROM COTACAOITEM CI INNER JOIN COTACAOMATERIALITEM CIM
ON CI.ID = CIM.ID
WHERE PRECOINICIAL <> PRECO;

COMMIT;
--CRIACAO DA TABELA HISTORICODEPRECO (FIM)

--ADICIONAR COLUNA NUMEROCOTACAO NA TABELA COTACAOMATERIAL (INICIO)
ALTER TABLE FORNECEDORPARTICIPANTE ADD NUMEROCOTACAO VARCHAR2(10 CHAR);
--ADICIONAR COLUNA NUMEROCOTACAO NA TABELA COTACAOMATERIAL (FIM

--FIM DAS MODIFICAÇÕES DO MERGE DE SUPRIMENTOS PARA INTEGRAR COM O FRETE

CREATE OR REPLACE VIEW AGENDAMENTODECARGAVISUALIZACAO AS 
--DADOS DO AGENDAMENTO DE DESCARREGAMENTO
SELECT 'DESC' || IDQUOTA || AC.ID || NF.NUMERO AS ID, IDQUOTA, AC.ID AS IDAGENDAMENTO, NULL AS IDORDEMTRANSPORTE, NULL AS IDCOLETA, M.DESCRICAO AS DESCRICAOMATERIAL, 2  AS CODIGOFLUXO, 'Descarregamento' AS DESCRICAOFLUXO, 
Q.DATA AS DATAAGENDAMENTO, PLACA,NF.NUMERO AS NUMERONF, CNPJEMITENTE, NOMEEMITENTE, AC.REALIZADO, Q.CODIGOTERMINAL, T.NOME AS DESCRICAOTERMINAL, NULL AS CODIGODEPOSITO
FROM agendamentodedescarregamento AD INNER JOIN AGENDAMENTODECARGA AC ON AD.ID = AC.ID
INNER JOIN QUOTA Q ON AC.IDQUOTA = Q.ID
INNER JOIN MATERIALDECARGA M ON Q.CODIGOMATERIAL = M.CODIGO
INNER JOIN notafiscal NF ON AD.ID = NF.IDAGENDAMENTODESCARREGAMENTO
INNER JOIN TERMINAL T ON Q.CODIGOTERMINAL = T.CODIGO

UNION ALL

--DADOS DO AGENDAMENTO DE CARREGAMENTO
SELECT 'DESC' || IDQUOTA || AC.ID AS ID, IDQUOTA, AC.ID AS IDAGENDAMENTO, NULL AS IDORDEMTRANSPORTE, NULL AS IDCOLETA, M.DESCRICAO AS DESCRICAOMATERIAL, 1  AS CODIGOFLUXO, 'Carregamento' AS DESCRICAOFLUXO, 
Q.DATA AS DATAAGENDAMENTO, PLACA,NULL AS NUMERONF, NULL AS CNPJEMITENTE, NULL AS NOMEEMITENTE, AC.REALIZADO, Q.CODIGOTERMINAL, T.NOME AS DESCRICAOTERMINAL, NULL AS CODIGODEPOSITO
FROM agendamentodecarregamento AD INNER JOIN AGENDAMENTODECARGA AC ON AD.ID = AC.ID
INNER JOIN QUOTA Q ON AC.IDQUOTA = Q.ID
INNER JOIN TERMINAL T ON Q.CODIGOTERMINAL = T.CODIGO
INNER JOIN MATERIALDECARGA M ON Q.CODIGOMATERIAL = M.CODIGO


UNION ALL

--DADOS DA COLETA
SELECT 'COLETA' || IDORDEMTRANSPORTE || C.ID || NFC.NUMERO AS ID, NULL AS IDQUOTA, NULL AS IDAGENDAMENTO, IDORDEMTRANSPORTE,C.ID AS IDCOLETA, P.DESCRICAO AS  DESCRICAOMATERIAL, 
2  AS CODIGOFLUXO, 'Descarregamento' AS DESCRICAOFLUXO, C.DATADEPREVISAODECHEGADA AS DATAAGENDAMENTO, C.PLACA, NFC.NUMERO AS NUMERONF,
FM.CNPJ AS CNPJEMITENTE, FM.NOME AS NOMEEMITENTE, C.REALIZADO,T.CODIGO AS CODIGOTERMINAL, T.NOME AS DESCRICAOTERMINAL,PF.CODIGODEPOSITO

FROM COLETA C INNER JOIN ORDEMDETRANSPORTE OT ON C.IDORDEMTRANSPORTE = OT.ID
INNER JOIN NOTAFISCALDECOLETA NFC ON NFC.IDCOLETA = C.ID
INNER JOIN PROCESSOCOTACAOFRETE PF ON OT.IDPROCESSOCOTACAOFRETE = PF.ID
INNER JOIN PROCESSOCOTACAO PC ON PF.ID = PC.ID
INNER JOIN PROCESSOCOTACAOITEM PCI ON PC.ID = PCI.IDPROCESSOCOTACAO
INNER JOIN PRODUTO P ON PCI.CODIGOPRODUTO = P.CODIGO
INNER JOIN TERMINAL T ON PF.CODIGOTERMINAL = T.CODIGO
LEFT JOIN FORNECEDOR FM ON PF.CODIGOFORNECEDOR = FM.CODIGO;


create or replace 
PROCEDURE RELATORDEMTRANSPORTEANALITICO (p_cursor OUT SYS_REFCURSOR, p_status int, p_dataDeValidadeInicial DATE, p_dataDeValidadeFinal DATE,
p_codigoMaterial Produto.Codigo%type, p_material Produto.Descricao%type,
p_codigoFornecedorDaMercadoria Fornecedor.Codigo%type, p_fornecedorDaMercadoria Fornecedor.Nome%type, 
p_codigoTransportadora Fornecedor.Codigo%type, p_transportadora Fornecedor.Nome%type, p_codigoTerminal Terminal.Codigo%type) AS
BEGIN

  open p_cursor for
  SELECT sys_guid() || '' AS "Id",CAST(OT.ID AS INT) AS "IdDaOrdemDeTransporte", Terminal.Nome AS "Terminal", P.CODIGO || ' - ' || P.DESCRICAO AS "Material", FM.CODIGO || ' - ' || FM.NOME AS "NomeDoFornecedorDaMercadoria",
  TO_CHAR(PCF.DATAVALIDADEINICIAL, 'dd/mm/yyyy') AS "DataDeValidadeInicial", TO_CHAR(PCF.DATAVALIDADEFINAL, 'dd/mm/yyyy') AS "DataDeValidadeFinal", CASE WHEN PCF.CLASSIFICACAO = 0 THEN 'Não' ELSE 'Sim' END AS "Classificacao",
  CAST(OT.CADENCIA AS DECIMAL)AS "Cadencia", IT.CODIGO || ' - ' || IT.DESCRICAO AS "Itinerario", MO.NOME || '/' || MO.UF AS "MunicipioDeOrigem",
  MD.NOME || '/' || MD.UF AS "MunicipioDeDestino", T.NOME AS "Transportadora", D.NOME AS "NomeDoDeposito", CAST(OT.QUANTIDADEADQUIRIDA AS DECIMAL) AS "QuantidadeContratada",
  CAST(OT.QUANTIDADELIBERADA AS DECIMAL) AS "QuantidadeLiberada", CAST(OT.QUANTIDADEDETOLERANCIA AS DECIMAL) AS "QuantidadeDeTolerancia",OT.QUANTIDADECOLETADA - OT.QUANTIDADEREALIZADA AS "QuantidadeEmTransito",
  CAST(NVL(CR.QuantidadeDeColetasRealizadas, 0) AS INTEGER) AS "QuantidadeDeColetasRealizadas", CAST(NVL(CR.QuantidadeDeDiasEmAtraso, 0) AS INTEGER) AS "QuantidadeDeDiasEmAtraso", NVL(CR.PercentualDeAtraso, 0) AS "PercentualDeAtraso",
  CAST(OT.QUANTIDADEREALIZADA AS DECIMAL) AS "QuantidadeRealizada", OT.QuantidadeLiberada - OT.QuantidadeRealizada AS "QuantidadePendente", UM.DESCRICAO AS "UnidadeDeMedida",
  MOTIVODEFECHAMENTO.DESCRICAO AS "MotivoDeFechamento", OT.ObservacaoDeFechamento AS "ObservacaoDeFechamento", 
  CAST(OT.VALORPLANEJADO AS DECIMAL) AS "ValorPlanejado", CAST(NVL(OTCT.VALORREALDOFRETE,0) AS DECIMAL) AS "ValorReal"
  FROM ORDEMDETRANSPORTE OT INNER JOIN PROCESSOCOTACAOFRETE PCF ON OT.IDPROCESSOCOTACAOFRETE = PCF.ID
  INNER JOIN PROCESSOCOTACAO PC ON PCF.ID = PC.ID
  INNER JOIN PROCESSOCOTACAOITEM PCI ON PC.ID = PCI.IDPROCESSOCOTACAO
  INNER JOIN PRODUTO P ON PCI.CODIGOPRODUTO = P.CODIGO
  INNER JOIN UNIDADEMEDIDA UM ON PCI.CODIGOUNIDADEMEDIDA = UM.CODIGOINTERNO
  INNER JOIN FORNECEDOR FM ON PCF.CODIGOFORNECEDOR = FM.CODIGO
  INNER JOIN ITINERARIO IT ON PCF.CODIGOITINERARIO = IT.CODIGO
  INNER JOIN MUNICIPIO MO ON PCF.CODIGOMUNICIPIOORIGEM = MO.CODIGO
  INNER JOIN MUNICIPIO MD ON PCF.CODIGOMUNICIPIODESTINO = MD.CODIGO
  INNER JOIN FORNECEDOR T ON OT.CODIGOFORNECEDOR = T.CODIGO
  INNER JOIN TERMINAL ON PCF.CODIGOTERMINAL = TERMINAL.CODIGO
  LEFT JOIN FORNECEDOR D ON PCF.CODIGODEPOSITO = D.CODIGO
  
  LEFT JOIN (
	SELECT CTOT.IDORDEMTRANSPORTE, SUM(VALORREALDOFRETE) AS VALORREALDOFRETE
	FROM CONHECIMENTODETRANSPORTE CT INNER JOIN CONHECIMENTO_ORDEMTRANSPORTE CTOT ON CT.CHAVEELETRONICA = CTOT.CHAVEELETRONICA
	WHERE CT.STATUS = 1
	GROUP BY CTOT.IDORDEMTRANSPORTE
  ) OTCT
  ON OT.ID = OTCT.IDORDEMTRANSPORTE
  
  LEFT JOIN (
    SELECT IDORDEMTRANSPORTE, COUNT(1) AS QuantidadeDeColetasRealizadas, 
    SUM(CASE WHEN DATADECHEGADA > DATADEPREVISAODECHEGADA THEN DATADECHEGADA - DATADEPREVISAODECHEGADA ELSE 0 END) AS QuantidadeDeDiasEmAtraso,
    ROUND(SUM(CASE WHEN DATADECHEGADA > DATADEPREVISAODECHEGADA THEN 1 ELSE 0 END) * 100 / COUNT(1),2) AS PercentualDeAtraso 
    FROM COLETA C 
    WHERE C.REALIZADO = 1
    GROUP BY IDORDEMTRANSPORTE
  ) CR
  ON OT.ID = CR.IDORDEMTRANSPORTE
  LEFT JOIN (
    SELECT 0 AS ID, 'Negociação de Tarifa' AS DESCRICAO FROM DUAL
    UNION 
    SELECT 1 AS ID,'Não cumprimento do Contrato' AS DESCRICAO FROM DUAL 
    UNION 
    SELECT 2 AS ID,'Alteração de local de coleta' AS DESCRICAO FROM DUAL
  ) MOTIVODEFECHAMENTO ON OT.MOTIVODEFECHAMENTO = MOTIVODEFECHAMENTO.ID
  WHERE (p_Status is null or (CASE WHEN OT.QUANTIDADEREALIZADA < OT.QUANTIDADELIBERADA THEN 0 ELSE 1 END) = p_Status)
  AND (p_dataDeValidadeInicial is null or pcf.DataValidadeFinal >= p_dataDeValidadeInicial)
  AND (p_dataDeValidadeFinal is null or pcf.DataValidadeInicial <= p_dataDeValidadeFinal)
  AND (p_CodigoMaterial is null or PCI.CODIGOPRODUTO = p_CodigoMaterial)
  AND (p_material is null or LOWER(P.Descricao) LIKE LOWER('%' || p_material ||'%'))

  AND (p_codigoFornecedorDaMercadoria is null or PCF.CODIGOFORNECEDOR = p_codigoFornecedorDaMercadoria)
  AND (p_fornecedorDaMercadoria is null or LOWER(FM.Nome) LIKE LOWER('%' || p_fornecedorDaMercadoria || '%'))

  AND (p_codigoTransportadora is null or OT.CODIGOFORNECEDOR = p_codigoTransportadora)
  AND (p_transportadora is null or LOWER(T.Nome) LIKE LOWER('%' || p_transportadora || '%'))
  
  AND (p_codigoTerminal is null or PCF.CodigoTerminal = p_codigoTerminal);

END;

create or replace 
PROCEDURE MONITORDEORDEMDETRANSPORTE (p_cursor OUT SYS_REFCURSOR, p_agrupamentos VARCHAR2, 
p_codigoMaterial Produto.Codigo%type, p_material Produto.Descricao%type,
p_codigoFornecedorDaMercadoria Fornecedor.Codigo%type, p_fornecedorDaMercadoria Fornecedor.Nome%type, 
p_codigoTransportadora Fornecedor.Codigo%type, p_transportadora Fornecedor.Nome%type, 
p_codigoDoMunicipioDeOrigem Municipio.Codigo%type,p_codigoDoMunicipioDeDestino Municipio.Codigo%type,
p_codigoDoTerminal Terminal.Codigo%type) AS

  v_query VARCHAR2(4000);
  v_subquery VARCHAR2(4000);
  v_agrupamentos VARCHAR2(4000):= 'GROUP BY Material';
  v_projecao VARCHAR2(4000):= 'SELECT sys_guid() || '''' as "Id", Material AS "Material"';
  v_projecaoDaSubQuery VARCHAR2(4000):= 'SELECT p.Descricao AS Material';
  v_from VARCHAR2(4000):= 'FROM ORDEMDETRANSPORTE ot ' ||
      'inner join ProcessoCotacaoFrete pcf on ot.IDPROCESSOCOTACAOFRETE=pcf.Id inner join ProcessoCotacao pc on pcf.Id=pc.Id ' ||
      'inner join ProcessoCotacaoItem pci on pcf.id = pci.IdProcessoCotacao ' ||
      'inner join ProcessoCotacaoItemFrete pcif on pci.id = pcif.Id ' ||
      'inner join Produto p on pci.CodigoProduto=p.Codigo ';
      
  v_where VARCHAR2(4000):= 'WHERE ot.QuantidadeLiberada - ot.QuantidadeColetada > 0';
  
BEGIN
  
  IF INSTR(p_agrupamentos, 'FornecedorDaMercadoria') > 0 OR p_fornecedorDaMercadoria IS NOT NULL THEN    

      v_from:= v_from || ' inner join Fornecedor FornecedorDaMercadoria ' ||
      'on pcf.CodigoFornecedor = FornecedorDaMercadoria.Codigo ';
      
  END IF;  
  
  IF INSTR(p_agrupamentos, 'Transportadora') > 0 OR p_transportadora IS NOT NULL THEN
      v_from:= v_from || 'inner join Fornecedor Transportadora ' ||
      'on ot.CodigoFornecedor = Transportadora.Codigo ';
  END IF;
  
  IF INSTR(p_agrupamentos, 'NumeroDoContrato') > 0 THEN
      v_projecaoDaSubQuery:= v_projecaoDaSubQuery || ', pcf.NumeroContrato';
      v_projecao:= v_projecao || ', NumeroContrato AS "NumeroDoContrato"';
      v_agrupamentos:= v_agrupamentos || ', NumeroContrato';
  ELSE
      v_projecao:= v_projecao || ', NULL AS "NumeroDoContrato"';
  END IF;

  IF INSTR(p_agrupamentos, 'FornecedorDaMercadoria') > 0 THEN
      v_projecaoDaSubQuery:= v_projecaoDaSubQuery || ', FornecedorDaMercadoria.Nome AS FornecedorDaMercadoria';
      v_projecao:= v_projecao || ', FornecedorDaMercadoria AS "FornecedorDaMercadoria"';
      v_agrupamentos:= v_agrupamentos || ', FornecedorDaMercadoria';
  ELSE
      v_projecao:= v_projecao || ', NULL AS "FornecedorDaMercadoria"';  
  END IF;
  
  IF INSTR(p_agrupamentos, 'Transportadora') > 0 THEN
      v_projecaoDaSubQuery:= v_projecaoDaSubQuery || ', Transportadora.Nome AS Transportadora';
      v_projecao:= v_projecao || ', Transportadora AS "Transportadora"';
      v_agrupamentos:= v_agrupamentos || ', Transportadora';
  ELSE
      v_projecao:= v_projecao || ', NULL AS "Transportadora"';  
  END IF;

  IF INSTR(p_agrupamentos, 'NumeroDaOrdemDeTransporte') > 0 THEN
      v_projecaoDaSubQuery:= v_projecaoDaSubQuery || ', TO_CHAR(ot.Id)AS NumeroDaOrdemDeTransporte';
      v_projecao:=  v_projecao || ', NumeroDaOrdemDeTransporte AS "NumeroDaOrdemDeTransporte"';
      v_agrupamentos:= v_agrupamentos || ', NumeroDaOrdemDeTransporte';
  ELSE
    v_projecao:=  v_projecao || ', NULL AS "NumeroDaOrdemDeTransporte"';
  END IF;
  
  IF INSTR(p_agrupamentos, 'MunicipioDeOrigem') > 0 THEN
      v_projecaoDaSubQuery:= v_projecaoDaSubQuery || ', MunicipioDeOrigem.Nome || ''/'' || MunicipioDeOrigem.UF AS MunicipioDeOrigem';
      v_projecao:= v_projecao || ', MunicipioDeOrigem AS "MunicipioDeOrigem"';
      v_from:= v_from || 
      'inner join Municipio MunicipioDeOrigem ' ||
      'on pcf.CodigoMunicipioOrigem = MunicipioDeOrigem.Codigo ';
      v_agrupamentos:= v_agrupamentos || ', MunicipioDeOrigem';
  ELSE
      v_projecao:= v_projecao || ', NULL AS "MunicipioDeOrigem"';
  END IF;
  
  IF INSTR(p_agrupamentos, 'MunicipioDeDestino') > 0 THEN
      v_projecaoDaSubQuery:= v_projecaoDaSubQuery || ', MunicipioDeDestino.Nome || ''/'' || MunicipioDeDestino.UF  AS MunicipioDeDestino';
      v_projecao:= v_projecao || ', MunicipioDeDestino AS "MunicipioDeDestino"';
      v_from:= v_from || 
      'inner join Municipio MunicipioDeDestino ' ||
      'on pcf.CodigoMunicipioDestino = MunicipioDeDestino.Codigo ';
      v_agrupamentos:= v_agrupamentos || ', MunicipioDeDestino';
  ELSE
      v_projecao:= v_projecao || ', NULL AS "MunicipioDeDestino"';
  END IF;
  
  IF INSTR(p_agrupamentos, 'Terminal') > 0 THEN
      v_projecaoDaSubQuery:= v_projecaoDaSubQuery || ', Terminal.Nome AS Terminal';
      v_projecao:= v_projecao || ', Terminal AS "Terminal"';
      v_from:= v_from || 
      'inner join Terminal ' ||
      'on pcf.CodigoTerminal = Terminal.Codigo ';
      v_agrupamentos:= v_agrupamentos || ', Terminal';
  
  ELSE
	v_projecao:= v_projecao || ', NULL AS "Terminal"'; 
  END IF;
  
  
  --filtro por material
  IF p_codigoMaterial IS NOT NULL THEN
      v_where:= v_where || ' AND p.Codigo like ''%' || p_codigoMaterial || '%''';
  END IF;
  
  IF p_material IS NOT NULL THEN
      v_where:= v_where || ' AND LOWER(p.Descricao) like LOWER(''%' || p_material || '%'')';
  END IF;
  
  --filtro por fornecedor da mercadoria
  
  IF p_codigoFornecedorDaMercadoria IS NOT NULL THEN
      v_where:= v_where || ' AND pcf.CodigoFornecedor like ''%' || p_codigoFornecedorDaMercadoria || '%''';
  END IF;
  
  IF p_fornecedorDaMercadoria IS NOT NULL THEN

      v_where:= v_where || ' AND LOWER(FornecedorDaMercadoria.Nome) like LOWER(''%' || p_fornecedorDaMercadoria || '%'')';
      
  END IF;
  
  --filtro por transportadora
  IF p_codigoTransportadora IS NOT NULL THEN
      v_where:= v_where || ' AND ot.CodigoFornecedor like ''%' || p_codigoTransportadora || '%''';
  END IF;
  
  IF p_transportadora IS NOT NULL THEN
  
      v_where:= v_where || ' AND LOWER(Transportadora.Nome) like LOWER(''%' || p_transportadora || '%'')';
  
  END IF;
  
  --filtro por municipio de origem
  IF p_codigoDoMunicipioDeOrigem IS NOT NULL THEN
     v_where:= v_where || ' AND pcf.CodigoMunicipioOrigem = ''' || p_codigoDoMunicipioDeOrigem || '''';
  END IF;
  
  --filtro por destino
  IF p_codigoDoMunicipioDeDestino IS NOT NULL THEN
     v_where:= v_where || ' AND pcf.CodigoMunicipioDestino = ''' || p_codigoDoMunicipioDeDestino || '''';
  END IF;
  
  --filtro por terminal
  IF p_codigoDoTerminal IS NOT NULL THEN
    v_where:= v_where || ' AND pcf.CodigoTerminal = ''' || p_codigoDoTerminal || '''';
  END IF;
  
  v_projecaoDaSubQuery:= v_projecaoDaSubQuery || ', ot.QuantidadeLiberada, ot.QuantidadeRealizada , ' || 
      'ot.QuantidadeColetada - ot.QuantidadeRealizada AS QuantidadeEmTransito,' ||
      'ot.QuantidadeLiberada - ot.QuantidadeColetada as QuantidadePendente,' ||
      'ot.QuantidadeColetada - ot.QuantidadeLiberada * ' ||
      '((CASE WHEN pcf.DataValidadeFinal < sysdate THEN pcf.DataValidadeFinal ELSE to_date(sysdate,''dd/mm/rrrr'') END - pcf.DataValidadeInicial) + 1) ' ||
      '/((pcf.DataValidadeFinal - pcf.DataValidadeInicial) + 1) AS SaldoProjetado, ' ||
      'COALESCE((SELECT sum(c.Peso) FROM Coleta c WHERE ot.Id = c.IdOrdemTransporte AND c.DataDePrevisaoDeChegada = to_date(sysdate,''dd/mm/rrrr'')),0) as PrevisaoDeChegadaNoDia ';
      
  v_projecao:= v_projecao || ', sum(QuantidadeLiberada) as "QuantidadeLiberada", ' || 
  'sum(QuantidadeRealizada) AS "QuantidadeRealizada", ' ||
  'round(sum(QuantidadePendente) * 100 / sum(QuantidadeLiberada),3) AS "PercentualPendente", ' ||
  'round(sum(SaldoProjetado) * 100 / sum(QuantidadeLiberada),3) As "PercentualProjetado", ' ||
  'sum(QuantidadeEmTransito) AS "QuantidadeEmTransito", ' ||
  'sum(QuantidadePendente) as "QuantidadePendente" , ' ||
  'sum(PrevisaoDeChegadaNoDia) as "PrevisaoDeChegadaNoDia" ';
      
  v_subquery:= v_projecaoDaSubQuery || v_from || v_where;
  
  v_query:= v_projecao ||
  ' FROM ( ' ||
    v_subquery ||
  ')' ||
  v_agrupamentos ||
  ' ORDER BY Material';
  
  --dbms_output.put_line(v_query);
  open p_cursor FOR v_query;

END;