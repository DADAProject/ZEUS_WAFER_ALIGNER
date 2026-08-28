/*
 * NUL's Class Libraries
 * by NUL
 * copyright JC Soft Lab. 2018, all rights reserved
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Management;


/***************************************************************************/
/* Class: ControlHelper                                                    */
/* Create:                                                                 */
/* Developer:                                                              */
/* Note:                                                                   */
/***************************************************************************/

public static class ControlHelper
    {
        public static void DoubleBuffered(this Control ctrl, bool setting)
        {
            if (ctrl == null) return;
            Type dgvType = ctrl.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(ctrl, setting, null);
        }
        //--------------------------------------------------------------------------
        public static int    AsInt(this Control ctrl, int defaultValue = 0)
        {
            try
            {
                return int.Parse(ctrl.Text);
            }
            catch(Exception)
            {
                return defaultValue;
            }
        }
        //--------------------------------------------------------------------------
        public static double AsDouble(this Control ctrl, double defaultValue = 0)
        {
            try
            {
                return double.Parse(ctrl.Text);
            }
            catch(Exception)
            {
                return defaultValue;
            }
        }
        //--------------------------------------------------------------------------
        public static float AsFloat(this Control ctrl, float defaultValue = 0)
        {
            try
            {
                return float.Parse(ctrl.Text);
            }
            catch(Exception)
            {
                return defaultValue;
            }
        }
        //--------------------------------------------------------------------------
        public static void   SetValue(this Control ctrl, object o)
        {
            ctrl.Text = o.ToString();
        }
        //--------------------------------------------------------------------------
        public static void   SetValue(this Control ctrl, double value, string fmt)
        {
            ctrl.Text = value.ToString(fmt);
        }
        //--------------------------------------------------------------------------
        /// <summary>
        /// 불필요한 UI Drawing 을 수행하지 않도록 한다
        /// http://stackoverflow.com/questions/778095/windows-forms-using-backgroundimage-slows-down-drawing-of-the-forms-controls
        /// </summary>
        public static void   SuspendDrawing(this Control ctrl)
        {
            if (ctrl.IsDisposed) return;

            WinAPI.SendMessage(ctrl.Handle, WinAPI.WM_SETREDRAW, 0, 0);
        }
        //--------------------------------------------------------------------------
        public static void   ResumeDrawing(this Control ctrl, bool redraw = true)
        {
            if (ctrl.IsDisposed) return;

            WinAPI.SendMessage(ctrl.Handle, WinAPI.WM_SETREDRAW, 1, 0);
            if (redraw) ctrl.Refresh();
        }
        //--------------------------------------------------------------------------
        /// <summary>
        /// generic invoke
        /// http://www.devpia.com/Maeul/Contents/Detail.aspx?BoardID=18&MAEULNO=8&indexdexdexdexdexdexdexdexdex=1723&page=8</summary>
        /// </summary>
        public static void InvokeIfNeeded(this Control ctrl, Action action)
        {
            try
            {
                if (ctrl.IsDisposed || ctrl.Disposing) return;

                if (ctrl.InvokeRequired)
                    ctrl.Invoke(action);
                else
                    action();
            }
            catch(Exception ex)
            {
                //cDEF.LOG.ExceptionTrace(ctrl.Name + ".InvokeIfNeeded()" + ex.ToString());
                Debug.WriteLine($"[Exception] {ex.Message}");
            }
        }
        //--------------------------------------------------------------------------
        /// <summary>
        /// generic invoke
        /// http://www.devpia.com/Maeul/Contents/Detail.aspx?BoardID=18&MAEULNO=8&indexdexdexdexdexdexdexdexdex=1723&page=8</summary>
        /// </summary>
        public static void InvokeIfNeeded<T>(this Control ctrl, Action<T> action, T args)
        {
            if (ctrl.IsDisposed || ctrl.Disposing) return;

            if (ctrl.InvokeRequired)
                ctrl.Invoke(action, args);
            else
                action(args);
        }
        //--------------------------------------------------------------------------
        /// <summary>
        /// Hide child forms
        /// </summary>
        public static void HideChildForms(this Control ctrl)
        {
            if (ctrl != null)
                foreach (Control f in ctrl.Controls)
                {
                    if (f is Form)
                    {
                        (f as Form).Hide();
                    }
                }
        }
        //--------------------------------------------------------------------------
        /// <summary>
        /// Close child forms
        /// </summary>
        public static void CloseChildForms(this Control ctrl)
        {
            if (ctrl != null)
                foreach (Control f in ctrl.Controls)
                {
                    if (f is Form)
                    {
                        (f as Form).Close();
                    }
                }
        }
        //--------------------------------------------------------------------------
        /// <summary>
        /// Double Buffering All DataGridView in control 
        /// </summary>
        public static void DoubleBufferingAllDataGrid(this Control ctrl)
        {
            foreach (var c in ctrl.Controls)
                if (c is DataGridView)
                    ((DataGridView)c).DoubleBuffered(true);
                else if (c is Control)
                    DoubleBufferingAllDataGrid((Control)c);
        }
        //--------------------------------------------------------------------------
        /// <summary>
        /// Disable child controls
        /// </summary>
        public static void DisableControls(this Control ctrl)
        {
            foreach (Control c in ctrl.Controls)
            {
                c.Enabled = false;
            }
        }
        //--------------------------------------------------------------------------
        /// <summary>
        /// Enable child controls
        /// </summary>
        public static void EanbleControls(this Control ctrl, int authority)
        {
            foreach (Control c in ctrl.Controls)
            {
                if (c.ForeColor.Equals(Color.Blue) || c.ForeColor.Equals(Color.Navy))
                    c.Enabled = authority > 0;
                else if (c.ForeColor == Color.Red || c.ForeColor == Color.Maroon)
                    c.Enabled = authority > 1;
                else if (c.ForeColor == Color.Purple)
                    c.Enabled = authority >= 2;
                else
                    c.Enabled = true;
            }
        }
        //--------------------------------------------------------------------------
        // https://stackoverflow.com/questions/3419159/how-to-get-all-child-controls-of-a-windows-forms-form-of-a-specific-type-button
        public static IEnumerable<Control> GetAllControls(this Control ctrl)
        {
            List<Control> controlList = new List<Control>();
            foreach (Control c in ctrl.Controls)
            {
                controlList.AddRange(GetAllControls(c));
                controlList.Add(c);
            }
            return controlList;
        }
        //--------------------------------------------------------------------------
        public static void SetScroll (this Control ctrl, int xPos, int yPos, bool bRedraw = true)
        {
            WinAPI.SetScrollPos(ctrl.Handle, 0, xPos, bRedraw);
            WinAPI.SetScrollPos(ctrl.Handle, 1, yPos, bRedraw);
            ctrl.Invalidate();
            //const int EM_LINESCROLL = 0x00B6;
            //WinApi.SendMessage(ctrl.Handle, EM_LINESCROLL, 0, xPos);
            //WinApi.SendMessage(ctrl.Handle, EM_LINESCROLL, 1, yPos);

        }
        //--------------------------------------------------------------------------
        public static void SetScrollX(this Control ctrl, int xPos, bool bRedraw = true)
        {
            WinAPI.SetScrollPos(ctrl.Handle, 0, xPos, bRedraw);
        }
        //--------------------------------------------------------------------------
        public static void SetScrollY(this Control ctrl, int yPos, bool bRedraw = true)
        {
            WinAPI.SetScrollPos(ctrl.Handle, 1, yPos, bRedraw);
        }
    }
   
    /***************************************************************************/
    /* Class: DataGridViewRowHelper                                            */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public static class DataGridViewRowHelper
    {
        public static void FitCells(this DataGridView self)
        {
            self.FitRows();

            self.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            foreach (DataGridViewColumn col in self.Columns)
            {
                col.FillWeight = 100;
            }
        }
        //--------------------------------------------------------------------------
        public static void FitRows(this DataGridView self)
        {
            self.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            int h = self.ClientSize.Height - 3;
            if (self.ScrollBars.In(ScrollBars.Both, ScrollBars.Horizontal)) h -= 8;

            if (self.ColumnHeadersVisible)
                h -= self.ColumnHeadersHeight;
            for (int i = 0; i < self.RowCount; i++)
            {
                self.Rows[i].Height = h / (self.RowCount - i);
                h -= self.Rows[i].Height;
            }
        }
        //--------------------------------------------------------------------------
        public static void DrawEmpty(this DataGridView self, Color color)
        {
            self.RowCount = 1;
            self.ColumnCount = 1;
            self[0, 0].Style.BackColor = color;
            self[0, 0].Value = "EMPTY";
            self.ClearSelection();
            self.ReadOnly = true;
            self.FitCells();
        }
        //--------------------------------------------------------------------------
        public static void DrawEmpty(this DataGridView self)
        {
            self.DrawEmpty(Color.DarkGray);
        }
    }


    /***************************************************************************/
    /* Class: FormHelper                                                       */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/

    public static class FormHelper
    {
        /// <summary>
        /// Show Fom in parent control and Close/Hide previous insided forms
        /// </summary>
        public static void ShowInside(this Form form, Control parent)
        {
            form.TopLevel = false;
            parent.Controls.Add(form);
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.Show();
        }
        public static void ShowInside(this UserControl form, Control parent)
        {
            parent.Controls.Add(form);
            form.Dock = DockStyle.Fill;
            form.Show();
        }
        //--------------------------------------------------------------------------
        public static void ShowAndFront(this Form self, Form main)
        {
            if (self.Visible) self.BringToFront();
            else self.Show(main);
        }
    }

    /***************************************************************************/
    /* Class: ComparableHelper                                                 */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public static class ComparableHelper
    {
        public static bool In<T>(this T self, params T[] args) where T : IComparable
        {
            if (args == null) throw new Exception("Comparable.In : args is NULL");

            foreach (T a in args)
                if (self.Equals(a)) return true;

            return false;
        }
        //--------------------------------------------------------------------------
        public static bool InRange<T>(this T self, T min, T max) where T : IComparable
        {
            return (self.CompareTo(min) >= 0 && self.CompareTo(max) <= 0);
        }
        //--------------------------------------------------------------------------
        public static T EnsureRange<T>(this T self, T min, T max) where T : IComparable
        {
            if (self.CompareTo(min) < 0) return min;
            else if (self.CompareTo(max) > 0) return max;
            return self;
        }
    }

    /***************************************************************************/
    /* Class: ComparableHelper                                                 */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public static class DoubleHelper
    {
        public static bool IsZero(this double self, double ES = 10E-7)
        {
            return (self >= -ES && self <= ES);
        }
        //--------------------------------------------------------------------------
        public static bool IsSame(this double self, double value, double ES = 10E-7)
        {
            return (self - value).IsZero(ES);
        }
    }


    /***************************************************************************/
    /* Class: ObjectHelper                                                     */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public static class ObjectHelper
    {
        public static bool In(this object self, params object[] args)
        {
            foreach (var a in args)
                if (self == a) return true;

            return false;
        }
    }

    /***************************************************************************/
    /* Class: StringHelper                                                     */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public static class StringHelper
    { 
        public static T ToEnum<T>(this string self)
        {
            return (T)Enum.Parse(typeof(T), self, true);
        }
        public static T FindEnumValue<T>(int index)
        {
            return (T)Enum.ToObject(typeof(T), index);
        }
        public static T FindEnumValue<T>(string str)
        {
            string[] enums = Enum.GetNames(typeof(T));

            T result = (T)Enum.ToObject(typeof(T), 0);

            for (int i = 0; i < enums.Length; i++)
            {
                if(str == enums[i])
                    result = (T)Enum.ToObject(typeof(T), i);
            }

            return result;
        }
        //--------------------------------------------------------------------------
        public static bool IsSame(this string self, string s)
        {
            return string.Equals(self, s, StringComparison.OrdinalIgnoreCase);
        }
        //--------------------------------------------------------------------------
        public static bool IsSame(this string self, params string[] args)
        {
            foreach (string a in args)
                if (self.IsSame(a)) return true;

            return false;
        }
        //--------------------------------------------------------------------------
        public static bool ExtractWords(this string self, char sep, ref string s1, ref string s2)
        {
            string[] words = self.Split(sep);

            if (words.Length < 2) return false;

            s1 = words[0];
            s2 = words[1];

            return true;
        }
        //--------------------------------------------------------------------------
        public static bool ExtractWords(this string self, char sep, ref double d1, ref double d2)
        {
            string s1 = "", s2 = "";
            double tmp1, tmp2;
            
            if (!self.ExtractWords(sep, ref s1, ref s2)) return false;

            if (double.TryParse(s1, out tmp1) && double.TryParse(s2, out tmp2))
            {
                d1 = tmp1; // 실패시 ref 값이 바뀌지 않게하기 위해
                d2 = tmp2;
                return true;
            }
            else
                return false;
        }
        //--------------------------------------------------------------------------
        public static bool ExtractWords(this string self, char sep, ref int i1, ref int i2)
        {
            string s1 = "", s2 = "";
            int tmp1, tmp2;
            
            if (!self.ExtractWords(sep, ref s1, ref s2)) return false;

            if (int.TryParse(s1, out tmp1) && int.TryParse(s2, out tmp2))
            {
                i1 = tmp1; // 실패시 ref 값이 바뀌지 않게하기 위해
                i2 = tmp2;
                return true;
            }
            else
                return false;
        }
    }

    /***************************************************************************/
    /* Class: ToolStripItemCollectionHelper                                    */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    // https://stackoverflow.com/questions/15380730/foreach-every-subitem-in-a-menustrip
    public static class ToolStripItemCollectionHelper
    {
        /// <summary>
        /// Recusively retrieves all menu items from the input collection
        /// </summary>
        public static IEnumerable<ToolStripMenuItem> GetAllMenuItems(this ToolStripItemCollection items)
        {
            var allItems = new List<ToolStripMenuItem>();
            foreach (var item in items.OfType<ToolStripMenuItem>())
            {
                allItems.Add(item);
                allItems.AddRange(GetAllMenuItems(item.DropDownItems));
            }
            return allItems;
        }
    }


    /***************************************************************************/
    /* Class: ColorHelper                                                      */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public static class ColorHelper
    {
        public static uint ToColorREF(this Color self)
        {
            return (uint)(self.R | (self.G << 8) | (self.B << 16));
        }

        public static Color FromColorREF(this Color self, uint rgb)
        {
            return Color.FromArgb((int)rgb & 0xFF, (int)(rgb & 0xFF00) >> 8, (int)rgb & (0xFF0000) >> 16);
        }

        public static string ToHtmlString(this Color self)
        {
            return string.Format("#{0:X}{1:X}{2:X}", self.R, self.G, self.B);
        }

    }
    /***************************************************************************/
    /* Class: CancellationTokenHelper                                          */
    /* Create:                                                                 */
    /* Developer:                                                              */
    /* Note:                                                                   */
    /***************************************************************************/
    public static class CancellationTokenHelper
	{
		struct Unit { }

		public static Task AsTask(this CancellationToken @this)
		{
			var tcs = new TaskCompletionSource<Unit>();

			@this.Register(() => tcs.SetResult(default(Unit)));

			return tcs.Task;
		}
	}

