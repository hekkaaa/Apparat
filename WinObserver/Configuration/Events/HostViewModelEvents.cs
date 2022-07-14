namespace Apparat.Configuration.Events
{
    public class HostViewModelEvents : IHostViewModelEvents
    {
        public VoidMethodHandler ErrorNameHostnameEvent { get; set; }
        public BoolMethodHandler ManagementEnableGeneralControlBtnEvent { get; set; }
        public BoolMethodHandler WorkingProggresbarInListBoxHostnameEvent { get; set; }

        public delegate void VoidMethodHandler();
        public delegate void BoolMethodHandler(bool x);
    }
}
