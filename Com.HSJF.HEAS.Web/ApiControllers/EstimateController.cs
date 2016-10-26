using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Estimate;
using Com.HSJF.Infrastructure.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using System.Xml.Linq;

namespace Com.HSJF.HEAS.Web.ApiControllers
{
    /// <summary>
    /// 房产估价接口
    /// </summary>
    [RoutePrefix("Api/Estimate")]
    public class EstimateController : ApiController
    {
        private static string _ShiLianPriceService;

        /// <summary>
        /// 世联估价接口地址
        /// </summary>
        private static string ShiLianPriceService
        {
            get
            {
                if (_ShiLianPriceService == null)
                    _ShiLianPriceService = ConfigurationManager.AppSettings["ShiLianPriceService"];
                return _ShiLianPriceService;
            }
        }

        /// <summary>
        /// 接口调用
        /// </summary>
        /// <param name="route">路由名称</param>
        /// <param name="keyValues">参数值</param>
        /// <returns></returns>
        private string GetHttpResult(string route, Dictionary<string, object> keyValues = null)
        {
            var parameters = string.Empty;
            if (keyValues != null)
            {
                foreach (var key in keyValues.Keys)
                {
                    parameters = string.Format("{0}{1}={2}&", parameters, key, keyValues[key]);
                }
            }

            var url = string.Format("{0}/{1}", ShiLianPriceService, route);
            if (!string.IsNullOrEmpty(parameters))
                url = string.Format("{0}?{1}", url, parameters);

            var httpHelper = new HttpHelper();
            var httpItem = new HttpItem() { URL = url, Timeout = 10000, Method = "GET" };
            var httpResult = httpHelper.GetHtml(httpItem);
            var xDocument = XDocument.Parse(httpResult.Html);

            return ((XElement)xDocument.FirstNode).Value;
        }

        /// <summary>
        /// 获取所有省
        /// http://localhost:6963/api/Estimate/GetProvince
        /// </summary>
        /// <returns></returns>
        [Route("GetProvince")]
        [HttpGet]
        public IHttpActionResult GetProvince()
        {
            var response = new BaseApiResponse<List<ProvinceViewModel>>();
            var data = string.Empty;

            try
            {
                data = GetHttpResult("Province/Get");
                var dataList = JsonConvert.DeserializeObject<List<ProvinceViewModel>>(data);

                response.Status = StatusEnum.Success.ToString();
                response.Data = dataList;
            }
            catch (Exception exception)
            {
                response.Message = string.IsNullOrEmpty(data) ? exception.Message : data;
                response.Status = StatusEnum.Error.ToString();
            }
            return Json(response);
        }

        /// <summary>
        /// 获取所有市
        /// http://localhost:6963/api/Estimate/GetCity?ProvinceID=2
        /// </summary>
        /// <param name="provinceID">省ID</param>
        /// <returns></returns>
        [Route("GetCity")]
        [HttpGet]
        public IHttpActionResult GetCity(int provinceID)
        {
            var response = new BaseApiResponse<List<CityViewModel>>();
            var data = string.Empty;

            try
            {
                data = GetHttpResult("City/Get", new Dictionary<string, object> {
                    { "ProvinceID",provinceID }
                });
                var dataList = JsonConvert.DeserializeObject<List<CityViewModel>>(data);

                response.Status = StatusEnum.Success.ToString();
                response.Data = dataList;
            }
            catch (Exception exception)
            {
                response.Message = string.IsNullOrEmpty(data) ? exception.Message : data;
                response.Status = StatusEnum.Error.ToString();
            }
            return Json(response);
        }

        /// <summary>
        /// 获取所有行政区
        /// http://localhost:6963/api/Estimate/GetArea?CityID=3
        /// </summary>
        /// <param name="cityID">市ID</param>
        /// <returns></returns>
        [Route("GetArea")]
        [HttpGet]
        public IHttpActionResult GetArea(int cityID)
        {
            var response = new BaseApiResponse<List<AreaViewModel>>();
            var data = string.Empty;

            try
            {
                data = GetHttpResult("Area/Get", new Dictionary<string, object> {
                    { "CityID",cityID }
                });
                var dataList = JsonConvert.DeserializeObject<List<AreaViewModel>>(data);

                response.Status = StatusEnum.Success.ToString();
                response.Data = dataList;
            }
            catch (Exception exception)
            {
                response.Message = string.IsNullOrEmpty(data) ? exception.Message : data;
                response.Status = StatusEnum.Error.ToString();
            }
            return Json(response);
        }

