namespace Senior2.Api.Models
{
    public class Follow
    {
        public int Id { get; set; }

        // Who is following
        public int FollowerId { get; set; }

        // Who is being followed
        public int FollowedId { get; set; }
    }
}