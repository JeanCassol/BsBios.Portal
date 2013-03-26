select *
from agendamentodecarga ag inner join agendamentodedescarregamento des
on ag.id= des.id

select *
from quota q inner join agendamentodecarga ag 
on q.id = ag.idquota
inner join agendamentodecarregamento car
on ag.id= car.id


select *
from quota q inner join agendamentodecarga ag 
on q.id = ag.idquota
inner join agendamentodedescarregamento des
on ag.id= des.id


select *
from quota


select *
from fornecedor
where cnpj is not null

27.383.067/0001-09


update fornecedor set cnpj = '27383067000109' where codigo = '0000101808';
update fornecedor set cnpj = '61765126000141' where codigo = '0000101809';
commit;
