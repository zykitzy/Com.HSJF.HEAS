using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Com.HSJF.Infrastructure.Utility
{
    /// <example>
    ///  HttpHelper http = new HttpHelper();
    ///  HttpItem item = new HttpItem()
    ///  {
    ///   URL = "http://www.cckan.net",//URL     必需项
    ///   Encoding = "gbk",//编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别
    ///   Method = "get",//URL     可选项 默认为Get
    ///   //Timeout = 100000,//连接超时时间     可选项默认为100000
    ///   //ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000
    ///   //IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写
    ///   Cookie = "",//字符串Cookie     可选项
    ///   // UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)",//用户的浏览器类型，版本，操作系统     可选项有默认值
    ///   // Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值
    ///   // ContentType = "text/html",//返回类型    可选项有默认值
    ///   Referer = "http://www.cckan.net",//来源URL     可选项
    ///   //Allowautoredirect = true,//是否根据３０１跳转     可选项
    ///   //CerPath = "d:\\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数
    ///   //Connectionlimit = 1024,//最大连接数     可选项 默认为1024
    ///   //Postdata = "username=sufei&pwd=cckan.net",//Post数据     可选项GET时不需要写
    ///   //ProxyIp = "192.168.1.105",//代理服务器ID     可选项 不需要代理 时可以不设置这三个参数
    ///   //ProxyPwd = "123456",//代理服务器密码     可选项
    ///   // ProxyUserName = "administrator",//代理服务器账户名     可选项
    ///  };
    ///  //得到HTML代码
    ///  string html = http.GetHtml(item);
    ///
    ///  //取出返回的Cookie
    ///  string cookie = item.Cookie;
    ///  //取出返回的Request
    ///  HttpWebRequest request = item.Request;
    ///  //取出返回的Response
    ///  HttpWebResponse response = item.Response;
    ///  //取出返回的Reader
    ///  StreamReader reader = item.Reader;
    ///  //取出返回的Headers
    ///  WebHeaderCollection header = response.Headers;
    /// </example>

    /// <summary>
    /// Http连接操作帮助类
    /// </summary>
    public class HttpHelper
    {
        #region 预定义方法或者变更

        //默认的编码
        private Encoding encoding = Encoding.Default;
        //Post数据编码
        private Encoding postencoding = Encoding.Default;
        //HttpWebRequest对象用来发起请求
        private HttpWebRequest request = null;
        //获取影响流的数据对象
        private HttpWebResponse response = null;
        /// <summary>
        /// 根据相传入的数据，得到相应页面数据
        /// </summary>
        /// <param name="objhttpitem">参数类对象</param>
        /// <returns>返回HttpResult类型</returns>
        public HttpResult GetHtml(HttpItem objhttpitem)
        {
            //返回参数
            HttpResult result = new HttpResult();
            try
            {
                //准备参数
                SetRequest(objhttpitem);
            }
            catch (Exception ex)
            {
                result = new HttpResult();
                result.Cookie = string.Empty;
                result.Header = null;
                result.Html = ex.Message;
                result.StatusDescription = "配置参数时出错：" + ex.Message;
                return result;
            }
            try
            {
                #region 得到请求的response
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    result.StatusCode = response.StatusCode;
                    result.StatusDescription = response.StatusDescription;
                    result.Header = response.Headers;
                    if (response.Cookies != null) result.CookieCollection = response.Cookies;
                    if (response.Headers["set-cookie"] != null) result.Cookie = response.Headers["set-cookie"];
                    MemoryStream _stream = new MemoryStream();
                    //GZIIP处理
                    if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //开始读取流并设置编码方式
                        //new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(_stream, 10240);
                        //.net4.0以下写法
                        _stream = GetMemoryStream(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress));
                    }
                    else
                    {
                        //开始读取流并设置编码方式
                        //response.GetResponseStream().CopyTo(_stream, 10240);
                        //.net4.0以下写法
                        _stream = GetMemoryStream(response.GetResponseStream());
                    }
                    //获取Byte
                    byte[] RawResponse = _stream.ToArray();
                    _stream.Close();
                    //是否返回Byte类型数据
                    if (objhttpitem.ResultType == ResultType.Byte) result.ResultByte = RawResponse;
                    //从这里开始我们要无视编码了
                    if (encoding == null)
                    {
                        Match meta = Regex.Match(Encoding.Default.GetString(RawResponse), "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase);
                        string charter = (meta.Groups.Count > 1) ? meta.Groups[2].Value.ToLower() : string.Empty;
                        if (charter.Length > 2)
                            encoding = Encoding.GetEncoding(charter.Trim().Replace("\"", "").Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk"));
                        else
                        {
                            if (string.IsNullOrEmpty(response.CharacterSet)) encoding = Encoding.UTF8;
                            else encoding = Encoding.GetEncoding(response.CharacterSet);
                        }
                    }
                    //得到返回的HTML
                    result.Html = encoding.GetString(RawResponse);
                }
                #endregion
            }
            catch (WebException ex)
            {
                //这里是在发生异常时返回的错误信息
                response = (HttpWebResponse)ex.Response;
                result.Html = ex.Message;
                if (response != null)
                {
                    result.StatusCode = response.StatusCode;
                    result.StatusDescription = response.StatusDescription;
                }
            }
            catch (Exception ex)
            {
                result.Html = ex.Message;
            }
            if (objhttpitem.IsToLower) result.Html = result.Html.ToLower();
            return result;
        }
        /// <summary>
        /// 4.0以下.net版本取数据使用
        /// </summary>
        /// <param name="streamResponse">流</param>
        private static MemoryStream GetMemoryStream(Stream streamResponse)
        {
            MemoryStream _stream = new MemoryStream();
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = streamResponse.Read(buffer, 0, Length);
            while (bytesRead > 0)
            {
                _stream.Write(buffer, 0, bytesRead);
                bytesRead = streamResponse.Read(buffer, 0, Length);
            }
            return _stream;
        }
        /// <summary>
        /// 为请求准备参数
        /// </summary>
        ///<param name="objhttpItem">参数列表</param>
        private void SetRequest(HttpItem objhttpItem)
        {
            // 验证证书
            SetCer(objhttpItem);
            //设置Header参数
            if (objhttpItem.Header != null && objhttpItem.Header.Count > 0) foreach (string item in objhttpItem.Header.AllKeys)
                {
                    request.Headers.Add(item, objhttpItem.Header[item]);
                }
            // 设置代理
            SetProxy(objhttpItem);
            if (objhttpItem.ProtocolVersion != null) request.ProtocolVersion = objhttpItem.ProtocolVersion;
            request.ServicePoint.Expect100Continue = objhttpItem.Expect100Continue;
            //请求方式Get或者Post
            request.Method = objhttpItem.Method;
            request.Timeout = objhttpItem.Timeout;
            request.ReadWriteTimeout = objhttpItem.ReadWriteTimeout;
            //Accept
            request.Accept = objhttpItem.Accept;
            //ContentType返回类型
            request.ContentType = objhttpItem.ContentType;
            //UserAgent客户端的访问类型，包括浏览器版本和操作系统信息
            request.UserAgent = objhttpItem.UserAgent;
            // 编码
            encoding = objhttpItem.Encoding;
            //设置Cookie
            SetCookie(objhttpItem);
            //来源地址
            request.Referer = objhttpItem.Referer;
            //是否执行跳转功能
            request.AllowAutoRedirect = objhttpItem.Allowautoredirect;
            //设置Post数据
            SetPostData(objhttpItem);
            //设置最大连接
            if (objhttpItem.Connectionlimit > 0) request.ServicePoint.ConnectionLimit = objhttpItem.Connectionlimit;
        }
        /// <summary>
        /// 设置证书
        /// </summary>
        /// <param name="objhttpItem"></param>
        private void SetCer(HttpItem objhttpItem)
        {
            if (!string.IsNullOrEmpty(objhttpItem.CerPath))
            {
                //这一句一定要写在创建连接的前面。使用回调的方法进行证书验证。
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                //初始化对像，并设置请求的URL地址
                request = (HttpWebRequest)WebRequest.Create(objhttpItem.URL);
                SetCerList(objhttpItem);
                //将证书添加到请求里
                request.ClientCertificates.Add(new X509Certificate(objhttpItem.CerPath));
            }
            else
            {
                //初始化对像，并设置请求的URL地址
                request = (HttpWebRequest)WebRequest.Create(objhttpItem.URL);
                SetCerList(objhttpItem);
            }
        }
        /// <summary>
        /// 设置多个证书
        /// </summary>
        /// <param name="objhttpItem"></param>
        private void SetCerList(HttpItem objhttpItem)
        {
            if (objhttpItem.ClentCertificates != null && objhttpItem.ClentCertificates.Count > 0)
            {
                foreach (X509Certificate item in objhttpItem.ClentCertificates)
                {
                    request.ClientCertificates.Add(item);
                }
            }
        }
        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="objhttpItem">Http参数</param>
        private void SetCookie(HttpItem objhttpItem)
        {
            if (!string.IsNullOrEmpty(objhttpItem.Cookie))
                //Cookie
                request.Headers[HttpRequestHeader.Cookie] = objhttpItem.Cookie;
            //设置Cookie
            if (objhttpItem.CookieCollection != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(objhttpItem.CookieCollection);
            }
        }
        /// <summary>
        /// 设置Post数据
        /// </summary>
        /// <param name="objhttpItem">Http参数</param>
        private void SetPostData(HttpItem objhttpItem)
        {
            //验证在得到结果时是否有传入数据
            if (request.Method.Trim().ToLower().Contains("post"))
            {
                if (objhttpItem.PostEncoding != null)
                {
                    postencoding = objhttpItem.PostEncoding;
                }
                byte[] buffer = null;
                //写入Byte类型
                if (objhttpItem.PostDataType == PostDataType.Byte && objhttpItem.PostdataByte != null && objhttpItem.PostdataByte.Length > 0)
                {
                    //验证在得到结果时是否有传入数据
                    buffer = objhttpItem.PostdataByte;
                }//写入文件
                else if (objhttpItem.PostDataType == PostDataType.FilePath && !string.IsNullOrEmpty(objhttpItem.Postdata))
                {
                    StreamReader r = new StreamReader(objhttpItem.Postdata, postencoding);
                    buffer = postencoding.GetBytes(r.ReadToEnd());
                    r.Close();
                } //写入字符串
                else if (!string.IsNullOrEmpty(objhttpItem.Postdata))
                {
                    buffer = postencoding.GetBytes(objhttpItem.Postdata);
                }
                if (buffer != null)
                {
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
            }
        }
        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="objhttpItem">参数对象</param>
        private void SetProxy(HttpItem objhttpItem)
        {
            if (!string.IsNullOrEmpty(objhttpItem.ProxyIp))
            {
                //设置代理服务器
                if (objhttpItem.ProxyIp.Contains(":"))
                {
                    string[] plist = objhttpItem.ProxyIp.Split(':');
                    WebProxy myProxy = new WebProxy(plist[0].Trim(), Convert.ToInt32(plist[1].Trim()));
                    //建议连接
                    myProxy.Credentials = new NetworkCredential(objhttpItem.ProxyUserName, objhttpItem.ProxyPwd);
                    //给当前请求对象
                    request.Proxy = myProxy;
                }
                else
                {
                    WebProxy myProxy = new WebProxy(objhttpItem.ProxyIp, false);
                    //建议连接
                    myProxy.Credentials = new NetworkCredential(objhttpItem.ProxyUserName, objhttpItem.ProxyPwd);
                    //给当前请求对象
                    request.Proxy = myProxy;
                }
                //设置安全凭证
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
        }
        /// <summary>
        /// 回调验证证书问题
        /// </summary>
        /// <param name="sender">流对象</param>
        /// <param name="certificate">证书</param>
        /// <param name="chain">X509Chain</param>
        /// <param name="errors">SslPolicyErrors</param>
        /// <returns>bool</returns>
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        #endregion
    }
    /// <summary>
    /// Http请求参考类
    /// </summary>
    public class HttpItem
    {
        string _URL = string.Empty;
        /// <summary>
        /// 请求URL必须填写
        /// </summary>
        public string URL
        {
            get { return _URL; }
            set { _URL = value; }
        }
        string _Method = "GET";
        /// <summary>
        /// 请求方式默认为GET方式,当为POST方式时必须设置Postdata的值
        /// </summary>
        public string Method
        {
            get { return _Method; }
            set { _Method = value; }
        }
        int _Timeout = 20000;
        /// <summary>
        /// 默认请求超时时间
        /// </summary>
        public int Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }
        int _ReadWriteTimeout = 30000;
        /// <summary>
        /// 默认写入Post数据超时时间
        /// </summary>
        public int ReadWriteTimeout
        {
            get { return _ReadWriteTimeout; }
            set { _ReadWriteTimeout = value; }
        }
        string _Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
        /// <summary>
        /// 请求标头值 默认为text/html, application/xhtml+xml, */*
        /// </summary>
        public string Accept
        {
            get { return _Accept; }
            set { _Accept = value; }
        }
        string _ContentType = "text/html";
        /// <summary>
        /// 请求返回类型默认 text/html
        /// </summary>
        public string ContentType
        {
            get { return _ContentType; }
            set { _ContentType = value; }
        }
        string _UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
        /// <summary>
        /// 客户端访问信息默认Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)
        /// </summary>
        public string UserAgent
        {
            get { return _UserAgent; }
            set { _UserAgent = value; }
        }
        Encoding _Encoding = null;
        /// <summary>
        /// 返回数据编码默认为NUll,可以自动识别,一般为utf-8,gbk,gb2312
        /// </summary>
        public Encoding Encoding
        {
            get { return _Encoding; }
            set { _Encoding = value; }
        }
        private PostDataType _PostDataType = PostDataType.String;
        /// <summary>
        /// Post的数据类型
        /// </summary>
        public PostDataType PostDataType
        {
            get { return _PostDataType; }
            set { _PostDataType = value; }
        }
        string _Postdata = string.Empty;
        /// <summary>
        /// Post请求时要发送的字符串Post数据
        /// </summary>
        public string Postdata
        {
            get { return _Postdata; }
            set { _Postdata = value; }
        }
        private byte[] _PostdataByte = null;
        /// <summary>
        /// Post请求时要发送的Byte类型的Post数据
        /// </summary>
        public byte[] PostdataByte
        {
            get { return _PostdataByte; }
            set { _PostdataByte = value; }
        }
        CookieCollection cookiecollection = null;
        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection
        {
            get { return cookiecollection; }
            set { cookiecollection = value; }
        }
        string _Cookie = string.Empty;
        /// <summary>
        /// 请求时的Cookie
        /// </summary>
        public string Cookie
        {
            get { return _Cookie; }
            set { _Cookie = value; }
        }
        string _Referer = string.Empty;
        /// <summary>
        /// 来源地址，上次访问地址
        /// </summary>
        public string Referer
        {
            get { return _Referer; }
            set { _Referer = value; }
        }
        string _CerPath = string.Empty;
        /// <summary>
        /// 证书绝对路径
        /// </summary>
        public string CerPath
        {
            get { return _CerPath; }
            set { _CerPath = value; }
        }
        private Boolean isToLower = false;
        /// <summary>
        /// 是否设置为全文小写，默认为不转化
        /// </summary>
        public Boolean IsToLower
        {
            get { return isToLower; }
            set { isToLower = value; }
        }
        private Boolean allowautoredirect = false;
        /// <summary>
        /// 支持跳转页面，查询结果将是跳转后的页面，默认是不跳转
        /// </summary>
        public Boolean Allowautoredirect
        {
            get { return allowautoredirect; }
            set { allowautoredirect = value; }
        }
        private int connectionlimit = 1024;
        /// <summary>
        /// 最大连接数
        /// </summary>
        public int Connectionlimit
        {
            get { return connectionlimit; }
            set { connectionlimit = value; }
        }
        private string proxyusername = string.Empty;
        /// <summary>
        /// 代理Proxy 服务器用户名
        /// </summary>
        public string ProxyUserName
        {
            get { return proxyusername; }
            set { proxyusername = value; }
        }
        private string proxypwd = string.Empty;
        /// <summary>
        /// 代理 服务器密码
        /// </summary>
        public string ProxyPwd
        {
            get { return proxypwd; }
            set { proxypwd = value; }
        }
        private string proxyip = string.Empty;
        /// <summary>
        /// 代理 服务IP
        /// </summary>
        public string ProxyIp
        {
            get { return proxyip; }
            set { proxyip = value; }
        }
        private ResultType resulttype = ResultType.String;
        /// <summary>
        /// 设置返回类型String和Byte
        /// </summary>
        public ResultType ResultType
        {
            get { return resulttype; }
            set { resulttype = value; }
        }
        private WebHeaderCollection header = new WebHeaderCollection();
        /// <summary>
        /// header对象
        /// </summary>
        public WebHeaderCollection Header
        {
            get { return header; }
            set { header = value; }
        }

        private Version _ProtocolVersion;

        /// <summary>
        /// 获取或设置用于请求的 HTTP 版本。返回结果:用于请求的 HTTP 版本。默认为 System.Net.HttpVersion.Version11。
        /// </summary>
        public Version ProtocolVersion
        {
            get { return _ProtocolVersion; }
            set { _ProtocolVersion = value; }
        }
        private Boolean _expect100continue = true;
        /// <summary>
        /// 获取或设置一个 System.Boolean 值，该值确定是否使用 100-Continue 行为。如果 POST 请求需要 100-Continue 响应，则为 true；否则为 false。默认值为 true。
        /// </summary>
        public Boolean Expect100Continue
        {
            get { return _expect100continue; }
            set { _expect100continue = value; }
        }
        private X509CertificateCollection _ClentCertificates;
        /// <summary>
        /// 设置509证书集合
        /// </summary>
        public X509CertificateCollection ClentCertificates
        {
            get { return _ClentCertificates; }
            set { _ClentCertificates = value; }
        }
        private Encoding _PostEncoding;
        /// <summary>
        /// 设置或获取Post参数编码,默认的为Default编码
        /// </summary>
        public Encoding PostEncoding
        {
            get { return _PostEncoding; }
            set { _PostEncoding = value; }
        }
    }
    /// <summary>
    /// Http返回参数类
    /// </summary>
    public class HttpResult
    {
        private string _Cookie;
        /// <summary>
        /// Http请求返回的Cookie
        /// </summary>
        public string Cookie
        {
            get { return _Cookie; }
            set { _Cookie = value; }
        }

        private CookieCollection _CookieCollection;
        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection
        {
            get { return _CookieCollection; }
            set { _CookieCollection = value; }
        }
        private string _Html;
        /// <summary>
        /// 返回的String类型数据 只有ResultType.String时才返回数据，其它情况为空
        /// </summary>
        public string Html
        {
            get { return _Html; }
            set { _Html = value; }
        }
        private byte[] _ResultByte;
        /// <summary>
        /// 返回的Byte数组 只有ResultType.Byte时才返回数据，其它情况为空
        /// </summary>
        public byte[] ResultByte
        {
            get { return _ResultByte; }
            set { _ResultByte = value; }
        }

        private WebHeaderCollection _Header;
        /// <summary>
        /// header对象
        /// </summary>
        public WebHeaderCollection Header
        {
            get { return _Header; }
            set { _Header = value; }
        }

        private string _StatusDescription;
        /// <summary>
        /// 返回状态说明
        /// </summary>
        public string StatusDescription
        {
            get { return _StatusDescription; }
            set { _StatusDescription = value; }
        }
        private HttpStatusCode _StatusCode;
        /// <summary>
        /// 返回状态码,默认为OK
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get { return _StatusCode; }
            set { _StatusCode = value; }
        }
    }
    /// <summary>
    /// 返回类型
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// 表示只返回字符串 只有Html有数据
        /// </summary>
        String,
        /// <summary>
        /// 表示返回字符串和字节流 ResultByte和Html都有数据返回
        /// </summary>
        Byte
    }
    /// <summary>
    /// Post的数据格式默认为string
    /// </summary>
    public enum PostDataType
    {
        /// <summary>
        /// 字符串类型，这时编码Encoding可不设置
        /// </summary>
        String,
        /// <summary>
        /// Byte类型，需要设置PostdataByte参数的值编码Encoding可设置为空
        /// </summary>
        Byte,
        /// <summary>
        /// 传文件，Postdata必须设置为文件的绝对路径，必须设置Encoding的值
        /// </summary>
        FilePath
    }

    public class CookieParser
    {
        public struct pairItem
        {
            public string key;
            public string value;
        };

        #region Fields

        const char replacedChar = '_';

        CookieCollection curCookies = null;

        string[] cookieFieldArr = { "expires", "domain", "secure", "path", "httponly", "version" };
        List<string> cookieFieldList = new List<string>();

        private Dictionary<string, DateTime> calcTimeList;

        #endregion


        #region Constructor

        public CookieParser()
        {
            //http related
            //set max enough to avoid http request is used out -> avoid dead while get response
            System.Net.ServicePointManager.DefaultConnectionLimit = 512;

            curCookies = new CookieCollection();

            // init const cookie keys
            foreach (string key in cookieFieldArr)
            {
                cookieFieldList.Add(key);
            }

            //init for calc time
            calcTimeList = new Dictionary<string, DateTime>();
        }

        #endregion


        #region public funcs

        #region int double

        /// <summary>
        /// equivalent of Math.Random() in Javascript
        /// get a 17 bit double value x, 0 < x < 1, eg:0.68637410117610087
        /// </summary>
        /// <returns></returns>
        public double MathRandom()
        {
            Random rdm = new Random();
            double betweenZeroToOne17Bit = rdm.NextDouble();
            return betweenZeroToOne17Bit;
        }

        #endregion

        #region Time

        /// <summary>
        /// init for calculate time span
        /// </summary>
        /// <param name="keyName"></param>
        public void ElapsedTimeSpanInit(string keyName)
        {
            calcTimeList.Add(keyName, DateTime.Now);
        }

        /// <summary>
        /// got calculated time span
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public double GetElapsedTimeSpan(string keyName)
        {
            double milliSec = 0.0;
            if (calcTimeList.ContainsKey(keyName))
            {
                DateTime startTime = calcTimeList[keyName];
                DateTime endTime = DateTime.Now;
                milliSec = (endTime - startTime).TotalMilliseconds;
            }
            return milliSec;
        }

        /// <summary>
        /// refer: http://bytes.com/topic/c-sharp/answers/713458-c-function-equivalent-javascript-gettime-function
        /// get current time in milli-second-since-epoch(1970/01/01)
        /// </summary>
        /// <returns></returns>
        public double GetCurTimeInMillisec()
        {
            DateTime st = new DateTime(1970, 1, 1);
            TimeSpan t = (DateTime.Now - st);
            return t.TotalMilliseconds; // milli seconds since epoch
        }

        /// <summary>
        /// parse the milli second to local DateTime value
        /// </summary>
        /// <param name="milliSecSinceEpoch"></param>
        /// <returns></returns>
        public DateTime MilliSecToDateTime(double milliSecSinceEpoch)
        {
            DateTime st = new DateTime(1970, 1, 1, 0, 0, 0);
            st = st.AddMilliseconds(milliSecSinceEpoch);
            return st;
        }

        #endregion

        #region String

        /// <summary>
        /// encode "!" to "%21"
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public string EncodeExclamationMark(string inputStr)
        {
            return inputStr.Replace("!", "%21");
        }

        /// <summary>
        /// Decode "%21" to "!"
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public string DecodeExclamationMark(string inputStr)
        {
            return inputStr.Replace("%21", "!");
        }

        /// <summary>
        /// using Regex to extract single string value
        /// caller should make sure the string to extract is Groups[1] == include single () !!!
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="extractFrom"></param>
        /// <param name="extractedStr"></param>
        /// <returns></returns>
        public bool ExtractSingleStr(string pattern, string extractFrom, out string extractedStr)
        {
            bool extractOK = false;
            Regex rx = new Regex(pattern);
            Match found = rx.Match(extractFrom);
            if (found.Success)
            {
                extractOK = true;
                extractedStr = found.Groups[1].ToString();
            }
            else
            {
                extractOK = false;
                extractedStr = "";
            }

            return extractOK;
        }

        /// <summary>
        /// quote the input dict values
        /// note: the return result for first para no '&'
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        public string QuoteParas(Dictionary<string, string> paras)
        {
            string quotedParas = "";
            bool isFirst = true;
            string val = "";
            foreach (string para in paras.Keys)
            {
                if (paras.TryGetValue(para, out val))
                {
                    if (isFirst)
                    {
                        isFirst = false;
                        quotedParas += para + "=" + HttpUtility.UrlPathEncode(val);
                    }
                    else
                    {
                        quotedParas += "&" + para + "=" + HttpUtility.UrlPathEncode(val);
                    }
                }
                else
                {
                    break;
                }
            }

            return quotedParas;
        }

        /// <summary>
        /// remove invalid char in path and filename
        /// </summary>
        /// <param name="origFileOrPathStr"></param>
        /// <returns></returns>
        public string RemoveInvChrInPath(string origFileOrPathStr)
        {
            string validFileOrPathStr = origFileOrPathStr;

            //filter out invalid title and artist char
            //char[] invalidChars = { '\\', '/', ':', '*', '?', '<', '>', '|', '\b' };
            char[] invalidChars = Path.GetInvalidPathChars();
            char[] invalidCharsInName = Path.GetInvalidFileNameChars();

            foreach (char chr in invalidChars)
            {
                validFileOrPathStr = validFileOrPathStr.Replace(chr.ToString(), "");
            }

            foreach (char chr in invalidCharsInName)
            {
                validFileOrPathStr = validFileOrPathStr.Replace(chr.ToString(), "");
            }

            return validFileOrPathStr;
        }


        #endregion

        #region Array

        /// <summary>
        /// given a string array 'origStrArr', get a sub string array from 'startIdx', length is 'len'
        /// </summary>
        /// <param name="origStrArr"></param>
        /// <param name="startIdx"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public string[] GetSubStrArr(string[] origStrArr, int startIdx, int len)
        {
            string[] subStrArr = new string[] { };
            if ((origStrArr != null) && (origStrArr.Length > 0) && (len > 0))
            {
                List<string> strList = new List<string>();
                int endPos = startIdx + len;
                if (endPos > origStrArr.Length)
                {
                    endPos = origStrArr.Length;
                }

                for (int i = startIdx; i < endPos; i++)
                {
                    //refer: http://zhidao.baidu.com/question/296384408.html
                    strList.Add(origStrArr[i]);
                }

                subStrArr = new string[len];
                strList.CopyTo(subStrArr);
            }

            return subStrArr;
        }


        #endregion

        #region Cookie

        /// <summary>
        /// extrat the Host from input url
        /// example: from https://skydrive.live.com/, extracted Host is "skydrive.live.com"
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ExtractHost(string url)
        {
            string domain = "";
            if ((url != "") && (url.Contains("/")))
            {
                string[] splited = url.Split('/');
                domain = splited[2];
            }
            return domain;
        }

        /// <summary>
        /// extrat the domain from input url
        /// example: from https://skydrive.live.com/, extracted domain is ".live.com"
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ExtractDomain(string url)
        {
            string host = "";
            string domain = "";
            host = ExtractHost(url);
            if (host.Contains("."))
            {
                domain = host.Substring(host.IndexOf('.'));
            }
            return domain;
        }

        /// <summary>
        /// add recognized cookie field: expires/domain/path/secure/httponly/version, into cookie
        /// </summary>
        /// <param name="ck"></param>
        /// <param name="pairInfo"></param>
        /// <returns></returns>
        public bool AddFieldToCookie(ref Cookie ck, pairItem pairInfo)
        {
            bool added = false;
            if (pairInfo.key != "")
            {
                string lowerKey = pairInfo.key.ToLower();
                switch (lowerKey)
                {
                    case "expires":
                        DateTime expireDatetime;
                        if (DateTime.TryParse(pairInfo.value, out expireDatetime))
                        {
                            // note: here coverted to local time: GMT +8
                            ck.Expires = expireDatetime;

                            //update expired filed
                            if (DateTime.Now.Ticks > ck.Expires.Ticks)
                            {
                                ck.Expired = true;
                            }

                            added = true;
                        }
                        break;
                    case "domain":
                        ck.Domain = pairInfo.value;
                        added = true;
                        break;
                    case "secure":
                        ck.Secure = true;
                        added = true;
                        break;
                    case "path":
                        ck.Path = pairInfo.value;
                        added = true;
                        break;
                    case "httponly":
                        ck.HttpOnly = true;
                        added = true;
                        break;
                    case "version":
                        int versionValue;
                        if (int.TryParse(pairInfo.value, out versionValue))
                        {
                            ck.Version = versionValue;
                            added = true;
                        }
                        break;
                    default:
                        break;
                }
            }

            return added;
        }//addFieldToCookie

        public bool IsValidCookieField(string cookieKey)
        {
            return cookieFieldList.Contains(cookieKey.ToLower());
        }

        //cookie field example:
        //WLSRDAuth=FAAaARQL3KgEDBNbW84gMYrDN0fBab7xkQNmAAAEgAAACN7OQIVEO14E2ADnX8vEiz8fTuV7bRXem4Yeg/DI6wTk5vXZbi2SEOHjt%2BbfDJMZGybHQm4NADcA9Qj/tBZOJ/ASo5d9w3c1bTlU1jKzcm2wecJ5JMJvdmTCj4J0oy1oyxbMPzTc0iVhmDoyClU1dgaaVQ15oF6LTQZBrA0EXdBxq6Mu%2BUgYYB9DJDkSM/yFBXb2bXRTRgNJ1lruDtyWe%2Bm21bzKWS/zFtTQEE56bIvn5ITesFu4U8XaFkCP/FYLiHj6gpHW2j0t%2BvvxWUKt3jAnWY1Tt6sXhuSx6CFVDH4EYEEUALuqyxbQo2ugNwDkP9V5O%2B5FAyCf; path=/; domain=.livefilestore.com;  HttpOnly;,
        //WLSRDSecAuth=FAAaARQL3KgEDBNbW84gMYrDN0fBab7xkQNmAAAEgAAACJFcaqD2IuX42ACdjP23wgEz1qyyxDz0kC15HBQRXH6KrXszRGFjDyUmrC91Zz%2BgXPFhyTzOCgQNBVfvpfCPtSccxJHDIxy47Hq8Cr6RGUeXSpipLSIFHumjX5%2BvcJWkqxDEczrmBsdGnUcbz4zZ8kP2ELwAKSvUteey9iHytzZ5Ko12G72%2Bbk3BXYdnNJi8Nccr0we97N78V0bfehKnUoDI%2BK310KIZq9J35DgfNdkl12oYX5LMIBzdiTLwN1%2Bx9DgsYmmgxPbcuZPe/7y7dlb00jNNd8p/rKtG4KLLT4w3EZkUAOcUwGF746qfzngDlOvXWVvZjGzA; path=/; domain=.livefilestore.com;  HttpOnly; secure;,
        //RPSShare=1; path=/;,
        //ANON=A=DE389D4D076BF47BCAE4DC05FFFFFFFF&E=c44&W=1; path=/; domain=.livefilestore.com;,
        //NAP=V=1.9&E=bea&C=VTwb1vAsVjCeLWrDuow-jCNgP5eS75JWWvYVe3tRppviqKixCvjqgw&W=1; path=/; domain=.livefilestore.com;,
        //RPSMaybe=; path=/; domain=.livefilestore.com; expires=Thu, 30-Oct-1980 16:00:00 GMT;

        /// <summary>
        /// check whether the cookie name is valid or not
        /// </summary>
        /// <param name="ckName"></param>
        /// <returns></returns>
        public bool IsValidCookieName(string ckName)
        {
            bool isValid = true;
            if (ckName == null)
            {
                isValid = false;
            }
            else
            {
                //string invalidP = @"\W+";

                //Regex rx = new Regex(invalidP);
                //Match foundInvalid = rx.Match(ckName);
                //if (foundInvalid.Success)
                //{
                //    isValid = false;
                //}
                string invalidP = @".+";
                Regex rx = new Regex(invalidP);
                Match foundInvalid = rx.Match(ckName);
                if (!foundInvalid.Success)
                {
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// check whether a cookie is expired
        /// if expired property is set, then just return it value
        /// if not set, check whether is a session cookie, if is, then not expired
        /// if expires is set, check its real time is expired or not
        /// </summary>
        /// <param name="ck"></param>
        /// <returns></returns>
        public bool IsCookieExpired(Cookie ck)
        {
            bool isExpired = false;

            if ((ck != null) && (ck.Name != ""))
            {
                if (ck.Expired)
                {
                    isExpired = true;
                }
                else
                {
                    DateTime initExpiresValue = (new Cookie()).Expires;
                    DateTime expires = ck.Expires;

                    if (expires.Equals(initExpiresValue))
                    {
                        // expires is not set, means this is session cookie, so here no expire
                    }
                    else
                    {
                        // has set expire value
                        if (DateTime.Now.Ticks > expires.Ticks)
                        {
                            isExpired = true;
                        }
                    }
                }
            }
            else
            {
                isExpired = true;
            }

            return isExpired;
        }


        /// <summary>
        /// parse the cookie name and value
        /// </summary>
        /// <param name="ckNameValueExpr"></param>
        /// <param name="pair"></param>
        /// <returns></returns>
        public bool ParseCookieNameValue(string ckNameValueExpr, out pairItem pair)
        {
            bool parsedOK = false;
            if (ckNameValueExpr == "")
            {
                pair.key = "";
                pair.value = "";
                parsedOK = false;
            }
            else
            {
                ckNameValueExpr = ckNameValueExpr.Trim();

                int equalPos = ckNameValueExpr.IndexOf('=');
                if (equalPos > 0) // is valid expression
                {
                    pair.key = ckNameValueExpr.Substring(0, equalPos);
                    pair.key = pair.key.Trim();
                    if (IsValidCookieName(pair.key))
                    {
                        // only process while is valid cookie field
                        pair.value = ckNameValueExpr.Substring(equalPos + 1);
                        pair.value = pair.value.Trim();
                        parsedOK = true;
                    }
                    else
                    {
                        pair.key = "";
                        pair.value = "";
                        parsedOK = false;
                    }
                }
                else
                {
                    pair.key = "";
                    pair.value = "";
                    parsedOK = false;
                }
            }
            return parsedOK;
        }

        /// <summary>
        /// parse cookie field expression
        /// </summary>
        /// <param name="ckFieldExpr"></param>
        /// <param name="pair"></param>
        /// <returns></returns>
        public bool ParseCookieField(string ckFieldExpr, out pairItem pair)
        {
            bool parsedOK = false;

            if (ckFieldExpr == "")
            {
                pair.key = "";
                pair.value = "";
                parsedOK = false;
            }
            else
            {
                ckFieldExpr = ckFieldExpr.Trim();

                //some specials: secure/httponly
                if (ckFieldExpr.ToLower() == "httponly")
                {
                    pair.key = "httponly";
                    //pair.value = "";
                    pair.value = "true";
                    parsedOK = true;
                }
                else if (ckFieldExpr.ToLower() == "secure")
                {
                    pair.key = "secure";
                    //pair.value = "";
                    pair.value = "true";
                    parsedOK = true;
                }
                else // normal cookie field
                {
                    int equalPos = ckFieldExpr.IndexOf('=');
                    if (equalPos > 0) // is valid expression
                    {
                        pair.key = ckFieldExpr.Substring(0, equalPos);
                        pair.key = pair.key.Trim();
                        if (IsValidCookieField(pair.key))
                        {
                            // only process while is valid cookie field
                            pair.value = ckFieldExpr.Substring(equalPos + 1);
                            pair.value = pair.value.Trim();
                            parsedOK = true;
                        }
                        else
                        {
                            pair.key = "";
                            pair.value = "";
                            parsedOK = false;
                        }
                    }
                    else
                    {
                        pair.key = "";
                        pair.value = "";
                        parsedOK = false;
                    }
                }
            }

            return parsedOK;
        }

        /// <summary>
        /// parse single cookie string to a cookie
        /// example:
        /// MSPShared=1; expires=Wed, 30-Dec-2037 16:00:00 GMT;domain=login.live.com;path=/;HTTPOnly= ;version=1
        /// PPAuth=CkLXJYvPpNs3w!fIwMOFcraoSIAVYX3K!CdvZwQNwg3Y7gv74iqm9MqReX8XkJqtCFeMA6GYCWMb9m7CoIw!ID5gx3pOt8sOx1U5qQPv6ceuyiJYwmS86IW*l3BEaiyVCqFvju9BMll7!FHQeQholDsi0xqzCHuW!Qm2mrEtQPCv!qF3Sh9tZDjKcDZDI9iMByXc6R*J!JG4eCEUHIvEaxTQtftb4oc5uGpM!YyWT!r5jXIRyxqzsCULtWz4lsWHKzwrNlBRbF!A7ZXqXygCT8ek6luk7rarwLLJ!qaq2BvS; domain=login.live.com;secure= ;path=/;HTTPOnly= ;version=1
        /// </summary>
        /// <param name="cookieStr"></param>
        /// <param name="ck"></param>
        /// <returns></returns>
        public bool ParseSingleCookie(string cookieStr, ref Cookie ck)
        {
            bool parsedOk = true;
            //Cookie ck = new Cookie();
            //string[] expressions = cookieStr.Split(";".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
            //refer: http://msdn.microsoft.com/en-us/library/b873y76a.aspx
            string[] expressions = cookieStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            //get cookie name and value
            pairItem pair = new pairItem();
            if (ParseCookieNameValue(expressions[0], out pair))
            {
                ck.Name = pair.key;
                ck.Value = pair.value;

                string[] fieldExpressions = GetSubStrArr(expressions, 1, expressions.Length - 1);
                foreach (string eachExpression in fieldExpressions)
                {
                    //parse key and value
                    if (ParseCookieField(eachExpression, out pair))
                    {
                        // add to cookie field if possible
                        AddFieldToCookie(ref ck, pair);
                    }
                    else
                    {
                        // if any field fail, consider it is a abnormal cookie string, so quit with false
                        parsedOk = false;
                        break;
                    }
                }
            }
            else
            {
                parsedOk = false;
            }

            return parsedOk;
        }

        /// <summary>
        /// parse xxx in "new Date(xxx)" of javascript to C# DateTime
        /// input example:
        /// new Date(1329198041411.84) / new Date(1329440307389.9) / new Date(1329440307483)
        /// </summary>
        /// <param name="newDateStr"></param>
        /// <param name="parsedDatetime"></param>
        /// <returns></returns>
        public bool ParseJsNewDate(string newDateStr, out DateTime parsedDatetime)
        {
            bool parseOK = false;
            parsedDatetime = new DateTime();

            if ((newDateStr != "") && (newDateStr.Trim() != ""))
            {
                string dateValue = "";
                if (ExtractSingleStr(@".*new\sDate\((.+?)\).*", newDateStr, out dateValue))
                {
                    double doubleVal = 0.0;
                    if (Double.TryParse(dateValue, out doubleVal))
                    {
                        // try whether is double/int64 milliSecSinceEpoch
                        parsedDatetime = MilliSecToDateTime(doubleVal);
                        parseOK = true;
                    }
                    else if (DateTime.TryParse(dateValue, out parsedDatetime))
                    {
                        // try normal DateTime string
                        //refer: http://www.w3schools.com/js/js_obj_date.asp
                        //October 13, 1975 11:13:00
                        //79,5,24 / 79,5,24,11,33,0
                        //1329198041411.3344 / 1329198041411.84 / 1329198041411
                        parseOK = true;
                    }
                }
            }

            return parseOK;
        }

        //parse Javascript string "$Cookie.setCookie(XXX);" to a cookie
        // input example:
        //$Cookie.setCookie('wla42','cHJveHktYmF5LnB2dC1jb250YWN0cy5tc24uY29tfGJ5MioxLDlBOEI4QkY1MDFBMzhBMzYsMSwwLDA=','live.com','/',new Date(1328842189083.44),1);
        //$Cookie.setCookie('wla42','YnkyKjEsOUE4QjhCRjUwMUEzOEEzNiwwLCww','live.com','/',new Date(1329198041411.84),1);
        //$Cookie.setCookie('wla42', 'YnkyKjEsOUE4QjhCRjUwMUEzOEEzNiwwLCww', 'live.com', '/', new Date(1329440307389.9), 1);
        //$Cookie.setCookie('wla42', 'cHJveHktYmF5LnB2dC1jb250YWN0cy5tc24uY29tfGJ5MioxLDlBOEI4QkY1MDFBMzhBMzYsMSwwLDA=', 'live.com', '/', new Date(1329440307483.5), 1);
        //$Cookie.setCookie('wls', 'A|eyJV-t:a*nS', '.live.com', '/', null, 1);
        //$Cookie.setCookie('MSNPPAuth','','.live.com','/',new Date(1327971507311.9),1);
        public bool ParseJsSetCookie(string singleSetCookieStr, out Cookie parsedCk)
        {
            bool parseOK = false;
            parsedCk = new Cookie();

            string name = "";
            string value = "";
            string domain = "";
            string path = "";
            string expire = "";
            string secure = "";

            //                                     1=name      2=value     3=domain     4=path   5=expire  6=secure
            string setckP = @"\$Cookie\.setCookie\('(\w+)',\s*'(.*?)',\s*'([\w\.]+)',\s*'(.+?)',\s*(.+?),\s*(\d?)\);";
            Regex setckRx = new Regex(setckP);
            Match foundSetck = setckRx.Match(singleSetCookieStr);
            if (foundSetck.Success)
            {
                name = foundSetck.Groups[1].ToString();
                value = foundSetck.Groups[2].ToString();
                domain = foundSetck.Groups[3].ToString();
                path = foundSetck.Groups[4].ToString();
                expire = foundSetck.Groups[5].ToString();
                secure = foundSetck.Groups[6].ToString();

                // must: name valid and domain is not null
                if (IsValidCookieName(name) && (domain != ""))
                {
                    parseOK = true;

                    parsedCk.Name = name;
                    parsedCk.Value = value;
                    parsedCk.Domain = domain;
                    parsedCk.Path = path;

                    // note, here even parse expire field fail
                    //do not consider it must fail to parse the whole cookie
                    if (expire.Trim() == "null")
                    {
                        // do nothing
                    }
                    else
                    {
                        DateTime expireTime;
                        if (ParseJsNewDate(expire, out expireTime))
                        {
                            parsedCk.Expires = expireTime;
                        }
                    }

                    if (secure == "1")
                    {
                        parsedCk.Secure = true;
                    }
                    else
                    {
                        parsedCk.Secure = false;
                    }
                }//if (isValidCookieName(name) && (domain != ""))
            }//foundSetck.Success

            return parseOK;
        }


        /// <summary>
        /// parse the Set-Cookie string (in http response header) to cookies
        /// Note: auto omit to parse the abnormal cookie string
        /// normal example for 'setCookieStr':
        /// MSPOK= ; expires=Thu, 30-Oct-1980 16:00:00 GMT;domain=login.live.com;path=/;HTTPOnly= ;version=1,PPAuth=Cuyf3Vp2wolkjba!TOr*0v22UMYz36ReuiwxZZBc8umHJYPlRe4qupywVFFcIpbJyvYZ5ZDLBwV4zRM1UCjXC4tUwNuKvh21iz6gQb0Tu5K7Z62!TYGfowB9VQpGA8esZ7iCRucC7d5LiP3ZAv*j4Z3MOecaJwmPHx7!wDFdAMuQUZURhHuZWJiLzHP1j8ppchB2LExnlHO6IGAdZo1f0qzSWsZ2hq*yYP6sdy*FdTTKo336Q1B0i5q8jUg1Yv6c2FoBiNxhZSzxpuU0WrNHqSytutP2k4!wNc6eSnFDeouX; domain=login.live.com;secure= ;path=/;HTTPOnly= ;version=1,PPLState=1; domain=.live.com;path=/;version=1,MSPShared=1; expires=Wed, 30-Dec-2037 16:00:00 GMT;domain=login.live.com;path=/;HTTPOnly= ;version=1,MSPPre= ;domain=login.live.com;path=/;Expires=Thu, 30-Oct-1980 16:00:00 GMT,MSPCID= ; HTTPOnly= ; domain=login.live.com;path=/;Expires=Thu, 30-Oct-1980 16:00:00 GMT,RPSTAuth=EwDoARAnAAAUWkziSC7RbDJKS1VkhugDegv7L0eAAOfCAY2+pKwbV5zUlu3XmBbgrQ8EdakmdSqK9OIKfMzAbnU8fuwwEi+FKtdGSuz/FpCYutqiHWdftd0YF21US7+1bPxuLJ0MO+wVXB8GtjLKZaA0xCXlU5u01r+DOsxSVM777DmplaUc0Q4O1+Pi9gX9cyzQLAgRKmC/QtlbVNKDA2YAAAhIwqiXOVR/DDgBocoO/n0u48RFGh79X2Q+gO4Fl5GMc9Vtpa7SUJjZCCfoaitOmcxhEjlVmR/2ppdfJx3Ykek9OFzFd+ijtn7K629yrVFt3O9q5L0lWoxfDh5/daLK7lqJGKxn1KvOew0SHlOqxuuhYRW57ezFyicxkxSI3aLxYFiqHSu9pq+TlITqiflyfcAcw4MWpvHxm9on8Y1dM2R4X3sxuwrLQBpvNsG4oIaldTYIhMEnKhmxrP6ZswxzteNqIRvMEKsxiksBzQDDK/Cnm6QYBZNsPawc6aAedZioeYwaV3Z/i3tNrAUwYTqLXve8oG6ZNXL6WLT/irKq1EMilK6Cw8lT3G13WYdk/U9a6YZPJC8LdqR0vAHYpsu/xRF39/On+xDNPE4keIThJBptweOeWQfsMDwvgrYnMBKAMjpLZwE=; domain=.live.com;path=/;HTTPOnly= ;version=1,RPSTAuthTime=1328679636; domain=login.live.com;path=/;HTTPOnly= ;version=1,MSPAuth=2OlAAMHXtDIFOtpaK1afG2n*AAxdfCnCBlJFn*gCF8gLnCa1YgXEfyVh2m9nZuF*M7npEwb4a7Erpb*!nH5G285k7AswJOrsr*gY29AVAbsiz2UscjIGHkXiKrTvIzkV2M; domain=.live.com;path=/;HTTPOnly= ;version=1,MSPProf=23ci9sti6DZRrkDXfTt1b3lHhMdheWIcTZU2zdJS9!zCloHzMKwX30MfEAcCyOjVt*5WeFSK3l2ZahtEaK7HPFMm3INMs3r!JxI8odP9PYRHivop5ryohtMYzWZzj3gVVurcEr5Bg6eJJws7rXOggo3cR4FuKLtXwz*FVX0VWuB5*aJhRkCT1GZn*L5Pxzsm9X; domain=.live.com;path=/;HTTPOnly= ;version=1,MSNPPAuth=CiGSMoUOx4gej8yQkdFBvN!gvffvAhCPeWydcrAbcg!O2lrhVb4gruWSX5NZCBPsyrtZKmHLhRLTUUIxxPA7LIhqW5TCV*YcInlG2f5hBzwzHt!PORYbg79nCkvw65LKG399gRGtJ4wvXdNlhHNldkBK1jVXD4PoqO1Xzdcpv4sj68U6!oGrNK5KgRSMXXpLJmCeehUcsRW1NmInqQXpyanjykpYOcZy0vq!6PIxkj3gMaAvm!1vO58gXM9HX9dA0GloNmCDnRv4qWDV2XKqEKp!A7jiIMWTmHup1DZ!*YCtDX3nUVQ1zAYSMjHmmbMDxRJECz!1XEwm070w16Y40TzuKAJVugo!pyF!V2OaCsLjZ9tdGxGwEQRyi0oWc*Z7M0FBn8Fz0Dh4DhCzl1NnGun9kOYjK5itrF1Wh17sT!62ipv1vI8omeu0cVRww2Kv!qM*LFgwGlPOnNHj3*VulQOuaoliN4MUUxTA4owDubYZoKAwF*yp7Mg3zq5Ds2!l9Q$$; domain=.live.com;path=/;HTTPOnly= ;version=1,MH=MSFT; domain=.live.com;path=/;version=1,MHW=; expires=Thu, 30-Oct-1980 16:00:00 GMT;domain=.live.com;path=/;version=1,MHList=; expires=Thu, 30-Oct-1980 16:00:00 GMT;domain=.live.com;path=/;version=1,NAP=V=1.9&E=bea&C=zfjCKKBD0TqjZlWGgRTp__NiK08Lme_0XFaiKPaWJ0HDuMi2uCXafQ&W=1;domain=.live.com;path=/,ANON=A=DE389D4D076BF47BCAE4DC05FFFFFFFF&E=c44&W=1;domain=.live.com;path=/,MSPVis=$9;domain=login.live.com;path=/,pres=; expires=Thu, 30-Oct-1980 16:00:00 GMT;domain=.live.com;path=/;version=1,LOpt=0; domain=login.live.com;path=/;version=1,WLSSC=EgBnAQMAAAAEgAAACoAASfCD+8dUptvK4kvFO0gS3mVG28SPT3Jo9Pz2k65r9c9KrN4ISvidiEhxXaPLCSpkfa6fxH3FbdP9UmWAa9KnzKFJu/lQNkZC3rzzMcVUMjbLUpSVVyscJHcfSXmpGGgZK4ZCxPqXaIl9EZ0xWackE4k5zWugX7GR5m/RzakyVIzWAFwA1gD9vwYA7Vazl9QKMk/UCjJPECcAAAoQoAAAFwBjcmlmYW4yMDAzQGhvdG1haWwuY29tAE8AABZjcmlmYW4yMDAzQGhvdG1haWwuY29tAAAACUNOAAYyMTM1OTIAAAZlCAQCAAB3F21AAARDAAR0aWFuAAR3YW5nBMgAAUkAAAAAAAAAAAAAAaOKNpqLi/UAANQKMk/Uf0RPAAAAAAAAAAAAAAAADgA1OC4yNDAuMjM2LjE5AAUAAAAAAAAAAAAAAAABBAABAAABAAABAAAAAAAAAAA=; domain=.live.com;secure= ;path=/;HTTPOnly= ;version=1,MSPSoftVis=@72198325083833620@:@; domain=login.live.com;path=/;version=1
        /// here now support parse the un-correct Set-Cookie:
        /// MSPRequ=/;Version=1;version&lt=1328770452&id=250915&co=1; path=/;version=1,MSPVis=$9; Version=1;version=1$250915;domain=login.live.com;path=/,MSPSoftVis=@72198325083833620@:@; domain=login.live.com;path=/;version=1,MSPBack=1328770312; domain=login.live.com;path=/;version=1
        /// </summary>
        /// <param name="setCookieStr"></param>
        /// <param name="curDomain"></param>
        /// <returns></returns>
        public CookieCollection ParseSetCookie(string setCookieStr, string curDomain)
        {
            CookieCollection parsedCookies = new CookieCollection();

            // process for expires and Expires field, for it contains ','
            //refer: http://www.yaosansi.com/post/682.html
            // may contains expires or Expires, so following use xpires
            string commaReplaced = Regex.Replace(setCookieStr, @"xpires=\w{3},\s\d{2}-\w{3}-\d{4}", new MatchEvaluator(ProcessExpireField));
            string[] cookieStrArr = commaReplaced.Split(',');
            foreach (string cookieStr in cookieStrArr)
            {
                Cookie ck = new Cookie();
                // recover it back
                string recoveredCookieStr = Regex.Replace(cookieStr, @"xpires=\w{3}" + replacedChar + @"\s\d{2}-\w{3}-\d{4}", new MatchEvaluator(RecoverExpireField));
                if (ParseSingleCookie(recoveredCookieStr, ref ck))
                {
                    if (needAddThisCookie(ck, curDomain))
                    {
                        parsedCookies.Add(ck);
                    }
                }
            }

            return parsedCookies;
        }

        /// <summary>
        /// parse Set-Cookie string part into cookies
        /// leave current domain to empty, means omit the parsed cookie, which is not set its domain value
        /// </summary>
        /// <param name="setCookieStr"></param>
        /// <returns></returns>
        public CookieCollection ParseSetCookie(string setCookieStr)
        {
            return ParseSetCookie(setCookieStr, "");
        }


        /// <summary>
        /// add a single cookie to cookies, if already exist, update its value
        /// </summary>
        /// <param name="toAdd"></param>
        /// <param name="cookies"></param>
        /// <param name="overwriteDomain"></param>
        public void AddCookieToCookies(Cookie toAdd, ref CookieCollection cookies, bool overwriteDomain)
        {
            bool found = false;

            if (cookies.Count > 0)
            {
                foreach (Cookie originalCookie in cookies)
                {
                    if (originalCookie.Name == toAdd.Name)
                    {
                        // !!! for different domain, cookie is not same,
                        // so should not set the cookie value here while their domains is not same
                        // only if it explictly need overwrite domain
                        if ((originalCookie.Domain == toAdd.Domain) ||
                            ((originalCookie.Domain != toAdd.Domain) && overwriteDomain))
                        {
                            //here can not force convert CookieCollection to HttpCookieCollection,
                            //then use .remove to remove this cookie then add
                            // so no good way to copy all field value
                            originalCookie.Value = toAdd.Value;

                            originalCookie.Domain = toAdd.Domain;

                            originalCookie.Expires = toAdd.Expires;
                            originalCookie.Version = toAdd.Version;
                            originalCookie.Path = toAdd.Path;

                            //following fields seems should not change
                            //originalCookie.HttpOnly = toAdd.HttpOnly;
                            //originalCookie.Secure = toAdd.Secure;

                            found = true;
                            break;
                        }
                    }
                }
            }

            if (!found)
            {
                if (toAdd.Domain != "")
                {
                    // if add the null domain, will lead to follow req.CookieContainer.Add(cookies) failed !!!
                    cookies.Add(toAdd);
                }
            }

        }//addCookieToCookies

        /// <summary>
        /// add singel cookie to cookies, default no overwrite domain
        /// </summary>
        /// <param name="toAdd"></param>
        /// <param name="cookies"></param>
        public void AddCookieToCookies(Cookie toAdd, ref CookieCollection cookies)
        {
            AddCookieToCookies(toAdd, ref cookies, false);
        }

        /// <summary>
        /// check whether the cookies contains the ckToCheck cookie
        /// support:
        /// ckTocheck is Cookie/string
        /// cookies is Cookie/string/CookieCollection/string[]
        /// </summary>
        /// <param name="ckToCheck"></param>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public bool IsContainCookie(object ckToCheck, object cookies)
        {
            bool isContain = false;

            if ((ckToCheck != null) && (cookies != null))
            {
                string ckName = "";
                Type type = ckToCheck.GetType();

                //string typeStr = ckType.ToString();

                //if (ckType.FullName == "System.string")
                if (type.Name.ToLower() == "string")
                {
                    ckName = (string)ckToCheck;
                }
                else if (type.Name == "Cookie")
                {
                    ckName = ((Cookie)ckToCheck).Name;
                }

                if (ckName != "")
                {
                    type = cookies.GetType();

                    // is single Cookie
                    if (type.Name == "Cookie")
                    {
                        if (ckName == ((Cookie)cookies).Name)
                        {
                            isContain = true;
                        }
                    }
                    // is CookieCollection
                    else if (type.Name == "CookieCollection")
                    {
                        foreach (Cookie ck in (CookieCollection)cookies)
                        {
                            if (ckName == ck.Name)
                            {
                                isContain = true;
                                break;
                            }
                        }
                    }
                    // is single cookie name string
                    else if (type.Name.ToLower() == "string")
                    {
                        if (ckName == (string)cookies)
                        {
                            isContain = true;
                        }
                    }
                    // is cookie name string[]
                    else if (type.Name.ToLower() == "string[]")
                    {
                        foreach (string name in ((string[])cookies))
                        {
                            if (ckName == name)
                            {
                                isContain = true;
                                break;
                            }
                        }
                    }
                }
            }

            return isContain;
        }

        /// <summary>
        /// update cookiesToUpdate to localCookies
        /// if omitUpdateCookies designated, then omit cookies of omitUpdateCookies in cookiesToUpdate
        /// </summary>
        /// <param name="cookiesToUpdate"></param>
        /// <param name="localCookies"></param>
        /// <param name="omitUpdateCookies"></param>
        public void UpdateLocalCookies(CookieCollection cookiesToUpdate, ref CookieCollection localCookies, object omitUpdateCookies)
        {
            if (cookiesToUpdate.Count > 0)
            {
                if (localCookies == null)
                {
                    localCookies = cookiesToUpdate;
                }
                else
                {
                    foreach (Cookie newCookie in cookiesToUpdate)
                    {
                        if (IsContainCookie(newCookie, omitUpdateCookies))
                        {
                            // need omit process this
                        }
                        else
                        {
                            AddCookieToCookies(newCookie, ref localCookies);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// update cookiesToUpdate to localCookies
        /// </summary>
        /// <param name="cookiesToUpdate"></param>
        /// <param name="localCookies"></param>
        public void UpdateLocalCookies(CookieCollection cookiesToUpdate, ref CookieCollection localCookies)
        {
            UpdateLocalCookies(cookiesToUpdate, ref localCookies, null);
        }

        /// <summary>
        /// given a cookie name ckName, get its value from CookieCollection cookies
        /// </summary>
        /// <param name="ckName"></param>
        /// <param name="cookies"></param>
        /// <param name="ckVal"></param>
        /// <returns></returns>
        public bool GetCookieVal(string ckName, ref CookieCollection cookies, out string ckVal)
        {
            //string ckVal = "";
            ckVal = "";
            bool gotValue = false;

            foreach (Cookie ck in cookies)
            {
                if (ck.Name == ckName)
                {
                    gotValue = true;
                    ckVal = ck.Value;
                    break;
                }
            }

            return gotValue;
        }


        #endregion

        #region Serialize/Deserialize

        /// <summary>
        /// serialize an object to string
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="serializedStr"></param>
        /// <returns></returns>
        public bool SerializeObjToStr(Object obj, out string serializedStr)
        {
            bool serializeOk = false;
            serializedStr = "";
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, obj);
                serializedStr = System.Convert.ToBase64String(memoryStream.ToArray());

                serializeOk = true;
            }
            catch
            {
                serializeOk = false;
            }

            return serializeOk;
        }


        /// <summary>
        /// deserialize the string to an object
        /// </summary>
        /// <param name="serializedStr"></param>
        /// <param name="deserializedObj"></param>
        /// <returns></returns>
        public bool DeserializeStrToObj(string serializedStr, out object deserializedObj)
        {
            bool deserializeOk = false;
            deserializedObj = null;

            try
            {
                byte[] restoredBytes = System.Convert.FromBase64String(serializedStr);
                MemoryStream restoredMemoryStream = new MemoryStream(restoredBytes);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                deserializedObj = binaryFormatter.Deserialize(restoredMemoryStream);

                deserializeOk = true;
            }
            catch
            {
                deserializeOk = false;
            }

            return deserializeOk;
        }


        #endregion


        #endregion


        #region private Funcs

        /// <summary>
        /// replace the replacedChar back to original ','
        /// </summary>
        /// <param name="foundPprocessedExpire"></param>
        /// <returns></returns>
        private string RecoverExpireField(Match foundPprocessedExpire)
        {
            string recovedStr = foundPprocessedExpire.Value.Replace(replacedChar, ',');
            return recovedStr;
        }

        /// <summary>
        /// replace ',' with replacedChar
        /// </summary>
        /// <param name="foundExpire"></param>
        /// <returns></returns>
        private string ProcessExpireField(Match foundExpire)
        {
            string replacedComma = foundExpire.Value.ToString().Replace(',', replacedChar);
            return replacedComma;
        }

        /// <summary>
        /// check whether need add/retain this cookie 
        /// not add for:
        /// ck is null or ck name is null
        /// domain is null and curDomain is not set
        /// expired and retainExpiredCookie==false
        /// </summary>
        /// <param name="ck"></param>
        /// <param name="curDomain"></param>
        /// <returns></returns>
        private bool needAddThisCookie(Cookie ck, string curDomain)
        {
            bool needAdd = false;

            if ((ck == null) || (ck.Name == ""))
            {
                needAdd = false;
            }
            else
            {
                if (ck.Domain != "")
                {
                    needAdd = true;
                }
                else// ck.Domain == ""
                {
                    if (curDomain != "")
                    {
                        ck.Domain = curDomain;
                        needAdd = true;
                    }
                    else // curDomain == ""
                    {
                        // not set current domain, omit this
                        // should not add empty domain cookie, for this will lead execute CookieContainer.Add() fail !!!
                        needAdd = false;
                    }
                }
            }

            return needAdd;
        }


        #endregion



        /*
        * Note: currently support auto handle cookies
        * currently only support single caller -> multiple caller of these functions will cause cookies accumulated
        * you can clear previous cookies to avoid unexpected result by call clearCurCookies
        */
        public void clearCurCookies()
        {
            if (curCookies != null)
            {
                curCookies = null;
                curCookies = new CookieCollection();
            }
        }

        /* get current cookies */
        public CookieCollection getCurCookies()
        {
            return curCookies;
        }

        /* set current cookies */
        public void setCurCookies(CookieCollection cookies)
        {
            curCookies = cookies;
        }

        /* get url's response
            * */
        public HttpWebResponse getUrlResponse(string url, Dictionary<string, string> headerDict,
            Dictionary<string, string> postDict, int timeout, string postDataStr)
        {
            //CookieCollection parsedCookies;

            HttpWebResponse resp = null;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            req.AllowAutoRedirect = true;
            req.Accept = "*/*";

            //const string gAcceptLanguage = "en-US"; // zh-CN/en-US
            //req.Headers["Accept-Language"] = gAcceptLanguage;

            req.KeepAlive = true;

            //IE8
            const string gUserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.3; .NET4.0C; .NET4.0E";
            //IE9
            //const string gUserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)"; // x64
            //const string gUserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)"; // x86
            //const string gUserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.3; .NET4.0C; .NET4.0E)";
            //Chrome
            //const string gUserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/533.4 (KHTML, like Gecko) Chrome/5.0.375.99 Safari/533.4";
            //Mozilla Firefox
            //const string gUserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; rv:1.9.2.6) Gecko/20100625 Firefox/3.6.6";
            req.UserAgent = gUserAgent;

            req.Headers["Accept-Encoding"] = "gzip, deflate";
            req.AutomaticDecompression = DecompressionMethods.GZip;

            req.Proxy = null;

            if (timeout > 0)
            {
                req.Timeout = timeout;
            }

            if (curCookies != null)
            {
                req.CookieContainer = new CookieContainer();
                req.CookieContainer.PerDomainCapacity = 40; // following will exceed max default 20 cookie per domain
                req.CookieContainer.Add(curCookies);
            }

            if (headerDict != null)
            {
                foreach (string header in headerDict.Keys)
                {
                    string headerValue = "";
                    if (headerDict.TryGetValue(header, out headerValue))
                    {
                        // following are allow the caller overwrite the default header setting
                        if (header.ToLower() == "referer")
                        {
                            req.Referer = headerValue;
                        }
                        else if (header.ToLower() == "allowautoredirect")
                        {
                            bool isAllow = false;
                            if (bool.TryParse(headerValue, out isAllow))
                            {
                                req.AllowAutoRedirect = isAllow;
                            }
                        }
                        else if (header.ToLower() == "accept")
                        {
                            req.Accept = headerValue;
                        }
                        else if (header.ToLower() == "keepalive")
                        {
                            bool isKeepAlive = false;
                            if (bool.TryParse(headerValue, out isKeepAlive))
                            {
                                req.KeepAlive = isKeepAlive;
                            }
                        }
                        else if (header.ToLower() == "accept-language")
                        {
                            req.Headers["Accept-Language"] = headerValue;
                        }
                        else if (header.ToLower() == "useragent")
                        {
                            req.UserAgent = headerValue;
                        }
                        else
                        {
                            req.Headers[header] = headerValue;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (postDict != null || postDataStr != "")
            {
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                if (postDict != null)
                {
                    postDataStr = QuoteParas(postDict);
                }

                //byte[] postBytes = Encoding.GetEncoding("utf-8").GetBytes(postData);
                byte[] postBytes = Encoding.UTF8.GetBytes(postDataStr);
                req.ContentLength = postBytes.Length;

                Stream postDataStream = req.GetRequestStream();
                postDataStream.Write(postBytes, 0, postBytes.Length);
                postDataStream.Close();
            }
            else
            {
                req.Method = "GET";
            }

            //may timeout, has fixed in:
            //http://www.crifan.com/fixed_problem_sometime_httpwebrequest_getresponse_timeout/
            resp = (HttpWebResponse)req.GetResponse();

            UpdateLocalCookies(resp.Cookies, ref curCookies);

            return resp;
        }

        public HttpWebResponse getUrlResponse(string url, Dictionary<string, string> headerDict, Dictionary<string, string> postDict)
        {
            return getUrlResponse(url, headerDict, postDict, 0, "");
        }

        public HttpWebResponse getUrlResponse(string url)
        {
            return getUrlResponse(url, null, null, 0, "");
        }

        // valid charset:"GB18030"/"UTF-8", invliad:"UTF8"
        public string getUrlRespHtml(string url, Dictionary<string, string> headerDict, string charset,
            Dictionary<string, string> postDict, int timeout, string postDataStr)
        {
            string respHtml = "";

            //HttpWebResponse resp = getUrlResponse(url, headerDict, postDict, timeout);
            HttpWebResponse resp = getUrlResponse(url, headerDict, postDict, timeout, postDataStr);

            //long realRespLen = resp.ContentLength;

            StreamReader sr;
            if ((charset != null) && (charset != ""))
            {
                Encoding htmlEncoding = Encoding.GetEncoding(charset);
                sr = new StreamReader(resp.GetResponseStream(), htmlEncoding);
            }
            else
            {
                sr = new StreamReader(resp.GetResponseStream());
            }
            respHtml = sr.ReadToEnd();

            return respHtml;
        }

        public string getUrlRespHtml(string url, Dictionary<string, string> headerDict, string charset, Dictionary<string, string> postDict, string postDataStr)
        {
            return getUrlRespHtml(url, headerDict, charset, postDict, 0, postDataStr);
        }

        public string getUrlRespHtml(string url, Dictionary<string, string> headerDict, string charset, Dictionary<string, string> postDict)
        {
            return getUrlRespHtml(url, headerDict, charset, postDict, 0, "");
        }

        public string getUrlRespHtml(string url, Dictionary<string, string> headerDict, Dictionary<string, string> postDict)
        {
            return getUrlRespHtml(url, headerDict, "", postDict, "");
        }

        public string getUrlRespHtml(string url, Dictionary<string, string> headerDict)
        {
            return getUrlRespHtml(url, headerDict, null);
        }

        public string getUrlRespHtml(string url, string charset, int timeout)
        {
            return getUrlRespHtml(url, null, charset, null, timeout, "");
        }

        public string getUrlRespHtml(string url, string charset)
        {
            return getUrlRespHtml(url, charset, 0);
        }

        public string getUrlRespHtml(string url)
        {
            return getUrlRespHtml(url, "");
        }


        public int getUrlRespStreamBytes(ref Byte[] respBytesBuf, string url, Dictionary<string, string> headerDict,
            Dictionary<string, string> postDict, int timeout)
        {
            int curReadoutLen;
            int curBufPos = 0;
            int realReadoutLen = 0;

            try
            {
                //HttpWebResponse resp = getUrlResponse(url, headerDict, postDict, timeout);
                HttpWebResponse resp = getUrlResponse(url, headerDict, postDict);
                int expectReadoutLen = (int)resp.ContentLength;

                Stream binStream = resp.GetResponseStream();
                //int streamDataLen  = (int)binStream.Length; // erro: not support seek operation

                do
                {
                    // here download logic is:
                    // once request, return some data
                    // request multiple time, until no more data
                    curReadoutLen = binStream.Read(respBytesBuf, curBufPos, expectReadoutLen);
                    if (curReadoutLen > 0)
                    {
                        curBufPos += curReadoutLen;
                        expectReadoutLen = expectReadoutLen - curReadoutLen;

                        realReadoutLen += curReadoutLen;
                    }
                } while (curReadoutLen > 0);
            }
            catch
            {
                realReadoutLen = -1;
            }

            return realReadoutLen;
        }

        /*********************************************************************/
        /* File */
        /*********************************************************************/

        //save binary bytes into file
        public bool saveBytesToFile(string fileToSave, ref Byte[] bytes, int dataLen, out string errStr)
        {
            bool saveOk = false;
            errStr = "未知错误！";

            try
            {
                int bufStartPos = 0;
                int bytesToWrite = dataLen;

                FileStream fs;
                fs = System.IO.File.Create(fileToSave, bytesToWrite);
                fs.Write(bytes, bufStartPos, bytesToWrite);
                fs.Close();

                saveOk = true;
            }
            catch (Exception ex)
            {
                errStr = ex.Message;
            }

            return saveOk;
        }


    }
}