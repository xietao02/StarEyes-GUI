namespace StarEyes_GUI.Models {
    public class DashboardModel {
        public string ID { get; set; }
        public int eventNum { get; set; }
        public string userToolTip { get; set; } = "用户ID:";
        public string? notifToolTip { get; set; }
        public string? bellSrc { get; set; }

        public DashboardModel() {
            //ID = StarEyesModel.ID;
            ID = "10001";
            eventNum = 0;
            userToolTip += ID;
        }

        
}
}
