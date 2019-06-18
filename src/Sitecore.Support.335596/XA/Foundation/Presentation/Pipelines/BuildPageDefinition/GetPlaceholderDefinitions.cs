using System.Collections.Generic;
using System.Xml.Linq;
using Sitecore.Data;
using Sitecore.DependencyInjection;
using Sitecore.Layouts;
using Sitecore.Mvc.Pipelines.Response.BuildPageDefinition;
using Sitecore.XA.Foundation.Abstractions;
using Sitecore.XA.Foundation.Presentation;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.XA.Foundation.Multisite.Extensions;

namespace Sitecore.Support.XA.Foundation.Presentation.Pipelines.BuildPageDefinition
{
    public class GetPlaceholderDefinitions : Sitecore.XA.Foundation.Presentation.Pipelines.BuildPageDefinition.GetPlaceholderDefinitions
    {
        public override void Process(BuildPageDefinitionArgs args)
        {
            if (!ServiceLocator.ServiceProvider.GetService<IContext>().Site.IsSxaSite())
            {
                return;
            }

            var placeholderPageDefinition = new PlaceholderPageDefinition(args.Result);
            AddPlaceholders(placeholderPageDefinition, args);
            args.Result = placeholderPageDefinition;
        }

        protected override void AddPlaceholders(PlaceholderPageDefinition definition, BuildPageDefinitionArgs args)
        {
            #region Fix 335956
            // Get layout definition to reuse it
            object layoutDefinition;
            args.CustomData.TryGetValue("xmlBasedLayoutDefinition", out layoutDefinition);
            if (layoutDefinition == null)
            {
                return;
            }
            #endregion

            Dictionary<ID, IEnumerable<PlaceholderDefinition>> placeholders = GetPlaceholders(layoutDefinition as XElement, args);
            definition.Placeholders = placeholders;
        }
    }
}