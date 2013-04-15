select count(1) as fornecedores
from fornecedor
where email is null;
select count(1) as produtos
from produto;
select count(1) as registroinfo
from produtofornecedor;
select count(1) as itinerarios
from itinerario;
select count(1) as condicoes
from condicaopagamento;
select count(1) as ivas
from iva;
select count(1) as incoterms
from incoterm;
select count(1) as unidades
from unidademedida
