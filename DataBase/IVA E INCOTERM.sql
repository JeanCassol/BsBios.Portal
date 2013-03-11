set define off;
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('AIR','Transp Aéreo Expresso');

INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('CFR','Custos e frete');
        
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('CIF','Custo, seguro && frete');
         
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('CIP','Seguro com frete pago');
         
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('CPT','Frete pago');
        
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('DAF','Entregue na fronteira');
       
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('DDP','Entregue desalfandegado');
       
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('DDU','Entregue sem desembaraço alf.');
      
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('DEQ','Entrega no cais(direitos pag.)');
       
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('DES','Entregue no navio');
        
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('DHL','DHL - Expresso Mundial');
        
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('EMY','Emery Mundial');
       
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('EXW','Na fábrica');
      
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('FAS','Posto ao lado do navio');
        
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('FCA','Transportador livre');
        
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('FED','Expresso Federal');
        
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('FH','Franco domicílio');
          
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('FOB','Franco a bordo');
       
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('UN','Porte/frete a pagar');
         
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('UPS','United Parcel Service');
       
INSERT INTO INCOTERM
(CODIGO, DESCRICAO)
VALUES
('USP','BR - Serviço Postal');
       

 
--IVA
 
           
INSERT INTO IVA
(CODIGO, DESCRICAO)
VALUES
('C0','Consumo: Sem impostos');

           
INSERT INTO IVA
(CODIGO, DESCRICAO)
VALUES
('C1','Consumo: ICMS');
           
INSERT INTO IVA
(CODIGO, DESCRICAO)
VALUES
('C2','Consumo: ICMS + S.T.');
INSERT INTO IVA
(CODIGO, DESCRICAO)
VALUES
('C3','Consumo: ICMS + IPI');
INSERT INTO IVA
(CODIGO, DESCRICAO)
VALUES
('C4','Consumo: ICMS + IPI + ST');
INSERT INTO IVA
(CODIGO, DESCRICAO)
VALUES
('C5','Consumption: IPI');
INSERT INTO IVA
(CODIGO, DESCRICAO)
VALUES
('C6','Consumo: ICMS + Dif Aliquota');
INSERT INTO IVA
(CODIGO, DESCRICAO)
VALUES
('C7','Consumo: ICMS +dif. Alíqu. COBRAD POR S.T.');
           
INSERT INTO IVA
(CODIGO, DESCRICAO)
VALUES
('C8','Consumo: ICMS + IPI + Dif Aliquota');
           
INSERT INTO IVA
(CODIGO, DESCRICAO)
VALUES
('C9','Consumo: ICMS + IPI + dif. Alíqu. COBRAD POR S.T.');
           
INSERT INTO IVA
(CODIGO, DESCRICAO)
VALUES
('CE','Consumo: Substituição Tributária');
           
INSERT INTO IVA
(CODIGO, DESCRICAO)
VALUES
('CS','Serviços');

INSERT INTO IVA
(CODIGO, DESCRICAO)
VALUES
('I0','Industrialização: Sem Impostos');
            
INSERT INTO IVA
(CODIGO, DESCRICAO)
VALUES
('I1','Industrialização: Só ICMS');
            
INSERT INTO IVA
(CODIGO, DESCRICAO)
VALUES
('I2','Industrialização: ICMS + S.T.');
            
INSERT INTO IVA
(CODIGO, DESCRICAO)
VALUES
('I3','Industrialização: ICMS + IPI');
            
INSERT INTO IVA
(CODIGO, DESCRICAO)
VALUES
('I4','Industrialização: ICMS + IPI + S.T.');
            
INSERT INTO IVA
(CODIGO, DESCRICAO)
VALUES
('I5','Industrialização: IPI');
            
COMMIT;

