namespace Eka.Web.YouTube
{
    public class PlaybackRestriction
    {
        public PlaybackRestriction(Relationship relationship)
        {
            Relationship = relationship;
        }

        public string Type { get; protected set; }
        public Relationship Relationship { get; protected set; }
    }
}