namespace Domain.Constants;

public static class Permissions
{
    public static List<string> GeneratePermissionsForModule(string module)
    {
        return
        [
            $"Permissions.{module}.Create",
            $"Permissions.{module}.View",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete"
        ];
    }

    public static class Meeting
    {
        public const string View = "Permissions.Meeting.View";
        public const string Create = "Permissions.Meeting.Create";
        public const string Edit = "Permissions.Meeting.Edit";
        public const string Delete = "Permissions.Meeting.Delete";
    }

    public static class Notification
    {
        public const string View = "Permissions.Notification.View";
        public const string Create = "Permissions.Notification.Create";
        public const string Send = "Permissions.Notification.Send";

    }


    public static class Role
    {
        public const string View = "Permissions.Role.View";
        public const string Create = "Permissions.Role.Create";
        public const string Edit = "Permissions.Role.Edit";
        public const string Delete = "Permissions.Role.Delete";
    }

    public static class User
    {
        public const string View = "Permissions.User.View";
        public const string Create = "Permissions.User.Create";
        public const string Edit = "Permissions.User.Edit";
    }

    public static class UserRole
    {
        public const string View = "Permissions.UserRole.View";
        public const string Create = "Permissions.UserRole.Create";
        public const string Delete = "Permissions.UserRole.Delete";
    }

}
