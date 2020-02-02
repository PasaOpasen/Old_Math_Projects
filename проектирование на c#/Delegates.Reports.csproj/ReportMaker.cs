using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Reports
{
    public class ReportMaker
    {
        Func<string, string> MakeCaption;
        Func<string> BeginList;
        Func<string, string, string> MakeItem;
        Func<string> EndList;

        public ReportMaker(Func<string, string> MakeCaption,
        Func<string> BeginList,
        Func<string, string, string> MakeItem,
        Func<string> EndList)
        {
            this.MakeCaption = MakeCaption;
            this.BeginList = BeginList;
            this.MakeItem = MakeItem;
            this.EndList = EndList;
        }

        public string MakeReport(IEnumerable<Measurement> measurements, Func<IEnumerable<double>, object> MakeStatistics,string Caption)
        {
            var data = measurements.ToList();
            var result = new StringBuilder();
            result.Append(MakeCaption(Caption));
            result.Append(BeginList());
            result.Append(MakeItem("Temperature", MakeStatistics(data.Select(z => z.Temperature)).ToString()));
            result.Append(MakeItem("Humidity", MakeStatistics(data.Select(z => z.Humidity)).ToString()));
            result.Append(EndList());
            return result.ToString();
        }
    }

    public static class ReportMakerHelper
    {
        static Func<IEnumerable<double>, object> MedianCalc = (data) =>
   {
       var list = data.OrderBy(z => z).ToList();
       if (list.Count % 2 == 0)
           return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;

       return list[list.Count / 2];
   };
        static Func<IEnumerable<double>, object> MeanCalc = (_data) =>
      {
          var data = _data.ToList();
          var mean = data.Average();
          var std = Math.Sqrt(data.Select(z => Math.Pow(z - mean, 2)).Sum() / (data.Count - 1));

          return new MeanAndStd
          {
              Mean = mean,
              Std = std
          };
      };

        public static ReportMaker MarkdownReportMaker = new ReportMaker(
            (caption) => $"## {caption}\n\n",
            () => "",
            (valueType, entry) => $" * **{valueType}**: {entry}\n\n",
            () => ""
            );

        public static ReportMaker HtmlReportMaker = new ReportMaker(
    (caption) => $"<h1>{caption}</h1>",
    () => "<ul>",
    (valueType, entry) => $"<li><b>{valueType}</b>: {entry}",
    () => "</ul>"
    );


        public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data)
        {
            return HtmlReportMaker.MakeReport(data, MeanCalc,"Mean and Std");
        }

        public static string MedianMarkdownReport(IEnumerable<Measurement> data)
        {
            return MarkdownReportMaker.MakeReport(data, MedianCalc, "Median");
        }

        public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> measurements)
        {
            return MarkdownReportMaker.MakeReport(measurements, MeanCalc, "Mean and Std");
        }

        public static string MedianHtmlReport(IEnumerable<Measurement> measurements)
        {
            return HtmlReportMaker.MakeReport(measurements, MedianCalc, "Median");
        }
    }
}
