insert into requisicaocompra 
(id, numero, numeroitem,logincriador, requisitante, codigofornecedorpretendido, descricao, codigomaterial, quantidade, unidademedida, centro, datasolicitacao,dataliberacao, dataremessa, codigogrupodecompra, mrp, gerouprocessodecotacao, status)
select requisicaocompra_id_sequence.nextval, '0010011510', numeroitem,logincriador, requisitante, codigofornecedorpretendido, descricao, codigomaterial, quantidade, unidademedida, centro, datasolicitacao,dataliberacao, dataremessa, codigogrupodecompra, mrp, 0,1
from requisicaocompra
where id = 42;
