using System;
using System.Collections.Generic;

namespace Infrastructure.DataModels
{
    public partial class Post
    {
        public int PostId { get; set; }
        public int? ClassId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public virtual Class? Class { get; set; }
    }
}
