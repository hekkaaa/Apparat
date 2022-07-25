namespace Apparat.Configuration.Events
{
    public interface IHostViewModelEvents
    {
        HostViewModelEvents.VoidMethodHandler ErrorNameHostnameEvent { get; set; }
        HostViewModelEvents.BoolMethodHandler ManagementEnableGeneralControlBtnEventAndPreloaderVisible { get; set; }
        HostViewModelEvents.BoolMethodHandler WorkingProggresbarInListBoxHostnameEvent { get; set; }
    }
}