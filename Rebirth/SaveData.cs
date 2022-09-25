using COTL_API.Saves;

namespace Rebirth;

public static class SaveData
{
    public class BornAgainFollowerData
    {
        public FollowerInfo FollowerInfo { get; }
        public bool BornAgain { get; }

        public BornAgainFollowerData(FollowerInfo followerInfo, bool bornAgain)
        {
            FollowerInfo = followerInfo;
            BornAgain = bornAgain;
        }

    }

    public static BornAgainFollowerData GetBornAgainFollowerData(FollowerInfo followerInfo)
    {
        var data = ModdedSaveManager.Data.GetValue<BornAgainFollowerData>(Plugin.PluginGuid, $"Rebirth.BornAgainFollowerData.{followerInfo.ID}");
        if (data == null)
        {
            Plugin.Log.LogError($"Error loading follower data for {followerInfo.Name}. May not exist yet.");
            return new BornAgainFollowerData(followerInfo, false);
        }
        Plugin.Log.LogWarning($"Loaded follower data for {followerInfo.Name}");
        return data;
    }
    
    public static void SetBornAgainFollowerData(BornAgainFollowerData bornAgainFollowerData)
    {
        ModdedSaveManager.Data.SetValue<BornAgainFollowerData>(Plugin.PluginGuid, $"Rebirth.BornAgainFollowerData.{bornAgainFollowerData.FollowerInfo.ID}", bornAgainFollowerData);
        Plugin.Log.LogWarning($"Saved follower data for {bornAgainFollowerData.FollowerInfo.Name}");
    }
}