public static class ArrayHelper
{
    public static void MemSet(this byte[] array, byte value)
    {
        if (array == null)
        {
            throw new ArgumentNullException("array");
        }
        const int blockSize = 4096; // bigger may be better to a certain extent
        int index = 0;
        int length = Math.Min(blockSize, array.Length);
        while (index < length)
        {
            array[index++] = value;
        }
        length = array.Length;
        while (index < length)
        {
            Buffer.BlockCopy(array, 0, array, index, Math.Min(blockSize, length - index));
            index += blockSize;
        }
    }
}

public static class NetworkAdapterHelper
{
    //////////////////////////////////////////////////////////////////////////////////////////////////// Method
    ////////////////////////////////////////////////////////////////////////////////////////// Static
    //////////////////////////////////////////////////////////////////////////////// Public

    #region 네트워크 어댑터 활성화 하기 - EnableNetworkAdapter(filter)

    /// <summary>
    /// 네트워크 어댑터 활성화 하기
    /// </summary>
    /// <param name="filter">필터</param>
    public static bool EnableNetworkAdapter(string filter, string Name)
    {
        string sName = string.Empty;
        bool FindNet = false;
        //
        foreach (ManagementObject managementObject in GetManagementObjectSearcher(filter).Get())
        {
            sName = managementObject["NetConnectionID"].ToString();
            if (sName == Name)
            {
                if (((bool)managementObject.Properties["NetEnabled"].Value) != true)
                {
                    managementObject.InvokeMethod("Enable", null);
                    FindNet = true;
                    break;
                }
                else
                {
                    FindNet = true;
                    break;
                }
            }
        }
        //
        return FindNet;
    }

