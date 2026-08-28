using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Serialization;

namespace Drv.CameraController
{
    public partial class frmControllerSetting : Form
    {
        private readonly List<cControllerData> mlstControllerData = new List<cControllerData>();

        public frmControllerSetting()
        {
            InitializeComponent();

        }

        private void frmControllerSetting_Load(object sender, EventArgs e)
        {
            trList.Nodes.Clear();

            cmbControllerType.Items.AddRange(Enum.GetNames(typeof(eControllerType)));
        }

        #region # BUTTON EVENTS #
        /// <summary>
        /// 컨트롤러 추가 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCtr_Click(object sender, EventArgs e)
        {
            cControllerData ctr = null;

            if ( trList.SelectedNode != null && trList.SelectedNode.Level == 0 )
            {
                ctr = mlstControllerData.FirstOrDefault(p => p.ControllerName == trList.SelectedNode.Name);
                SaveControllerData(ctr);
            }

            ctr = new cControllerData
            {
                ControllerName = "NewCtr"
            };

            if (mlstControllerData.Any(p => p.ControllerName == ctr.ControllerName) == false)
            {
                mlstControllerData.Add(ctr);
            }
            else
            {
                MessageBox.Show(string.Format("'{0}'이 이미 있습니다.", ctr.ControllerName));
            }

            ReDrawList();

            trList.SelectedNode = trList.Nodes["NewCtr"];
        }

        /// <summary>
        /// 축 추가 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddAxis_Click(object sender, EventArgs e)
        {
            if (trList.SelectedNode == null)
            {
                return;
            }
            else
            {
                cControllerData ctr = null;
                if (trList.SelectedNode.Level == 0)
                {
                    ctr = mlstControllerData.FirstOrDefault(p => p.ControllerName == trList.SelectedNode.Name);
                    SaveControllerData(ctr);
                }
                else
                {
                    ctr = mlstControllerData.FirstOrDefault(p => p.ControllerName == trList.SelectedNode.Parent.Name);
                    cCameraDatas camera = trList.SelectedNode.Tag as cCameraDatas;
                    SaveCameraData(camera);
                }

                if (ctr != null)
                {
                    int Idx = 0;
                    string Name = "NewCamera";

                    if (ctr.CameraDatas == null || ctr.CameraDatas.Length == 0)
                    {
                        ctr.CameraDatas = new cCameraDatas[0];
                    }
                    else
                    {
                        Idx = ctr.CameraDatas.Max(p => p.ID) + 1;
                    }

                    if (ctr.CameraDatas.Any(p => p.CameraName == Name) == false)
                    {
                        cCameraDatas[] temp = ctr.CameraDatas;
                        ctr.CameraDatas = new cCameraDatas[temp.Length + 1];

                        for (int i = 0; i < temp.Length; i++)
                        {
                            ctr.CameraDatas[i] = temp[i];
                        }
                        cCameraDatas data = new cCameraDatas
                        {
                            CameraName = Name,
                            ID = Idx
                        };
                        ctr.CameraDatas[temp.Length] = data;

                        ReDrawList();
                        trList.SelectedNode = trList.Nodes[ctr.ControllerName].Nodes["NewCamera"];
                    }
                    else
                    {
                        MessageBox.Show(string.Format("'{0}'이 이미 있습니다.", Name));
                    }
                }
            }
        }

        /// <summary>
        /// 축/컨트롤러 삭제 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (trList.SelectedNode == null)
            {
                return;
            }
            else
            {
                if (trList.SelectedNode.Level == 0)
                {
                    cControllerData ctr = mlstControllerData.FirstOrDefault(p => p.ControllerName == trList.SelectedNode.Name);
                    mlstControllerData.Remove(ctr);
                }
                else
                {
                    cControllerData ctr = mlstControllerData.FirstOrDefault(p => p.ControllerName == trList.SelectedNode.Parent.Name);
                    cCameraDatas deleteCamera = trList.SelectedNode.Tag as cCameraDatas;


                    cCameraDatas[] temp = ctr.CameraDatas;
                    ctr.CameraDatas = new cCameraDatas[temp.Length - 1];

                    for (int i = 0, idx = 0; i < temp.Length; i++)
                    {
                        if (deleteCamera.Equals(temp[i]) == false)
                        {
                            ctr.CameraDatas[idx++] = temp[i];
                        }
                    }
                }
                pnAxisData.Visible = false;
                ReDrawList();
            }
        }

        /// <summary>
        /// 파일 불러 오기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "XML files|*.xml";

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    mlstControllerData.Clear();

                    XmlSerializer serializer = new XmlSerializer(typeof(cControllerData[]));

