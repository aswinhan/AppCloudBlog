﻿namespace AppCloudBlog.Domain.Entities;

public class Category : AuditableBaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public ICollection<BlogPost> BlogPosts { get; set; } = [];
}