        /// <summary>
        /// 获取所有楼盘
        /// http://localhost:6963/api/Estimate/GetConstruction?CityID=3&Keyword=nh
        /// </summary>
        /// <param name="cityID">城市ID</param>
        /// <param name="keyword">楼盘关键字，名称，别名，拼音缩写等</param>
        /// <returns></returns>
        [Route("GetConstruction")]
        [HttpGet]
        public IHttpActionResult GetConstruction(int cityID, string keyword)
        {
            var response = new BaseApiResponse<List<ConstructionViewModel>>();
            var data = string.Empty;

            try
            {
                data = GetHttpResult("Construction/Get", new Dictionary<string, object> {
                    { "CityID",cityID },
                    { "Keyword", keyword }
                });
                var dataList = JsonConvert.DeserializeObject<List<ConstructionViewModel>>(data);

                response.Status = StatusEnum.Success.ToString();
                response.Data = dataList;
            }
            catch (Exception exception)
            {
                response.Message = string.IsNullOrEmpty(data) ? exception.Message : data;
                response.Status = StatusEnum.Error.ToString();
            }
            return Json(response);
        }

        /// <summary>
        /// 获取所有楼栋
        /// http://localhost:6963/api/Estimate/GetBuilding?CityID=3&ConstructionID=7842
        /// </summary>
        /// <param name="cityID">城市ID</param>
        /// <param name="constructionID">楼盘ID</param>
        /// <returns></returns>
        [Route("GetBuilding")]
        [HttpGet]
        public IHttpActionResult GetBuilding(int cityID, int constructionID)
        {
            var response = new BaseApiResponse<List<BuildingViewModel>>();
            var data = string.Empty;

            try
            {
                data = GetHttpResult("Building/Get", new Dictionary<string, object> {
                    { "CityID",cityID },
                    { "ConstructionID", constructionID }
                });
                var dataList = JsonConvert.DeserializeObject<List<BuildingViewModel>>(data);

                response.Status = StatusEnum.Success.ToString();
                response.Data = dataList;
            }
            catch (Exception exception)
            {
                response.Message = string.IsNullOrEmpty(data) ? exception.Message : data;
                response.Status = StatusEnum.Error.ToString();
            }
            return Json(response);
        }

        /// <summary>
        /// 获取所有房间
        /// http://localhost:6963/api/Estimate/GetHouse?CityID=3&BuildingID=195748
        /// </summary>
        /// <param name="cityID">城市ID</param>
        /// <param name="buildingID">楼栋ID</param>
        /// <returns></returns>
        [Route("GetHouse")]
        [HttpGet]
        public IHttpActionResult GetHouse(int cityID, int buildingID)
        {
            var response = new BaseApiResponse<List<HouseViewModel>>();
            var data = string.Empty;

            try
            {
                data = GetHttpResult("House/Get", new Dictionary<string, object> {
                    { "CityID",cityID },
                    { "BuildingID", buildingID }
                });
                var dataList = JsonConvert.DeserializeObject<List<HouseViewModel>>(data);

                response.Status = StatusEnum.Success.ToString();
                response.Data = dataList;
            }
            catch (Exception exception)
            {
                response.Message = string.IsNullOrEmpty(data) ? exception.Message : data;
                response.Status = StatusEnum.Error.ToString();
            }
            return Json(response);
        }

        /// <summary>
        /// 获取报价
        /// http://localhost:6963/api/Estimate/GetAutoPrice?CityID=3&ConstructionID=7842&BuildingID=195748&HouseID=7876829&Size=100
        /// </summary>
        /// <param name="cityID">城市ID</param>
        /// <param name="constructionID">楼盘ID</param>
        /// <param name="buildingID">楼栋ID</param>
        /// <param name="houseID">房号ID</param>
        /// <param name="size">面积，小数两位</param>
        /// <returns></returns>
        [Route("GetAutoPrice")]
        [HttpGet]
        public IHttpActionResult GetAutoPrice(int cityID, int constructionID, int buildingID, int houseID, double size)
        {
            var response = new BaseApiResponse<AutoPriceViewModel>();
            var data = string.Empty;

            try
            {
                data = GetHttpResult("AutoPrice/Get", new Dictionary<string, object> {
                    { "CityID",cityID },
                    { "ConstructionID", constructionID },
                    { "BuildingID", buildingID },
                    { "HouseID", houseID },
                    { "Size", size }
                });
                var result = JsonConvert.DeserializeObject<AutoPriceViewModel>(data);

                response.Status = StatusEnum.Success.ToString();
                response.Data = result;
            }
            catch (Exception exception)
            {
                response.Message = string.IsNullOrEmpty(data) ? exception.Message : data;
                response.Status = StatusEnum.Error.ToString();
            }
            return Json(response);
        }
    }
}