select des.*, ag.*, q.*, f.nome as fornecedor
from agendamentodedescarregamento des inner join agendamentodecarga ag on des.id = ag.id
inner join quota q on ag.idquota = q.id
inner join fornecedor f on q.codigofornecedor = f.codigo
left join notafiscal nf on des.id = nf.idagendamentodescarregamento
where nf.idagendamentodescarregamento is null
order by q.data desc;


select *
from fornecedor