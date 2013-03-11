using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaPerfil : IConsultaPerfil
    {
        private readonly IBuilder<Enumeradores.Perfil, PerfilVm> _builder;

        public ConsultaPerfil(IBuilder<Enumeradores.Perfil, PerfilVm> builder)
        {
            _builder = builder;
        }

        public IList<PerfilVm> Listar()
        {
            //var retorno = new List<PerfilVm>();
            //var enumType = typeof (Enumeradores.Perfil);

            //var fields = from field in enumType.GetFields()
            //             where field.IsLiteral
            //             select field;


            //foreach (FieldInfo field in fields)
            //{
            //    var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            //    if (attributes.Length > 0)
            //    {
            //        object value = field.GetValue(enumType);
            //        var perfilVm = new PerfilVm()
            //            {
            //                Codigo = (int) value,
            //                Descricao = attributes[0].Description
            //            };
            //        retorno.Add(perfilVm);
                                        
            //    }
            //}

            var perfis = Enum.GetValues(typeof(Enumeradores.Perfil)).Cast<Enumeradores.Perfil>().ToList();
            
            return _builder.BuildList(perfis);

        }
    }
}