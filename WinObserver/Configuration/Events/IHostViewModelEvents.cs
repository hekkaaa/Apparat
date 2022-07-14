namespace Apparat.Configuration.Events
{
    public interface IHostViewModelEvents
    {
        HostViewModelEvents.VoidMethodHandler ErrorNameHostnameEvent { get; set; }
        HostViewModelEvents.BoolMethodHandler ManagementEnableGeneralControlBtnEvent { get; set; }
        HostViewModelEvents.BoolMethodHandler WorkingProggresbarInListBoxHostnameEvent { get; set; }
    }
}