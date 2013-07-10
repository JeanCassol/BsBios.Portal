using System.Collections.Generic;
using System.Linq;

namespace BsBios.Portal.Infra.Queries.Builders
{
    public interface IBuilder<TModel, TViewModel>
    {
        TViewModel BuildSingle(TModel model);
        IList<TViewModel> BuildList(IList<TModel> models);
    }

    public abstract class Builder<TModel, TViewModel>: IBuilder<TModel, TViewModel>
    {
        public abstract TViewModel BuildSingle(TModel model);
        public IList<TViewModel> BuildList(IList<TModel> models)
        {
            return models.Select(BuildSingle).ToList();
        }
    }
}