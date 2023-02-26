using AcerPro.Database.Context;
using AcerPro.Dto;
using AcerPro.Dto.Base;
using AcerPro.Dto.Models;
using AcerPro.Dto.ViewModels;
using AcerPro.Extensions;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace AcerPro.Business.Repository
{
    public class RepositoryBase<TModel, TViewModel> : IRepository<TModel, TViewModel>
    where TModel : ModelBase
    where TViewModel : ViewModelBase
    {
        private readonly AcerProDbContext _dbContext;
        public IQueryable<TModel> Table { get { return _dbContext.Set<TModel>().AsNoTracking(); } }

        public RepositoryBase(AcerProDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TViewModel Take(int id)
        {
            return this.TakeModel(id)?.AsViewModel<TViewModel>();
        }

        public TViewModel TakeBeforeChange(int id)
        {
            var beforeChange = _dbContext.Logs
                               .Where(l => l.EntitiyId == id && l.EntityName == typeof(TModel).Name)
                               .OrderByDescending(l => l.Date)
                               .Take(1)
                               .Select(l => l.BeforeChange)
                               .FirstOrDefault();

            TViewModel obj = JsonConvert.DeserializeObject<TViewModel>(beforeChange);

            return obj;
        }

        public TViewModel Take(Func<TModel, bool> predicate)
        {
            return Table.FirstOrDefault(predicate)?.AsViewModel<TViewModel>();
        }

        public void Save(TViewModel viewModel)
        {
            TModel model;
            EntityState entityState;

            var currentSessionUser = (UserViewModel)HttpContext.Current.Session[Constants.SessionKey];

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (viewModel.Id == 0)
                    {
                        entityState = EntityState.Added;
                        model = viewModel.AsModel<TModel>();

                        model.CreatedAt = DateTime.UtcNow;
                        model.CreatedBy = currentSessionUser.Name;

                        _dbContext.Set<TModel>().Add(model);
                    }
                    else
                    {
                        entityState = EntityState.Modified;
                        model = viewModel.AsModel(Table);

                        model.CreatedAt = DateTime.UtcNow;
                        model.CreatedBy = currentSessionUser.Name;

                        _dbContext.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    }

                    _dbContext.SaveChanges();

                    this.Log(model, entityState, currentSessionUser);

                    transaction.Commit();

                    viewModel.Id = model.Id;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public void Delete(int id)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var currentSessionUser = (UserViewModel)HttpContext.Current.Session[Constants.SessionKey];
                TModel model = this.TakeModel(id);
                try
                {
                    if (model != null)
                    {
                        this.Log(model, EntityState.Deleted, currentSessionUser);
                        _dbContext.Set<TModel>().Remove(model);
                        _dbContext.SaveChanges();
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        #region Private

        private TModel TakeModel(int id)
        {
            var param = Expression.Parameter(typeof(TModel));
            var condition = Expression.Lambda<Func<TModel, bool>>(Expression.Equal(Expression.Property(param, "Id"), Expression.Constant(id, typeof(int))), param).Compile();

            return Table.FirstOrDefault(condition);
        }

        private void Log(TModel model, EntityState entityState, UserViewModel user)
        {
            _dbContext.Set<TModel>().Attach(model);

            _dbContext.Set<Log>().Add(new Log
            {
                UserName = user.Name,
                UserEmail = user.Email,
                Date = DateTime.Now,
                EntitiyId = model.Id,
                EntityName = model.GetType().Name,
                BeforeChange = _dbContext.Entry(model).OriginalValues.ToObject().ToJson(),
                EntityState = entityState
            });

            _dbContext.SaveChanges();
        }

        #endregion
    }
}
