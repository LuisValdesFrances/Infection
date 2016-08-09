using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infection
{
    public interface IPlayable
    {
        void SetOponent(IPlayable other);
        void Fight(IPlayable other);
    }
}
