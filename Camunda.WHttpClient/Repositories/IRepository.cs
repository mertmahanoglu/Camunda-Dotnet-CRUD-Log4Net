using System;
using System.Collections.Generic;
using System.Text;

namespace UdemyNLayerProject.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);

    }
}
