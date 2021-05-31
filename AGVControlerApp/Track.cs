using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVControlerApp
{
    enum Color { BLACK = 7, YELLOW = 4, RED = 5, WHITE = 6, OTHERBLACK = 1 };

    class Track
    {
        Track parent;
        Color color;

        public Track(Color color)
        {
            this.color = color;
            this.parent = null;
        }

        public Track(Color color, Track parent)
        {
            this.color = color;
            this.parent = parent;
        }

        public int getColor()
        {
            return color.GetHashCode();
        }

        public void setColor(Color color)
        {
            this.color = color;
        }

        public void setParent(Track track)
        {
            parent = track;
        }

        public Track getParent()
        {
            return parent;
        }
    }
}