    #endregion
    #region 네트워크 어댑터 비활성화 하기 - DisableNetworkAdapter(filter)

    /// <summary>
    /// 네트워크 어댑터 비활성화 하기
    /// </summary>
    /// <param name="filter">필터</param>
    public static bool DisableNetworkAdapter(string filter, string Name)
    {
        string sName = string.Empty;
        bool FindNet = false;
        //
        foreach (ManagementObject managementObject in GetManagementObjectSearcher(filter).Get())
        {
            sName = managementObject["NetConnectionID"].ToString();
            if (sName == Name)
            {
                if (((bool)managementObject.Properties["NetEnabled"].Value) == true)
                {
                    managementObject.InvokeMethod("Disable", null);
                    FindNet = true;
                    break;
                }
            }
        }
        //
        return FindNet;
    }
    #endregion
    #region 관리 객체 탐색자 구하기 - GetManagementObjectSearcher(filter)

    /// <summary>
    /// 관리 객체 탐색자 구하기
    /// </summary>
    /// <param name="filter">필터</param>
    /// <returns>관리 객체 탐색자</returns>
    private static ManagementObjectSearcher GetManagementObjectSearcher(string filter)
    {
        string query = "SELECT * FROM Win32_NetworkAdapter";

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query += String.Format(" WHERE Name LIKE '%{0}%' ", filter);
        }

        WqlObjectQuery wqlObjectQuery = new WqlObjectQuery(query);

        ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(wqlObjectQuery);

        return managementObjectSearcher;
    }
    #endregion
}



