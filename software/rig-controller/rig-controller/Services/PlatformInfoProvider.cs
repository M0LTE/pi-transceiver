namespace rig_controller.Services
{
    public class PlatformInfoProvider
    {
        public bool IsWindows => Environment.OSVersion.Platform == PlatformID.Win32NT;
    }
}
