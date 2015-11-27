using Abp.Web.Mvc.Views;

namespace LyuAdmin.Web.Views
{
    public abstract class LyuAdminWebViewPageBase : LyuAdminWebViewPageBase<dynamic>
    {

    }

    public abstract class LyuAdminWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected LyuAdminWebViewPageBase()
        {
            LocalizationSourceName = LyuAdminConsts.LocalizationSourceName;
        }
    }
}