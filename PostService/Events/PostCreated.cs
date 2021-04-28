using System;
using System.Text.Json;
using PostService.Models;
using PostService.Outbox.Events;

namespace PostService.Events
{
    public class PostCreated : IOutboxEvent<Post>
    {
        public string Id { get; }
        public string AggregateType { get; set; }
        public string AggregateId { get; set; }
        public string Type { get; set; }
        public Post Payload { get; set; }

        public PostCreated(Post post)
        {
            Id = Guid.NewGuid().ToString();
            AggregateType = "Post";
            AggregateId = post.Id.ToString();
            Type = "PostCreated";
            Payload = post;
        }

        public Outbox.Models.Outbox ToOutboxModel()
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