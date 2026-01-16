using System.Collections.Generic;

namespace MovieHub.Data.Entities
{
    public class Actor
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        // ✅ List вместо HashSet
        public List<MovieActor> MovieActors { get; set; } = new();
    }
}
