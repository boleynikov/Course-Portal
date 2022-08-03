using Domain;
using Domain.Abstract;
using Domain.CourseMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Abstract
{
    public interface IDbContext
    {
        List<T> Get<T>() where T : BaseEntity;

        bool Update<T>(List<T> listEntities) where T : BaseEntity;

    }
}
