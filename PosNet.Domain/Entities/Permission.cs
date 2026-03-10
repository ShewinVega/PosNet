using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace PosNet.Domain.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public virtual ICollection<RolesPermission> RolesPermissions { get; set; } = [];

        public Permission(string name, string description = "")
        {
            Name = name;
            Description = description;
        }

        public Permission() { }


        public static Permission Create(string name) => new Permission(name);

    }
}
