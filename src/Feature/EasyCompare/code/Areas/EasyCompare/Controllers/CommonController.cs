using System.Web.Mvc;
using Glass.Mapper.Sc;
using Sitecore.Mvc.Presentation;
using Sitecore.Web;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare.Models.Modules;
using System.Net.Mail;
using System;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Mvc.Configuration;
using Sitecore.Feature.EasyCompare.Areas.EasyCompare.Repositories;
using AutoGeneralTH.ApiModel.Portal;
using AutoGeneralTH.ApiModel.Common;
using System.Collections.Generic;

namespace Sitecore.Feature.EasyCompare.Areas.EasyCompare
{
    public class CommonController : Controller
    {
        private readonly SitecoreContext _sitecoreContext;
        private ECQuoteAndBuyRepository qbRepository;

        // GET: Common
        public CommonController()
        {
            _sitecoreContext = new SitecoreContext();
            qbRepository = new ECQuoteAndBuyRepository();

        }
        public ActionResult Header()
        {
            IHeaderModel header = default(IHeaderModel);
            if (!string.IsNullOrWhiteSpace(RenderingContext.Current.Rendering.DataSource))
            {
                header = _sitecoreContext.GetItem<IHeaderModel>(RenderingContext.Current.Rendering.DataSource);
            }
            var parameters = WebUtil.ParseUrlParameters(RenderingContext.Current.Rendering["Parameters"]);

            header.IsFixedHeader = false;
            if (parameters != null)
            {
                header.IsFixedHeader = (parameters["Is Fixed Header"] != null && parameters["Is Fixed Header"] == "1") ? true : false;
            }
            return View("~/Areas/EasyCompare/Views/Common/HeaderView.cshtml", header);

        }

        public ActionResult TopNavigation()
        {
            ITopNavigatioModel header = default(ITopNavigatioModel);
            if (!string.IsNullOrWhiteSpace(RenderingContext.Current.Rendering.DataSource))
            {
                header = _sitecoreContext.GetItem<ITopNavigatioModel>(RenderingContext.Current.Rendering.DataSource);
            }
            return View("~/Areas/EasyCompare/Views/Common/TopNavigationView.cshtml", header);

        }

        public ActionResult CarouselBanner()
        {
            BannerCarousel banner = default(BannerCarousel);
            if (!string.IsNullOrWhiteSpace(RenderingContext.Current.Rendering.DataSource))
            {
                banner = _sitecoreContext.GetItem<BannerCarousel>(RenderingContext.Current.Rendering.DataSource);
            }
            return View("~/Areas/EasyCompare/Views/Components/CarouselBanner.cshtml", banner);

        }

