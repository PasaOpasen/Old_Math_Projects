using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Qocr.Core.Data.Serialization;
using Qocr.Core.Utils;
using Qocr.Generator;
using Qocr.Generator.Data;
using Qocr.Tester.Annotations;
using Qocr.Tester.Helpers;

using FontFamily = System.Drawing.FontFamily;

namespace Qocr.Tester.Windows.Views
{
    public class LanguageInfo
    {
        public LanguageInfo(string name, string displayName, char minChar, char maxChar)
        {
            Name = name;
            DisplayName = displayName;
            MinChar = minChar;
            MaxChar = maxChar;
        }

        public string DisplayName { get; private set; }

        public string Name { get; private set; }
         
        public char MinChar { get; private set; }

        public char MaxChar { get; private set; }

        public string MinFont { get; set; } = 10.ToString();

        public string MaxFont { get; set; } = 36.ToString();
    }

    /// <summary>
    /// Interaction logic for GeneratorWindow.xaml
    /// </summary>
    public partial class GeneratorWindow : INotifyPropertyChanged
    {
        const string LastSelectedFontsFileName = "LastFonts.txt";

        private readonly EulerGenerator _generator = new EulerGenerator();
        
        private int _genImageNumber = 0;

        private Font _currentFont;

        private LanguageInfo _selectedLanguage;

        public GeneratorWindow()
        {
            InitData();
            DataContext = this;
            InitializeComponent();
            UpdateFileFontNames();

        }

        private void UpdateFileFontNames()
        {
            SavedFonts.Text = Environment.NewLine + File.ReadAllText(LastSelectedFontsFileName);
        }

        private void InitData()
        {
            if (!File.Exists(LastSelectedFontsFileName))
            {
                File.WriteAllLines(LastSelectedFontsFileName,
                    new []
                    {
                        "Arial",
                        "Tahoma",
                        "Times New Roman"
                    });
            }
            
            var ruRU = CreateLanguage("RU-ru", "Russian", 'а', 'я');
            var enEN = CreateLanguage("en", "English", 'a', 'z');

            Languages = new List<LanguageInfo>()
            {
                ruRU,
                enEN
            };
        }

        public LanguageInfo SelectedLanguage
        {
            get
            {
                return _selectedLanguage;
            }
            set
            {
                _selectedLanguage = value;
                RaisePropertyChanged(nameof(SelectedLanguage));
            }
        }

        private LanguageInfo CreateLanguage(string name, string displayName, char minChar, char maxChar)
        {
            return new LanguageInfo(name, displayName, minChar, maxChar);
        }

