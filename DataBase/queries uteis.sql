select codigofornecedor, count(1) as contador
from produtofornecedor
group by codigofornecedor
order by contador desc



select codigoproduto, count(1) as contador
from produtofornecedor
group by codigoproduto
order by contador desc


select *
from produto
where codigo = '000000000000001385'

select count(1)
from fornecedor
where email is not null


select length('TREEBUUCHET EQUP PROT INDIVIDUAL LT') from dual


update fornecedor set email = 'mauro.leal@fusionconsultoria.com.br'
where codigo = '$000446401'

update fornecedor set email = 'mauroscl@gmail.com'
where codigo = '$000446601'


update usuario set email = 'mauro.leal@fusionconsultoria.com.br'
where login = '$000446401';
update usuario set email = 'mauroscl@gmail.com'
where login = '$000446601';
commit;


select *
from usuario
where login in ('$000446401','$000446601')



update usuario set senha = null
where login in ('$000446401','$000446601');
commit;

select *
from processocotacao

update processocotacao set status = 1


select *
from usuario
where lower(login) = 'fusion_wylly'

UPDATE USUARIO SET EMAIL = 'mauro.leal@fusionconsultoria.com.br'
where LOGIN = 'FUSION_WYLLY'