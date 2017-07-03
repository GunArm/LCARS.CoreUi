using LCARS.CoreUi.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;

namespace LCARS.CoreUi.Assets.Access
{
    public class LcarsSound
    {
        private const string soundsPrefix = "LCARS.CoreUi.Assets.Sounds.";

        public static SoundPlayer soundPlayer = new SoundPlayer();

        public static List<string> AlertWavs;
        public static List<string> BeepWavs;
        public static List<string> FailWavs;
        public static List<string> ProcessingWavs;

        static LcarsSound()
        {
            AlertWavs = new List<string>(GetResourceNamesFromNamespace(soundsPrefix + "Alert"));
            BeepWavs = new List<string>(GetResourceNamesFromNamespace(soundsPrefix + "Beep"));
            FailWavs = new List<string>(GetResourceNamesFromNamespace(soundsPrefix + "Fail"));
            ProcessingWavs = new List<string>(GetResourceNamesFromNamespace(soundsPrefix + "Processing"));
        }

        public static void Play(LcarsSoundAsset assetKey)
        {
            var resourceSting = GetGetResourceString(assetKey);
            if (resourceSting == null) return;

            Play(resourceSting);
        }

        private static void Play(string resourceSting)
        {
            lock (soundPlayer)
            {
                var thisAssembly = Assembly.GetExecutingAssembly();
                using (Stream wavStream = thisAssembly.GetManifestResourceStream(resourceSting))
                {
                    soundPlayer.Stream = wavStream;
                    soundPlayer.Play();
                }
            }
        }

        private static string GetGetResourceString(LcarsSoundAsset assetKey)
        {
            switch (assetKey)
            {
                case LcarsSoundAsset.RandomAlert:
                    return AlertWavs.Random();
                case LcarsSoundAsset.RandomBeep:
                    return BeepWavs.Random();
                case LcarsSoundAsset.RandomFail:
                    return FailWavs.Random();
                case LcarsSoundAsset.RandomProcessing:
                    return ProcessingWavs.Random();

                case LcarsSoundAsset.PlainBeep:
                    return soundsPrefix + "Beep.004.wav";
                case LcarsSoundAsset.PlainProcessing:
                    return soundsPrefix + "Processing.001.wav";
                case LcarsSoundAsset.TurnOn1:
                    return soundsPrefix + "Toggle.003_turn_on.wav";
                case LcarsSoundAsset.TurnOff1:
                    return soundsPrefix + "Toggle.004_turn_off.wav";
                case LcarsSoundAsset.TurnOn2:
                    return soundsPrefix + "Toggle.001_doorbell_on.wav";
                case LcarsSoundAsset.TurnOff2:
                    return soundsPrefix + "Toggle.002_doorbell_off.wav";

                case LcarsSoundAsset.Unset:
                default:
                    return null;
            }
        }

        private static string[] GetResourceNamesFromNamespace(string nameSpace)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            return executingAssembly.GetManifestResourceNames()
                .Where(r => r.StartsWith(nameSpace) && r.EndsWith(".wav"))
                .ToArray();
        }
    }

    public enum LcarsSoundAsset
    {
        Unset = 0,

        RandomAlert,
        RandomBeep,
        RandomFail,
        RandomProcessing,

        PlainBeep,
        PlainProcessing,

        TurnOn1,
        TurnOff1,
        TurnOn2,
        TurnOff2,
    }
}
