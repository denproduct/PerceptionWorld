using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptionWorld.Utils;

public class State
{
    public static ContentManager Content {  get; private set; }

    public State(ContentManager content)
    {
        Content = content;
    }
}
