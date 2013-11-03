using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eka.Web.YouTube
{
    public class PlaybackRestriction
    {
        public string Type { get; protected set; }
        public Relationship Relationship { get; protected set; }

        public PlaybackRestriction(Relationship relationship)
        {
            this.Relationship = relationship;
        }
    }
}
