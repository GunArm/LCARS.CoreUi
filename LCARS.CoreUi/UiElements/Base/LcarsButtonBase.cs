using LCARS.CoreUi.Assets.Access;
using LCARS.CoreUi.Colors;
using LCARS.CoreUi.Enums;
using LCARS.CoreUi.Helpers;
using LCARS.CoreUi.Interfaces;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace LCARS.CoreUi.UiElements.Base
{
    /// <summary>
    /// Generic LCARS button that all others inherit from
    /// </summary>
    /// <remarks>
    /// This class can be placed directly on a form in the designer. If you do so, you will wind up with something resembling a Standard 
    /// Button, but with fewer features. This class should only be used directly when you don't know what kind of LCARS button you're
    /// dealing with. Almost everything in modBusiness is declared that way for exactly that reason. 
    /// </remarks>
    [DefaultEvent("Click"), Designer(typeof(LcarsButtonBaseDesigner))]
    public partial class LcarsButtonBase : Control, IAlertable, IBeeping, IColorable
    {
        /// <summary>
        /// Creates a new instance of the LcarsButtonBase object
        /// </summary>
        /// <remarks></remarks>
        public LcarsButtonBase() : base()
        {
            // for designer, InitEvents (esp resize) before InitializeComponent()
            InitEvents();

            textSize = Size;
            font = FontProvider.Lcars(textHeight);
            InitializeComponent();
            base.DoubleBuffered = true;
            try
            {
                // try/catch because this pukes in the designer where SoundProvider ctor apparently gets omitted
                sound = new System.Media.SoundPlayer(SoundProvider.PlainBeep);
                sound.Load();
            }
            catch { }
        }

        public void DoClick()
        {
            Base_Click(this, null);
        }

        private IContainer components;
        private ContentAlignment oAlign;


        private void InitializeComponent()
        {
            SuspendLayout();
            //
            //StandardButton
            //
            Name = "LcarsButton";
            Size = new Size(147, 36);
            ResumeLayout(false);
            Text = "LCARS";
        }

        private Bitmap normalButton;
        private Bitmap unlitButton;
        private bool isPressed = false;
        private static System.Media.SoundPlayer sound;
        private System.Windows.Forms.Timer textScrollTimer = new System.Windows.Forms.Timer();
        protected EllipsisModes ellipsisMode = EllipsisModes.Character;


        private Thread flasher;
        private bool flasherInvertLit; // toggled back and forth when flashing is on
        private void FlasherThread()
        {
            try
            {
                while (flashing)
                {
                    flasherInvertLit = !flasherInvertLit;
                    Invalidate();
                    Application.DoEvents();
                    Thread.Sleep(flashingInterval);
                }
            }
            catch (ThreadAbortException t)
            {
                flasherInvertLit = false;
                Invalidate();
            }
        }

        private void PlaySound()
        {
            string soundPath = new SettingsStore("LCARS").Load("Application", "ButtonSound", "");
            if (sound == null | sound.SoundLocation != soundPath)
            {
                if (System.IO.File.Exists(soundPath)) sound = new System.Media.SoundPlayer(soundPath);
                else sound = new System.Media.SoundPlayer(SoundProvider.PlainBeep);
            }
            sound.Play();
        }

        private void DoButtonDownActions()
        {
            if (doesBeep) PlaySound();
        }
    }
}
