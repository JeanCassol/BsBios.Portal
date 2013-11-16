insert into fornecedor
(codigo, cnpj, nome, endereco, transportadora, email, municipio, uf)
values('001', '53526262443', 'Transportadora Merc�rio', 'Av. assis brasil, 1567', 1, 'mauroscl@gmail.com', 'Passo Fundo', 'RS');

insert into fornecedor
(codigo, cnpj, nome, endereco, transportadora, email, municipio, uf)
values('004', '53526262443', 'Fornecedor de Soja', 'Av. Pres. Castelo Branco, 914', 1, 'mauroscl@gmail.com', 'Santa Maria', 'RS');

insert into fornecedor
(codigo, cnpj, nome, endereco, transportadora, email, municipio, uf)
values('010', '535434262443', 'Dep�sito', 'Av. Pres. Castelo Branco, 914', 0, 'mauroscl@gmail.com', 'Santa Maria', 'RS');

insert into usuario
(login, nome, senha,email, status)
values
('001','Transportadora Merc�rio','nMt6vfHriwbmCFAim+R8qw==', 'mauroscl@gmail.com', 1);

insert into usuario
(login, nome, senha,email, status)
values
('010','Dep�sito','nMt6vfHriwbmCFAim+R8qw==', 'mauroscl@gmail.com', 1);

insert into itinerario
(codigo, descricao)
values
('001', 'Porto Alegre => Passo Fundo');

insert into unidademedida
(codigointerno, codigoexterno, descricao)
values
('TON', 'TON', 'Tonelada');

insert into produto
(codigo, descricao,tipo) 
values
('912', 'Soja Tipo A', 'SOJA');