using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro;

namespace UserLib
{
    public partial class ProductStatistics: UserControl
    {
        public ProductStatistics()
        {
            InitializeComponent();
        }

        private void ProductStatistics_Load(object sender, EventArgs e)
        {
            Result = true;
            cmbTriggerIndex.SelectedIndex = 1;
            cmbTriggerFunction.SelectedIndex = 1;
            cmbTriggerCode.SelectedIndex = 1;           
        }

        string _stationName = "";
        public string StationName
        {
            set
            {

                this._stationName = value;
                lbl_name.Text = _stationName;
            }
            get
            {
                return this._stationName;
            }
        }



        #region 显示结果
        bool _result;
        public bool Result
        {
            set
            {
                _result = value;
                if (value)
                {
                    lblResult.Text = "Pass";
                    lblResult.BackColor = Color.Green;
                }
                else
                {
                    lblResult.Text = "Fail";
                    lblResult.BackColor = Color.Red;
                }
            }
        }
        #endregion

        #region 统计
        int _passCount, _failCount;
        public int PassCount
        {
            set
            {

                this._passCount = value;
                lblPassCount.Text = _passCount.ToString();
                lblYield.Text = Yield.ToString("p2");
            }
            get
            {
                return this._passCount;
            }
        }

        public int FailCount
        {
            set
            {
                this._failCount = value;
                lblFailCount.Text = _failCount.ToString();
                lblYield.Text = Yield.ToString("p2");
            }
            get
            {
                return this._failCount;
            }
        }

        public int TotalCount
        {
            get
            {
                return PassCount + FailCount;
            }
        }
        public double Yield
        {
            get
            {
                return TotalCount == 0 ? 0 : PassCount * 1.0 / TotalCount;
            }
        }

     
        #endregion

        #region 节拍
        public double cycleTime
        {
            set
            {
                lblCT.Text = value.ToString("f0") + "ms";
            }
        }
        #endregion

        #region 触发
        int _TriggerCode, _TriggerFunction, _TriggerIndex, _ProductType, _PlcId;
         
        public int TriggerCode
        {
            set
            {

                this._TriggerCode = value;
                cmbTriggerCode.SelectedIndex = this._TriggerCode;
            }
            get
            {
                return this._TriggerCode;
            }
        }
        public int TriggerFunction
        {
            set
            {

                this._TriggerFunction = value;
                cmbTriggerFunction.SelectedIndex = this._TriggerFunction;
            }
            get
            {
                return this._TriggerFunction;
            }
        }
        public int TriggerIndex
        {
            set
            {

                this._TriggerIndex = value;
                cmbTriggerIndex.SelectedIndex = this._TriggerIndex;
            }
            get
            {
                return this._TriggerIndex;
            }
        }
        public int ProductType
        {
            set
            {

                this._ProductType = value;
            }
            get
            {
                return this._ProductType;
            }
        }

        public int PlcId
        {
            set
            {
                this._PlcId = value;
            }
            get
            {
                return this._PlcId;
            }
        }
        #endregion

        #region 坐标
        double _x, _y, _a;

        
        private void cmbTriggerCode_SelectedValueChanged(object sender, EventArgs e)
        {
            TriggerCode = cmbTriggerCode.SelectedIndex;
        }

        private void cmbTriggerFunction_SelectedValueChanged(object sender, EventArgs e)
        {
            TriggerFunction = cmbTriggerFunction.SelectedIndex;
        }

        private void cmbTriggerIndex_SelectedValueChanged(object sender, EventArgs e)
        {
            TriggerIndex = cmbTriggerIndex.SelectedIndex;
        }

        private void cmbProductType_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void numX_ValueChanged(object sender, EventArgs e)
        {
            X = (double)numX.Value;
        }

        private void numY_ValueChanged(object sender, EventArgs e)
        {
            Y = (double)numY.Value;
        }

        private void numA_ValueChanged(object sender, EventArgs e)
        {
            A = (double)numA.Value;
        }

        private void cmbProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProductType = cmbProductType.SelectedIndex + 1;
        }
        ICogImage _image;
        public ICogImage mImage
        {
            set
            {

                this._image = value;
            }
            get
            {
                return this._image;
            }
        }
        
        public ICogImage LoadImage()
        {
            mImage = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            CogImageFileTool LoadImageTool = new CogImageFileTool();
            openFileDialog1.Filter = "图像文件|*.idb;*.cdb;*.bmp;*.jpeg;*.jpg;*.png";

            if (openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return mImage;
            }
            string Filename = openFileDialog1.FileName;
            LoadImageTool.Operator.Open(Filename, CogImageFileModeConstants.Read);
            LoadImageTool.Run();
            mImage = LoadImageTool.OutputImage;
            return mImage;
        }

        private void cmbPlcId_SelectedValueChanged(object sender, EventArgs e)
        {
            PlcId= cmbPlcId.SelectedIndex;
        }

        public double X
        {
            set
            {

                this._x = value;
                numX.Value = (decimal)this._x;

            }
            get
            {
                return this._x;
            }
        }
        public double Y
        {
            set
            {

                this._y = value;
                numY.Value = (decimal)this._y;
            }
            get
            {
                return this._y;
            }
        }
        public double A
        {
            set
            {

                this._a = value;
                numA.Value = (decimal)this._a;
            }
            get
            {
                return this._a;
            }
        }
        #endregion

        #region 权限控制
        bool _btnEnabled;
        public bool BtnEnabled
        {
            set
            {
                this._btnEnabled = value;
                ControlEnabled();
            }
            get
            {
                return this._btnEnabled;
            }
        }
        private void ControlEnabled()
        {
            if (BtnEnabled)
            {
                btnLoad.Enabled = true;
                btnReset.Enabled = true;
                btnTrigger.Enabled = true;
                btnReadCoordinate.Enabled = true;
            }
            else
            {
                btnLoad.Enabled = false;
                btnReset.Enabled = false;
                btnTrigger.Enabled = false;
                btnReadCoordinate.Enabled = false;
            }
        }
        #endregion

    }
}
