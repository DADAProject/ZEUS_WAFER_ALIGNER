using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Drv.CameraController
{
   public delegate void TEventHandler<Parameter1>(object sender, Parameter1 p1);
   public delegate void TEventHandler<Parameter1,Parameter2>(object sender, Parameter1 p1, Parameter2 p2);
}
