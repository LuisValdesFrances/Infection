using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infection
{
    public interface IPlayable
    {
        void Fight(IPlayable other);

    }
}
