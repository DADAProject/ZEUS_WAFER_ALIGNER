using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Calendar
{
    public partial class cFrmCalendar : Form
    {

        private class cDate
        {
            public RectangleF   Bounds      { get; set; }
            public DateTime     Value       { get; set; }
            public bool         IsSelected  { get; set; }
            public bool         IsMouseOver { get; set; }
            
            public void DrawDayString(Graphics pG, Font pFont, Brush pBrush, StringFormat pSf)
            {
                pG.DrawString(Value.Day.ToString(), pFont, pBrush, Bounds, pSf);

                if(IsSelected)
                {
                    using(Pen p = new Pen(Color.DodgerBlue, 5))
                    {
                        pG.DrawRectangle(p, Rectangle.Ceiling(Bounds));
                    }
                }
            }
            public void DrawMonthString(Graphics pG, Font pFont, Brush pBrush, StringFormat pSf)
            {
                pG.DrawString(Value.Month.ToString(), pFont, pBrush, Bounds, pSf);

                if (IsSelected)
                {
                    using (Pen p = new Pen(Color.DodgerBlue, 5))
                    {
                        pG.DrawRectangle(p, Rectangle.Ceiling(Bounds));
                    }
                }
            }
        }

        private enum eScreenMode {Day, Month, Year };
        public event EventHandler<DateTime> DateChangedEvent;


        private readonly cDate[]        mDateArray  = new cDate[7 * 6];
        private readonly cDate[]        mMonthArray = new cDate[5 * 4];
        private readonly StringFormat   mStringFormat;

        private readonly string[] mWeekNames         = new string[]{"일","월","화","수","목","금","토"};
        private readonly Brush    mDefaultBrush      = Brushes.White;
        private readonly Brush    mAnotherMonthBrush = Brushes.Gray;

        private eScreenMode mScreenMode    = eScreenMode.Day;
        private RectangleF  mWeekDrawBound = new RectangleF(0, 45, 450, 45);

        private int mSelectedYear;
        private int mSelectedMonth;

        public DateTime SelectedDate { get; private set; }

        public cFrmCalendar()
        {
            mStringFormat = new StringFormat
            {
                Alignment     = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            for (int i = 0; i < mDateArray.Length; i++) mDateArray[i] = new cDate();
            for (int i = 0; i < mMonthArray.Length; i++) mMonthArray[i] = new cDate();

            InitializeComponent();

            SelecteDay(DateTime.Now.Year, DateTime.Now.Month);
        }

        private bool DateEquals(DateTime pDt1, DateTime pDt2)
        {
            return pDt1.Year == pDt2.Year && pDt1.Month == pDt2.Month && pDt1.Day == pDt2.Day;

        }

        private void UpdateDays(int pYear, int pMonth)
        {
            if(pMonth >= 13)
            {
                pYear++;
                pMonth = 1;
            }
            else if(pMonth <= 0)
            {
                pYear--;
                pMonth = 12;
            }

            


            if (mScreenMode == eScreenMode.Day)
            {
                if (pYear == mSelectedYear && pMonth == mSelectedMonth) return;
                mSelectedYear  = pYear;
                mSelectedMonth = pMonth;

                DateTime    firstDay   = new DateTime(pYear, pMonth, 1);
                DayOfWeek   week       = firstDay.DayOfWeek;
                int         weekNumber = (int)firstDay.DayOfWeek;

                DateTime thisDays = firstDay;
                DateTime oldDays  = firstDay;

                for(int i = weekNumber -1; i >= 0; i--)
                {
                   oldDays =oldDays.Subtract(new TimeSpan(1,0,0,0));
                   mDateArray[i].Value = oldDays;
                }

                for(int i = weekNumber; i < mDateArray.Length; i++)
                {
                    mDateArray[i].Value = thisDays;
                    thisDays = thisDays.AddDays(1);
                }

                lbYear.Text = $"{mSelectedYear:0000}년 {mSelectedMonth:00}월";

                foreach(cDate date in mDateArray) date.IsSelected = false;
                cDate selectDay = mDateArray.FirstOrDefault(p => DateEquals(p.Value, SelectedDate));
                if (selectDay != null)
                {
                    selectDay.IsSelected = true;
                }
            }
            else
            {
                mSelectedYear = pYear;
                mSelectedMonth = pMonth;

                for (int i = 0; i < 4; i++)
                {
                    mMonthArray[i].Value = new DateTime(pYear - 1, 9 + i, 1);
                }
                for (int i = 0; i < 12; i++)
                {
                    mMonthArray[i + 4].Value = new DateTime(pYear , i + 1, 1);
                }
                for (int i = 0; i < 4; i++)
                {
                    mMonthArray[i + 4 + 12].Value = new DateTime(pYear + 1 , i + 1, 1);
                }

                lbYear.Text = $"{mSelectedYear:0000}년";
            }

            Invalidate();
        }

        public void SelecteDay(int pYear, int pMonth, int pDay = -1)
        {
            UpdateDays(pYear, pMonth);

            if (pDay > 0)
            {
                cDate selectedDay = mDateArray.FirstOrDefault(p => p.Value.Year == pYear && p.Value.Month == pMonth && p.Value.Day == pDay);
                if (selectedDay != null)
                {
                    selectedDay.IsSelected = true;
                    if(DateEquals(selectedDay.Value, SelectedDate) == false)
                    {
                        SelectedDate = selectedDay.Value;
                        DateChangedEvent?.Invoke(this,SelectedDate);
                    }
                    
                    Invalidate();
                }
            }
        }

        #region # EVENTS #

        protected override void OnResize(EventArgs e)
        {
            int index = 0;
            
            float margin = 10;
            float boundWidth  = (Width  - margin)  / 7F;
            float boundHeight = (Height - margin - 45) / 7F;

            mWeekDrawBound = new RectangleF(0, 45, Width, boundHeight);

            for (int y = 0; y < 6; y++)
            {
                float yPt = y * boundHeight + (margin /2) + mWeekDrawBound.Bottom;;
                for (int x = 0; x < 7; x++)
                {
                    float xPt = x * boundWidth + (margin /2);
                    mDateArray[index++].Bounds = new RectangleF(xPt, yPt, boundWidth, boundHeight);
                }
            }

            
            index = 0;
            boundWidth  = (Width  - margin)  / 4F;
            boundHeight = (Height - margin - 45) / 5F;
            for (int y = 0; y < 5; y++)
            {
                float yPt = y * boundHeight + (margin /2) + 45;
                for (int x = 0; x < 4; x++)
                {
                    float xPt = x * boundWidth + (margin /2);
                    mMonthArray[index++].Bounds = new RectangleF(xPt, yPt, boundWidth, boundHeight);
                }
            }

            base.OnResize(e);
        }

        private void PaintEvent(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if(mScreenMode == eScreenMode.Day)
            {
                float margin = 10;
                float boundWidth = (Width - margin) / 7F;
                for (int x = 0; x < 7; x++)
                {
                    float xPt = x * boundWidth + (margin / 2);
                    e.Graphics.DrawString(mWeekNames[x], Font, mDefaultBrush, new RectangleF(xPt, mWeekDrawBound.Y, boundWidth, mWeekDrawBound.Height), mStringFormat);
                }

                for (int i = 0; i < mDateArray.Length; i++)
                {
                    Brush b;
                    if (mDateArray[i].IsMouseOver)
                    {
                        b = Brushes.DodgerBlue;
                    }
                    else
                    {
                        b = mDateArray[i].Value.Year == mSelectedYear && mDateArray[i].Value.Month == mSelectedMonth ? mDefaultBrush : mAnotherMonthBrush;
                    }

                    mDateArray[i].DrawDayString(e.Graphics, Font, b, mStringFormat);
                }
            }
            else
            {
                for (int i = 0; i < mMonthArray.Length; i++)
                {
                    Brush b;
                    if (mMonthArray[i].IsMouseOver)
                    {
                        b = Brushes.DodgerBlue;
                    }
                    else
                    {
                        b = mMonthArray[i].Value.Year == mSelectedYear ? mDefaultBrush : mAnotherMonthBrush;
                    }
                    mMonthArray[i].DrawMonthString(e.Graphics, Font, b, mStringFormat);
                }
            }

        }

        private void MouseMoveEvent(object sender, MouseEventArgs e)
        {
            cDate[] dateArray = mScreenMode == eScreenMode.Day? mDateArray: mMonthArray;

            foreach (cDate day in dateArray)
            {
                bool isMouseOver = new Region(day.Bounds).IsVisible(e.Location);
                bool needDraw = day.IsMouseOver != isMouseOver;
                day.IsMouseOver = isMouseOver;
                if (needDraw) Invalidate();
                    
            }

        }

        private void MouseDownEvent(object sender, MouseEventArgs e)
        {
            if(mScreenMode == eScreenMode.Day)
            {
                bool needDraw = false;

                cDate selectedDate = null;
            
                foreach(cDate date in mDateArray)
                {
                    bool isSelected = new Region(date.Bounds).IsVisible(e.Location);
                    needDraw = needDraw != false || date.IsSelected != isSelected;

                    if (isSelected)
                    {
                        selectedDate = date;
                        break;
                    }
                }

                if (selectedDate != null)
                {
                    if(DateEquals(selectedDate.Value, SelectedDate) == false)
                    {
                        needDraw  = false;
                        foreach(cDate date in mDateArray) date.IsSelected = false;
                        int year  = selectedDate.Value.Year;
                        int month = selectedDate.Value.Month;
                        int day   = selectedDate.Value.Day;
                        SelecteDay(year, month, day);
                    }
                }
                if (needDraw) Invalidate();
            }
            else
            {
                foreach(cDate date in mMonthArray)
                {
                    bool isSelected = new Region(date.Bounds).IsVisible(e.Location);
                    if (isSelected)
                    {
                        mScreenMode = eScreenMode.Day;
                        UpdateDays(date.Value.Year, date.Value.Month);
                        Invalidate();
                        break;
                    }
                }
            }
        }

        private void MouseEnterEvent(object sender, EventArgs e)
        {
            if (sender is Control ctr)
            {
                ctr.ForeColor = Color.DodgerBlue;
            }
        }

        private void MouseLeaveEvent(object sender, EventArgs e)
        {
            if (sender is Control ctr)
            {
                ctr.ForeColor = Color.White;
            }
        }

        private void lbUpClickEvent(object sender, EventArgs e)
        {
            if(mScreenMode == eScreenMode.Month) UpdateDays(mSelectedYear + 1, mSelectedMonth);
            else UpdateDays(mSelectedYear, mSelectedMonth + 1);
        }

        private void lbDownClickEvent(object sender, EventArgs e)
        {
            if(mScreenMode  == eScreenMode.Month) UpdateDays(mSelectedYear - 1, mSelectedMonth);
            else UpdateDays(mSelectedYear, mSelectedMonth - 1);
        }

        private void lbYearClickEvent(object sender, EventArgs e)
        {
            mScreenMode = mScreenMode == eScreenMode.Day ? eScreenMode.Month : eScreenMode.Day;
            UpdateDays(mSelectedYear, mSelectedMonth);
            Invalidate();
        }
        #endregion
    }
}
