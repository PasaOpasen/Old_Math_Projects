using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Win32;

using Qocr.Application.Wpf.Helpers;
using Qocr.Application.Wpf.Views;
using Qocr.Core.Approximation;
using Qocr.Core.Data.Serialization;
using Qocr.Core.Recognition;
using Qocr.Core.Utils;

namespace Qocr.Application.Wpf.ViewModels
{
    /// <summary>
    /// ViewModel для <see cref="MainWindow"/>.
    /// </summary>
    public class MainWindowViewModel : NotificationObject
    {
        private readonly Dictionary<string, EulerContainer> _eulerContainersCache =
            new Dictionary<string, EulerContainer>();

        private ImageSource _sourceImage;

        private string _selectedLanguage;

        private string _recognitionResult;

        private bool _isBlackAndWhite;

        private Bitmap _sourceBitmap;

        private string _recognitionTime;

        /// <summary>
        /// Создание экземпляра класса <see cref="MainWindowViewModel"/>.
        /// </summary>
        public MainWindowViewModel()
        {
            PropertyChanged += AnyPropertyChanged;
            InitLanguages();
            OpenCommand = new DelegateCommand(OpenHandler, () => !string.IsNullOrEmpty(SelectedLanguage));
            Recognize = new DelegateCommand(
                RecognizeHandler,
                () =>
                    SourceImage != null && SourceImage.Height > 0 && SourceImage.Width > 0 &&
                    !string.IsNullOrEmpty(SelectedLanguage));
        }

        /// <summary>
        /// Исходное изображение выбранное пользователем.
        /// </summary>
        public ImageSource SourceImage
        {
            get
            {
                return _sourceImage;
            }

            private set
            {
                _sourceImage = value;
                RaisePropertyChanged(() => SourceImage);
            }
        }

        /// <summary>
        /// Результат распознания.
        /// </summary>
        public string RecognitionResult
        {
            get
            {
                return _recognitionResult;
            }

            set
            {
                _recognitionResult = value;
                RaisePropertyChanged(() => RecognitionResult);
            }
        }

        /// <summary>
        /// Команда "Открыть".
        /// </summary>
        public DelegateCommand OpenCommand { get; }

        /// <summary>
        /// Команда "Распознать".
        /// </summary>
        public DelegateCommand Recognize { get; }

        /// <summary>
        /// Список доступных языков.
        /// </summary>
        public ObservableCollection<string> Languages { get; } = new ObservableCollection<string>();

        /// <summary>
        /// Выбранный язык.
        /// </summary>
        public string SelectedLanguage
        {
            get
            {
                return _selectedLanguage;
            }
            set
            {
                _selectedLanguage = value;
                RaisePropertyChanged(() => SelectedLanguage);
            }
        }

        /// <summary>
        /// Отобразить изображение в черно белом цвете.
        /// </summary>
        public bool IsBlackAndWhite
        {
            get
            {
                return _isBlackAndWhite;
            }
            set
            {
                _isBlackAndWhite = value;
                RaisePropertyChanged(() => IsBlackAndWhite);
                ChangeToBlackAndWhite();
            }
        }

        /// <summary>
        /// Время распознавания.
        /// </summary>
        public string RecognitionTime
        {
            get
            {
                return _recognitionTime;
            }
            set
            {
                _recognitionTime = value;
                RaisePropertyChanged(() => RecognitionTime);
            }
        }

        private static EulerContainer GetEulerContainer(string path)
        {
            using (var file = File.Open(path, FileMode.Open))
            {
                return CompressionUtils.Decompress<EulerContainer>(file);
            }
        }

        private void RecognizeHandler()
        {
            DateTime dNow = DateTime.Now;
            var container = _eulerContainersCache[SelectedLanguage];
            TextRecognizer recognizer = new TextRecognizer(container);
            var report = recognizer.Recognize(_sourceBitmap);

            // Выводим
            RecognitionTime = (DateTime.Now - dNow).ToString();
            RecognitionResult = report.RawText();

            // визуализируем найденные итоги
            var visualizedImage = new Bitmap(_sourceBitmap);
            RecognitionVisualizerUtils.Visualize(visualizedImage, report);
            SourceImage = visualizedImage.ToBitmapSource();
        }

        private void ChangeToBlackAndWhite()
        {
            if (IsBlackAndWhite)
            {
                var approximator = new LuminosityApproximator();
                SourceImage = approximator.Approximate(_sourceBitmap).ToBitmap().ToBitmapSource();
            }
            else
            {
                SourceImage = _sourceBitmap.ToBitmapSource();
            }
        }

        private void AnyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OpenCommand.RaiseCanExecuteChanged();
            Recognize.RaiseCanExecuteChanged();
        }

        private void InitLanguages()
        {
            const string DictionaryName = "Qocr.Dics";
            var current = new DirectoryInfo(Directory.GetCurrentDirectory());

            do
            {
                DirectoryInfo dicsDir = null;
                if (current.GetDirectories().Any(dic => (dicsDir = dic).Name == DictionaryName))
                {
                    foreach (var file in dicsDir.GetFiles("*.bin"))
                    {
                        var container = GetEulerContainer(file.FullName);
                        foreach (var lang in container.Languages)
                        {
                            _eulerContainersCache.Add(lang.LocalizationName, container);
                            Languages.Add(lang.LocalizationName);
                        }
                    }
                }

                current = current.Parent;
                if (current == null || !current.Exists)
                {
                    break;
                }
            } while (true);
        }

        private void OpenHandler()
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
                using (var bitmapFile = new Bitmap(openFileDialog.FileName))
                {
                    var bitmap = new Bitmap(bitmapFile);
                    _sourceBitmap = bitmap;
                    IsBlackAndWhite = false;
                }
            }
        }
    }
}