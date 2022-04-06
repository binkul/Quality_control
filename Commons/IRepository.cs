using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quality_Control.Commons
{
    public interface IRepository<T>
    {
        T Save(T data);
    }
}
