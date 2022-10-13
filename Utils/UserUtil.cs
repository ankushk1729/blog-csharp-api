using SM.Data;
using SM.Entities;

namespace SM.Utils
{
    public class UserUtil
    {
        public static Role GetRoleFromName(ApiDBContext dBContext, string roleName) {
            var role = dBContext.Roles.FirstOrDefault(role => role.RoleName == roleName);

            return role!;
        }

        public static Role GetRoleFromId(ApiDBContext dBContext, Guid roleId) {
            var role = dBContext.Roles.FirstOrDefault(role => role.Id == roleId);

            return role!;
        }

        public static string GetUserRole(ApiDBContext dBContext, Guid UserId) {
            var userRole = dBContext.UserRoles.FirstOrDefault(ur => ur.UserId == UserId);

            return UserUtil.GetRoleFromId(dBContext, userRole!.RoleId).RoleName;
        }

        public static void CreateUserRoleMapping(ApiDBContext dBContext, User user, string roleName) {
            var role = UserUtil.GetRoleFromName(dBContext, roleName);
            var newUserRoleMapping = new UserRole() {
                Id = Guid.NewGuid(),
                User = user, 
                Role = role,
                CreatedAt = DateTimeOffset.Now
            };
            dBContext.UserRoles.Add(newUserRoleMapping);
            dBContext.SaveChanges();
        }
    }
}