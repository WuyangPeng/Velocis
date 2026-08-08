namespace Game.Scripts.Main.Runtime.Definition.DataStruct
{
    public class BuildInfo
    {
        public string GameVersion
        {
            get;
            set;
        }

        public int InternalGameVersion
        {
            get;
            set;
        }

        public string CheckVersionUrl
        {
            get;
            set;
        }

        public string WindowsAppUrl
        {
            get;
            set;
        }

        public string MacOSAppUrl
        {
            get;
            set;
        }

        public string IOSAppUrl
        {
            get;
            set;
        }

        public string AndroidAppUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 游戏公告列表 HTTP 接口地址（如 https://host/api/v1/announcements）。
        /// </summary>
        public string AnnouncementUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 问题反馈提交 HTTP 接口地址（如 https://host/api/v1/feedback/submit）。
        /// </summary>
        public string FeedbackUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 反馈截图 OSS 上传地址。
        /// </summary>
        public string FeedbackOssUploadUrl
        {
            get;
            set;
        }
    }
}
