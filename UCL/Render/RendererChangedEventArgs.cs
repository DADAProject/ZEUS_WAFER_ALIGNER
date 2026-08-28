using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BeforeRenderingEventArgs
{
    public ToggleSwitchRendererBase Renderer { get; set; }

    public BeforeRenderingEventArgs(ToggleSwitchRendererBase renderer)
    {
        Renderer = renderer;
    }
}