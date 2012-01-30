using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace VidPub.Web.Infrastructure {
    public class CSVResult:System.Web.Mvc.FileResult {
        IEnumerable<dynamic> _data;
        public CSVResult(IEnumerable<dynamic> data, string fileName):base("text/csv") {
            _data = data;
            this.FileDownloadName = fileName;
        }

        protected override void WriteFile(HttpResponseBase response) {
            using (var stream = new MemoryStream()) {
                //write the file to the memory stream...
                WriteData(stream);
                //write the memory stream to the response
                response.OutputStream.Write(stream.GetBuffer(), 0, (int)stream.Length);
            }
        }
        void WriteData(Stream stream) {
            var writer = new StreamWriter(stream);

            //add the keys
            var first = _data.First();
            var dc = (IDictionary<string, object>)first;
            foreach (var key in dc.Keys) {
                //need to parse this!
                writer.Write(WriteVal(key));
            }
            writer.WriteLine();
            foreach (var line in _data) {
                dc = (IDictionary<string, object>)line;
                var vals = dc.Values;
                foreach (var v in vals) {
                    writer.Write(WriteVal(v.ToString()) ?? "");
                }
                writer.WriteLine();
            }
            writer.Flush();
        }
        string WriteVal(string val) {
            return string.Format("\"{0}\",",val.Replace("\"", "\"\""));
        }
    }
}