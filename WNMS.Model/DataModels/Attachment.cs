using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class Attachment
    {
        public int FileId { get; set; }
        public int Affiliation { get; set; }
        public int FileType { get; set; }
        public string FileUrl { get; set; }
        public DateTime UploadTime { get; set; }
        public string Suffix { get; set; }
        public double FileSize { get; set; }
        public string FileUnit { get; set; }
        public byte Classify { get; set; }
        public string FileName { get; set; }
    }
}
