ALTER TABLE Quota ADD PesoRealizado NUMBER(13,3);

update quota set pesorealizado = 
(select coalesce(sum(car.peso),0)
from agendamentodecarga ag inner join agendamentodecarregamento car
on ag.id = car.id
where ag.idquota = quota.id
and realizado = 1
)
where fluxodecarga = 1;


update quota set pesorealizado = 
(select coalesce(sum(nf.peso),0)
from agendamentodecarga ag inner join agendamentodedescarregamento descar
on ag.id = descar.id
inner join notafiscal nf
on descar.id = nf.idagendamentodescarregamento
where ag.idquota = quota.id
and realizado = 1
)
where fluxodecarga = 2;

COMMIT;

ALTER TABLE Quota MODIFY PesoRealizado NUMBER(13,3) NOT NULL;