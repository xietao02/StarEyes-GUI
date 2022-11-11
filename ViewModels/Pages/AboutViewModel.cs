
using StarEyes_GUI.Utils;
using System.Windows.Documents;

namespace StarEyes_GUI.ViewModels.Pages {
    public class AboutViewModel : PageViewModelBase {

        public DelegateCommand cmd => new DelegateCommand(act, func);

        void act(object obj) {
            System.Diagnostics.Trace.WriteLine("act!!");
            info a = obj as info;
            System.Diagnostics.Trace.WriteLine(a.name + a.value);
        }

        bool func(object obj) {
            return true;
        }

        public class info {
            public string name { get; set; }
            public string value { get; set; }
        }

    }
}
