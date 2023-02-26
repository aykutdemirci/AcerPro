using AcerPro.Dto.Base;
using System;
using System.Linq;

namespace AcerPro.Business.Repository
{
    public interface IRepository<TModel, TViewModel>
    where TModel : ModelBase
    where TViewModel : ViewModelBase
    {
        IQueryable<TModel> Table { get; }

        TViewModel Take(int id);

        TViewModel Take(Func<TModel, bool> predicate);

        TViewModel TakeBeforeChange(int id);

        void Save(TViewModel viewModel);

        void Delete(int id);
    }
}
