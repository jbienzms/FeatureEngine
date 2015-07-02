using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.FeatureEngine
{
    /// <summary>
    /// Resolves assets from feature packs.
    /// </summary>
    public interface IFeatureAssetResolver
    {
        /// <summary>
        /// Gets a feature action.
        /// </summary>
        /// <param name="featureId">
        /// The ID of the feature containing the action.
        /// </param>
        /// <param name="actionId">
        /// The ID of the action.
        /// </param>
        /// <returns>
        /// The action.
        /// </returns>
        IAction GetAction(string featureId, string actionId);

        /// <summary>
        /// Gets a feature.
        /// </summary>
        /// <param name="featureId">
        /// The ID of the feature.
        /// </param>
        /// <returns>
        /// The feature.
        /// </returns>
        IFeature GetFeature(string featureId);

        /// <summary>
        /// Gets an item template.
        /// </summary>
        /// <param name="featureId">
        /// The ID of the feature containing the template.
        /// </param>
        /// <param name="templateId">
        /// The ID of the template.
        /// </param>
        /// <returns>
        /// The template.
        /// </returns>
        IItemTemplate GetItemTemplate(string featureId, string templateId);

        /// <summary>
        /// Gets a project template.
        /// </summary>
        /// <param name="featureId">
        /// The ID of the feature containing the template.
        /// </param>
        /// <param name="templateId">
        /// The ID of the template.
        /// </param>
        /// <returns>
        /// The template.
        /// </returns>
        IProjectTemplate GetProjectTemplate(string featureId, string templateId);
    }
}
