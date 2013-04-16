ALTER TABLE AgendamentoDeCarga ADD PesoTotal NUMBER(13,3);

UPDATE (SELECT PESO, PESOTOTAL from agendamentodecarga inner join agendamentodecarregamento car
on agendamentodecarga.id = car.id
)T
SET T.PESOTOTAL = T.PESO;

update agendamentodecarga set pesototal =
(select  sum(nf.peso)
from agendamentodedescarregamento descar
inner join notafiscal nf
on descar.id = nf.idagendamentodescarregamento 
where agendamentodecarga.id = descar.id)
where exists
(
  select 1
  from agendamentodedescarregamento descar
  where agendamentodecarga.id = descar.id
);
COMMIT;

ALTER TABLE AgendamentoDeCarga MODIFY PesoTotal NUMBER(13,3) NOT NULL;
