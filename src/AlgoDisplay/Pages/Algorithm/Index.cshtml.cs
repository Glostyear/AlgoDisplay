#define DEBUG
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Runtime;
using System.Windows;

namespace AlgoDisplay.Pages.Algorithm
{
    public class IndexModel : PageModel
    {

//#if DEBUG
//        public const string dllPath = @"C:\Users\caijunfu\Desktop\Archive\AlgoDisplay\bin\Debug\net6.0\yoloV7-test1.dll";
//        public const string machineVision = @"C:\Users\caijunfu\Desktop\Archive\AlgoDisplay\bin\Debug\net6.0\machinevision.dll";
//#else
//        public const string dllPath = @"C:\Users\caijunfu\Desktop\Archive\AlgoDisplay\bin\Release\net6.0\yoloV7-test1.dll";
//#endif

        [DllImport("yoloV7-test1.dll", EntryPoint = "yoloTest", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int yolo_test(IntPtr sourceImg, ref byte strPath);
        //*****************************
        //yolov5 5s模型测试
        //*****************************
        //* @param:   imgData[IN]-输入图片路径;
        //* @param:   outData[OUT]-输出图片路径;
        //extern "C" MV_MODULE_PORT int yoloTest(GrayBitmap* imgData, char* outData);
        //public static extern int yolo_test([MarshalAs(UnmanagedType.LPStr)] string sourceImg, [MarshalAs(UnmanagedType.LPStr)] string strPath);
        
        
        private ConfigParam conPara;
        public enum MarkType
        {
            SingleCircle = 1,
            CircleMatrix,
            ComplexCircles,
            Rectangle,
            Corner,
            Pattern,
            PlumHoles,
            Cross,
            DoubleRectangle,
            Line,
            CircleRing
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MVDetectParameter
        {
            public MarkType MarkType;
            public int IsWhite;
            public double Width;
            public double Height;
            public double Diameter;
            public int Rows;
            public int Cols;
            public double Rowspan;
            public double Colspan;
            public double Threshold;
            [MarshalAs(UnmanagedType.LPStr)]
            public string MarkData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MVDetectParameterV2
        {
            public MarkType MarkType;
            public int IsWhite;
            public double Width;
            public double Height;
            public double Diameter;
            public int Rows;
            public int Cols;
            public double Rowspan;
            public double Colspan;
            public double Threshold;
            public double Diameter2;
            public int IsWhite2;
            public double Threshold2;
            public double CenterWeight;
            [MarshalAs(UnmanagedType.LPStr)]
            public string MarkData;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MVDetectResult
        {
            public double Width;
            public double Height;
            public double CenterX;
            public double CenterY;
            public int CircleCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public MVDetectedCircle[] Circles;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MVDetectedCircle
        {
            public double CenterX;
            public double CenterY;
            public double Diameter;
            public double Roundness;
        }

        public MVDetectResult FindMark(Bitmap bmpSrc, MVDetectParameterV2 param, ref int nDetectState)
        {
            MVDetectResult result = new MVDetectResult();
            result.Circles = new MVDetectedCircle[32];
            if (bmpSrc == null)
                return result;
            GCHandle imgHandle = new GCHandle();
            try
            {
                var img = bmpSrc;
                if (img != null)
                {
                    using (img) using (GrayBitmap bmp = GrayBitmap.FromBitmap(img))
                    {
                        imgHandle = GCHandle.Alloc(bmp, GCHandleType.Pinned);
                        nDetectState = DetectMark(imgHandle.AddrOfPinnedObject(), param, ref result);
                    }
                }
            }
            finally
            {
                if (imgHandle.IsAllocated)
                    imgHandle.Free();
            }
            return result;
        }

        public MVDetectResult FindMark(Bitmap bmpSrc, MVDetectParameter param, ref int nDetectState)
        {
            MVDetectResult result = new MVDetectResult();
            result.Circles = new MVDetectedCircle[32];
            if (bmpSrc == null)
                return result;
            GCHandle imgHandle = new GCHandle();
            try
            {
                var img = bmpSrc;
                if (img != null)
                {
                    using (img) using (GrayBitmap bmp = GrayBitmap.FromBitmap(img))
                    {
                        imgHandle = GCHandle.Alloc(bmp, GCHandleType.Pinned);
                        nDetectState = FindMark(imgHandle.AddrOfPinnedObject(), param, ref result);
                    }
                }
            }
            finally
            {
                if (imgHandle.IsAllocated)
                    imgHandle.Free();
            }
            return result;
        }

        public MVDetectResult FindIMark(Bitmap bmpSrc, MVDetectParameter param, ref int nDetectState)
        {
            MVDetectResult result = new MVDetectResult();
            result.Circles = new MVDetectedCircle[32];
            if (bmpSrc == null)
                return result;
            GCHandle imgHandle = new GCHandle();
            try
            {
                var img = bmpSrc;
                if (img != null)
                {
                    using (img) using (GrayBitmap bmp = GrayBitmap.FromBitmap(img))
                    {
                        imgHandle = GCHandle.Alloc(bmp, GCHandleType.Pinned);
                        nDetectState = FindIMark(imgHandle.AddrOfPinnedObject(), param, ref result);
                    }
                }
            }
            finally
            {
                if (imgHandle.IsAllocated)
                    imgHandle.Free();
            }
            return result;
        }

        /// <summary>
        /// 修改配置文件
        /// </summary>
        private void getConfig()
        {
            //修改算法配置文件
            //setAlgorithmConfig();
            //单孔写入            
            //conPara.IsPolarChange = Convert.ToInt32(checkPolor.IsChecked);
            //conPara.nWhiteEdge = Convert.ToInt32(checkWhiteEdge.IsChecked);
            //conPara.nEllipseDetection = Convert.ToInt32(checkEllipse.IsChecked);
            //conPara.nEnhanceMap = Convert.ToInt32(checkEnhance.IsChecked);
            //conPara.nCircleSocreThreshold = textCirThre.Text != string.Empty ? Convert.ToInt32(textCirThre.Text) : 0;
            //孔矩阵

            //复合孔            
            //if (RadioButtonInner.IsChecked == true)
            //    conPara.IsInnerRing = 1;
            //else if (RadioButtonExtern.IsChecked == true)
            //    conPara.IsInnerRing = 2;
            //梅花孔          
            //conPara.IsDeepCalculation = Convert.ToInt32(checkExactCalc.IsChecked);
            //conPara.nSaveDetectedResult = Convert.ToInt32(checkSaveResult.IsChecked);
            //十字
        }

        /// <summary>
        /// 算法配置写入配置文件
        /// </summary>
        //private void setAlgorithmConfig()
        //{
        //    int nMarkTypeIndex = comboBox_MarkType.SelectedIndex;
        //    switch (nMarkTypeIndex)
        //    {
        //        case 0: //单孔
        //            if (radioButtonOne.IsChecked == true)
        //                conPara.ConAlgorithm.nCircleAlgorithm = 1;
        //            else if (radioButtonTwo.IsChecked == true)
        //                conPara.ConAlgorithm.nCircleAlgorithm = 2;
        //            else if (radioButtonThree.IsChecked == true)
        //                conPara.ConAlgorithm.nCircleAlgorithm = 3;
        //            conPara.ConMatchAlgorithm.nCircleMatchAlgorithm = Convert.ToInt32(comboBox_Roi.SelectedIndex);
        //            conPara.ConMatch.nCircleNumber = Convert.ToInt32(textBox_DetNumber.Text);
        //            break;
        //        case 1://孔矩阵
        //            if (radioButtonOne.IsChecked == true)
        //                conPara.ConAlgorithm.nHoleMatrixAlgorithm = 1;
        //            else if (radioButtonTwo.IsChecked == true)
        //                conPara.ConAlgorithm.nHoleMatrixAlgorithm = 2;
        //            else if (radioButtonThree.IsChecked == true)
        //                conPara.ConAlgorithm.nHoleMatrixAlgorithm = 3;
        //            conPara.ConMatchAlgorithm.nHoleMatrixMatchAlgorithm = Convert.ToInt32(comboBox_Roi.SelectedIndex);
        //            conPara.ConMatch.nHoleMatrixNumber = Convert.ToInt32(textBox_DetNumber.Text);
        //            break;
        //        case 2://复合孔
        //            if (radioButtonOne.IsChecked == true)
        //            {
        //                if (Convert.ToString(comboBox_Algorithm.SelectedItem) == "circle_ring")
        //                    conPara.ConAlgorithm.nCircleRingAlgorithm = 1;
        //                else if (Convert.ToString(comboBox_Algorithm.SelectedItem) == "complex_plum")
        //                    conPara.ConAlgorithm.nRingPlumAlgorithm = 1;
        //            }
        //            else if (radioButtonTwo.IsChecked == true)
        //            {
        //                if (Convert.ToString(comboBox_Algorithm.SelectedItem) == "circle_ring")
        //                    conPara.ConAlgorithm.nCircleRingAlgorithm = 2;
        //                else if (Convert.ToString(comboBox_Algorithm.SelectedItem) == "complex_plum")
        //                    conPara.ConAlgorithm.nRingPlumAlgorithm = 2;
        //            }
        //            else if (radioButtonThree.IsChecked == true)
        //            {
        //                if (Convert.ToString(comboBox_Algorithm.SelectedItem) == "circle_ring")
        //                    conPara.ConAlgorithm.nCircleRingAlgorithm = 3;
        //                else if (Convert.ToString(comboBox_Algorithm.SelectedItem) == "complex_plum")
        //                    conPara.ConAlgorithm.nRingPlumAlgorithm = 3;
        //            }
        //            if (Convert.ToString(comboBox_Algorithm.SelectedItem) == "complex_plum")
        //            {
        //                conPara.ConMatchAlgorithm.nRingPlumMatchAlgorithm = Convert.ToInt32(comboBox_Roi.SelectedIndex);
        //                conPara.ConMatch.nRingPlumNumber = Convert.ToInt32(textBox_DetNumber.Text);
        //            }
        //            break;
        //        case 3://矩形
        //            if (radioButtonOne.IsChecked == true)
        //                conPara.ConAlgorithm.nRectangleAlgorithm = 1;
        //            else if (radioButtonTwo.IsChecked == true)
        //                conPara.ConAlgorithm.nRectangleAlgorithm = 2;
        //            conPara.ConMatchAlgorithm.nRectangleMatchAlgorithm = Convert.ToInt32(comboBox_Roi.SelectedIndex);
        //            conPara.ConMatch.nRectangleNumber = Convert.ToInt32(textBox_DetNumber.Text);

        //            break;
        //        case 4://梅花孔
        //            if (radioButtonOne.IsChecked == true)
        //                conPara.ConAlgorithm.nRingPlumAlgorithm = 1;
        //            else if (radioButtonTwo.IsChecked == true)
        //                conPara.ConAlgorithm.nRingPlumAlgorithm = 2;
        //            else if (radioButtonThree.IsChecked == true)
        //                conPara.ConAlgorithm.nRingPlumAlgorithm = 3;
        //            conPara.ConMatchAlgorithm.nRingPlumMatchAlgorithm = Convert.ToInt32(comboBox_Roi.SelectedIndex);
        //            conPara.ConMatch.nRingPlumNumber = Convert.ToInt32(textBox_DetNumber.Text);
        //            break;
        //        case 5://十字
        //            if (radioButtonOne.IsChecked == true)
        //                conPara.ConAlgorithm.nCrossAlgorithm = 1;
        //            else if (radioButtonTwo.IsChecked == true)
        //                conPara.ConAlgorithm.nCrossAlgorithm = 2;
        //            else if (radioButtonThree.IsChecked == true)
        //                conPara.ConAlgorithm.nCrossAlgorithm = 3;
        //            conPara.ConMatchAlgorithm.nCrossMatchAlgorithm = Convert.ToInt32(comboBox_Roi.SelectedIndex);
        //            conPara.ConMatch.nCrossNumber = Convert.ToInt32(textBox_DetNumber.Text);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //public double DetectedRadius
        //{
        //    get { return (double)GetValue(DetectedRadiusProperty); }
        //    set { SetValue(DetectedRadiusProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for DetectedRadius.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty DetectedRadiusProperty =
        //    DependencyProperty.Register("DetectedRadius", typeof(double), typeof(markConfig), new PropertyMetadata(0.0));

        //public Point DetectedCenter
        //{
        //    get { return (Point)GetValue(DetectedCenterProperty); }
        //    set { SetValue(DetectedCenterProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for DetectedCenter.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty DetectedCenterProperty =
        //    DependencyProperty.Register("DetectedCenter", typeof(Point), typeof(markConfig), new PropertyMetadata(new Point()));

        //public double DetectedScore
        //{
        //    get { return (double)GetValue(DetectedScoreProperty); }
        //    set { SetValue(DetectedScoreProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for DetectedScore.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty DetectedScoreProperty =
        //    DependencyProperty.Register("DetectedScore", typeof(double), typeof(markConfig), new PropertyMetadata(0.0));

        public class StatResults //统计结果
        {
            public int nRepeatCnt;
            public int nSuccessCnt;
            public int nDetectState;
            public double fMaxErrorX;
            public double fMaxErrorY;
            public double fTotalTime;
            public double preCenterX;
            public double preCenterY;
            public StatResults()
            {
                nRepeatCnt = 0;
                nSuccessCnt = 0;
                nDetectState = -10;
                fMaxErrorX = 0;
                fMaxErrorY = 0;
                fTotalTime = 0;
                preCenterX = 0;
                preCenterY = 0;
            }
        }

        private StatResults tmpStatResults = null;

        [DllImport("machinevision.dll", EntryPoint = "DetectMark", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern int DetectMark(IntPtr sourceImg, MVDetectParameterV2 param, ref MVDetectResult result);

        [DllImport("machinevision.dll", EntryPoint = "FindMark", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private static extern int FindMark(IntPtr sourceImg, MVDetectParameter param, ref MVDetectResult result);

        [DllImport("machinevision.dll", EntryPoint = "FindIMark", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int FindIMark(IntPtr sourceImg, MVDetectParameter param, ref MVDetectResult result);
        

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        public static unsafe void CopyUnmanagedMemory(IntPtr srcPtr, int srcOffset, IntPtr dstPtr, int dstOffset, int count)
        {
            var pSrc = (byte*)srcPtr.ToPointer();
            var pDst = (byte*)dstPtr.ToPointer();

            pSrc += srcOffset;
            pDst += dstOffset;
            memcpy(pDst, pSrc, count);
        }

        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        private static extern unsafe byte* memcpy(byte* dst, byte* src, int count);
        //public void CopyUnmanagedMemory(IntPtr dest, IntPtr src, int startIndex, IntPtr bmpData, int offset, int length)
        //{
        //    CopyMemory(dest + startIndex, bmpData + offset, (uint)length);
        //}

        [StructLayout(LayoutKind.Sequential)]
        public struct GrayBitmap : IDisposable
        {
            public int Width;
            public int Height;
            public int Stride;
            public int Channels;
            public IntPtr BitmapData;

            public void Dispose()
            {
                if (BitmapData != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(BitmapData);
                }
            }

            public static unsafe GrayBitmap FromBitmap(Bitmap img)
            {
                var bmp = new GrayBitmap();
                if (img != null)
                {
                    bmp.Width = img.Width;
                    bmp.Height = img.Height;
                    bmp.Channels = 3;
                    BitmapData imgData = null;
                    try
                    {
                        imgData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height),
                            ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                        int dataLength = imgData.Stride * imgData.Height;
                        bmp.Stride = imgData.Stride;
                        bmp.BitmapData = Marshal.AllocHGlobal(dataLength);
                        IndexModel.CopyUnmanagedMemory(imgData.Scan0, 0, bmp.BitmapData, 0, dataLength);
                    }
                    finally
                    {
                        if (imgData != null)
                            img.UnlockBits(imgData);
                    }
                }
                return bmp;
            }
        }

        //将IFormFile上传的图片转化为位图对象
        public static Bitmap ConvertToBitmap(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var bitmap = new Bitmap(stream);
                return bitmap;
            }
        }


        //算法配置文件
        [StructLayout(LayoutKind.Sequential)]
        public struct ConfigAlgorithm
        {
            public int nCircleAlgorithm;        //圆检测算法
            public int nRingPlumAlgorithm;      //环形梅花孔检测算法        
            public int nPlumLittleCircieAlgorithm; //梅花孔小圆检测算法
            public int nHoleMatrixAlgorithm;     //孔矩阵抓取算法
            public int nRectangleAlgorithm;      //矩形抓取算法
            public int nCircleRingAlgorithm;     //圆环抓取算法
            public int nCrossAlgorithm;          //十字抓取算法 
            public int nLineAlgorithm;           //直线检测算法
        };

        //算法配置文件
        [StructLayout(LayoutKind.Sequential)]
        public struct MatchConfig
        {
            public int nCircleNumber;
            public int nRingPlumNumber;
            public int nRectPlumNumber;
            public int nHoleMatrixNumber;
            public int nRectangleNumber;
            public int nCircleRingNumber;
            public int nCrossNumber;
            public int nTriangleNumber;
        };

        //特征点匹配算法配置
        [StructLayout(LayoutKind.Sequential)]
        public struct TextureMatchParam
        {
            public int nCornerRoi;              //拐角区域检测
            public int nPointType;              //特征点算子
            public int nMatchType;              //匹配算法
        };

        //匹配算法配置
        [StructLayout(LayoutKind.Sequential)]
        public struct MatchAlgorithm
        {
            public int nCircleMatchAlgorithm;
            public int nRingPlumMatchAlgorithm;      //环形梅花孔匹配算法
            public int nRectPlumMatchAlgorithm;     //矩形梅花孔匹配算法
            public int nHoleMatrixMatchAlgorithm;     //孔矩阵匹配算法
            public int nRectangleMatchAlgorithm;      //矩形匹配算法
            public int nCircleRingMatchAlgorithm;     //圆环匹配算法
            public int nCrossMatchAlgorithm;          //十字匹配算法

        };

        //算法库配置文件
        [StructLayout(LayoutKind.Sequential)]
        public struct ConfigParam
        {
            public int nCircleNums;             //梅花孔小圆数量
            public int nCircleDistance;         //中心圆和小圆中心点距离
            public int nSaveImage;              //是否自动保存图片
            public int isCenterCirclePlum;      //是否为中心圆梅花孔
            public int nWhiteEdge;              //靶标边缘颜色
            public int IsRectPlum;             //矩形梅花孔（梅花孔）	
            public int IsInnerRing;            //检测内环（圆环）
            public int nPixelRate;            //最大像素占比（测试是否已曝光）
            public int nLinePointRate;        //半边直线删选比率
            public int nLittleCircleRate;      //梅花孔小圆抓取成功占比
            public int IsPolarChange;          //极性变化
            public int IsMatchPrior;          //通过匹配得分确定匹配返回值
            public int IsDeepCalculation;        //梅花开精确计算
            public int nSaveDetectedResult;     //保存抓取结果图片
            public int nCircleSocreThreshold;
            public int nCircleRingSocre;
            public int nEllipseDetection;
            public int nEnhanceMap;
            public ConfigAlgorithm ConAlgorithm;
            public MatchConfig ConMatch;
            public MatchAlgorithm ConMatchAlgorithm;
            public TextureMatchParam TextureParam;
        };

        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(string section, string key,
        string val, string filePath);
        public static void IniWriteValue(string Section, string Key, string Value, string filePath)
        {
            WritePrivateProfileString(Section, Key, Value, filePath);
        }

        public static void InitLogFile(string strLogPath)
        {
            if (System.IO.File.Exists(strLogPath))
                System.IO.File.Delete(strLogPath);
        }

        /// <summary>
        /// 靶标参数写入配置文件
        /// </summary>
        /// <param name="conPara"></param>
        /// <param name="strConfig"></param>
        public void writeConfigParam(ConfigParam conPara, string strConfig)
        {
            //ConfigParams
            //Utility.IniWriteValue("ConfigParams", "CircleNums", Convert.ToString(conPara.nCircleNums), strConfig);
            //Utility.IniWriteValue("ConfigParams", "CircleDistance", Convert.ToString(conPara.nCircleDistance), strConfig);
            IniWriteValue("ConfigParams", "isSaveImage", Convert.ToString(conPara.nSaveImage), strConfig);
            //Utility.IniWriteValue("ConfigParams", "isCenterCirclePlum", Convert.ToString(conPara.isCenterCirclePlum), strConfig);
            IniWriteValue("ConfigParams", "isWhiteEdge", Convert.ToString(conPara.nWhiteEdge), strConfig);
            IniWriteValue("ConfigParams", "isRectPlum", Convert.ToString(conPara.IsRectPlum), strConfig);
            IniWriteValue("ConfigParams", "InnerRing", Convert.ToString(conPara.IsInnerRing), strConfig);
            IniWriteValue("ConfigParams", "PixelRate", Convert.ToString(conPara.nPixelRate), strConfig);
            IniWriteValue("ConfigParams", "LinePointRate", Convert.ToString(conPara.nLinePointRate), strConfig);
            IniWriteValue("ConfigParams", "LittleCircleRate", Convert.ToString(conPara.nLittleCircleRate), strConfig);
            IniWriteValue("ConfigParams", "PolarChange", Convert.ToString(conPara.IsPolarChange), strConfig);
            IniWriteValue("ConfigParams", "MatchPrior", Convert.ToString(conPara.IsMatchPrior), strConfig);
            IniWriteValue("ConfigParams", "DeepCalculation", Convert.ToString(conPara.IsDeepCalculation), strConfig);
            IniWriteValue("ConfigParams", "SaveDetectedResult", Convert.ToString(conPara.nSaveDetectedResult), strConfig);
            IniWriteValue("ConfigParams", "CircleSocreThreshold", Convert.ToString(conPara.nCircleSocreThreshold), strConfig);
            IniWriteValue("ConfigParams", "CircleRingSocre", Convert.ToString(conPara.nCircleRingSocre), strConfig);
            IniWriteValue("ConfigParams", "EllipseDetection", Convert.ToString(conPara.nEllipseDetection), strConfig);
            IniWriteValue("ConfigParams", "EnhanceMap", Convert.ToString(conPara.nEnhanceMap), strConfig);
            //Match
            IniWriteValue("Match", "CircleNumber", Convert.ToString(conPara.ConMatch.nCircleNumber), strConfig);
            IniWriteValue("Match", "RingPlumNumber", Convert.ToString(conPara.ConMatch.nRingPlumNumber), strConfig);
            IniWriteValue("Match", "RectPlumNumber", Convert.ToString(conPara.ConMatch.nRectPlumNumber), strConfig);
            IniWriteValue("Match", "HoleMatrixNumber", Convert.ToString(conPara.ConMatch.nHoleMatrixNumber), strConfig);
            IniWriteValue("Match", "RectangleNumber", Convert.ToString(conPara.ConMatch.nRectangleNumber), strConfig);
            IniWriteValue("Match", "CircleRingNumber", Convert.ToString(conPara.ConMatch.nCircleRingNumber), strConfig);
            IniWriteValue("Match", "CrossNumber", Convert.ToString(conPara.ConMatch.nCrossNumber), strConfig);
            IniWriteValue("Match", "TriangleNumber", Convert.ToString(conPara.ConMatch.nTriangleNumber), strConfig);
            //AlgorithmParams
            IniWriteValue("AlgorithmParams", "CircleAlgorithm", Convert.ToString(conPara.ConAlgorithm.nCircleAlgorithm), strConfig);
            IniWriteValue("AlgorithmParams", "RingPlumAlgorithm", Convert.ToString(conPara.ConAlgorithm.nRingPlumAlgorithm), strConfig);
            IniWriteValue("AlgorithmParams", "RectPlumAlgorithm", Convert.ToString(conPara.ConAlgorithm.nPlumLittleCircieAlgorithm), strConfig);
            IniWriteValue("AlgorithmParams", "HoleMatrixAlgorithm", Convert.ToString(conPara.ConAlgorithm.nHoleMatrixAlgorithm), strConfig);
            IniWriteValue("AlgorithmParams", "RectangleAlgorithm", Convert.ToString(conPara.ConAlgorithm.nRectangleAlgorithm), strConfig);
            IniWriteValue("AlgorithmParams", "CircleRingAlgorithm", Convert.ToString(conPara.ConAlgorithm.nCircleRingAlgorithm), strConfig);
            IniWriteValue("AlgorithmParams", "CrossAlgorithm", Convert.ToString(conPara.ConAlgorithm.nCrossAlgorithm), strConfig);
            IniWriteValue("AlgorithmParams", "LineAlgorithm", Convert.ToString(conPara.ConAlgorithm.nLineAlgorithm), strConfig);
            //MatchAlgorithm
            IniWriteValue("MatchAlgorithm", "CircleMatchAlgorithm", Convert.ToString(conPara.ConMatchAlgorithm.nCircleMatchAlgorithm), strConfig);
            IniWriteValue("MatchAlgorithm", "RingPlumMatchAlgorithm", Convert.ToString(conPara.ConMatchAlgorithm.nRingPlumMatchAlgorithm), strConfig);
            IniWriteValue("MatchAlgorithm", "RectPlumMatchAlgorithm", Convert.ToString(conPara.ConMatchAlgorithm.nRectPlumMatchAlgorithm), strConfig);
            IniWriteValue("MatchAlgorithm", "HoleMatrixMatchAlgorithm", Convert.ToString(conPara.ConMatchAlgorithm.nHoleMatrixMatchAlgorithm), strConfig);
            IniWriteValue("MatchAlgorithm", "RectangleMatchAlgorithm", Convert.ToString(conPara.ConMatchAlgorithm.nRectangleMatchAlgorithm), strConfig);
            IniWriteValue("MatchAlgorithm", "CircleRingMatchAlgorithm", Convert.ToString(conPara.ConMatchAlgorithm.nCircleRingMatchAlgorithm), strConfig);
            IniWriteValue("MatchAlgorithm", "CrossMatchAlgorithm", Convert.ToString(conPara.ConMatchAlgorithm.nCrossMatchAlgorithm), strConfig);
            //TextureMatch
            IniWriteValue("TextureMatch", "CornerRoi", Convert.ToString(conPara.TextureParam.nCornerRoi), strConfig);
            IniWriteValue("TextureMatch", "PointType", Convert.ToString(conPara.TextureParam.nPointType), strConfig);
            IniWriteValue("TextureMatch", "MatchType", Convert.ToString(conPara.TextureParam.nMatchType), strConfig);
        }
        public void OnGet()
        {
        }

       
        private readonly IWebHostEnvironment _webHostEnvironment;

        public IndexModel(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public static string? SelectedAlgorithm { get; set; }

        public async Task<IActionResult> OnPostAsync(string algorithm, IFormFile image)
        {
            // 获取wwwroot目录的完整路径
            var webRootPath = _webHostEnvironment.WebRootPath;
            //Console.WriteLine(webRootPath);

            // 检查是否选择了算法和上传了图片
            if (string.IsNullOrEmpty(algorithm) || image == null || image.Length == 0)
            {
                return Page();
            }

            var originImagePath = "";
            var processedImagePath = "";
            // 将上传的图片保存到wwwroot目录下的images文件夹中
            string imgData = Path.Combine(webRootPath, "images", "original.jpg");
            using var stream = new FileStream(imgData, FileMode.Create);
            await image.CopyToAsync(stream);


            // 根据选择的算法对图像进行处理
            if (algorithm == "algorithm1")
            {
                // 执行ִ算法1
                // ...
                Stopwatch sw = new Stopwatch();
                Bitmap CurPreviewImage = ConvertToBitmap(image);
                GCHandle imgHandle = new GCHandle();
                byte[] s = new byte[256];
                using (GrayBitmap bmp = GrayBitmap.FromBitmap(CurPreviewImage))
                {
                    sw.Start();
                    imgHandle = GCHandle.Alloc(bmp, GCHandleType.Pinned);
                    try
                    {
                        int result = yolo_test(imgHandle.AddrOfPinnedObject(), ref s[0]);
                    }
                    catch(Exception e) {
                        Console.WriteLine(e.Message);
                    }

                    finally
                    {
                        sw.Stop();
                        imgHandle.Free();
                        //processedImagePath = Encoding.Default.GetString(s, 0, s.Length).Replace("\0", "");
                        //在根目录中创建processed.jpg
                        processedImagePath = Path.Combine(webRootPath, "images", "processed.jpg");

                        // 检查是否存在processed.jpg
                        if (System.IO.File.Exists(processedImagePath))
                        {
                            System.IO.File.Delete(processedImagePath);
                        }
                        // 将result.jpg保存至根目录
                        //System.IO.File.Move(Path.Combine("../AlgoDisplay", "result.jpg"), processedImagePath);
                        //System.IO.File.Move(Path.Combine("../", "result.jpg"), processedImagePath);
                        Console.WriteLine(processedImagePath);
                    }

                }
            }

            else if (algorithm == "algorithm2")
            {
                //// ִ执行算法2
                //// ...
                //Bitmap CurPreviewImage = ConvertToBitmap(image);
                ////m_strLog.Clear();
                //tmpStatResults = new StatResults();
                //Stopwatch sw = new Stopwatch();
                ////MVService detect = new MVService();
                //MVDetectResult outResult = new MVDetectResult();
                //MVDetectParameterV2 paramV2 = new MVDetectParameterV2();
                //MVDetectParameter param = new MVDetectParameter();
                ////MVParameter par = new MVParameter();
                //int nRtn = -1;
                ////if (!getDetectParam(ref param, ref paramV2))
                ////    return;
                //string message = "";
                ////参数写入配置文件
                //getConfig();
                //writeConfigParam(conPara, Directory.GetCurrentDirectory() + "\\" + m_strMarkConfig);
                //string selectedOption = IndexModel.SelectedAlgorithm;
                //m_MarkInfo.MarkName = Convert.ToString(selectedOption);
                ////选择靶标类型
                //int nMarkTypeIndex = comboBox_MarkType.SelectedIndex;
                //detRsult.Clear();
                //switch (nMarkTypeIndex)
                //{
                //    case 0: //单孔                    
                //        paramV2.MarkType = param.MarkType = MarkType.SingleCircle;
                //        //m_MarkInfo.MarkType = Convert.ToInt32(MarkType.SingleCircle);
                //        if (checkImark.IsChecked == true)
                //        {
                //            InitLogFile("check_InnerCircle.txt");
                //        }
                //        else
                //        {
                //            InitLogFile("check_Circle.txt");
                //        }
                //        break;
                //    case 1://孔矩阵                    
                //        paramV2.MarkType = param.MarkType = MarkType.CircleMatrix;
                //        //m_MarkInfo.MarkType = Convert.ToInt32(MarkType.CircleMatrix);
                //        InitLogFile("check_HoleMatrix.txt");
                //        break;
                //    case 2://复合孔                    
                //        paramV2.MarkType = param.MarkType = MarkType.ComplexCircles;
                //        //m_MarkInfo.MarkType = Convert.ToInt32(MarkType.ComplexCircles);
                //        InitLogFile("check_" + m_MarkInfo.MarkName + ".txt");
                //        break;
                //    case 3://矩形                    
                //        paramV2.MarkType = param.MarkType = MarkType.Rectangle;
                //        //m_MarkInfo.MarkType = Convert.ToInt32(MarkType.Rectangle);
                //        InitLogFile("check_Rectangle.txt");
                //        break;
                //    case 4://梅花孔                    
                //        paramV2.MarkType = param.MarkType = MarkType.PlumHoles;
                //        //m_MarkInfo.MarkType = Convert.ToInt32(MarkType.PlumHoles);
                //        InitLogFile("check_RingPlum.txt");
                //        break;
                //    case 5://十字
                //        paramV2.MarkType = param.MarkType = MarkType.Cross;
                //        //m_MarkInfo.MarkType = Convert.ToInt32(MarkType.Cross);
                //        InitLogFile("check_Cross.txt");
                //        break;
                //    default:
                //        break;
                //}
                //if (m_strImageFileName != string.Empty)
                //{
                //    //CurPreviewImage = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(m_strImageFileName);
                //    //选择算法
                //    string strMarkName = comboBox_Algorithm.SelectedValue != null ? comboBox_Algorithm.SelectedValue.ToString() : string.Empty;

                //    var mark = complexMarks.FirstOrDefault(m => m.MarkName == strMarkName);
                //    if (mark != null)
                //    {
                //        GCHandle imgHandle = new GCHandle();
                //        try
                //        {
                //            using (CurPreviewImage) using (GrayBitmap bmp = GrayBitmap.FromBitmap(CurPreviewImage))
                //            {
                //                //内层
                //                if (checkImark.IsChecked == true && nMarkTypeIndex == 0)
                //                {
                //                    sw.Start();
                //                    imgHandle = GCHandle.Alloc(bmp, GCHandleType.Pinned);
                //                    nRtn = FindIMark(imgHandle.AddrOfPinnedObject(), param, ref outResult);
                //                    sw.Stop();
                //                }
                //                else//外层靶标
                //                {
                //                    sw.Start();
                //                    imgHandle = GCHandle.Alloc(bmp, GCHandleType.Pinned);
                //                    nRtn = mark.FindMark(imgHandle.AddrOfPinnedObject(), paramV2, ref outResult);
                //                    sw.Stop();
                //                }
                //                detRsult.Add(outResult);
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            Console.WriteLine("检测失败", "提示！");
                //        }
                //        finally
                //        {
                //            //ReadTxtContent(getLogPath());
                //        }
                //    }
                //}
                //if (nRtn == 0)
                //{
                //    DetectedCenter = new Point(outResult.CenterX, outResult.CenterY);
                //    DetectedRadius = outResult.Circles[0].Diameter / 2;
                //    DetectedScore = outResult.Circles[0].Roundness;
                //}
                //else { 
                //    //System.Windows.Forms.MessageBox.Show("靶标抓取失败", "提示！");
                //    message = "靶标抓取失败";
                //}
                //processedImagePath = @"C:\Users\caijunfu\Desktop\Archive\AlgoDisplay\wwwroot\images\bus.jpg";
            }

            else if (algorithm == "algorithm3")
            {
                // ִ执行算法3
                // ...
                processedImagePath = @"images\bus.jpg";
            }

            else if (algorithm == "algorithm4")
            {
                // ִ执行算法4
                // ...
                processedImagePath = @"images\bus.jpg";
            }

            else
            {
                // 未知算法名称
                return Page();
            }

            //Marshal.FreeHGlobal(imgDataPtr);
            // 处理后的图像输出
            //processedImagePath = @"C:\Users\caijunfu\Desktop\Archive\AlgoDisplay\wwwroot\images\bus.jpg";
            //var processedImageData = System.IO.File.ReadAllBytes(processedImagePath);
            originImagePath = "/images/original.jpg";
            processedImagePath = "/images/processed.jpg";
            //ViewData["ProcessedImageUrl"] = processedImagePath;
            //return File(processedImageData, "image/jpeg");
            // 设置视图数据项以便在视图中使用新的文件路径
            ViewData["OriginalImageUrl"] = originImagePath;
            ViewData["ProcessedImageUrl"] = processedImagePath;
            //网页测试根目录地址
            ViewData["webRootPath"] = webRootPath;
            Console.WriteLine(webRootPath);
            return Page();
        }
    }
}