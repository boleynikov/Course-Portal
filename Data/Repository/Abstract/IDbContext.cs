using Domain.Abstract;
using System.Collections.Generic;

namespace Data.Repository.Abstract
{
    public interface IDbContext
    {
        List<T> Get<T>() where T : BaseEntity;

        bool Update<T>(List<T> listEntities) where T : BaseEntity;

    }
}
