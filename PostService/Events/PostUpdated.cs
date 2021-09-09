using System;
using System.Text.Json;
using Outbox.Outbox.Events;
using PostService.Models;

namespace PostService.Events
{
    public class PostUpdated : IOutboxEvent<Post>
    {
        public string Id { get; }
        public string AggregateType { get; set; }
        public string AggregateId { get; set; }
        public string Type { get; set; }
        public Post Payload { get; set; }

        public PostUpdated(Post post)
        {
            Id = Guid.NewGuid().ToString();
            AggregateType = "Post";
            AggregateId = post.Id.ToString();
            Type = "PostUpdated";
            Payload = post;
        }

        public Outbox.Outbox.Models.Outbox ToOutboxModel()
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
