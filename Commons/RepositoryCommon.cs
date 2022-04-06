using System;

namespace Quality_Control.Commons
{
    public class RepositoryCommon<T> : IRepository<T>
    {
        public virtual T Save(T data)
        {
            throw new NotImplementedException();
        }
    }
}
