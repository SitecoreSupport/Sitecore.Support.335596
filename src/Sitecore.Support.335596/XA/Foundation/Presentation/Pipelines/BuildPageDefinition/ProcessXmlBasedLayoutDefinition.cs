using System.Collections.Generic;
using System.Xml.Linq;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines;
using Sitecore.Mvc.Pipelines.Response.BuildPageDefinition;
using Sitecore.Mvc.Pipelines.Response.GetXmlBasedLayoutDefinition;
using Sitecore.Mvc.Presentation;

namespace Sitecore.Support.XA.Foundation.Presentation.Pipelines.BuildPageDefinition
{
    public class ProcessXmlBasedLayoutDefinition : Sitecore.Mvc.Pipelines.Response.BuildPageDefinition.ProcessXmlBasedLayoutDefinition
    {
        protected override void AddRenderings(PageDefinition pageDefinition, BuildPageDefinitionArgs args)
        {
            XElement layoutDefinition = PipelineService.Get().RunPipeline<GetXmlBasedLayoutDefinitionArgs, XElement>("mvc.getXmlBasedLayoutDefinition", new GetXmlBasedLayoutDefinitionArgs(), a => a.Result);
            if (layoutDefinition == null)
            {
                return;
            }

            #region Fix 335596
            // Save layout definition to reuse it later
            args.CustomData.Add("xmlBasedLayoutDefinition", layoutDefinition);
            #endregion

            IEnumerable<Rendering> renderings = GetRenderings(layoutDefinition, args);
            pageDefinition.Renderings.AddRange(renderings);
        }

        public override void Process([NotNull] BuildPageDefinitionArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            var pageDefinition = args.Result;

            if (pageDefinition == null)
            {
                return;
            }

            this.AddRenderings(pageDefinition, args);
        }
    }
}