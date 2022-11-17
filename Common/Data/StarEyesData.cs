using StarEyes_GUI.Common.Utils;

namespace StarEyes_GUI.Common.Data {
    /// <summary>
    /// 客户端基础数据存储
    /// </summary>
    public static class StarEyesData {
        /// <summary>
        /// 登录账号ID
        /// </summary>
        public static string ID { get; set; } = "10001";    // For Debug 

        /// <summary>
        /// 登录账号所属组织
        /// </summary>
        public static string Organization { get; set; } = "507";    // For Debug 

        /// <summary>
        /// 组织下未处理事件总数
        /// </summary>
        public static int UnhandledEventNum { get; set; } = 0;
    }

}
