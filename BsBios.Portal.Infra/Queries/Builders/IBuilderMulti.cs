using System.Collections.Generic;
using System.Linq;

namespace BsBios.Portal.Infra.Queries.Builders
{
    public interface IBuilderMulti<T1, in T2, TViewModel>
    {
        TViewModel BuildSingle(T1 model1, T2 model2);
        IList<TViewModel> BuildList(IList<T1> models, T2 model2);
    }

    public abstract class BuilderMulti<T1, T2, TViewModel>: IBuilderMulti<T1, T2, TViewModel>
    {
        public abstract TViewModel BuildSingle(T1 model1, T2 model2);
        public IList<TViewModel> BuildList(IList<T1> models, T2 model2)
        {
            return models.Select(x => BuildSingle(x, model2)).ToList();
        }
    }
}