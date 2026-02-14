using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Bases
{
    public interface IEntity<TKey>
    {
        TKey GetKey();
        void SetKey(TKey key);
    }
}