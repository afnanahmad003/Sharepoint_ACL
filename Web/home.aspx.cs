using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace selftprovisioning.web
{
    public partial class home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Refresh();

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["code"]))
                {
                    GetToken();
                }
            }
        }
        private void GetToken()
        {
            var authority = "https://login.microsoftonline.com";
            var resource = "https://graph.microsoft.com";

            var tenant = "common";
            var authorizeSuffix = "oauth2";

            string code = Request.QueryString["code"];
            string clientId = "9d52b93f-7461-4e5f-bff3-6ae331418493";
            string secrect = "a42CzqQl71gyMCJv.~7_SOtY6~j0_.I5hE";
            string redirectURL = ConfigurationManager.AppSettings["ReturnUrl"];
            try
            {

                {
                    #region Get Token

                    string authGuid = System.Guid.NewGuid().ToString();
                    string errorMessage = string.Empty;
                    string UPN = string.Empty;
                    string IPAddr = string.Empty;
                    string name = string.Empty;
                    string accessToken = string.Empty;
                    string refreshToken = string.Empty;

                    try

                    {
                        var EndPointUrl = String.Format("{0}/{1}/{2}", authority, tenant, authorizeSuffix);


                        var parameters = new Dictionary<string, string>
                        {
                            { "resource", resource},
                            { "client_id", clientId },
                            { "code",  code},
                            { "grant_type", "authorization_code" },
                            { "redirect_uri", redirectURL},
                            { "client_secret",secrect}
                        };


                        var list = new List<string>();

                        foreach (var parameter in parameters)
                        {
                            if (!string.IsNullOrEmpty(parameter.Value))
                                list.Add(string.Format("{0}={1}", parameter.Key, HttpUtility.UrlEncode(parameter.Value)));
                        }
                        var strParameters = string.Join("&", list);


                        var content = new StringContent(strParameters, Encoding.GetEncoding("utf-8"), "application/x-www-form-urlencoded");

                        var client = new HttpClient();

                        var url = string.Format("{0}/token", EndPointUrl);

                        var response = client.PostAsync(url, content).Result;

                        var text = response.Content.ReadAsStringAsync().Result;

                        var result = JsonConvert.DeserializeObject(text) as JObject;

                        accessToken = result.GetValue("access_token").Value<string>();
                        refreshToken = result.GetValue("refresh_token").Value<string>();

                        Session.Add("accessToken", accessToken);
                        Session.Add("refreshToken", refreshToken);

                        Response.Redirect("~/manage-access.aspx");
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                    }


                    #endregion Get Token
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string requestURL = string.Empty;
            var authority = "https://login.microsoftonline.com";

            var tenant = "common";
            var authorizeSuffix = "oauth2";

            //string clientId = "460b3770-5b13-42ca-96c6-2ad32a1fe02c";
            //string clientId = "2ba0a098-7d57-46a5-843d-a191c9b8b6ba";
            string clientId = "9d52b93f-7461-4e5f-bff3-6ae331418493";
            //string secrect = "pfTlX4FuJ[PT-I7[c-Iqqyh1pvA1mVD8";
            //string secrect = "fJ6_nN34Tsn~st_Cra3AoB--J6aJ9.VOXX";
            //string secrect = "a42CzqQl71gyMCJv.~7_SOtY6~j0_.I5hE";
            string redirectURL = ConfigurationManager.AppSettings["ReturnUrl"];


            {
                var EndPointUrl = String.Format("{0}/{1}/{2}/authorize?", authority, tenant, authorizeSuffix);

                var parameters = new Dictionary<string, string>
                    {
                        { "response_type", "code" },
                        { "client_id", clientId },
                        { "redirect_uri", redirectURL },
                        { "prompt", "select_account"}
                    };

                var list = new List<string>();

                foreach (var parameter in parameters)
                {
                    if (!string.IsNullOrEmpty(parameter.Value))
                        list.Add(string.Format("{0}={1}", parameter.Key, HttpUtility.UrlEncode(parameter.Value)));
                }
                var strParameters = string.Join("&", list);
                requestURL = String.Concat(EndPointUrl, strParameters);
            }


            Response.Redirect(requestURL);
        }


        //private void Refresh()
        //{
        //    //string EndPointUrl = "https://login.microsoftonline.com/b82a07dd-224d-48d4-8db1-d393a57a4777/oauth2/v2.0/token";
        //    string EndPointUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/token";

        //    string clientId = "9d52b93f-7461-4e5f-bff3-6ae331418493";
        //    string secrect = "a42CzqQl71gyMCJv.~7_SOtY6~j0_.I5hE";

        //    try
        //    {

        //        {
        //            #region Get Token

        //            string authGuid = System.Guid.NewGuid().ToString();
        //            string errorMessage = string.Empty;
        //            string UPN = string.Empty;
        //            string IPAddr = string.Empty;
        //            string name = string.Empty;
        //            string accessToken = string.Empty;
        //            string refreshToken = string.Empty;

        //            try

        //            {
        //                var parameters = new Dictionary<string, string>
        //                {
        //                    { "client_id", clientId },
        //                    { "grant_type", "refresh_token" },
        //                    { "refresh_token",  "BeaAQABAAAAAAAGV_bv21oQQ4ROqh0_1-tA1gFAPI8EFoiY4FSWWiCqJfTINQqxChxHC4CnRmBD6SXSrteqEEeG_vaneaOZwUX2VCexzcQrFhzrc3pUejaPreoJYMrAni2GFR2OLgXTGsf0-t4ZZxEt_zBmsQors3-Yx2QFzIewGaTbYaqzB20hmS5b15PjWHp0imSYpxXsXSK6pOcy5nUfJbrh8EtxfD8AwCIm_VMNvjR9jdCrbn6WGXGkcwFgy0-Z0q-wS8KKeTJ6ZWVzr7Ww5cFKDJTPnGrrC1F-IZNyDWRTIsa2cOwUCcEcXfHGGzZ1tlPlVlLXV_-ZiGlDP8KkcXiKEySLU-1Npk7ebEHpxGHkdiJO66Gu-W1IZjcqWIc_Hf_aqfKyQc60axaXvKkxJ5SJEZpaya7xZLuhvHdmfFiFNTuDRRpMh0tfcW69y-_6M9aFJfYi4AtLWCBFgDltpcRXZvbF3JiYhCDGQcGjkghYsCMDfAbgY1pBgQwzVZJKZn_GeB3irebjdpMOASWi9Q9y2GEmpTUUUIVkfJ5N4TMuNk6Rl9DtI2-RqdhR87FogQN7J7tLhLURyCDErVkYDd3s149toWwEKUULC19ncIuMXhPK6mbIh2EhZzXebYXXPaXHH4siRByup-LNYGdb63ojMYm_Onr5RHlejHO_XrsOj-PXKQLrA_CdpkmwKJrvLEyG5umkij8WS7ATOCJYm8BqqOV6044i_t741gU6y6Ic78MKNsDZPjyfSHdlVzubWLxDdDPA6dF5mJN5KEDM0ysLYVtIEgeMbE_NgoYxyv7AvL0_QGJKw_RML0uEsk34drouHNi5qAnTXN2BTkgr3rCqCoAVEqZo2xRlyYCkucdUgIGXc-RcS4NRgfReweWYb-4TOJaA7oYzc7JZe8j1Rq9IwS5lfwHyGEH8loxbn18xBpENvs8ges2AnLLkgRvVUUe4Nc_olCIgAArer" },
        //                    { "client_secret",secrect}
        //                };


        //                var list = new List<string>();

        //                foreach (var parameter in parameters)
        //                {
        //                    if (!string.IsNullOrEmpty(parameter.Value))
        //                        list.Add(string.Format("{0}={1}", parameter.Key, HttpUtility.UrlEncode(parameter.Value)));
        //                }
        //                var strParameters = string.Join("&", list);

        //                strParameters = "{\"client_id\":\"9d52b93f-7461-4e5f-bff3-6ae331418493\",\"refresh_token\":\"BeaAQABAAAAAAAGV_bv21oQQ4ROqh0_1-tA1gFAPI8EFoiY4FSWWiCqJfTINQqxChxHC4CnRmBD6SXSrteqEEeG_vaneaOZwUX2VCexzcQrFhzrc3pUejaPreoJYMrAni2GFR2OLgXTGsf0-t4ZZxEt_zBmsQors3-Yx2QFzIewGaTbYaqzB20hmS5b15PjWHp0imSYpxXsXSK6pOcy5nUfJbrh8EtxfD8AwCIm_VMNvjR9jdCrbn6WGXGkcwFgy0-Z0q-wS8KKeTJ6ZWVzr7Ww5cFKDJTPnGrrC1F-IZNyDWRTIsa2cOwUCcEcXfHGGzZ1tlPlVlLXV_-ZiGlDP8KkcXiKEySLU-1Npk7ebEHpxGHkdiJO66Gu-W1IZjcqWIc_Hf_aqfKyQc60axaXvKkxJ5SJEZpaya7xZLuhvHdmfFiFNTuDRRpMh0tfcW69y-_6M9aFJfYi4AtLWCBFgDltpcRXZvbF3JiYhCDGQcGjkghYsCMDfAbgY1pBgQwzVZJKZn_GeB3irebjdpMOASWi9Q9y2GEmpTUUUIVkfJ5N4TMuNk6Rl9DtI2-RqdhR87FogQN7J7tLhLURyCDErVkYDd3s149toWwEKUULC19ncIuMXhPK6mbIh2EhZzXebYXXPaXHH4siRByup-LNYGdb63ojMYm_Onr5RHlejHO_XrsOj-PXKQLrA_CdpkmwKJrvLEyG5umkij8WS7ATOCJYm8BqqOV6044i_t741gU6y6Ic78MKNsDZPjyfSHdlVzubWLxDdDPA6dF5mJN5KEDM0ysLYVtIEgeMbE_NgoYxyv7AvL0_QGJKw_RML0uEsk34drouHNi5qAnTXN2BTkgr3rCqCoAVEqZo2xRlyYCkucdUgIGXc-RcS4NRgfReweWYb-4TOJaA7oYzc7JZe8j1Rq9IwS5lfwHyGEH8loxbn18xBpENvs8ges2AnLLkgRvVUUe4Nc_olCIgAArer\",\"grant_type\":\"refresh_token\",\"client_secret\":\"a42CzqQl71gyMCJv.%7E7_SOtY6%7Ej0_.I5hE\"}";

        //                //var content = new StringContent(strParameters, Encoding.GetEncoding("utf-8"), "application/x-www-form-urlencoded");
        //                var content = new StringContent(strParameters, Encoding.GetEncoding("utf-8"), "application/json");



        //                var client = new HttpClient();


        //                var response = client.PostAsync(EndPointUrl, content).Result;

        //                var text = response.Content.ReadAsStringAsync().Result;

        //                var result = JsonConvert.DeserializeObject(text) as JObject;

        //                accessToken = result.GetValue("access_token").Value<string>();
        //                refreshToken = result.GetValue("refresh_token").Value<string>();

        //                Session.Add("accessToken", accessToken);
        //                Session.Add("refreshToken", refreshToken);

        //                Response.Redirect("~/tokens.aspx");

        //                //string userData = accessToken.Split('.')[1].Replace('-', '+').Replace('_', '/');

        //                //while ((userData.Length % 4) > 0)
        //                //{
        //                //    userData = userData += "=";

        //                //}


        //                //byte[] data = null;

        //                //try
        //                //{
        //                //    data = Convert.FromBase64String(userData);
        //                //    string decodedString = Encoding.ASCII.GetString(data);
        //                //    JToken token = JObject.Parse(decodedString);

        //                //    UPN = token["upn"].Value<string>();
        //                //    IPAddr = token["ipaddr"].Value<string>();
        //                //    name = token["name"].Value<string>();
        //                //}
        //                //catch (Exception ex)
        //                //{

        //                //    errorMessage = "Parse :" + ex.Message;
        //                //}
        //            }
        //            catch (Exception ex)
        //            {
        //                errorMessage = ex.Message;
        //            }


        //            #endregion Get Token
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }
}