        public ActionResult CallMeBackMarketing()
        {
            try
            {
                var u = new UrlHelper(this.Request.RequestContext);
                string callMeBackUrl = u.Action(actionName: "CallMeBack", controllerName: "Common", routeValues: null);

                var pageData = new
                {
                    callMeBackUrl = callMeBackUrl,
                    MasterData = new
                    {
                        Today = DateTime.Today,
                        ErrMsgs = new
                        {
                            NameTitle = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "Name"),
                            NameRequiredMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "name-required-message"),
                            EmailTitle = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "E-mail"),
                            EmailRequiredMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "email-required-message"),
                            EmailInvalidMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "email-invalid-message"),
                            PhoneTitle = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "Phone Number"),
                            PhoneRequiredMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "phone-number-required-message"),
                            LineTitle = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "Line ID"),
                            LineRequiredMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "line-id-required-message"),
                            CallbackTimeTitle = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "callback-time-label"),
                            CallbackTimeRequiredMsg = Sitecore.Globalization.Translate.TextByDomain(Constants.ContactListDomain, "callback-time-required-message"),
                        }
                    }
                };

                this.ViewBag.pageConfigData = qbRepository.GetJsonStringTrimBlank(pageData);
            }
            catch (Exception ex)
            {
                this.qbRepository.HandleException(ex, true);
            }

            return View("~/Areas/EasyCompare/Views/Components/CallMeBackMarketing.cshtml");
        }

        [HttpPost]
        [Celes.Framework.Common.Attributes.CelesAntiForgeryValidate]
        public ActionResult CallMeBack(CallMeBackViewModel form)
        {
            try
            {
                this.ValidateCallMeBackFormInput(form);

                var commonService = new AutoGeneralTH.ApiProxy.CommonService();

                var emailMessage = new ApiEmailRequest();

                emailMessage.TemplateName = "Marketing - Call Me Back - EasyCompare";
                emailMessage.ToList = new System.Collections.Generic.List<ApiEmailAddress>();

                var toList = Sitecore.Globalization.Translate.TextByDomain(Constants.CallMeBackDomain, "call-me-back-marketing-to-list");

                if (!string.IsNullOrEmpty(toList))
                {
                    var toEmails = toList.Split(new string[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var email in toEmails)
                    {
                        emailMessage.ToList.Add(new ApiEmailAddress()
                        {
                            Name = string.Empty,
                            Email = email.Trim()
                        });
                    }
                }

                emailMessage.FieldValueList = new System.Collections.Generic.Dictionary<string, string>();

                emailMessage.FieldValueList.Add("-customername-", form.Name);
                emailMessage.FieldValueList.Add("-phonenumber-", form.OptionValue);
                emailMessage.FieldValueList.Add("-timeofday-", form.CallBackTime);

                commonService.SendEmail(emailMessage);

                var settings = this.qbRepository.GetQuoteAndBuySettings();

                Session["market-call-me-back-contact"] = form;

                var result = new
                {
                    status = "S",
                    redirectUrl = this.Url.Content(settings.MarketingCallMeBackThankYouUrl),
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(qbRepository.HandleException(ex));
            }
        }

        private void ValidateCallMeBackFormInput(CallMeBackViewModel form)
        {
            this.qbRepository.ValidateInputString(form.Name);
            this.qbRepository.ValidateInputString(form.Option);
            this.qbRepository.ValidateInputString(form.OptionValue);
            this.qbRepository.ValidateInputString(form.SKey);
            this.qbRepository.ValidateInputString(form.CallBackTime);
        }

        public ActionResult AskAQuestion(AskAQuestionModel askQuestionModel)
        {
            if (askQuestionModel != null && askQuestionModel.EmailId!=null)
            {
                var msg = GetMailMessage(askQuestionModel);
                SendEmailMessage(msg);
            }
            Item item = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse("{062BE162-0D46-4459-9A7D-CB7C80CF9BB4}"));
            var pathInfo = LinkManager.GetItemUrl(item, UrlOptions.DefaultOptions);
            return RedirectToRoute(MvcSettings.SitecoreRouteName, new { pathInfo = pathInfo.TrimStart(new char[] { '/' }) });

        }
        public void SendEmailMessage(MailMessage msg)
        {
            
            try
            {
                using (var smtpClient = new SmtpClient { DeliveryMethod = SmtpDeliveryMethod.Network ,EnableSsl = true})
                {
                    smtpClient.Send(msg);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error in Email Send" + ex.Message + ex.InnerException.Message +ex.StackTrace ,this);
            }
            finally
            {

            }
        }
        private MailMessage GetMailMessage(AskAQuestionModel askQuestionModel)
        {
            
            var msg = new MailMessage();
            if (askQuestionModel.EmailId != null)
            {
                AskAQuestionModel sitecoreEmail = new AskAQuestionModel();
                sitecoreEmail = _sitecoreContext.GetItem<AskAQuestionModel>("{51E71645-55C2-4607-8BD0-8C12274561CF}");              
                msg.To.Add(new MailAddress(askQuestionModel.EmailId));
                msg.Subject = sitecoreEmail.Subject;
                msg.From = new MailAddress(sitecoreEmail.FromEmailId);
                msg.Body = sitecoreEmail.Body;
                if (!string.IsNullOrEmpty(askQuestionModel.Name))
                {
                    msg.Body = msg.Body.Replace("$$FirstName$$", askQuestionModel.Name);
                }
                if (!string.IsNullOrEmpty(askQuestionModel.PhoneNumber))
                {
                    msg.Body = msg.Body.Replace("$$PhoneNumber$$", askQuestionModel.PhoneNumber);
                }
                if (!string.IsNullOrEmpty(askQuestionModel.PhoneNumber))
                {
                    msg.Body = msg.Body.Replace("$$LineId$$", askQuestionModel.LineId);
                }
                if (!string.IsNullOrEmpty(askQuestionModel.PhoneNumber))
                {
                    msg.Body = msg.Body.Replace("$$WhenWeCanCallYou$$", askQuestionModel.WhenWecanCall);
                }
                if (!string.IsNullOrEmpty(askQuestionModel.PhoneNumber))
                {
                    msg.Body = msg.Body.Replace("$$YourQuestion$$", askQuestionModel.YourQuestion);
                }
                msg.IsBodyHtml = true;               
            }
            return msg;
        }
    }
}