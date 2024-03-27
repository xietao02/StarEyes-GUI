using StarEyes_GUI.Common.Utils;
using StarEyes_GUI.UserControls;
using System;
using System.Collections.Generic;
using System.Web;

namespace StarEyes_GUI.Common.Data {
    /// <summary>
    /// 客户端基础数据存储
    /// </summary>
    public static class StarEyesData {
        /// <summary>
        /// 登录账号ID
        /// </summary>
        public static string ID { get; set; } = "";

        /// <summary>
        /// 登录账号用户名
        /// </summary>
        public static string UserName { get; set; } = "";

        /// <summary>
        /// 登录账号微信号
        /// </summary>
        public static string Wechat { get; set; }

        /// <summary>
        /// 登录账号邮箱
        /// </summary>
        public static string Email { get; set; } = "";

        /// <summary>
        /// 登录账号电话号码
        /// </summary>
        public static string Phone { get; set; }


        /// <summary>
        /// 登录账号所属组织
        /// </summary>
        public static string Organization { get; set; } = "";


        /// <summary>
        /// 摄像头列表
        /// </summary>
        public static List<CameraItem> CameraList = new();
        
    }

}
