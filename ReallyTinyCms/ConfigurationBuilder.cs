using System;
using System.Web.Routing;
using ReallyTinyCms.Core;
using ReallyTinyCms.Core.ContentPipline;

namespace ReallyTinyCms
{
    public class ConfigurationBuilder
    {
        public ContentService ContentService { get; private set; }

        public ConfigurationBuilder(ContentService contentService)
        {
            if (contentService == null)
            {
                throw new ArgumentNullException("contentService", "Configuration builder requires a content service to configure.");
            }
                
            ContentService = contentService;
        }

        public ConfigurationBuilder WhenCacheRefreshes(Action action)
        {
            if (action == null)
            {
                action = () => { };
            }

            ContentService.CacheRefreshCallback = action;
            return this;
        } 

        public ConfigurationBuilder WhenContentIsRequested(Action<string, string> action)
        {
            if (action == null)
            {
                action = (x,y) => { };
            }

            ContentService.ContentForCallback = action;
            return this;
        }   

        public ConfigurationBuilder EditModeShouldBeEnabledWhen(Func<RequestContext, bool> funcWhichVerifiesRequesterIsAllowedToEdit)
        {
            if (funcWhichVerifiesRequesterIsAllowedToEdit == null)
            {
                return this; // Keep defaults.
            }

            ContentService.ContentRegistration.RequesterIsAllowedToEditContent = funcWhichVerifiesRequesterIsAllowedToEdit;
            return this;
        }    

        public ConfigurationBuilder WithFilters(params IContentPipelineFilter[] filters)
        {
            if (filters == null)
            {
                filters = new IContentPipelineFilter[] {};
            }

            foreach (var contentPipelineFilter in filters)
            {
                ContentService.ContentRegistration.ContentPipelineFilters.Add(contentPipelineFilter);
            }

            return this;
        }  
    }
}