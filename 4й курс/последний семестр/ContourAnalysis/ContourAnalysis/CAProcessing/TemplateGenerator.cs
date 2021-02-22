using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV;

namespace ContourAnalysisNS
{
    public static class TemplateGenerator
    {
        public static void GenerateChars(ImageProcessor processor, char[] chars, Font font)
        {
            Bitmap bmp = new Bitmap(400, 400);
            font = new Font(font.FontFamily, 140, font.Style);
            Graphics gr = Graphics.FromImage(bmp);
            //
            processor.onlyFindContours = true;
            foreach (char c in chars)
            {
                gr.Clear(Color.White);
                gr.DrawString(c.ToString(), font, Brushes.Black, 5, 5);
                GenerateTemplate(processor, bmp, c.ToString());
            }
            processor.onlyFindContours = false;
        }

        public static void GenerateAntipatterns(ImageProcessor processor)
        {
            Bitmap bmp = new Bitmap(200, 200);
            Graphics gr = Graphics.FromImage(bmp);
            //
            processor.onlyFindContours = true;
            //square
            gr.Clear(Color.White);
            gr.FillRectangle(Brushes.Black, new Rectangle(10, 10, 80, 80));
            GenerateTemplate(processor, bmp, "antipattern");
            //rect1
            gr.Clear(Color.White);
            gr.FillRectangle(Brushes.Black, new Rectangle(10, 10, 50, 100));
            GenerateTemplate(processor, bmp, "antipattern");
            //rect2
            gr.Clear(Color.White);
            gr.FillRectangle(Brushes.Black, new Rectangle(10, 10, 20, 100));
            GenerateTemplate(processor, bmp, "antipattern");
            //circle
            gr.Clear(Color.White);
            gr.FillEllipse(Brushes.Black, new Rectangle(10, 10, 100, 100));
            GenerateTemplate(processor, bmp, "antipattern");

            processor.onlyFindContours = false;
        }

        private static void GenerateTemplate(ImageProcessor processor, Bitmap bmp, string name)
        {
            processor.ProcessImage(new Image<Bgr, byte>(bmp));
            //find max contour
            if (processor.samples.Count > 0)
            {
                processor.samples.Sort((t1, t2) => -t1.sourceArea.CompareTo(t2.sourceArea));
                processor.samples[0].name = name;
                processor.templates.Add(processor.samples[0]);
            }
        }
    }
}
