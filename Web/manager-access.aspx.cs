using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


namespace selftprovisioning.web
{
    public partial class gerenciar_acessos : Page
    {
        private string Bearer
        {
            get
            {
                if (Session["accessToken"] != null)
                    return Session["accessToken"].ToString();
                else
                    return ConfigurationManager.AppSettings["Bearer"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            string EndPointUrl = "https://graph.microsoft.com/v1.0/d0427131-a616-45ca-851c-b61c02fe302d/drive/root:/{0}:/children";

            try

            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Bearer);

                var response = client.GetAsync(string.Format(EndPointUrl, TreeView1.SelectedNode.ToolTip)).Result;

                var text = response.Content.ReadAsStringAsync().Result;

                JObject result = JsonConvert.DeserializeObject(text) as JObject;

                TreeView1.SelectedNode.ChildNodes.Clear();

                foreach (var item in result.GetValue("value"))
                {
                    TreeView1.SelectedNode.ChildNodes.Add(new TreeNode() { ImageUrl = "~/images/folder.png", Expanded = true, Text = item["name"].ToString(), Value = item["id"].ToString(), ToolTip = TreeView1.SelectedNode.ToolTip + "\\" + item["name"].ToString() });
                }

                LoadPermission(TreeView1.SelectedNode.ToolTip);
            }
            catch (Exception ex)
            {
             
            }
        }

        private void LoadPermission(string path)
        {

            string EndPointUrl = string.Format("https://graph.microsoft.com/v1.0/d0427131-a616-45ca-851c-b61c02fe302d/drive/root:/{0}:/permissions", path);

            try

            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Bearer);

                var response = client.GetAsync(EndPointUrl).Result;

                var text = response.Content.ReadAsStringAsync().Result;

                JObject result = JsonConvert.DeserializeObject(text) as JObject;

                List<PermissionData> permissions = new List<PermissionData>();

                foreach (var item in result.GetValue("value"))
                {
                    PermissionData permissionData = new PermissionData();

                    if (item["hasPassword"] != null)
                        permissionData.HasPassword = (item["hasPassword"].ToString().ToLower() == "true");

                    if (item["id"] != null)
                        permissionData.Id = item["id"].ToString();

                    if (item["link"] != null && item["link"]["type"] != null)
                        permissionData.LinkType = item["link"]["type"].ToString();

                    if (item["link"] != null && item["link"]["preventsDownload"] != null)
                        permissionData.PreventsDownload = (item["link"]["preventsDownload"].ToString() == "true");


                    if (item["link"] != null && item["link"]["scope"] != null)
                        permissionData.Scope = item["link"]["scope"].ToString();

                    if (item["link"] != null && item["link"]["webUrl"] != null)
                        permissionData.WebUrl = item["link"]["webUrl"].ToString();

                    if (item["roles"] != null && item["roles"][0] != null)
                        permissionData.Roles = item["roles"][0].ToString();

                    permissionData.IsInherited = (item["inheritedFrom"] != null);

                    if (item["grantedToIdentities"] != null && item["grantedToIdentities"][0]["user"]["email"] != null)
                        permissionData.Email = item["grantedToIdentities"][0]["user"]["email"].ToString();

                    if (item["grantedToIdentities"] != null && item["grantedToIdentities"][0]["user"]["displayName"] != null)
                        permissionData.DisplayName = item["grantedToIdentities"][0]["user"]["displayName"].ToString();

                    if (item["grantedTo"] != null && item["grantedTo"]["user"]["displayName"] != null)
                        permissionData.DisplayName = item["grantedTo"]["user"]["displayName"].ToString();

                    permissions.Add(permissionData);
                }

                rptPermission.DataSource = permissions;
                rptPermission.DataBind();
            }
            catch (Exception ex)
            {

            }

        }

        private void LoadData()
        {

            string EndPointUrl = "https://graph.microsoft.com/v1.0/d0427131-a616-45ca-851c-b61c02fe302d/drive/root/children";

            try

            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Bearer);

                var response = client.GetAsync(EndPointUrl).Result;

                var text = response.Content.ReadAsStringAsync().Result;

                JObject result = JsonConvert.DeserializeObject(text) as JObject;

                foreach (var item in result.GetValue("value"))
                {
                    TreeView1.Nodes.Add(new TreeNode() { ImageUrl="~/images/folder.png",  Text = item["name"].ToString(), Value = item["id"].ToString(), ToolTip = item["name"].ToString() });
                }

            }
            catch (Exception ex)
            {
            }

        }

        protected void rptPermission_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.ItemType==ListItemType.Item || e.Item.ItemType==ListItemType.AlternatingItem)
            {
                HtmlTableCell tdUser = e.Item.FindControl("tdUser") as HtmlTableCell;
                HtmlTableCell tdAccess = e.Item.FindControl("tdAccess") as HtmlTableCell;
                HtmlTableCell tdShare = e.Item.FindControl("tdShare") as HtmlTableCell;
                CheckBox chkItem = e.Item.FindControl("chkItem") as CheckBox;

                PermissionData permissionData = e.Item.DataItem as PermissionData;

                tdUser.InnerText = permissionData.DisplayName;

                tdAccess.InnerText = permissionData.Roles;
                tdShare.InnerText = permissionData.WebUrl;

                chkItem.Enabled = !permissionData.IsInherited;
            }
        }
    }
}
