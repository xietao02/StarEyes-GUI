using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using StarEyes_GUI.ViewModels;

namespace StarEyes_GUI.Models {
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