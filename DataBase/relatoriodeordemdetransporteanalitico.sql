CREATE OR REPLACE PROCEDURE RELATORDEMTRANSPORTEANALITICO (p_cursor OUT SYS_REFCURSOR, p_status int, p_dataDeValidadeInicial DATE, p_dataDeValidadeFinal DATE,
p_codigoMaterial Produto.Codigo%type, p_material Produto.Descricao%type,
p_codigoFornecedorDaMercadoria Fornecedor.Codigo%type, p_fornecedorDaMercadoria Fornecedor.Nome%type, 
p_codigoTransportadora Fornecedor.Codigo%type, p_transportadora Fornecedor.Nome%type) AS
BEGIN

  open p_cursor for
  SELECT sys_guid() || '' AS "Id",CAST(OT.ID AS INT) AS "IdDaOrdemDeTransporte", P.CODIGO || ' - ' || P.DESCRICAO AS "Material", FM.CODIGO || ' - ' || FM.NOME AS "NomeDoFornecedorDaMercadoria",
  TO_CHAR(PCF.DATAVALIDADEINICIAL, 'dd/mm/yyyy') AS "DataDeValidadeInicial", TO_CHAR(PCF.DATAVALIDADEFINAL, 'dd/mm/yyyy') AS "DataDeValidadeFinal", CASE WHEN PCF.CLASSIFICACAO = 0 THEN 'Não' ELSE 'Sim' END AS "Classificacao",
  CAST(OT.CADENCIA AS DECIMAL)AS "Cadencia", IT.CODIGO || ' - ' || IT.DESCRICAO AS "Itinerario", MO.NOME || '/' || MO.UF AS "MunicipioDeOrigem",
  MD.NOME || '/' || MD.UF AS "MunicipioDeDestino", T.NOME AS "Transportadora", D.NOME AS "NomeDoDeposito", CAST(OT.QUANTIDADEADQUIRIDA AS DECIMAL) AS "QuantidadeContratada",
  CAST(OT.QUANTIDADELIBERADA AS DECIMAL) AS "QuantidadeLiberada", CAST(OT.QUANTIDADEDETOLERANCIA AS DECIMAL) AS "QuantidadeDeTolerancia",OT.QUANTIDADECOLETADA - OT.QUANTIDADEREALIZADA AS "QuantidadeEmTransito",
  CAST(OT.QUANTIDADEREALIZADA AS DECIMAL) AS "QuantidadeRealizada", OT.QuantidadeLiberada - OT.QuantidadeRealizada AS "QuantidadePendente", UM.DESCRICAO AS "UnidadeDeMedida"
  FROM ORDEMDETRANSPORTE OT INNER JOIN PROCESSOCOTACAOFRETE PCF ON OT.IDPROCESSOCOTACAOFRETE = PCF.ID
  INNER JOIN PROCESSOCOTACAO PC ON PCF.ID = PC.ID
  INNER JOIN PRODUTO P ON PC.CODIGOPRODUTO = P.CODIGO
  INNER JOIN UNIDADEMEDIDA UM ON PC.CODIGOUNIDADEMEDIDA = UM.CODIGO
  INNER JOIN FORNECEDOR FM ON PCF.CODIGOFORNECEDOR = FM.CODIGO
  INNER JOIN ITINERARIO IT ON PCF.CODIGOITINERARIO = IT.CODIGO
  INNER JOIN MUNICIPIO MO ON PCF.CODIGOMUNICIPIOORIGEM = MO.CODIGO
  INNER JOIN MUNICIPIO MD ON PCF.CODIGOMUNICIPIODESTINO = MD.CODIGO
  INNER JOIN FORNECEDOR T ON OT.CODIGOFORNECEDOR = T.CODIGO
  LEFT JOIN FORNECEDOR D ON PCF.CODIGODEPOSITO = D.CODIGO
  LEFT JOIN (
    SELECT IDORDEMTRANSPORTE, COUNT(1) AS QuantidadeDeColetasRealizadas, 
    SUM(CASE WHEN DATADECHEGADA > DATADEPREVISAODECHEGADA THEN DATADECHEGADA - DATADEPREVISAODECHEGADA ELSE 0 END) AS QuantidadeDeDiasEmAtraso,
    SUM(CASE WHEN DATADECHEGADA > DATADEPREVISAODECHEGADA THEN 1 ELSE 0 END) * 100 / COUNT(1) AS PercentualDeAtraso 
    FROM COLETA C 
    WHERE C.REALIZADO = 1
    GROUP BY IDORDEMTRANSPORTE
  ) CR
  ON OT.ID = CR.IDORDEMTRANSPORTE
  WHERE (p_Status is null or (CASE WHEN OT.QUANTIDADEREALIZADA < OT.QUANTIDADELIBERADA THEN 0 ELSE 1 END) = p_Status)
  AND (p_dataDeValidadeInicial is null or pcf.DataValidadeFinal >= p_dataDeValidadeInicial)
  AND (p_dataDeValidadeFinal is null or pcf.DataValidadeInicial <= p_dataDeValidadeFinal)
  AND (p_CodigoMaterial is null or PC.CODIGOPRODUTO = p_CodigoMaterial)
  AND (p_material is null or LOWER(P.Descricao) LIKE LOWER('%' || p_material ||'%'))

  AND (p_codigoFornecedorDaMercadoria is null or PCF.CODIGOFORNECEDOR = p_codigoFornecedorDaMercadoria)
  AND (p_fornecedorDaMercadoria is null or LOWER(FM.Nome) LIKE LOWER('%' || p_fornecedorDaMercadoria || '%'))

  AND (p_codigoTransportadora is null or OT.CODIGOFORNECEDOR = p_codigoTransportadora)
  AND (p_transportadora is null or LOWER(T.Nome) LIKE LOWER('%' || p_transportadora || '%'));

END;
