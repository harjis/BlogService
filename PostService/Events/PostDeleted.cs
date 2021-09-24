using System;
using System.Text.Json;
using Outbox.Producer.Events;
using PostService.Models;

namespace PostService.Events
{
    public class PostDeleted : IOutboxEvent<Post>
    {
        public string Id { get; }
        public string AggregateType { get; set; }
        public string AggregateId { get; set; }
        public string Type { get; set; }
        public Post Payload { get; set; }

        public PostDeleted(Post post)
        {
            Id = Guid.NewGuid().ToString();
            AggregateType = "Post";
            AggregateId = post.Id.ToString();
            Type = "PostDeleted";
            Payload = post;
        }

        public Outbox.Producer.Models.Outbox ToOutboxModel()
        {
            return new()
            {
                id = Id,
                aggregatetype = AggregateType,
                aggregateid = AggregateId,
                type = Type,
                payload = JsonSerializer.Serialize(Payload)
            };
        }
    }
}
