using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using Buffalo.Kernel;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    /// <summary>
    /// 模版到代码的转换器
    /// </summary>
    public class ModelCompiler
    {

        private CodesManger man = new CodesManger();
        private string _content;
        private string _workspace;
        /// <summary>
        /// 模版到代码的转换器
        /// </summary>
        /// <param name="content">模版内容</param>
        public ModelCompiler(string content,string workspace) 
        {
            _content = content;
            _workspace = workspace;
        }


        /// <summary>
        /// 处理Script标签
        /// </summary>
        public string GetCode(string className) 
        {
            //string strRef = @"(?isx)<[#]script\stype=""(?<type>[^""]+)"">(?<content>[^<]+)</[#]script>";
            string strRef = @"(?isx)<[#]script\stype=""(?<type>[^""]+)"">(?<content>(.*?))</[#]script>";
            MatchCollection matches = new Regex(strRef).Matches(_content);
            foreach (Match ma in matches)
            {
                if (ma.Groups["type"] == null) 
                {
                    continue;
                }
                string type = ma.Groups["type"].Value;
                string content=ma.Groups["content"].Value;
                CodeGeneration com=new CodeGeneration(content);
                Queue<ExpressionItem> queitem=com.ExpressionItems;
                if (type.Equals("linked",StringComparison.CurrentCultureIgnoreCase)) 
                {
                    LinkOutputer outputer = new LinkOutputer();
                    List<string> str = outputer.GetCode(queitem,_workspace);
                    man.Link.AddRange(str);
                }
                else if (type.Equals("using", StringComparison.CurrentCultureIgnoreCase)) 
                {
                    UsingOutputer outputer = new UsingOutputer();
                    string str = outputer.GetCode(queitem);
                    man.Using.Append(str);
                }
                else if (type.Equals("code", StringComparison.CurrentCultureIgnoreCase))
                {
                    CodeOutputer outputer = new CodeOutputer();
                    string str = outputer.GetCode(queitem);
                    man.Code.Append(str);
                }
                else if (type.Equals("method", StringComparison.CurrentCultureIgnoreCase))
                {
                    MethodOutputer outputer = new MethodOutputer();
                    string str = outputer.GetCode(queitem);
                    man.Method.Append(str);
                }
            }
            return man.ToCode(className);
        }



        /// <summary>
        /// 获取编译后的类型
        /// </summary>
        /// <param name="errorMessage">如果出错此为信息</param>
        /// <returns></returns>
        public Type GetCompileType(string className, StringBuilder errorMessage)
        {
            string code = GetCode(className);
            string ret = null;
            Type objType = SourceCodeCompiler.DoCompiler(code, CodesManger.CompilerNamespace+"."+className, man.Link, errorMessage);

            return objType;
        }
    }
}
