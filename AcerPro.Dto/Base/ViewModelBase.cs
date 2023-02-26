using AcerPro.Dto.Mapper;
using AutoMapper;
using System.Linq;

namespace AcerPro.Dto.Base
{
    public class ViewModelBase : DtoBase
    {
        public TModel AsModel<TModel>(IQueryable<TModel> table = null) where TModel : ModelBase
        {
            TModel model = null;

            if (this.Id == 0)
                model = DtoMapper.Mapper.Map<TModel>(this);
            else
            {
                var entity = table.FirstOrDefault(q => q.Id == this.Id);
                model = DtoMapper.Mapper.Map<TModel>(this);
            }

            return model;
        }
    }
}
