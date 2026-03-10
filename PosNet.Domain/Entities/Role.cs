using PosNet.Domain.Constants;

namespace PosNet.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<User>? Users { get; set; } = [];

        public virtual ICollection<RolesPermission>? RolesPermissions { get; set; } = [];

        public void AddPermission(int permissionId)
        {
            if (RolesPermissions.Any(rp => rp.PermissionId == permissionId)) return;
            RolesPermissions.Add(new RolesPermission(Id, permissionId));
        }

        public void AddPermissions(List<Permission> permissions)
        {
            foreach (var permission in permissions)
            {
                if (RolesPermissions.Any(rp => rp.PermissionId == permission.Id)) return;
                RolesPermissions.Add(new RolesPermission(Id, permission.Id));
            }
        }

        public void RemovePermission(int permissionId)
        {
            var permission = RolesPermissions.FirstOrDefault(rp => rp.PermissionId == permissionId);
            if(permission != null)
            {
                RolesPermissions.Remove(permission);
            }
        }

        public Role(string name)
        { 
            Name = name;
        }

        public Role() { }
        
    }
}
