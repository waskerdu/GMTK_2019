using Malee;
using System;
using static Utilities;

/* When upgrading post-launch, tag new fields with:
 * [OptionalField(VersionAdded = int)]
 * This allows old save files to be upgraded to the new format
 * Remember to initialize any fields that might import as null before saving again.
*/

#region GameData
[Serializable]
public struct GameData
{
    public PlayerInfo playerInfo;
}
#endregion

#region Player Info
[Serializable]
public struct PlayerInfo
{
    public Vector3Serializer position;
    public Vector2Serializer rotation;
}
#endregion