using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BigMansStuff.LocusEffects;


    public class cScreenEffects
    {
        public enum eEffectType
        {
            FullTextLocusEffect,

            BeaconLocusEffect,

            TextLocusEffect,

            BitmapLocusEffect
        }

        private readonly TextLocusEffect      mDefaultLocusEffectText = new TextLocusEffect();
        private readonly BitmapLocusEffect    mImageEffect            = new BitmapLocusEffect();
        private readonly LocusEffectsProvider mLocusEffectsProvider;
        private readonly TextLocusEffect      mScreenTextEffect       = new TextLocusEffect();                                    
        private BeaconLocusEffect             mBeacon                 = new BeaconLocusEffect(); 
        private BaseLocusEffect               mEffect;

        public bool Active = true;

        public cScreenEffects(IContainer pComponents, int pFramesPerSecond)
        {
            // 스크린 텍스트 등록
            this.mLocusEffectsProvider = new LocusEffectsProvider(pComponents);
            this.mLocusEffectsProvider.Initialize();

            this.mLocusEffectsProvider.FramesPerSecond = pFramesPerSecond;
            this.mDefaultLocusEffectText.Name          = "DefaultLocusEffectText";
            this.mScreenTextEffect.Name                = "FullScreenText";
            this.mBeacon.Name                          = "CustomBeacon_Shrinking";

            this.mLocusEffectsProvider.AddLocusEffect(this.mScreenTextEffect);
            this.mLocusEffectsProvider.AddLocusEffect(this.mBeacon);
            this.mLocusEffectsProvider.AddLocusEffect(this.mDefaultLocusEffectText);
        }


        /// <summary>
        ///     풀스크린 텍스트 설정
        /// </summary>
        /// <param name="pText"></param>
        /// <param name="pSe"></param>
        /// <param name="pFont"></param>
        /// <param name="pAngle"></param>
        public void SetTextEffect(string pText, Color[] pSe, Font pFont, int pAngle)
        {
            this.mScreenTextEffect.Text                = pText;
            this.mScreenTextEffect.AnimationStartColor = pSe[0];
            this.mScreenTextEffect.AnimationEndColor   = pSe[1];
            this.mScreenTextEffect.AnchoringMode       = AnchoringMode.CenterMonitor;
            this.mScreenTextEffect.Font                = pFont;
            this.mScreenTextEffect.RotationAngle       = pAngle;
        }

        /// <summary>
        ///     init_BeaconEffect
        /// </summary>
        /// <param name="pStyle"></param>
        public void initBeaconEffect(BeaconEffectStyles pStyle)
        {
            this.mBeacon = new BeaconLocusEffect { Style = pStyle };

            if (pStyle == BeaconEffectStyles.Shrink)
            {
                this.mBeacon.InitialSize         = new Size(60, 60);
                this.mBeacon.AnimationTime       = 1500;
                this.mBeacon.AnimationStartColor = Color.Black;
                this.mBeacon.AnimationEndColor   = Color.LightBlue;
                this.mBeacon.AnimationOuterColor = Color.Crimson;
                this.mBeacon.BrokenRing          = true;
                this.mBeacon.RingWidth           = 5;
                this.mBeacon.OuterRingWidth      = 3;
                this.mBeacon.Rotate              = false;
                this.mBeacon.RotatationSpeed     = 120;
                this.mBeacon.ShowShadow          = true;
            }
            else if (pStyle == BeaconEffectStyles.None)
                this.mBeacon = new BeaconLocusEffect
                               {
                                   Name                = "CustomBeacon2",
                                   InitialSize         = new Size(100, 100),
                                   AnimationTime       = 2500,
                                   LeadInTime          = 0,
                                   LeadOutTime         = 0,
                                   AnimationStartColor = Color.HotPink,
                                   AnimationEndColor   = Color.HotPink,
                                   AnimationOuterColor = Color.Pink,
                                   RingWidth           = 6,
                                   OuterRingWidth      = 3,
                                   BodyFadeOut         = true,
                                   Style               = BeaconEffectStyles.HeartBeat
                               };
            else if (pStyle == BeaconEffectStyles.HeartBeat)
                this.mBeacon = new BeaconLocusEffect
                               {
                                   Name                = "CustomBeacon3",
                                   InitialSize         = new Size(100, 100),
                                   AnimationTime       = 2500,
                                   LeadInTime          = 0,
                                   LeadOutTime         = 0,
                                   AnimationStartColor = Color.Red,
                                   AnimationEndColor   = Color.DarkRed,
                                   AnimationOuterColor = Color.HotPink,
                                   RingWidth           = 6,
                                   OuterRingWidth      = 2,
                                   BodyFadeOut         = true,
                                   Style               = BeaconEffectStyles.HeartBeat
                               };
            this.mLocusEffectsProvider.AddLocusEffect(this.mBeacon);
        }

        /// <summary>
        ///     이미지 Effect
        /// </summary>
        /// <param name="pImg"></param>
        public void initImageEffect(Image pImg)
        {
            this.mImageEffect.Name                = "CustomImage";
            this.mImageEffect.AnimationStartColor = Color.DimGray;
            this.mImageEffect.AnimationEndColor   = Color.Lime;
            this.mImageEffect.AnimationTime       = 2500; // msec
            this.mImageEffect.Bitmap              = pImg as Bitmap;
            this.mImageEffect.ShadowOpacity       = 0; // %
            this.mImageEffect.ShadowOffset        = new Point(1, 1); // %
            this.mImageEffect.AnchoringMode       = AnchoringMode.CenterOffset;
            this.mImageEffect.AnchoringOffset     = new Point(0, 0);
            this.mImageEffect.MovementMode        = MovementMode.Custom;
            this.mImageEffect.LeadInTime          = 500;
            this.mImageEffect.BodyFadeOut         = true;
            this.mImageEffect.MovementAmplitude   = 10;

            this.mLocusEffectsProvider.AddLocusEffect(this.mImageEffect);
        }

        public void InitText(Font pFont)
        {
            InitText(Color.Red, Color.DarkRed, pFont);
        }

        public void InitText(Color pStartColor,Color pEndColor, Font pFont)
        {
            this.mDefaultLocusEffectText.AnimationStartColor = pStartColor;
            this.mDefaultLocusEffectText.AnimationEndColor   = pEndColor;
            this.mDefaultLocusEffectText.LeadInTime          = 500;
            this.mDefaultLocusEffectText.AnimationTime       = 1000;
            this.mDefaultLocusEffectText.LeadOutTime         = 500;
            this.mDefaultLocusEffectText.Font                = pFont;
            this.mDefaultLocusEffectText.ShadowOffset        = new Point(1, 1);
        }

        public void InitText(TextLocusEffect pTextEffect)
        {
            this.mDefaultLocusEffectText.AnimationStartColor = pTextEffect.AnimationStartColor;
            this.mDefaultLocusEffectText.AnimationEndColor   = pTextEffect.AnimationEndColor;
            this.mDefaultLocusEffectText.LeadInTime          = pTextEffect.LeadInTime;
            this.mDefaultLocusEffectText.AnimationTime       = pTextEffect.AnimationTime;
            this.mDefaultLocusEffectText.LeadOutTime         = pTextEffect.LeadOutTime;
            this.mDefaultLocusEffectText.Font                = pTextEffect.Font;
            this.mDefaultLocusEffectText.Text                = pTextEffect.Text;
            this.mDefaultLocusEffectText.ShadowOffset        = pTextEffect.ShadowOffset;
        }

        #region # Show Effects #

        public void ShowEffect(Point pP, eEffectType pType, bool pEnd)
        {
            try
            {
                if (!Active) return;
                if (this.mEffect != null && this.mEffect.IsAnimating)
                {
                    if (!pEnd) return;
                    this.mEffect.StopEffect();
                }

                switch (pType)
                {
                    case eEffectType.BeaconLocusEffect:
                        this.mEffect = this.mBeacon;
                        break;
                    case eEffectType.TextLocusEffect:
                        this.mEffect = this.mScreenTextEffect;
                        break;
                    case eEffectType.BitmapLocusEffect:
                        this.mEffect = this.mImageEffect;
                        break;
                    default:
                        this.mEffect = this.mBeacon;
                        break;
                }
                if (Form.ActiveForm != null) this.mEffect.ShowEffect(Form.ActiveForm, new Rectangle(new Point(pP.X, pP.Y), new Size(10, 10)));
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void ShowEffect(Point pP, eEffectType pType, string pText, Form pActiveForm = null)
        {
            try
            {
                if (!Active) return;
                if (this.mEffect != null && this.mEffect.IsAnimating) this.mEffect.StopEffect();

                switch (pType)
                {
                    case eEffectType.BeaconLocusEffect:
                        this.mEffect                      = this.mBeacon;
                        break;
                    case eEffectType.FullTextLocusEffect:
                        this.mScreenTextEffect.Text       = pText;
                        this.mEffect                      = this.mScreenTextEffect;
                        break;
                    case eEffectType.TextLocusEffect:
                        this.mDefaultLocusEffectText.Text = pText;
                        this.mEffect                      = this.mDefaultLocusEffectText;
                        break;
                    default:
                        this.mEffect                      = this.mBeacon;
                        break;
                }
                if (pActiveForm == null) pActiveForm = Form.ActiveForm;
                if (pActiveForm != null) this.mEffect.ShowEffect(pActiveForm, new Rectangle(new Point(pP.X, pP.Y), new Size(10, 10)));
            }
            catch (Exception)
            {
                // ignored
            }

        }

        public void ShowEffect(Point pP, eEffectType pType, Color pCol, bool pEnd)
        {
            try
            {
                if (!Active) return;
                if (this.mEffect != null && this.mEffect.IsAnimating)
                {
                    if (!pEnd) return;
                    this.mEffect.StopEffect();
                }

                switch (pType)
                {
                    case eEffectType.BeaconLocusEffect:
                        this.mEffect = this.mBeacon;
                        break;
                    case eEffectType.TextLocusEffect:
                        this.mEffect = this.mScreenTextEffect;
                        break;
                    case eEffectType.BitmapLocusEffect:
                        this.mImageEffect.AnimationEndColor = pCol;
                        this.mEffect = this.mImageEffect;
                        break;
                    default:
                        this.mEffect = this.mBeacon;
                        break;
                }
                if (Form.ActiveForm != null) this.mEffect.ShowEffect(Form.ActiveForm, new Rectangle(new Point(pP.X, pP.Y), new Size(10, 10)));
            }
            catch (Exception)
            {
                // ignored
            }
        }

        #endregion
    }

