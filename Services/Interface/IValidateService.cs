using System.Collections.Generic;

namespace Services.Interface
{
    /// <summary>
    /// Entity Id validation service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValidateService<T>
    {
        /// <summary>
        /// Validate Id in string format and get entity
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="strEntityId">String Id of Entity</param>
        /// <param name="entity">Entity</param>
        /// <returns></returns>
        public bool Validate(List<T> Entities, string strEntityId, out T entity);
    }
}