        public IEnumerable<LanguageInfo> Languages { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Font CurrentFont
        {
            get
            {
                return _currentFont;
            }
            set
            {
                if (Equals(value, _currentFont))
                {
                    return;
                }

                _currentFont?.Dispose();
                _currentFont = value;
                RaisePropertyChanged(nameof(CurrentFont));
            }
        }

        public static System.Drawing.Bitmap CombineBitmap(params Bitmap[] bitmaps)
        {
            //read all images into memory
            List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();
            System.Drawing.Bitmap finalImage = null;

            try
            {
                int width = 0;
                int height = 0;

                foreach (Bitmap bitmap in bitmaps)
                {
                    //update the size of the final bitmap
                    width += bitmap.Width;
                    height = bitmap.Height > height ? bitmap.Height : height;

                    images.Add(bitmap);
                }

                //create a bitmap to hold the combined image
                finalImage = new System.Drawing.Bitmap(width, height);

                //get a graphics object from the image so we can draw on it
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                {
                    //set background color
                    //g.Clear(System.Drawing.Color.Black);

                    //go through each image and draw it on the final image
                    int offset = 0;
                    foreach (System.Drawing.Bitmap image in images)
                    {
                        g.DrawImage(image, new System.Drawing.Rectangle(offset, 0, image.Width, image.Height));
                        offset += image.Width;
                    }
                }

                return finalImage;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                {
                    finalImage.Dispose();
                }

                throw ex;
            }
            finally
            {
                //clean up memory
                foreach (System.Drawing.Bitmap image in images)
                {
                    image.Dispose();
                }
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private async void OnStartClick(object sender, RoutedEventArgs e)
        {
            const string DefaultDictionaries = "../../Qocr.Dics";
            if (!Directory.Exists(DefaultDictionaries))
            {
                MessageBox.Show("Check \"Qocr.Dics\" Folder existence");
                return;
            }

            this.IsEnabled = false;
            _genImageNumber = 0;

            if (IsPrintDebug.IsChecked.GetValueOrDefault())
            {
                RecreateTestDir();
            }

            DateTime dNow = DateTime.Now;

            var container = new EulerContainer();
            var fontFamilies = GetFileFonts().ToArray();
            
            if (IsPrintDebug.IsChecked.GetValueOrDefault())
            {
                _generator.BitmapCreated += GeneratorOnBitmapCreated;
            }

            var lang = await GenerateLanguage(
                SelectedLanguage.Name, 
                int.Parse(SelectedLanguage.MinFont),
                int.Parse(SelectedLanguage.MaxFont),
                SelectedLanguage.MinChar,
                SelectedLanguage.MaxChar, 
                fontFamilies);

            var specialChars = new[]
            {
                '0',
                '1',
                '2',
                '3',
                '4',
                '5',
                '6',
                '7',
                '8',
                '9',
                '@',
                '$',
                '#',
                '&',
                '(',
                ')',
                '*',
                '/',
                '\\'
            };

            var specialCharsResult = await _generator.GenerateSpecialChars(
                specialChars, 
                int.Parse(SelectedLanguage.MinFont),
                int.Parse(SelectedLanguage.MaxFont), 
                fontFamilies);
            
            lang.FontFamilyNames = fontFamilies.Select(font => font.Name).ToList();
            
            container.Languages.Add(lang);
            container.SpecialChars = specialCharsResult;

            var compression = CompressionUtils.Compress(container);
            using (FileStream fileStream = new FileStream(Path.Combine(DefaultDictionaries, $"{SelectedLanguage.Name}.bin"), FileMode.Create))
            {
                compression.Position = 0;
                compression.CopyTo(fileStream);
            }

            MessageBox.Show($"Время создания {DateTime.Now - dNow}");
            IsEnabled = true;
        }

        private IEnumerable<FontFamily> GetFileFonts()
        {
            var lastFonts = File.ReadAllLines(LastSelectedFontsFileName);
            return
                lastFonts.Select(font => System.Drawing.FontFamily.Families.First(f => f.Name == font))
                    .ToArray();

        }

        private void RecreateTestDir()
        {
            if (Directory.Exists("BmpDebug"))
            {
                Directory.Delete("BmpDebug", true);
            }

            Directory.CreateDirectory("BmpDebug");
            while (!Directory.Exists("BmpDebug"))
            {
                ;
            }
        }

        private async Task<Language> GenerateLanguage(
            string localization,
            int minFont,
            int maxFont,
            char startChr,
            char endChr,
            FontFamily[] fontFamilies)
        {
            List<char> chars = new List<char>();
            for (char c = startChr; c <= endChr; c++)
            {
                chars.Add(c);
            }

            return await _generator.GenerateLanguage(chars.ToArray(), minFont, maxFont, localization, fontFamilies);
        }

        private void GeneratorOnBitmapCreated(object sender, BitmapEventArgs args)
        {
            Application.Current.Dispatcher.Invoke(
                new Action(
                    () =>
                    {
                        try
                        {
                            var chr = args.Chr;
                            var bmp = args.GeneratedBitmap;
                            var font = args.CurrentFont;
                            var fontName = font.FontFamily.Name;
                            var fontSize = font.Size;
                            var fontStyle = font.Style;
                            var debugDir = "BmpDebug";

                            var chrEx = chr.ToString();
                            if (char.IsLetter(chr))
                            {
                                chrEx = char.IsUpper(chr) ? $"^{chr}" : $"{chr}";
                            }

                            var chrDir = Path.Combine(debugDir, chrEx);
                            if (!Directory.Exists(chrDir))
                            {
                                Directory.CreateDirectory(chrDir);
                                while (!Directory.Exists(chrDir))
                                {
                                    ;
                                }
                            }

                            var debugFileName = $"{fontName} {fontSize} {fontStyle}.png";

                            //bmp.Save(Path.Combine(chrDir, debugFileName), ImageFormat.Png);

                            PreviewImage.Source = BitmapUtils.SourceFromBitmap(bmp);

                            _genImageNumber++;
                            bmp.Dispose();
                            CurrentFont = args.CurrentFont;
                        }
                        catch (Exception)
                        {
                        }
                    }));
        }

        private void OnNewFontsClick(object sender, RoutedEventArgs e)
        {
            List<FontFamily> allowedFonts = new List<FontFamily>();
            foreach (var fontFamily in System.Drawing.FontFamily.Families)
            {
                var fontStyle = fontFamily.IsStyleAvailable(System.Drawing.FontStyle.Regular)
                    ? System.Drawing.FontStyle.Regular
                    : fontFamily.IsStyleAvailable(System.Drawing.FontStyle.Italic)
                        ? System.Drawing.FontStyle.Italic
                        : System.Drawing.FontStyle.Bold;

                var tempFont = new Font(fontFamily, 10, fontStyle, GraphicsUnit.Pixel);
                var preview = new[]
                {
                    EulerGenerator.PrintChar('ъ', tempFont).ToBitmap(),
                    EulerGenerator.PrintChar('ф', tempFont).ToBitmap(),
                    EulerGenerator.PrintChar('е', tempFont).ToBitmap(),
                    EulerGenerator.PrintChar('А', tempFont).ToBitmap(),
                    EulerGenerator.PrintChar('Ы', tempFont).ToBitmap(),
                    EulerGenerator.PrintChar('ф', tempFont).ToBitmap(),
                    EulerGenerator.PrintChar('у', tempFont).ToBitmap(),
                    EulerGenerator.PrintChar('ю', tempFont).ToBitmap(),
                    EulerGenerator.PrintChar('я', tempFont).ToBitmap(),
                    EulerGenerator.PrintChar('а', tempFont).ToBitmap(),
                    EulerGenerator.PrintChar('м', tempFont).ToBitmap(),
                };

                PreviewImage.Source = BitmapUtils.SourceFromBitmap(CombineBitmap(preview));

                var dialogResult = MessageBox.Show(
                    $"\"{fontFamily.Name}\" Используем ?",
                    "",
                    MessageBoxButton.YesNoCancel);

                if (dialogResult == MessageBoxResult.Cancel)
                {
                    PreviewImage.Source = null;
                    return;
                }

                if (dialogResult == MessageBoxResult.Yes)
                {
                    allowedFonts.Add(fontFamily);
                }
            }

            File.WriteAllLines(LastSelectedFontsFileName, allowedFonts.Select(font => font.Name));
            UpdateFileFontNames();
        }
    }
}