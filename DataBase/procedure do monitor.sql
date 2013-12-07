CREATE OR REPLACE PROCEDURE MONITORDEORDEMDETRANSPORTE (p_cursor OUT SYS_REFCURSOR, p_agrupamentos VARCHAR2, 
p_codigoMaterial Produto.Codigo%type, p_material Produto.Descricao%type,
p_codigoFornecedorDaMercadoria Fornecedor.Codigo%type, p_fornecedorDaMercadoria Fornecedor.Nome%type, 
p_codigoTransportadora Fornecedor.Codigo%type, p_transportadora Fornecedor.Nome%type, 
p_codigoDoMunicipioDeOrigem Municipio.Codigo%type,p_codigoDoMunicipioDeDestino Municipio.Codigo%type) AS

  v_query VARCHAR2(4000);
  v_subquery VARCHAR2(4000);
  v_agrupamentos VARCHAR2(4000):= 'GROUP BY Material';
  v_projecao VARCHAR2(4000):= 'SELECT sys_guid() || '''' as "Id", Material AS "Material"';
  v_projecaoDaSubQuery VARCHAR2(4000):= 'SELECT p.Descricao AS Material';
  v_from VARCHAR2(4000):= 'FROM ORDEMDETRANSPORTE ot ' ||
      'inner join ProcessoCotacaoFrete pcf on ot.IDPROCESSOCOTACAOFRETE=pcf.Id inner join ProcessoCotacao pc on pcf.Id=pc.Id ' ||
      'inner join Produto p on pc.CodigoProduto=p.Codigo ';
      
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
  
  dbms_output.put_line(v_query);
  open p_cursor FOR v_query;

END;
/


/*
SET SERVEROUTPUT ON;
VARIABLE X REFCURSOR;
EXEC TESTEDINAMICO(:X);
PRINT X;
*/
