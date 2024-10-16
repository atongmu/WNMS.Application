
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Analysis.Tokenattributes;
using Lucene.Net.Search;
using PanGu;
using PanGu.HighLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Utility.PanGu
{
    public class SplitContent
    {



        /// <summary>
        /// 分词方法
        /// </summary>
        /// <param name="words">待分词内容</param>
        /// <param name="analyzer"></param>
        /// <returns></returns>
        public static string cutWords(string words)
        {
            string resultStr = "";
            //         System.IO.StringReader reader = new System.IO.StringReader(words);
            //         Lucene.Net.Analysis.TokenStream ts = analyzer.TokenStream(words, reader);
            //         bool hasNext = ts.IncrementToken();
            //TermAttribute ita;
            Segment.Init();
            Segment segment = new Segment();
            ICollection<WordInfo> words1 = segment.DoSegment(words);
            foreach (var word in words1)
            {
                resultStr += word.Word + "|";

            }

            //while (hasNext)
            //{

            //    ita = ts.GetAttribute(tr);

            //    resultStr += ts.TermText() + "|";
            //    hasNext = ts.IncrementToken();
            //}
            //ts.CloneAttributes();
            //reader.Close();
            //analyzer.Close();
            return resultStr;
        }
        public static string[] SplitWords(string content)
        {
            List<string> strList = new List<string>();
            Analyzer analyzer = new PanGuAnalyzer();
            TokenStream tokenStream = analyzer.TokenStream("", new StringReader(content));
            Lucene.Net.Analysis.Token token = null;
            while ((token = tokenStream.Next()) != null)
            { //Next继续分词 直至返回null
                strList.Add(token.TermText()); //得到分词后结果
            }
            //return strList.ToArray();
            //List<string> strList = new List<string>();
            //Analyzer analyzer = new PanGuAnalyzer();
            //TokenStream tokenStream = analyzer.TokenStream("", new StringReader(content));
            //Lucene.Net.Analysis.Token token = null;

            //bool hasnext = tokenStream.IncrementToken();
            //while (hasnext)
            //{
            //    string word = token.TermText(); // token.TermText() 取得当前分词
            //    strList.Add(word); //得到分词后结果
            //    hasnext = tokenStream.IncrementToken();
            //}

            return strList.ToArray();
        }
        //需要添加PanGu.HighLight.dll的引用
        /// <summary>
        /// 搜索结果高亮显示
        /// </summary>
        /// <param name="keyword"> 关键字 </param>
        /// <param name="content"> 搜索结果 </param>
        /// <returns> 高亮后结果 </returns>
        public static string HightLight(string keywords, string content)
        {
            //创建HTMLFormatter,参数为高亮单词的前后缀
            SimpleHTMLFormatter simpleHTMLFormatter =
                  new SimpleHTMLFormatter("<font color='red'><b>", "</b></font>");
            //创建 Highlighter ，输入HTMLFormatter 和 盘古分词对象Semgent
            Highlighter highlighter =
                           new Highlighter(simpleHTMLFormatter,
                           new Segment());
            ////设置每个摘要段的字符数
            highlighter.FragmentSize = int.MaxValue;
            string res = "";
            //获取最匹配的摘要段
            res = highlighter.GetBestFragment(keywords, content);
            return res;
            // SimpleHTMLFormatter：这个类是一个HTML的格式类，构造函数有两个，一个是开始标签，一个是结束标签。
            //SimpleHTMLFormatter simpleHTMLFormatter =
            //    new SimpleHTMLFormatter("<font style=\"color:red;" +
            //                            "font-family:'Cambria'\"><b>", "</b></font>");
            //// 创建 Highlighter ，输入HTMLFormatter 和 盘古分词对象Semgent
            //Highlighter highlighter =
            //    new Highlighter(simpleHTMLFormatter,
            //        new Segment());
            //// 设置每个摘要段的字符数
            //highlighter.FragmentSize = int.MaxValue;
            //// 获取最匹配的摘要段
            //var str = highlighter.GetBestFragment(keyword, content);
            //return str;
        }


        public static EarlyWarningPlan SetHighlighter(string[] Keywords, EarlyWarningPlan model)
        {
             
            //SimpleHTMLFormatter simpleHTMLFormatter = new SimpleHTMLFormatter("<font color='red'><b>", "</b></font>");
           // Highlighter highlighter = new Highlighter(simpleHTMLFormatter, new Segment());
            //highlighter.FragmentSize = int.MaxValue;
            

            if (Keywords != null && Keywords.Length > 0)
            {


                foreach (var keyword in Keywords)
                {
                    model.Contents= model.Contents.Replace(keyword, "<font color='red'><b>"+ keyword + "</b></font>");
                    model.Solution= model.Solution.Replace(keyword, "<font color='red'><b>" + keyword + "</b></font>");
                    model.Title= model.Title.Replace(keyword, "<font color='red'><b>" + keyword + "</b></font>");
                    //    string Contents = highlighter.GetBestFragment(Keywords, model.Contents);
                    //string Solution = highlighter.GetBestFragment(Keywords, model.Solution);
                    //string Title = highlighter.GetBestFragment(Keywords, model.Title);
                    //if (!string.IsNullOrEmpty(Contents))
                    //{
                    //    model.Contents = Contents;
                    //}
                    //if (!string.IsNullOrEmpty(Title))
                    //{
                    //    model.Title = Title;
                    //}
                    //if (!string.IsNullOrEmpty(Solution))
                    //{
                    //    model.Solution = Solution;
                    //}

                }

            }

            return model;
        }
    
    }
}
