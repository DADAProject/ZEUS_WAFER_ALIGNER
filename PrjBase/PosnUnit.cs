using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Data;

namespace eMachine
{
    public enum EN_POS_ID  : int
    {
        NONE,
        NORM,
        COMM,
        VIEW,
        PARA
    };

    /***************************************************************************/
    /* Class: TSetItem                                                         */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public class TSetItem
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */

        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        public string m_sName      ;
        public string m_sUnit      ;
        public double m_dVal       ;   
        public int    m_iDigit     ;
        public int    m_iMotor     ;
        public double m_dMin       ;
        public double m_dMax       ;
        public int    m_iManNo     ;
        public int    m_iPosnKind  ;
        public int    m_iPosnId    ;
        public bool   m_bHomeOffset;
        public bool   m_bDefUserMan;

        
        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int _int
        {
            get { return (int)m_dVal; }
            set { m_dVal = (double)value; }
        }
        public double _Pos
        {
            get { return m_dVal*1000; }
            set { m_dVal = value/1000; }
        }
        public String _Text
        {
            get { return tFormat(m_dVal); }
            set { m_dVal = Convert.ToDouble(value); }
        }
        public double _Val
        {
            get { return m_dVal; }
            set { m_dVal = value; }
        }
        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TSetItem()
        {
        }
        ~TSetItem() { }
        //--------------------------------------------------------------------------
        public string tFormat (double dVal)
        {
            string sTemp;
            if(m_iDigit == 0) sTemp = String.Format("{0:F4}", m_dVal);
            else
            {
                     if(m_iDigit == 1) sTemp = String.Format("{0:F1}" , m_dVal);
                else if(m_iDigit == 2) sTemp = String.Format("{0:F2}" , m_dVal);
                else if(m_iDigit == 3) sTemp = String.Format("{0:F3}" , m_dVal);
                else                   sTemp = String.Format("{0:F4}" , m_dVal);
            }
            return sTemp;
        }
        //--------------------------------------------------------------------------
        public string Add(string sStr, bool bMode)
        {
            double dVal = Convert.ToDouble(sStr); 
            double dAdd = 1;
            for(int i=0; i<m_iDigit-2; i++) dAdd = dAdd * 10;
            if(dAdd == 0) dAdd = 1.0;
            else          dAdd = 1.0/ dAdd;

            if(bMode) { dVal += dAdd; if(dVal>m_dMax) dVal = m_dMax; }
            else      { dVal -= dAdd; if(dVal<m_dMin) dVal = m_dMin; }
            return tFormat(dVal);
        }
    }


    /***************************************************************************/
    /* Class: TSetPart                                                         */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TSetPart
    {

        public TSetItem[]  Set = new TSetItem[vDEF.MAXITEM];
        public int         m_iItemCnt ;
        public int         m_iMotorCnt;
        public int[]       m_iPosnCnt = new int[(int)EN_MOTR_ID.EndOfId];
        public string      m_sName    ;    
        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TSetPart()
        {
            for (int i=0; i<vDEF.MAXITEM; i++) {
                Set[i] = new TSetItem (); 
            }
        }
        ~TSetPart() { }
    };


    /***************************************************************************/
    /* Class: TPosnUnit                                                        */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public class TPosnUnit
    {
        //Vars.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //private:   /* Member Var.             */



        //protected: /* Inheritable Vars.        */

        //public:    /* Direct Accessable Vars.  */
        string  m_sCrntDvcName;

        //int     m_iColCnt     ;
        int     m_iPartCnt    ;
        int     m_iSelPart    ;
	    int     m_iSelMotor   ;
        int     m_iLErrNo     ;
        int     m_iLManNo     ;

        public TSetPart[]  Dat = new TSetPart[vDEF.MAX_SEQ_PART ];


        //Property.
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public int _iSelMotor
        {
            get { return m_iSelMotor; }
            set { m_iSelMotor = value; }
        }
        public int _iSelPart
        {
            get { return m_iSelPart; }
            set { m_iSelPart = value; }
        }
        public int _iLErrNo
        {
            get { return m_iLErrNo; }
        }
        public int _iLManNo
        {
            get { return m_iLManNo; }
        }

        //Member Class 
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        System.Windows.Forms.DataGridView itemGrid;        
            
        //Method
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //생성자 & 소멸자. (Constructor & Destructor)
        public TPosnUnit()
        {
            Init();
        }
        ~TPosnUnit() { }
        //--------------------------------------------------------------------------
        //Display
        public void DisplayItem(ref System.Windows.Forms.DataGridView pGrid, Color backColor, bool sLoad = true)
        {
	        int[]    iWidth = {0, 100, 50, 90};
	        string[] sItem  = {"NAME", "VALUE", "UNIT", "MOVE"};

            int    iItemCnt = 0;
            int    iPart    = m_iSelPart;
            int    iPosnId ;
            int    iMotrId ;
            int    iManNo  ;
            int    iTotWidth = 0;
	        double dPosnData;
            string sName    ;
            Color  RowColor ;

            if (pGrid == null) return;
            itemGrid = pGrid;

            if (m_iSelPart < 0 || m_iSelPart >= vDEF.MAX_SEQ_PART) return;

            if(sLoad) {

                DataGridViewButtonColumn btnMove = new DataGridViewButtonColumn();  //버튼 추가
	            btnMove.HeaderText = "";
	            btnMove.Name = "btnMove"; 
                btnMove.FlatStyle =  FlatStyle.Popup;
                btnMove.DefaultCellStyle.BackColor = Color.PeachPuff;
                btnMove.DefaultCellStyle.Font = new System.Drawing.Font("Century Gothic", 11);

                pGrid.Dock = System.Windows.Forms.DockStyle.Fill;
                FNC.SetGridStyle(ref itemGrid, 40);
                

                for(int i=0;i<4;i++) 
                {
                    if(i==3) itemGrid.Columns.Add(btnMove );
                    else     itemGrid.Columns.Add(sItem[i] , sItem[i]);
                    itemGrid.Columns[i].Width = iWidth[i];
                    iTotWidth += iWidth[i];
                }
                itemGrid.Columns[0].Width = itemGrid.Width - iTotWidth-20;
                itemGrid.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;


                itemGrid.Columns[0].ReadOnly = true;
                itemGrid.Columns[2].ReadOnly = true;
                itemGrid.Columns[3].ReadOnly = true;


                itemGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                itemGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                for (int i = 0; i < Dat[iPart].m_iItemCnt; i++)
                {
                    if(i>=vDEF.MAXITEM) continue;
                    iPosnId = Dat[iPart].Set[i].m_iPosnId;
                    if (iPosnId < 0 || iPosnId >= vDEF.MAX_POSN) continue;
                    if(m_iSelMotor >= 0)
                    {
                       if (m_iSelMotor != Dat[iPart].Set[i].m_iMotor)
                       {
                           itemGrid.RowCount = iItemCnt;
                           continue;
                       }
                    }

                    iMotrId = Dat[iPart].Set[i].m_iMotor;
                    if(iMotrId<0 || iMotrId>=cDEF.MOTR._iNumOfMotr   ) continue;
                    iManNo = Dat[iPart].Set[i].m_iManNo;
                    if (Dat[iPart].Set[i].m_bHomeOffset)
                    {
                        dPosnData = cDEF.MOTR[iMotrId].MP.dPosn[iPosnId] + cDEF.MOTR[iMotrId].m_dHomeOff;
                    }
                    else
                    {
                        dPosnData = cDEF.MOTR[iMotrId].MP.dPosn[iPosnId];
                    }
                    bool isPara = Dat[iPart].Set[i].m_iPosnKind == (int)EN_POS_ID.PARA;

                    Dat[iPart].Set[i]._Val = dPosnData;
			        sItem[0] = Dat[iPart].Set[i].m_sName;
			        sItem[1] = Dat[iPart].Set[i]._Text  ;
                    sItem[2] = Dat[iPart].Set[i].m_sUnit;   
                    sItem[3] = isPara? String.Format("GO[M{0,4:0000}]", 0) : String.Format("GO[M{0,4:0000}]", iManNo);

                    itemGrid.Rows.Add(sItem); 

                    RowColor = itemGrid.DefaultCellStyle.BackColor;
                    if (Dat[iPart].Set[i].m_iPosnKind == (int)EN_POS_ID.COMM) RowColor = Color.FromArgb(255, 192, 192);
                    if (Dat[iPart].Set[i].m_iPosnKind == (int)EN_POS_ID.VIEW) RowColor = Color.FromArgb(192, 255, 255);
                    if (Dat[iPart].Set[i].m_iPosnKind == (int)EN_POS_ID.PARA) RowColor = Color.LightYellow;

                    itemGrid.Rows[iItemCnt].DefaultCellStyle.BackColor = RowColor;   
                    iItemCnt++;
                }
				for (int index = 0; index < itemGrid.RowCount; index++)
				{
					itemGrid[0, index].Style.Font = new Font("Century Gothic", 12, FontStyle.Bold);
                    itemGrid[1, index].Style.Font = new Font("Century Gothic", 11, FontStyle.Regular);
                    itemGrid[2, index].Style.Font = new Font("Century Gothic", 12, FontStyle.Regular);
                    itemGrid[3, index].Style.Font = new Font("Century Gothic", 11, FontStyle.Regular);
                }
                //              
                pGrid.BackgroundColor = backColor; //Color.FromArgb(66, 72, 88);

                foreach (DataGridViewColumn item in itemGrid.Columns) { item.SortMode = DataGridViewColumnSortMode.NotSortable; }
            }
            else 
            {
               int iGetDatRow = 0;
                for(int i=0; i<Dat[iPart].m_iItemCnt; i++) 
                {
                    if(i>=vDEF.MAXITEM) continue;
                    iPosnId = Dat[iPart].Set[i].m_iPosnId;
                    if(iPosnId < 0 || iPosnId >= vDEF.MAX_POSN) continue;
                    if (m_iSelMotor >= 0) { if (m_iSelMotor != Dat[iPart].Set[i].m_iMotor) continue; }
                    iMotrId = Dat[iPart].Set[i].m_iMotor;
                    if(iMotrId<0 || iMotrId>=cDEF.MOTR._iNumOfMotr   ) continue;
                    Dat[iPart].Set[i]._Text = Convert.ToString(itemGrid[1,iGetDatRow++].Value);      
                    sName      = String.Format("{0}_{1}", GetPartName(iPart).Trim(), Dat[iPart].Set[i].m_sName);
                    if (Dat[iPart].Set[i].m_bHomeOffset) dPosnData = Dat[iPart].Set[i]._Val - cDEF.MOTR[iMotrId].m_dHomeOff;
		            else                                 dPosnData = Dat[iPart].Set[i]._Val;
		            if(Dat[iPart].Set[i].m_iPosnKind == (int)EN_POS_ID.COMM) {
                        WriteDatChLog(0, ref cDEF.MOTR[iMotrId].CMP.dPosn[iPosnId], dPosnData, sName);
                        cDEF.MOTR[iMotrId].MP.dPosn[iPosnId] = cDEF.MOTR[iMotrId].CMP.dPosn[iPosnId];
		            }
                    else 
                    {
                       WriteDatChLog(1, ref cDEF.MOTR[iMotrId].MP.dPosn[iPosnId], dPosnData, sName);
                    }
                    iItemCnt++;
                }
            //Save.
            }
            itemGrid.Visible  = true;
        }
        //--------------------------------------------------------------------------
        public void DisplayPart(ref CheckedListBox pclb)
        {
            String sPName                 ;
            int    iPartCnt = GetPartCnt();
            if(pclb == null) return;

            pclb.Items.Clear();
            for(int i=0;i<iPartCnt; i++) 
            {
                sPName   = GetPartName(i);
                pclb.Items.Add(sPName);
            }
        }
        //--------------------------------------------------------------------------
        public void DisplayPart(int iSelPart, ref System.Windows.Forms.DataGridView pGrid, Color backColor, bool IsDisAll = false, bool IsDisSys = false)
        {
            String sPName                 ;
            int    iPartCnt = GetPartCnt();
            int    iRowCnt  = iPartCnt    ;
            if(pGrid == null) return;
            if(IsDisAll     ) iRowCnt++;
            if(IsDisSys     ) iRowCnt++;
            int iRowHeight = 50;

            //
            FNC.SetGridStyle(ref pGrid , 50, false, false);
            pGrid.Dock = System.Windows.Forms.DockStyle.Top;
            pGrid.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
            pGrid.BackgroundColor = backColor; //Color.FromArgb(66, 72, 88);

            pGrid.DefaultCellStyle.ForeColor = Color.Black;
            pGrid.DefaultCellStyle.BackColor = Color.FromArgb(153, 153, 153);

            DataGridViewButtonColumn btnPart = new DataGridViewButtonColumn();  //버튼 추가
	        btnPart.HeaderText = "";
	        btnPart.Name = "btnPart";  
			btnPart.FlatStyle =  FlatStyle.Flat;

            pGrid.Columns.Add(btnPart );
            pGrid.Columns[0].Width = pGrid.Width-2;
            //pGrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            if(IsDisAll) {
                iSelPart += 1;    
                pGrid.Rows.Add("ALL");  
            }
            for(int i=0;i<iPartCnt; i++) {
                sPName   = GetPartName(i);
                pGrid   .Rows.Add(sPName); 
            }
            if(IsDisSys) pGrid.Rows.Add("SYSTEM"); 

            pGrid.Height = pGrid.RowCount * iRowHeight + 5; 

            if(iSelPart<0 || iSelPart>=pGrid.RowCount) iSelPart = 0;
            pGrid.Visible                 = true;
            pGrid.Rows[iSelPart].Selected = true; 

			for (int index = 0; index < pGrid.RowCount; index++)
			{
                //
                for (int c = 0; c < pGrid.ColumnCount; c++)
                {
                    pGrid[c, index].Style.Font = new Font("Century Gothic", 11, FontStyle.Bold);
                }
			}
        }
        //--------------------------------------------------------------------------
        public void DisplayMotor(ref System.Windows.Forms.DataGridView motrGrid, int Idx, Color backColor)
        {
            int iTotWidth = 0;
            String sMotrName;

	        int[]    iWidth = {0, 80, 70, 70};
	        String[] sItem  = {"NAME", "POS", "(+)","(-)"};

            if(Idx<0 || Idx>=vDEF.MAX_SEQ_PART) Idx = 0;

            DataGridViewButtonColumn btnJogP = new DataGridViewButtonColumn();  //버튼 추가
	        btnJogP.HeaderText = "(+)";
	        btnJogP.Name = "btnJogP";
            btnJogP.FlatStyle =  FlatStyle.Popup;  

            DataGridViewButtonColumn btnJogN = new DataGridViewButtonColumn();  //버튼 추가
	        btnJogN.HeaderText = "(-)";
	        btnJogN.Name = "btnJogN";  
            btnJogN.FlatStyle =  FlatStyle.Popup;

            motrGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            FNC.SetGridStyle(ref motrGrid, 50, false);
            for(int i=0;i<4;i++) 
            {
                if(i==2) 
                {
                    motrGrid.Columns.Add(btnJogP );
                    motrGrid.Columns[2].DefaultCellStyle.Font = new System.Drawing.Font("Wingdings 3", 15); 
                }
                else if(i==3) 
                {
                    motrGrid.Columns.Add(btnJogN ); 
                    motrGrid.Columns[3].DefaultCellStyle.Font = new System.Drawing.Font("Wingdings 3", 15);    
                }
                else
                {
                    motrGrid.Columns.Add(sItem[i], sItem[i]);
                    if (i == 1) motrGrid.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                motrGrid.Columns[i].Width = iWidth[i];
                iTotWidth += iWidth[i];

                motrGrid.Columns[i].ReadOnly = true;
            }
            motrGrid.Columns[0].Width = motrGrid.Width - iTotWidth-20;
            motrGrid.Columns[0].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

		    if(Idx <0 || Idx >=vDEF.MAX_SEQ_PART     ) return;
	        for(int i=0; i<cDEF.MOTR._iNumOfMotr;i++) 
            {
                if(Dat [Idx].m_iPosnCnt[i] < 0) continue;
                if(cDEF.MOTR[i]._iNoUseMotr==1 ) continue;

                //byte[] bt = Encoding.UTF8.GetBytes(cDEF.MOTR[i].m_sImgP);
                //char[] b = cDEF.MOTR[i].m_sImgP.ToCharArray();
                //string str = System.Text.Encoding.Unicode.GetString(bt);

                btnJogP.Text = cDEF.MOTR[i].m_sImgP;
                btnJogN.Text = cDEF.MOTR[i].m_sImgN;
                sMotrName = string.Format("#{0,2:00}-{1}", i, cDEF.MOTR[i].m_sNameAxis)+ "\r\n" +  cDEF.MOTR[i].m_sName;
		        sItem[0] = sMotrName   ;
		        sItem[1] =  "";
		        sItem[2] =  cDEF.MOTR[i].m_sImgP;
		        sItem[3] =  cDEF.MOTR[i].m_sImgN;
                motrGrid.Rows.Add(sItem);                
		    }
            
            for (int index = 0; index < motrGrid.RowCount; index++)
            {
				motrGrid[0, index].Style.Font = new Font("Century Gothic", 13, FontStyle.Bold);
                motrGrid[1, index].Style.Font = new Font("Century Gothic", 10, FontStyle.Regular);
            }
            //
            motrGrid.BackgroundColor = backColor; //Color.FromArgb(66, 72, 88);
            motrGrid.Visible   = true;

            foreach (DataGridViewColumn item in motrGrid.Columns) { item.SortMode = DataGridViewColumnSortMode.NotSortable; }

        }
        //------------------------------------------------------------------------
        public void SetGridFont(ref System.Windows.Forms.DataGridView pGrid, int size = 11, bool bold = false)
        {
            if (pGrid == null) return;
            itemGrid = pGrid;

            for (int index = 0; index < itemGrid.RowCount; index++)
            {
                for (int c = 0; c < itemGrid.ColumnCount; c++)
                { 
                    itemGrid[c, index].Style.Font = new Font("Century Gothic", size, bold ? FontStyle.Bold : FontStyle.Regular);
                }
            }

            //
            pGrid.Font = new System.Drawing.Font("Century Gothic", 11, FontStyle.Regular);
            pGrid.DefaultCellStyle.Font              = new System.Drawing.Font("Century Gothic", 11);
            pGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Century Gothic", 11);
            pGrid.RowHeadersDefaultCellStyle.Font    = new System.Drawing.Font("Century Gothic", 11);


        }
        //--------------------------------------------------------------------------
        public void DisplayPos(ref System.Windows.Forms.DataGridView motrGrid,  int Idx, int iSingle = 0)
        {
	        String sTemp;
            int iMotrNo = 0;
            if(motrGrid  == null) return;
            for(int i=0; i<motrGrid.RowCount;i++) 
            {
                //iMotrNo  = Convert.ToInt32(motrGrid[0,i].Value.ToString().Substring(1,2));
                int.TryParse(motrGrid[0, i].Value.ToString().Substring(1, 2), out iMotrNo);
                if (iMotrNo<0 || iMotrNo>=(int)EN_MOTR_ID.EndOfId) continue;
                sTemp = string.Format("{0:F4}",cDEF.MOTR[iMotrNo].GetEncPos());
		        motrGrid[1,i].Value = sTemp;
            }
        }
        //--------------------------------------------------------------------------
        public void DisplayMotorStat(ref System.Windows.Forms.DataGridView mGrid, int iSetPart = -1)
        {
            try
            {
                DataTable dt = new DataTable();
                int iTotWidth = 0;
                int[]    iWidth = {30, 0, 80, 80, 60, 50, 50, 50, 50, 50, 50};
                String[] sItem  = {"NO"              ,
                                   "NAME"            ,
                                   "Command\nPos[mm]",
                                   "Encoder\nPos[mm]",
                                   "Torque\n[%]"     ,
                                   "Home\nEnd"       ,
                                   "Servo"           ,
                                   "Alarm"           ,
                                   "Home\nSensor."   ,
                                   "POT\nSensor"     ,
                                   "NOT\nSensor"   };
                                   

                mGrid.Dock = System.Windows.Forms.DockStyle.Left;
                FNC.SetGridStyle(ref mGrid);
                mGrid.ColumnHeadersDefaultCellStyle.Font       = new System.Drawing.Font("Century Gothic", 9);

                for (int i = 0; i < 11; i++) dt.Columns.Add(sItem[i], typeof(string));

                for (int i = 0; i < cDEF.MOTR._iNumOfMotr; i++)
                {
                    if (iSetPart >= 0)
                    {
                        if (iSetPart < 0 || iSetPart >= vDEF.MAX_SEQ_PART) continue;
                        if (iSetPart >= 0 && Dat[iSetPart].m_iPosnCnt[i] < 0) continue;
                    }

                    sItem[0] = Convert.ToString(i);
                    sItem[1] = string.Format("#{0,2:00}-{1}\n[{2}]", i, cDEF.MOTR[i].m_sNameAxis, cDEF.MOTR[i].m_sName);
                    sItem[2] = string.Format("{0:F4}", cDEF.MOTR[i].GetCmdPos());
                    sItem[3] = string.Format("{0:F4}", cDEF.MOTR[i].GetEncPos());

                    if (cDEF.MOTR[i]._iNoUseMotr == 1)
                    {
                        dt.Rows.Add(i, "NO_USE", "", "", "", "", "", "", "", "", "");
                        continue;
                    }

                    if (cDEF.MOTR[i].m_iMotrKind == (int)EN_MOTR_KIND.ABS) sItem[1] += "-ABS";

                    sItem[4] = string.Format("{0:F1}%", (float)cDEF.MOTR[i].GetTorque());
                    sItem[5]  = cDEF.MOTR[i].GetHomeEnd()? "END": "NOT";
                    sItem[6]  = cDEF.MOTR[i].GetServo  ()? "ON" : "OFF";
                    sItem[7]  = cDEF.MOTR[i].GetAlarm  ()? "ON" : "OFF";
                    sItem[8]  = cDEF.MOTR[i].GetHome   ()? "ON" : "OFF";
                    sItem[9]  = cDEF.MOTR[i].GetCW     ()? "ON" : "OFF";
                    sItem[10] = cDEF.MOTR[i].GetCCW    ()? "ON" : "OFF";

                    dt.Rows.Add(sItem);
                }
                //
                mGrid.DataSource = dt;
                //
                for (int i = 0; i < 11; i++)
                {
                    //mGrid.Columns.Add(sItem[i] , sItem[i]);
                    mGrid.Columns[i].Width = iWidth[i];
                    iTotWidth += iWidth[i];
                }
                mGrid.Columns[1].Width = mGrid.Width - iTotWidth - 20;

                //JUNG/220302
                //SetGridFont(ref mGrid);

                //
                for (int n = 0; n < mGrid.ColumnCount; n++)
                {
                    if (n != 1) 
                        mGrid.Columns[n].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    if (n >= 5)
                        mGrid.Columns[n].DefaultCellStyle.Font = new System.Drawing.Font("Century Gothic", 9);
                }
                //
                mGrid.Visible = true;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DisplayMotorStat:" + ex.Message);
            	
            }

        }
        //--------------------------------------------------------------------------
        public void UpdateMotorStat(ref System.Windows.Forms.DataGridView mGrid)
        {
            if (mGrid == null) return; 
            int iMotrNo;

            for (int i=0;i<mGrid.RowCount;i++)
            {
                iMotrNo  = Convert.ToInt32(mGrid[0,i].Value.ToString());
                if (cDEF.MOTR[i]._iNoUseMotr==1) continue;
                mGrid[2 , i].Value = string.Format("{0:F4}"  ,cDEF.MOTR[iMotrNo].GetCmdPos());
                mGrid[3 , i].Value = string.Format("{0:F4}"  ,cDEF.MOTR[iMotrNo].GetEncPos());
                mGrid[4 , i].Value = string.Format("{0:F4}" ,(float)cDEF.MOTR[iMotrNo].GetTorque());
                mGrid[5 , i].Value = cDEF.MOTR[iMotrNo].GetHomeEnd() ? "END": "NOT";
                mGrid[6 , i].Value = cDEF.MOTR[iMotrNo].GetServo  () ? "ON" : "OFF";
                mGrid[7 , i].Value = cDEF.MOTR[iMotrNo].GetAlarm  () ? "ON" : "OFF";
                mGrid[8 , i].Value = cDEF.MOTR[iMotrNo].GetHome   () ? "ON" : "OFF";
                mGrid[9 , i].Value = cDEF.MOTR[iMotrNo].GetCW     () ? "ON" : "OFF";
                mGrid[10, i].Value = cDEF.MOTR[iMotrNo].GetCCW    () ? "ON" : "OFF";

                mGrid[5 , i].Style.BackColor = cDEF.MOTR[iMotrNo].GetHomeEnd() ? Color.Lime  : Color.Gray;
                mGrid[6 , i].Style.BackColor = cDEF.MOTR[iMotrNo].GetServo  () ? Color.Lime  : Color.Gray;
                mGrid[7 , i].Style.BackColor = cDEF.MOTR[iMotrNo].GetAlarm  () ? Color.Red   : Color.Gray;
                mGrid[8 , i].Style.BackColor = cDEF.MOTR[iMotrNo].GetHome   () ? Color.Blue  : Color.Gray;
                mGrid[9 , i].Style.BackColor = cDEF.MOTR[iMotrNo].GetCW     () ? Color.Lime  : Color.Gray;
                mGrid[10, i].Style.BackColor = cDEF.MOTR[iMotrNo].GetCCW    () ? Color.Lime  : Color.Gray;
            }
        }
        //--------------------------------------------------------------------------
        //Update
        public void UpdateSpdByGrid(bool toTable, int iPart, ref System.Windows.Forms.DataGridView mGrid, bool IsComm = false)
        { 
            int    iMotr    ;
            int    iWidth   ;
            String sMotrName;
            TMOTN_PARA tmpMP; 
			DataTable dt = new DataTable();

            String[] sItem   = {"No"                     ,
                                "Name"                   ,
                                "Auto\nSpeed\n[mm/s]"    ,
                                "Manual\nSpeed\n[mm/s]"  ,
                                "Home\nSpeed\n[mm/s]"    ,
                                "Jog High\nSpeed\n[mm/s]",
                                "Jog Low\nSpeed\n[mm/s]" ,
                                "Acc.\nTime\n[ms]"       ,
                                "Dcc.\nTime\n[ms] "      ,
                                "InPos\n[mm]"            ,
                                "Stop\nDelay[ms]"        ,
                                "Spd#1\n[mm/s]"          ,//"User\nSpeed#1\n[mm/s]"  ,
                                "Acc.#1[ms]"             ,//"User\nAcc.#1[ms]"       ,
                                "Spd#2\n[mm/s]"          ,//"User\nSpeed#2\n[mm/s]"  ,
                                "Acc.#2[ms]"             ,//"User\nAcc.#2[ms]"       ,
                                "Spd#3\n[mm/s]"          ,//"User\nSpeed#3\n[mm/s]"  ,
                                "Acc.#3[ms]"             ,//"User\nAcc.#3[ms]"       ,
                                "Spd#4\n[mm/s]"          ,//"User\nSpeed#4\n[mm/s]"  ,
                                "Acc.#4[ms]"             ,//"User\nAcc.#4[ms]"       ,
                                "Spd#5\n[mm/s]"          ,//"User\nSpeed#5\n[mm/s]"  ,
                                "Acc.#5[ms]"             };//"User\nAcc.#5[ms]"       };


            if(mGrid == null) return;
            



            if(toTable)
            { 
				mGrid.Dock = System.Windows.Forms.DockStyle.Fill;
                FNC.SetGridStyle(ref mGrid);
                mGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Century Gothic", 9);
                mGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                mGrid.ColumnHeadersHeight = 50;

                for (int i = 0; i < 21; i++)
                {
                    dt.Columns.Add(sItem[i], typeof(string));
                }

                for(int i=0; i<cDEF.MOTR._iNumOfMotr;i++) 
                {
                    if(iPart >=0)
                    {
                        if(Dat [iPart].m_iPosnCnt[i] < 0) continue;
                    }
                    
                    if (cDEF.MOTR[i]._iNoUseMotr == 1)
                    {
                        for (int k = 0; k < sItem.Length; k++)
                        {
                            sItem[k] = string.Format("-");
                        }
                        sItem[0] = Convert.ToString(i);
                        sItem[1] = "UNUSED";

                        dt.Rows.Add(sItem);

                        continue;
                    }

                    sMotrName = string.Format("#{0,2:00}-{1}\n[{2}]", i, cDEF.MOTR[i].m_sNameAxis, cDEF.MOTR[i].m_sName);
                    tmpMP = (!IsComm) ? cDEF.MOTR[i].MP : cDEF.MOTR[i].CMP;
                    
                    sItem[0 ] = Convert.ToString(i);
                    sItem[1 ] = sMotrName;
                    sItem[2 ] = Convert.ToString(tmpMP.dVel [(int)EN_MOTR_VEL.Work  ]);
                    sItem[3 ] = Convert.ToString(tmpMP.dVel [(int)EN_MOTR_VEL.Dry   ]);
                    sItem[4 ] = Convert.ToString(tmpMP.dVel [(int)EN_MOTR_VEL.Home  ]);
                    sItem[5 ] = Convert.ToString(tmpMP.dVel [(int)EN_MOTR_VEL.HJog  ]);
                    sItem[6 ] = Convert.ToString(tmpMP.dVel [(int)EN_MOTR_VEL.LJog  ]);
                    sItem[7 ] = Convert.ToString(tmpMP.dAcc [(int)EN_MOTR_VEL.Work  ]);
                    sItem[8 ] = Convert.ToString(tmpMP.dDec [(int)EN_MOTR_VEL.Work  ]);
                    sItem[9 ] = Convert.ToString(tmpMP.dPosn[(int)EN_POSN_ID.InPos  ]);
                    sItem[10] = Convert.ToString(tmpMP.dTime[(int)EN_MOTR_DELAY.Stop]);
                    sItem[11] = Convert.ToString(tmpMP.dVel [(int)EN_MOTR_VEL.User1 ]);
                    sItem[12] = Convert.ToString(tmpMP.dAcc [(int)EN_MOTR_VEL.User1 ]);
                    sItem[13] = Convert.ToString(tmpMP.dVel [(int)EN_MOTR_VEL.User2 ]);
                    sItem[14] = Convert.ToString(tmpMP.dAcc [(int)EN_MOTR_VEL.User2 ]);
                    sItem[15] = Convert.ToString(tmpMP.dVel [(int)EN_MOTR_VEL.User3 ]);
                    sItem[16] = Convert.ToString(tmpMP.dAcc [(int)EN_MOTR_VEL.User3 ]);
                    sItem[17] = Convert.ToString(tmpMP.dVel [(int)EN_MOTR_VEL.User4 ]);
                    sItem[18] = Convert.ToString(tmpMP.dAcc [(int)EN_MOTR_VEL.User4 ]);
                    sItem[19] = Convert.ToString(tmpMP.dVel [(int)EN_MOTR_VEL.User5 ]);
                    sItem[20] = Convert.ToString(tmpMP.dAcc [(int)EN_MOTR_VEL.User5 ]);

                    dt.Rows.Add(sItem);
                }
				//
				mGrid.DataSource = dt;
				
				//
                mGrid.Columns[0].ReadOnly = true;
                mGrid.Columns[1].ReadOnly = true;

                for (int i = 0; i < 21; i++)
                {
                    //mGrid.Columns.Add(sItem[i], sItem[i]);
                    if (i == 0) iWidth = 30;
                    else if (i == 1) iWidth = mGrid.Width - 30 - (80 * 10);
                    else iWidth = 80;

                    mGrid.Columns[i].Width = iWidth;
                }
                //
                for (int n = 0; n < mGrid.ColumnCount; n++)
                {
                    if (n != 1)
                        mGrid.Columns[n].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            else 
            {
                for(int i=0; i<mGrid.RowCount ;i++) 
                {
                    sMotrName  = mGrid[0,i].Value.ToString();
                    iMotr      = Convert.ToInt32(sMotrName);
                    if(iMotr < 0 || iMotr>=cDEF.MOTR._iNumOfMotr) continue;
                    if(sMotrName == ""                          ) continue;

                    if(IsComm) sMotrName = string.Format("{0,2:00}_Common_", i);
                    else       sMotrName = string.Format("{0,2:00}_"       , i);

                    tmpMP = (!IsComm) ? cDEF.MOTR[iMotr].MP : cDEF.MOTR[iMotr].CMP;

                    WriteDatChLog(2, ref tmpMP.dVel [(int)EN_MOTR_VEL.  Work ] ,mGrid[2 , i].Value , sMotrName + sItem[2 ]);
                    WriteDatChLog(2, ref tmpMP.dVel [(int)EN_MOTR_VEL.  Dry  ], mGrid[3 , i].Value , sMotrName + sItem[3 ]);
                    WriteDatChLog(2, ref tmpMP.dVel [(int)EN_MOTR_VEL.  Home ], mGrid[4 , i].Value , sMotrName + sItem[4 ]);
                    WriteDatChLog(2, ref tmpMP.dVel [(int)EN_MOTR_VEL.  HJog ], mGrid[5 , i].Value , sMotrName + sItem[5 ]);
                    WriteDatChLog(2, ref tmpMP.dVel [(int)EN_MOTR_VEL.  LJog ], mGrid[6 , i].Value , sMotrName + sItem[6 ]);
                    WriteDatChLog(2, ref tmpMP.dAcc [(int)EN_MOTR_VEL.  Work ], mGrid[7 , i].Value , sMotrName + sItem[7 ]);
                    WriteDatChLog(2, ref tmpMP.dDec [(int)EN_MOTR_VEL.  Work ], mGrid[8 , i].Value , sMotrName + sItem[8 ]);
                    WriteDatChLog(2, ref tmpMP.dPosn[(int)EN_POSN_ID .  InPos], mGrid[9 , i].Value , sMotrName + sItem[9 ]);
                    WriteDatChLog(2, ref tmpMP.dTime[(int)EN_MOTR_DELAY.Stop ], mGrid[10, i].Value , sMotrName + sItem[10]);
                    WriteDatChLog(2, ref tmpMP.dVel [(int)EN_MOTR_VEL.  User1], mGrid[11, i].Value , sMotrName + sItem[11]);
                    WriteDatChLog(2, ref tmpMP.dAcc [(int)EN_MOTR_VEL.  User1], mGrid[12, i].Value , sMotrName + sItem[12]);
                    WriteDatChLog(2, ref tmpMP.dVel [(int)EN_MOTR_VEL.  User2], mGrid[13, i].Value , sMotrName + sItem[13]);
                    WriteDatChLog(2, ref tmpMP.dAcc [(int)EN_MOTR_VEL.  User2], mGrid[14, i].Value , sMotrName + sItem[14]);
                    WriteDatChLog(2, ref tmpMP.dVel [(int)EN_MOTR_VEL.  User3], mGrid[15, i].Value , sMotrName + sItem[15]);
                    WriteDatChLog(2, ref tmpMP.dAcc [(int)EN_MOTR_VEL.  User3], mGrid[16, i].Value , sMotrName + sItem[16]);
                    WriteDatChLog(2, ref tmpMP.dVel [(int)EN_MOTR_VEL.  User4], mGrid[17, i].Value , sMotrName + sItem[17]);
                    WriteDatChLog(2, ref tmpMP.dAcc [(int)EN_MOTR_VEL.  User4], mGrid[18, i].Value , sMotrName + sItem[18]);
                    WriteDatChLog(2, ref tmpMP.dVel [(int)EN_MOTR_VEL.  User5], mGrid[19, i].Value , sMotrName + sItem[19]);
                    WriteDatChLog(2, ref tmpMP.dAcc [(int)EN_MOTR_VEL.  User5], mGrid[20, i].Value , sMotrName + sItem[20]);
                                                                                                                    
                    if(IsComm) cDEF.MOTR[iMotr].CMP  = tmpMP;
                    else       cDEF.MOTR[iMotr].MP   = tmpMP;

                }

            }

            //SetGridFont(ref mGrid);

            mGrid.Visible  = true;
        }
        //--------------------------------------------------------------------------
        //Set Data
        public void Init()
        {
            for (int i = 0; i < vDEF.MAX_SEQ_PART; i++)
            {
                Dat[i] = new TSetPart();
            }
            for (int i = 0; i < vDEF.MAX_SEQ_PART; i++)
            {
                Dat[i].m_sName   = "";
                Dat[i].m_iItemCnt = 0;
                for (int j = 0; j < (int)EN_MOTR_ID.EndOfId; j++) Dat[i].m_iPosnCnt[j] = -1;
            }
        }

        //--------------------------------------------------------------------------
        public void Set(EN_SEQ_ID iSeqId, string sPart, string sName, string sUnit,
                                        EN_POSN_ID PosnId, int sDigit = 0, EN_POS_ID iPosnKind = EN_POS_ID.NORM, EN_MOTR_ID iMotorId = EN_MOTR_ID.None, int iManNo = -1, bool bHomeOffset = false)
        {
            int iSeqPart =(int)iSeqId  ;
            int iMotor   =(int)iMotorId;
            int cRow     = Dat[iSeqPart].m_iItemCnt;
            String sDesc;

            if(iSeqPart<0 || iSeqPart>=vDEF.MAX_SEQ_PART) iSeqPart = 0;
            if(cRow    <0 || cRow    >=vDEF.MAXITEM     ) cRow     = 0;
            Dat [iSeqPart].m_sName = sPart;
            m_iPartCnt = iSeqPart;

            //if(sName == "" ) return;
            if(iPosnKind == (int)EN_POS_ID.NONE )
            {
                if(iMotor>=0 && iMotor<cDEF.MOTR._iNumOfMotr)
                {
                    Dat [iSeqPart].m_iPosnCnt[iMotor] +=1;
                    cDEF.MOTR[iMotor].SetPart(iSeqPart);
                }
                int MtrCnt = 0;
                for(int i=0;i<cDEF.MOTR._iNumOfMotr;i++) 
                {
                    if (iMotorId < 0) continue;
                    if(Dat[iSeqPart].m_iPosnCnt[iMotor]>=0) MtrCnt ++;
                }
                Dat [iSeqPart].m_iMotorCnt = MtrCnt;
                Dat [iSeqPart].Set[cRow].m_iMotor = iMotor;
				Dat[iSeqPart].m_iItemCnt ++;
                return;
            }

            //
            Dat [iSeqPart].Set[cRow].m_sName        = sName         ;
            Dat [iSeqPart].Set[cRow].m_sUnit        = sUnit         ;
            Dat [iSeqPart].Set[cRow].m_iDigit       = sDigit        ;
            Dat [iSeqPart].Set[cRow].m_iMotor       = iMotor        ;
            Dat [iSeqPart].Set[cRow].m_iPosnKind    = (int)iPosnKind;
            Dat [iSeqPart].Set[cRow].m_iPosnId      = (int)PosnId   ;
            Dat [iSeqPart].Set[cRow].m_bHomeOffset  = bHomeOffset   ;
            Dat [iSeqPart].Set[cRow].m_bDefUserMan  = false         ;

            cDEF.MOTR[iMotor].MP.iPosnKind[(int)PosnId] = (int)iPosnKind;  

            MinMaxLoad(true, iSeqPart, cRow);
            if(Dat[iSeqPart].Set[cRow]._Val < Dat[iSeqPart].Set[cRow].m_dMin) Dat[iSeqPart].Set[cRow]._Val = Dat[iSeqPart].Set[cRow].m_dMin;
            if(Dat[iSeqPart].Set[cRow]._Val > Dat[iSeqPart].Set[cRow].m_dMax) Dat[iSeqPart].Set[cRow]._Val = Dat[iSeqPart].Set[cRow].m_dMax;

            Dat[iSeqPart].m_iItemCnt ++;

            //
            if(iMotor<0                     ) return;
            if(iMotor>=cDEF.MOTR._iNumOfMotr) return;

            Dat [iSeqPart].m_iPosnCnt[iMotor] +=1;
                           
            //if (iManNo<0)  Dat [iSeqPart].Set[cRow].m_iManNo = cDEF.MOTR[iMotor].m_iManHome + Dat[iSeqPart].m_iPosnCnt[iMotor] + 1;
            //else          {Dat [iSeqPart].Set[cRow].m_iManNo = iManNo; Dat [iSeqPart].Set[cRow].m_bDefUserMan = true;       }

            //
            Dat [iSeqPart].Set[cRow].m_iManNo  = cDEF.MOTR[iMotor].m_iManHome + Dat[iSeqPart].m_iPosnCnt[iMotor] + 1;

            cDEF.MOTR[iMotor].SetPart(iSeqPart);
            Dat [iSeqPart].m_iMotorCnt ++;


            sDesc = string.Format("{0}[{1}]{2}_{3}", cDEF.MOTR[iMotor].m_sName      ,
                                                     cDEF.MOTR[iMotor].m_sNameAxis  ,
                                                     GetPartName(iSeqPart)            ,
                                                     Dat[iSeqPart].Set[cRow].m_sName  );
            cDEF.MOTR[iMotor].MP.sPosn_Desc[(int)PosnId] = sDesc;
        }
        //--------------------------------------------------------------------------
        public TSetItem Get(ref int iPart, ref int iIndex)
        {
            GetPartIndex(ref iPart, ref iIndex, m_iSelMotor);

            if(iPart <0 || iPart >=vDEF.MAX_SEQ_PART) iPart  = 0;
            if(iIndex<0 || iIndex>=vDEF.MAXITEM) iIndex = 0;

            return Dat [iPart].Set[iIndex];
        }
        //--------------------------------------------------------------------------
        public void SetMotor(EN_MOTR_ID iMotrId, string sName, string sAxis, string sImgP, string sImgN, 
                                                 int iErrNo = -1, int iManNo = -1, int iPartHomeNo = -1)
        {
            int iMotr = Convert.ToInt32(iMotrId);

            if(iMotr<0 || iMotr>=cDEF.MOTR._iNumOfMotr) return; //iMotr = 0;

            cDEF.MOTR[iMotr].m_sName       = sName   ;
            cDEF.MOTR[iMotr].m_sNameAxis   = sAxis   + " AXIS";
            cDEF.MOTR[iMotr].m_sImgP       = sImgP   ;
            cDEF.MOTR[iMotr].m_sImgN       = sImgN   ;

            //Define Manual No
            cDEF.MOTR[iMotr].m_iManStop     = iManNo  + (25 * iMotr);
            cDEF.MOTR[iMotr].m_iManJog      = iManNo  + (25 * iMotr)+ 1;
            cDEF.MOTR[iMotr].m_iManPitch    = iManNo  + (25 * iMotr)+ 2;
            cDEF.MOTR[iMotr].m_iManServo    = iManNo  + (25 * iMotr)+ 3;
            cDEF.MOTR[iMotr].m_iManAlarm    = iManNo  + (25 * iMotr)+ 4;
            cDEF.MOTR[iMotr].m_iManDirect   = iManNo  + (25 * iMotr)+ 5;
            cDEF.MOTR[iMotr].m_iManHome     = iManNo  + (25 * iMotr)+ 6;
            cDEF.MOTR[iMotr].m_iManPartHome = iPartHomeNo;

            //Define Error  No
            cDEF.MOTR[iMotr].m_iErrAlarm    = iErrNo  + (10 * iMotr);
            cDEF.MOTR[iMotr].m_iErrCW       = iErrNo  + (10 * iMotr)+ 1;
            cDEF.MOTR[iMotr].m_iErrCCW      = iErrNo  + (10 * iMotr)+ 2;
            cDEF.MOTR[iMotr].m_iErrHome     = iErrNo  + (10 * iMotr)+ 3;
            cDEF.MOTR[iMotr].m_iErrControl  = iErrNo  + (10 * iMotr)+ 4;
            cDEF.MOTR[iMotr].m_iErrHold     = iErrNo  + (10 * iMotr)+ 5;
            cDEF.MOTR[iMotr].m_iErrPos      = iErrNo  + (10 * iMotr)+ 6;
            cDEF.MOTR[iMotr].m_iErrVel      = iErrNo  + (10 * iMotr)+ 7;
            cDEF.MOTR[iMotr].m_iErrAcc      = iErrNo  + (10 * iMotr)+ 8;

            //
            m_iLManNo = iManNo  + (20 * iMotr) + 20 + 100;
            m_iLErrNo = iErrNo  + (10 * iMotr) + 10 + 100;
        }
        //--------------------------------------------------------------------------
        public void SetDevice(string sDevice)
        {
            m_sCrntDvcName = sDevice;
        }
        //--------------------------------------------------------------------------                                
        public void UpdateCheck()
        {
            String sTemp;
            int iPart  = m_iSelPart   ;

            if(itemGrid             == null) return;
            if(itemGrid.CurrentCell == null) return;

            int iIndex = itemGrid.CurrentCell.RowIndex;

            GetPartIndex(ref iPart, ref iIndex,m_iSelMotor);
            if(iPart <0 || iPart >=vDEF.MAX_SEQ_PART) return;
            if(iIndex<0 || iIndex>=vDEF.MAXITEM) return;

            double sVal = Convert.ToDouble(itemGrid[1,iIndex].Value);
            double sMin = Dat[iPart].Set[iIndex].m_dMin;
            double sMax = Dat[iPart].Set[iIndex].m_dMax;

            if(sVal>=sMin && sVal<=sMax) 
            {
               itemGrid[1,iIndex].Value = Dat[iPart].Set[iIndex].tFormat(sVal);
            }
            else 
            {
               sTemp = string.Format("Input Between  {0:F4} ~  {1:F4} ",sMin, sMax);
               MsgBox.Error(sTemp);
               itemGrid[1,iIndex].Value = Dat[iPart].Set[iIndex]._Text;
            }
        }
        //--------------------------------------------------------------------------
        public void ShowKeyPad(bool bChMinMax = false)
        {    
            String sTemp;
            int iPart  = m_iSelPart   ;

            if(itemGrid             == null) return;
            if(itemGrid.CurrentCell == null) return;

            int iGridCol   = itemGrid.CurrentCell.ColumnIndex;
            int iGridRow   = itemGrid.CurrentCell.RowIndex;
            int iIndex     = iGridRow;

            GetPartIndex(ref iPart, ref iIndex,m_iSelMotor);
            if (iPart <0 || iPart >=vDEF.MAX_SEQ_PART                    ) return;
            if (iIndex<0 || iIndex>=vDEF.MAXITEM                         ) return;
            if (Dat[iPart].Set[iIndex].m_iPosnKind == (int)EN_POS_ID.VIEW) return;
            if (Dat[iPart].Set[iIndex].m_iPosnKind == (int)EN_POS_ID.PARA) return;

            sTemp = string.Format(" {0} - {1} Input", Dat[iPart].m_sName, Dat[iPart].Set[iIndex].m_sName);

            int nMotr      = Dat[iPart].Set[iIndex].m_iMotor;
            FrmInputPos frmInputPos = new FrmInputPos();

            frmInputPos.m_sTitle      = sTemp;
            frmInputPos.m_dValue      = Dat[iPart].Set[iIndex]._Val         ;
            frmInputPos.m_iDigit      = Dat[iPart].Set[iIndex].m_iDigit     ;
            frmInputPos.m_dMaxVal     = cDEF.MOTR[nMotr].m_dMaxPosn         ; //Dat[iPart].Set[iIndex].m_dMax  ;
            frmInputPos.m_dMinVal     = cDEF.MOTR[nMotr].m_dMinPosn         ; //Dat[iPart].Set[iIndex].m_dMin  ;
            frmInputPos.m_iMotor      = Dat[iPart].Set[iIndex].m_iMotor     ;
            frmInputPos.m_bHomeOffset = Dat[iPart].Set[iIndex].m_bHomeOffset;
            frmInputPos.Left          = 180;
            frmInputPos.Top           = 150;

            if (frmInputPos.ShowDialog() == DialogResult.Yes) 
            {
               Dat[iPart].Set[iIndex]._Val = frmInputPos.m_dValue; 
               itemGrid[iGridCol,iGridRow].Value = Dat[iPart].Set[iIndex]._Text;
               itemGrid.Rows[iGridRow].ReadOnly = false;
            }

           frmInputPos = null;
        }
        //--------------------------------------------------------------------------
        public void MinMaxLoad(bool sLoad, int sPart, int sIndex)
        {
        }
        //--------------------------------------------------------------------------
        public void WriteDatChLog(int wKind, ref string refData, object newData, object objLog)
        {
            String Temp   = "";
            String StrLog = "";
            String sData  = ""; //newData.ToString();
            String sMsg   = objLog .ToString();

            if (newData == null) sData = ""; else sData = newData.ToString();

            bool IsWriteLog = false;
            if(sData != refData) {
               if(wKind == 0 ) Temp = "Common Position Change ";
               if(wKind == 1 ) Temp = m_sCrntDvcName + " Position Change"    ;
               if(wKind == 2 ) Temp = m_sCrntDvcName + " Velocity Change"    ;
               if(wKind == 3 ) sMsg = m_sCrntDvcName + " Project DATA Change";
               if(wKind == 4 ) Temp = m_sCrntDvcName + " Setting DATA Change";
               if(wKind == 5 ) Temp = m_sCrntDvcName + " DSTB Change"        ;

               StrLog = string.Format("{0}-{1}[{2} -> {3}]", Temp, sMsg, refData, sData);
               IsWriteLog = true;
               }
            if(IsWriteLog) WriteLog(StrLog);
            refData = sData;

        }
        //--------------------------------------------------------------------------
        public void WriteDatChLog(int wKind, ref double refData, object objData, object objLog)
        {
            String Temp   = "";
            String StrLog = "";
            double dData  = 0;
            String sData  = objData.ToString();
            String sMsg   = objLog .ToString();

            try
            {
                if(!double.TryParse(sData, out dData)) return;
            }
            catch (Exception err) { System.Diagnostics.Debug.WriteLine("Exception:" + err.Message); }
            
            bool IsWriteLog = false;
            if(dData != refData) {
               if(wKind == 0 ) Temp = "Common Position Change ";
               if(wKind == 1 ) Temp = m_sCrntDvcName + " Position Change"    ;
               if(wKind == 2 ) Temp = m_sCrntDvcName + " Velocity Change"    ;
               if(wKind == 3 ) Temp = m_sCrntDvcName + " Project DATA Change";
               if(wKind == 4 ) Temp = m_sCrntDvcName + " Setting DATA Change";
               if(wKind == 5 ) Temp = m_sCrntDvcName + " DSTB Change"        ;

               StrLog = string.Format("{0}-{1}[{2} -> {3}]", Temp, sMsg, refData, dData);
               IsWriteLog = true;
               }
            if(IsWriteLog) WriteLog(StrLog);
            refData = dData;

        }
        //--------------------------------------------------------------------------
        public void WriteDatChLog(int wKind, ref int refData, object objData, object objLog)
        {
            String Temp   = "";
            String StrLog = "";
            int    iData  = 0;
            bool IsWriteLog = false;
            String sData = objData.ToString().Trim();
            String sMsg  = objLog .ToString().Trim();

            try
            {
                if(!int.TryParse(sData, out iData)) return;
            }
            catch (Exception err) { System.Diagnostics.Debug.WriteLine("Exception:" + err.Message); }

            if(iData != refData) 
            {
               if(wKind == 0 ) Temp = "Common Position Change ";
               if(wKind == 1 ) Temp = m_sCrntDvcName + " Position Change"    ;
               if(wKind == 2 ) Temp = m_sCrntDvcName + " Velocity Change"    ;
               if(wKind == 3 ) Temp = m_sCrntDvcName + " Project DATA Change";
               if(wKind == 4 ) Temp = m_sCrntDvcName + " Setting DATA Change";
               if(wKind == 5 ) Temp = m_sCrntDvcName + " DSTB Change"        ;

               StrLog = string.Format("{0}-{1}[{2} -> {3}]", Temp, sMsg, refData, iData);
               IsWriteLog = true;
            }

            if(IsWriteLog) WriteLog(StrLog);
            
            refData = iData;
        }
        //--------------------------------------------------------------------------
        public void WriteDatChLog(int wKind, ref bool refData, object objData, object objLog)
        {
            String Temp   = "";
            String StrLog = "";
            bool   bData;
            bool  IsWriteLog = false;
			
			//if (objData == null)  return;
			//if (objLog  == null)  return;

            String sData = objData.ToString();
            String sMsg  = objLog .ToString();

            if(!bool.TryParse(sData, out bData)) 
            {
                int Val;
                if (!int.TryParse(sData, out Val)) return;
                bData = Convert.ToBoolean(Val);
            }

            if(bData != refData) {
               if(wKind == 0 ) Temp = "Common Position Change ";
               if(wKind == 1 ) Temp = m_sCrntDvcName + " Position Change"    ;
               if(wKind == 2 ) Temp = m_sCrntDvcName + " Velocity Change"    ;
               if(wKind == 3 ) Temp = m_sCrntDvcName + " Project DATA Change";
               if(wKind == 4 ) Temp = m_sCrntDvcName + " Setting DATA Change";
               if(wKind == 5 ) Temp = m_sCrntDvcName + " DSTB Change"        ;

               StrLog = string.Format("{0}-{1}[{2} -> {3}]", Temp, sMsg, refData, bData);
               IsWriteLog = true;
               }
            if(IsWriteLog) WriteLog(StrLog);
            refData = bData;
        }
        //--------------------------------------------------------------------------
        public void WriteLog(string sMsg)
        {
            String sPath = Application.StartupPath + "\\LOG\\DataChangeLog\\";
            FNC.CreateDir(sPath);

            String FileName = sPath + string.Format("{0:yyMMdd}", DateTime.Now)+ ".Log";
            //File Open.
            FileStream fp = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write); 
            
            StreamWriter sw = new StreamWriter(fp, Encoding.Unicode);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sMsg = string.Format("[{0:yy/MM/dd:  HH:mm:ss}]", DateTime.Now) + sMsg + "\n\n";
            sw.Write(sMsg);
            sw.Flush();
            sw.Close();
        }
        //--------------------------------------------------------------------------
        public int GetManNo()
        {
            int iPart  = m_iSelPart   ;

            if(itemGrid.CurrentCell == null) return 0;

            int iCol   = itemGrid.CurrentCell.ColumnIndex;
            int iIndex = itemGrid.CurrentCell.RowIndex;

            GetPartIndex(ref iPart, ref iIndex,m_iSelMotor);
            if(iPart <0 || iPart >=vDEF.MAX_SEQ_PART) return 0;
            if(iIndex<0 || iIndex>=vDEF.MAXITEM) return 0;

            return Dat[iPart].Set[iIndex].m_iManNo;
        }
        //--------------------------------------------------------------------------
	    //GetPart
        public string GetPartName(int iPart, bool IsNor = true)
        {
            String pName;
            if(iPart == GetPartCnt()         ) return "SYSTEM";
            if(iPart<0 || iPart>=GetPartCnt()) return "ALL"   ;
            String Item = Dat[iPart].m_sName;

            if(!IsNor) return Item;

            int iPos = Item.IndexOf("\n");
            if(iPos > 0) 
            {
               String Item1 = Item.Substring(1, iPos - 1);
               String Item2 = Item.Substring(iPos+2) ;
               pName = Item1 + " " + Item2;
            }
            else pName = Item;

            return pName;
        }
        //--------------------------------------------------------------------------
        public int GetPartCnt()
        {
            return m_iPartCnt+1;
        }
        //--------------------------------------------------------------------------
        public bool GetMotorPart(ref int iPart, ref int iIndex, int iMotorNo)
        {
            for(int i=0; i<vDEF.MAX_SEQ_PART; i++) {
                for(int j=0; j<Dat[i].m_iItemCnt; j++) {
                    if(j>=vDEF.MAXITEM) continue;
                    if(iMotorNo == Dat[i].Set[j].m_iMotor) {
                        iPart  = i;
                        iIndex = j;
                        return true;
                    }
                }
            }
            return false;
        }
        //--------------------------------------------------------------------------
        public bool GetPosnPart(ref int iPart, ref int iIndex, int iMotorNo, int iPosnId)
        {
            for(int i=0; i<vDEF.MAX_SEQ_PART; i++) {
                for(int j=0; j<Dat[i].m_iItemCnt; j++) {
                    if(j>=vDEF.MAXITEM) continue;
                    if(iMotorNo == Dat[i].Set[j].m_iMotor && iPosnId == Dat[i].Set[j].m_iPosnId) {
                        iPart  = i;
                        iIndex = j;
                        return true;
                    }
                }
            }
            return false;
        }
        //--------------------------------------------------------------------------
        public void GetPartIndex(ref int Part, ref int Index, int iMotorNo)
        {
            int i          ;
            int ItemCnt = 0;
            int iPart   = Part ;
            int iitm    = Index;

            if(iPart<0 || iPart>=vDEF.MAX_SEQ_PART) return;
            if(iMotorNo>=0)
            {
                for(i=0; i<Dat[iPart].m_iItemCnt; i++) {
                    if(i>=vDEF.MAXITEM) continue;
                    if(Dat[iPart].Set[i].m_iMotor == iMotorNo) {
                        if(Index == ItemCnt) {iitm = i; break; }
                        ItemCnt ++;
                        }
                    }
            }
            if(iPart <0 || iPart >=vDEF.MAX_SEQ_PART) iPart  = 0;
            if(iitm  <0 || iitm  >=vDEF.MAXITEM) iitm   = 0;

            Part  = iPart;
            Index = iitm ;
        }
        //--------------------------------------------------------------------------
        public bool GetPosnByManNo(int iMotr, int iManNo, out double dPosn)
        {
            int iPosnId;
            dPosn = 0.0;
            for(int i=0; i<vDEF.MAX_SEQ_PART; i++) {
                for(int j=0; j<Dat[i].m_iItemCnt; j++) {
                    if(j>=vDEF.MAXITEM) continue;
                    if(iMotr == Dat[i].Set[j].m_iMotor && iManNo == Dat[i].Set[j].m_iManNo) {

                        if(Dat [i].Set[j].m_bDefUserMan) return false; 

                        iPosnId = Dat [i].Set[j].m_iPosnId;

                        if(iPosnId<0 || iPosnId>=vDEF.MAX_POSN) continue;
                        if(Dat[i].Set[j].m_iPosnKind == (int)EN_POS_ID.COMM) dPosn   = cDEF.MOTR[iMotr].CMP.dPosn[iPosnId];
                        else                                                 dPosn   = cDEF.MOTR[iMotr].MP .dPosn[iPosnId];
                        return true;
                    }
                }
            }

            return false;
        }
        //--------------------------------------------------------------------------
        //Motor Control
        public string GetMotorName(int iMotr)
        {
            String sName;
            if (iMotr < 0 || iMotr >= cDEF.MOTR._iNumOfMotr) return "";
            sName = string.Format("#{0,2:00}-{1} [{2}]", iMotr, cDEF.MOTR[iMotr].m_sNameAxis, cDEF.MOTR[iMotr].m_sName);
            return sName;
        }

    }
}

