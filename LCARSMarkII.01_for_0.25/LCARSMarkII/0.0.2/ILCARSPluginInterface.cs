
namespace LCARSMarkII
{
    public interface ILCARSPlugin
    {
        string subsystemName { get; }
        string subsystemDescription { get; }
        string subsystemStation { get; }
        float subsystemPowerLevel_standby { get; }
        float subsystemPowerLevel_running { get; }
        float subsystemPowerLevel_additional { get; }
        bool subsystemIsPartModule { get; }
        bool subsystemIsDamagable { get; }
        bool subsystemPanelState { get; set; }

        void SetVessel(Vessel vessel);
        void getGUI();
        void customCallback(string key, string value1 = "", string value2 = "", string value3 = "", string value4 = "", string value5 = "");
    }

}