                    using (FileStream fs = new FileStream(dlg.FileName, FileMode.OpenOrCreate))
                    {
                        if (serializer.Deserialize(fs) is cControllerData[] st)
                        {
                            mlstControllerData.AddRange(st);
                            ReDrawList();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 파일 저장 하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Windows.Forms.SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.Filter = "XML files|*.xml";

                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Type type = typeof(cControllerData[]);
                        XmlSerializer serializer = new XmlSerializer(type);

                        using (TextWriter writer = new StreamWriter(dlg.FileName, false))
                        {
                            serializer.Serialize(writer, mlstControllerData.ToArray());
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        } 
        #endregion

        #region # TREE VIEW EVENTS #
        /// <summary>
        /// 트리뷰 선택 후 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            pnControllerData.Enabled = (e.Node.Level == 0);
            pnControllerInitData.Enabled = (e.Node.Level == 0);
            pnAxisData.Visible = (e.Node.Level == 1);

            if (trList.SelectedNode.Level == 0)
            {
                cControllerData ctr = mlstControllerData.FirstOrDefault(p => p.ControllerName == trList.SelectedNode.Name);
                SetControllerDataView(ctr);

            }
            else
            {
                cControllerData ctr = mlstControllerData.FirstOrDefault(p => p.ControllerName == trList.SelectedNode.Parent.Name);
                SetControllerDataView(ctr);

                if (trList.SelectedNode.Tag is cCameraDatas camera)
                {
                    SetCameraDataView(camera);
                }
            }
        }

        /// <summary>
        /// 트리뷰 선택 전
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trList_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (trList.SelectedNode != null)
            {
                if (trList.SelectedNode.Level == 0)
                {
                    cControllerData ctr = mlstControllerData.FirstOrDefault(p => p.ControllerName == trList.SelectedNode.Name); //선택한 컨트롤러 가져옴

                    if (ctr != null)                                                                //선택한 노드가 변경되기전 컨트롤러 데이터 저장
                    {
                        if (SaveControllerData(ctr) == false)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                else                                                                                //선택한 노드가 변경되기전 축 데이터 저장
                {
                    if (trList.SelectedNode.Tag is cCameraDatas camera)
                    {
                        if (SaveCameraData(camera) == false)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
        } 
        #endregion

        #region # PRIVATE METHODS #
        /// <summary>
        /// 컨트롤러 데이터 저장
        /// </summary>
        /// <param name="pData">저장할 컨트롤러 데이터</param>
        private bool SaveControllerData(cControllerData pData)
        {
            if (mlstControllerData.Any(p => pData.Equals(p) == false && p.ControllerName == txtControllerName.Text) == true)
            {
                MessageBox.Show(string.Format("'{0}'이 이미 있습니다.", txtControllerName.Text));
                return false;
            }
            if (cmbControllerType.SelectedIndex < 0)
            {
                MessageBox.Show("컨트롤러 타입 선택이 잘못 되었습니다.");
                return false;
            }


            pData.ControllerName = txtControllerName.Text;
            pData.ControllerType = (eControllerType)cmbControllerType.SelectedIndex;
            return true;
        }

        /// <summary>
        /// 카메라 데이터 저장
        /// </summary>
        /// <param name="pData">저장할 카메라 데이터</param>
        private bool SaveCameraData(cCameraDatas pData)
        {
            foreach (cControllerData ctrData in mlstControllerData)
            {
                if(ctrData.CameraDatas.Any(p => p.Equals(pData)))
                {
                    if (ctrData.CameraDatas.Any(p => pData.Equals(p) == false && p.CameraName == txtAxisName.Text) == true)
                    {
                        MessageBox.Show(string.Format("'{0}'이 이미 있습니다.", txtAxisName.Text));
                        return false;
                    }
                    int id = Convert.ToInt32(numAxisIndex.Value);
                    if (ctrData.CameraDatas.Any(p => pData.Equals(p) == false && p.ID == id) == true)
                    {
                        MessageBox.Show(string.Format("'{0}' ID가 이미 있습니다.", id));
                        return false;
                    }
                }
            }


            return true;
        }

        private void SetControllerDataView(cControllerData pData)
        {
            txtControllerName.Text = pData.ControllerName;
            cmbControllerType.SelectedIndex = (int)pData.ControllerType;
        }

        private void SetCameraDataView(cCameraDatas pData)
        {
            pData.CameraName = txtAxisName.Text = pData.CameraName;
            numAxisIndex.Value = pData.ID;
            
        }

        private void ReDrawList()
        {
            trList.Nodes.Clear();
            foreach (cControllerData ctrData in mlstControllerData)
            {
                TreeNode ctrNode = new TreeNode(ctrData.ControllerName)
                {
                    Name = ctrData.ControllerName
                };

                trList.Nodes.Add(ctrNode);

                foreach (cCameraDatas axisData in ctrData.CameraDatas)
                {
                    TreeNode axisNode = new TreeNode(string.Format("{0}: {1}", axisData.ID, axisData.CameraName))
                    {
                        Name = axisData.CameraName
                    };

                    ctrNode.Nodes.Add(axisNode);

                    axisNode.Tag = axisData;
                }
            }
            trList.ExpandAll();
        } 
        #endregion

    }
}
