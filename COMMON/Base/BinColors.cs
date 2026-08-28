using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


    /***************************************************************************/
    /* Class: TColorItem                                                       */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TColorItem
    {
        public eRank  Status;
        public string Name;
        public Color  iColor;

        public TColorItem()
        {
            Status   = eRank.Empty;
            Name     = "";
            iColor   = new Color();
        }

        public void Set(int no, int stsno, string name, byte r, byte g, byte b)
        {
            Status   = (eRank)stsno;
            Name     = name;
            iColor   = Color.FromArgb(r,g,b);
        }
    }


    /***************************************************************************/
    /* Class: TBinColors                                                       */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TBinColors
    {
        public const int RANK_COLOR_COUNT =256;
        public TColorItem[] RankColors;


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TBinColors()
        {
        }
        ~TBinColors() { }

        public void Init(string DevName)
        {
            RankColors = new TColorItem[RANK_COLOR_COUNT];
            for (int i = 0; i < RANK_COLOR_COUNT; i++)
            {
                RankColors[i] = new TColorItem();
                RankColors[i].Status = (eRank)i;
                RankColors[i].Name   = "R"+i.ToString("D03");
            }

            Load(true, DevName);     
        }
        //--------------------------------------------------------------------------
        public void Load(bool IsLoad, String DevName)
        {

            int r=0,g=0,b=0,sts=0;
            String sPath;
            String sFile = "BinColors";
            String sSection = sFile;
            TIniUnit ini = new TIniUnit();

            //Make Dir.
            FNC.CreateDirOnWork("Project");
            FNC.CreateDirOnWork("Project\\" + DevName);
            sPath = Application.StartupPath + "\\Project\\" + DevName + "\\" + sFile + ".INI";

            if(IsLoad) {
                for (int i = 0; i < RANK_COLOR_COUNT; i++)
                {
                    ini.Load(sPath, String.Format("_{0}RankColor", i+1), "Status", out sts);
                    ini.Load(sPath, String.Format("_{0}RankColor", i+1), "ColorR", out r  );
                    ini.Load(sPath, String.Format("_{0}RankColor", i+1), "ColorG", out g  );
                    ini.Load(sPath, String.Format("_{0}RankColor", i+1), "ColorB", out b  );
                                                              
                    RankColors[i].Name   = Convert.ToString(i);
                    RankColors[i].Status = (eRank)sts;
                    RankColors[i].iColor = Color.FromArgb(r,g,b);

                }

            }
            else {

                for (int i = 0; i < RANK_COLOR_COUNT; i++)
                {
                    sts = (int)RankColors[i].Status;
                    r   = (int)RankColors[i].iColor.R;
                    g   = (int)RankColors[i].iColor.G;
                    b   = (int)RankColors[i].iColor.B;
                    ini.Save(sPath, String.Format("_{0}RankColor", i+1), "Status", sts);
                    ini.Save(sPath, String.Format("_{0}RankColor", i+1), "ColorR", r  );
                    ini.Save(sPath, String.Format("_{0}RankColor", i+1), "ColorG", g  );
                    ini.Save(sPath, String.Format("_{0}RankColor", i+1), "ColorB", b  );

                }
            }
        }
        //--------------------------------------------------------------------------
        public Color ColorEmpty
        {
            get { return RankColors[0].iColor; }
            set { RankColors[0].iColor = value;}
        }
        //--------------------------------------------------------------------------
        public Color ColorEnd
        {
            get { return RankColors[251].iColor; }
            set { RankColors[251].iColor = value;}
        }
        //--------------------------------------------------------------------------
        public Color ColorError1
        {
            get { return RankColors[252].iColor; }
            set { RankColors[252].iColor = value;}
        }
        //--------------------------------------------------------------------------
        public Color ColorError2
        {
            get { return RankColors[253].iColor; }
            set { RankColors[253].iColor = value;}

        }
        //--------------------------------------------------------------------------
        public Color ColorUnknown
        {
            get { return RankColors[254].iColor; }
            set { RankColors[254].iColor = value;}
        }
        //--------------------------------------------------------------------------
        public Color ColorNone
        {
            get { return RankColors[255].iColor; }
            set { RankColors[255].iColor = value;}
        }
    }

