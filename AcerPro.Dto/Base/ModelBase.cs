using AcerPro.Dto.Mapper;

namespace AcerPro.Dto.Base
{
    public class ModelBase : DtoBase
    {
        public TViewModel AsViewModel<TViewModel>() where TViewModel : ViewModelBase
        {
            var viewModel = DtoMapper.Mapper.Map<TViewModel>(this);
            return viewModel;
        }
    }
}
