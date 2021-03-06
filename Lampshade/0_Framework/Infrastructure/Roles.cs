namespace _0_Framework.Infrastructure
{
    public static class Roles
    {
        public const string Administrator = "1";
        public const string SystemUser = "2";
        public const string ContentUploader = "10002";
        public const string ColleagueUser = "10003";


        public static string GetRoleBy(long id)
        {
            switch (id)
            {
                case 1:
                    return "مدیر سیستم";
                case 10002:
                    return "محتوا گذار";
                case 10003:
                    return "کاربر همکار";
                default:
                    return "";
            }
        }
    }
}
