using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Win32;

using Qocr.Core.Approximation;
using Qocr.Core.Data;
using Qocr.Core.Data.Serialization;
using Qocr.Core.Recognition;
using Qocr.Core.Utils;
using Qocr.Tester.Helpers;
using Qocr.Tester.Windows.Views;

namespace Qocr.Tester.Windows.ViewModels
{
    public class MainWindowViewModel : NotificationObject
    {
        private ImageSource _approximatedImage;

        private ImageSource _sourceImage;

        private bool _isPastProcessing;

        private ImageSource _currentGenImage;

        private TextRecognizer _recognizer;

        private string _eulerValue;

        public MainWindowViewModel()
        {
            AnalyzeCommand = new DelegateCommand(Analyze);
            OpenSourceImageCommand = new DelegateCommand(OpenSourceImage);
            ImagePastCommand = new DelegateCommand(ImagePast);
            GenCommand = new DelegateCommand(GenStart);
            TestCommand = new DelegateCommand(Test);
        }

        public ImageSource CurrentGenImage
        {
            get
            {
                return _currentGenImage;
            }

            set
            {
                _currentGenImage = value;
                RaisePropertyChanged(() => CurrentGenImage);
            }
        }

        public ImageSource SourceImage
        {
            get
            {
                return _sourceImage;
            }
            set
            {
                _sourceImage = value;

                if (value != null)
                {
                    var appx = new LuminosityApproximator();
                    var resultImage = appx.Approximate(BitmapUtils.BitmapFromSource((BitmapSource)value));
                    ApproximatedImage = BitmapUtils.SourceFromBitmap(resultImage.ToBitmap());
                }
                else
                {
                    ApproximatedImage = null;
                }

                RaisePropertyChanged(() => SourceImage);
            }
        }

        public string EulerValue
        {
            get
            {
                return _eulerValue;
            }
            set
            {
                _eulerValue = value;
                RaisePropertyChanged(() => EulerValue);
            }
        }

        public ImageSource ApproximatedImage
        {
            get
            {
                return _approximatedImage;
            }
            set
            {
                _approximatedImage = value;
                RaisePropertyChanged(() => ApproximatedImage);
                if (value != null)
                {
                    EulerValue =
                        EulerCharacteristicComputer.Compute2D(
                            new Monomap(BitmapUtils.BitmapFromSource((BitmapSource)value))).ToString();
                }
            }
        }

        public DelegateCommand TestCommand { get; private set; }

        public DelegateCommand GenCommand { get; private set; }

        public DelegateCommand ImagePastCommand { get; private set; }

        public DelegateCommand OpenSourceImageCommand { get; private set; }

        public DelegateCommand AnalyzeCommand { get; private set; }

        private EulerContainer GetEulerContainer(string path)
        {
            using (var english = File.Open(path, FileMode.Open))
            {
                return CompressionUtils.Decompress<EulerContainer>(english);
            }
        }

        private void Analyze()
        {
            //var dic = GetEulerContainer(@"..\..\..\Qocr.Dics\RU-ru.bin");
            var dic = GetEulerContainer(@"..\..\Qocr.Dics\EN-en.bin");

            DateTime nowInit = DateTime.Now;

            // ИСПОЛЬЗУЙ Gen.bin
            _recognizer = _recognizer ?? new TextRecognizer(dic);

            DateTime nowRecognition = DateTime.Now;
            var bitmap = BitmapUtils.BitmapFromSource((BitmapSource)ApproximatedImage);
            var report = _recognizer.Recognize(bitmap);
            MessageBox.Show(
                $"Init time: {nowRecognition - nowInit}\n\rRecognition time: {DateTime.Now - nowRecognition}");

            RecognitionVisualizerUtils.Visualize(bitmap, report);
            ApproximatedImage = BitmapUtils.SourceFromBitmap(bitmap);
        }

        private void Test()
        {
            //var debugFolder = new DirectoryInfo(@"BmpDebug");
            //foreach (var file in debugFolder.GetFiles())
            //{
            //    file.Delete();
            //}

            //TextRecognizer recognizer = new TextRecognizer();
            //var bm = (Bitmap)Image.FromFile("MiniTest.png");

            //// recognizer.PrintTest = PrintTest;
            //var q = recognizer.Recognize(bm);
        }

        private void GenStart()
        {
            new GeneratorWindow().ShowDialog();
        }

        private void ImagePast()
        {
            if (_isPastProcessing)
            {
                return;
            }

            _isPastProcessing = true;

            var clipboardImage = Clipboard.GetImage();
            if (clipboardImage == null)
            {
                MessageBox.Show("Image clipboard is empty");
                return;
            }

            SourceImage = clipboardImage;

            _isPastProcessing = false;
        }

        private void OpenSourceImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files(*.BMP;*.JPG;*.GIF,*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Multiselect = false
            };

            var result = openFileDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                var bitmap = (Bitmap)Image.FromFile(openFileDialog.FileName);

                BitmapSource sourceImage = Imaging.CreateBitmapSourceFromHBitmap(
                    bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

                SourceImage = sourceImage;
            }
        }
    }
}