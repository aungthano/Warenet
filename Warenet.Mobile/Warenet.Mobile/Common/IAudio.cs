using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warenet.Mobile
{
    public interface IAudio
    {
        bool PlayWavSuccess();

        bool PlayWavFail();
    }
}
