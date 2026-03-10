namespace PosNet.Domain.Entities
{
    public class RolesPermission
    {
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public int PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;

        public RolesPermission(int roleId, int permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }

        public RolesPermission() { }
    }
}
