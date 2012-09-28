using System;
using System.Collections.Generic;

namespace MvcCrudDemo.Models
{
    public class BaseResource
    {
        public Guid Id { get; set; } // TODO: should Id be string or int?  thinking string.
        public string Name { get; set; }
        public static List<BaseResource> ListResources()
        {
            // TODO: determine available resources to list for current user.
            return new List<BaseResource> {
                new BaseResource { Id = Guid.NewGuid(), Name = "First Resource" },
                new BaseResource { Id = Guid.NewGuid(), Name = "Second Resource" },
                new BaseResource { Id = Guid.NewGuid(), Name = "Third Resource" }
            };
        }
    }
}