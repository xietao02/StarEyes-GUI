namespace StarEyes_GUI.Models {
    /// <summary>
    /// 登录界面模型
    /// </summary>
    public class LoginModel {
        public string ID { get; set; }
        public string PW { get; set; }
        public bool Auth { get; set; }

        public LoginModel() {
            ID = "";
            PW = "";
            Auth = false;
        }
    }
    
}