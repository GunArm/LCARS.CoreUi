using System.IO;
using System.Media;

namespace LCARS.CoreUi.Assets.Access
{
    class LcarsSound
    {
        public static SoundPlayer soundPlayer = new SoundPlayer();
        public static void Play(LcarsSoundAsset assetKey)
        {
            var resourceStream = GetGetResourceStream(assetKey);
            if (resourceStream == null) return;

            lock (soundPlayer)
            {
                soundPlayer.Stream = resourceStream;
                soundPlayer.Play();
            }
        }

        private static Stream GetGetResourceStream(LcarsSoundAsset assetKey)
        {
            switch (assetKey)
            {
                case LcarsSoundAsset.Unset:
                    return null;
            }
            return null;
        }
    }

    public enum LcarsSoundAsset
    {
        Unset = 0,
    }
}
