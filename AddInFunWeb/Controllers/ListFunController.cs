using AddInFunWeb.Helpers;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AddInFunWeb.Controllers
{
    public class ListFunController : Controller
    {
        // GET: ListFun
        public ActionResult Index(Boolean? isSuccess)
        {
            if (isSuccess.HasValue && isSuccess.Value)
            {
                ViewBag.Message = "Yes";
            }

            return View();
        }
        [HttpPost]
        public ActionResult Index(string NewListTitle, string SPHostUrl)
        {

            try
            {
                using (ClientContext ctx = ContextHelper.GetContext())
                {
                    List list = ctx.Web.CreateList(ListTemplateType.GenericList, NewListTitle, false, true);

                }
            }
            catch
            {
                return View();
            }

                return RedirectToAction("Index", new { SPHostUrl = SPHostUrl, isSuccess = true });
        }

        public ActionResult CreateListitem(Boolean? isSuccess)
        {
            if (isSuccess.HasValue && isSuccess.Value)
            {
                ViewBag.Message = "Yes item created";
            }
            List<ListCollection> listcol = new List<ListCollection>();
            using (ClientContext ctx = ContextHelper.GetContext())
            {
                List list = ctx.Web.GetListByTitle("TimsAddedList");
                list.Update();
                ctx.ExecuteQuery();

                ListItemCollection item = list.GetItems(CamlQuery.CreateAllItemsQuery());
                ctx.Load(item);
                ctx.ExecuteQuery();


                ViewBag.list = item.ToList(); 


                return View();
            }

            
        }
        [HttpPost]
        public ActionResult CreateListitem(string NewItemTitle, string SPHostUrl)
        {
            try
            {
                using (ClientContext ctx = ContextHelper.GetContext())
                {
                    List list = ctx.Web.GetListByTitle("TimsAddedList");

                    ListItem item = list.AddItem(new ListItemCreationInformation());
                    item["Title"] = NewItemTitle;
                    item.Update();
                    ctx.ExecuteQuery();
                }
            }
            catch
            {
                return View();
            }

            return RedirectToAction("CreateListitem", new { SPHostUrl = SPHostUrl, isSuccess = true });
        }


    }
}