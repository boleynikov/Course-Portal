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
        /// <param name="entities"></param>
        /// <param name="strEntityId">String Id of Entity</param>
        /// <param name="entity">Entity</param>
        /// <returns></returns>
        public bool Validate(IEnumerable<T> entities, string strEntityId, out T entity);
    }
}
