using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;

namespace DVModApi
{
    public class DVModAssets
    {
        /// <summary>
        /// Load Asset bundle from yours mod folder
        /// </summary>
        /// <param name="modEntry">Your mod entry</param>
        /// <param name="bundleFileName">Bundle file name to load from yours mod folder</param>
        /// <returns>AssetBundle</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static AssetBundle LoadAssetBundle(UnityModManager.ModEntry modEntry, string bundleFileName)
        {
            string bundle = Path.Combine(modEntry.Path, bundleFileName);

            if (File.Exists(bundle))
            {
                LogHelper.Log($"Loading AssetBundle: {bundle}...");
                return AssetBundle.LoadFromMemory(File.ReadAllBytes(bundle));
            }
            else
            {
                throw new FileNotFoundException($"<color=#00ffffff>[DVModAPI]</color> <color=red>LoadAssetBundle() Error:</color> File not found: {bundle} {Environment.NewLine}", bundle);
            }
        }

        /// <summary>
        /// Load texture (*.png, *.jpg) from yours mod folder
        /// </summary>
        /// <param name="modEntry">Your mod entry</param>
        /// <param name="fileName">File name to load from assets folder (for example "texture.png")</param>
        /// <returns>Returns unity Texture2D</returns>
        public static Texture2D LoadTexture(UnityModManager.ModEntry modEntry, string fileName)
        {
            string fn = Path.Combine(modEntry.Path, fileName);

            if (!File.Exists(fn))
            {
                throw new FileNotFoundException($"<color=#00ffffff>[DVModAPI]</color> <color=red>LoadTexture() Error:</color> File not found: {fn}{Environment.NewLine}", fn);
            }
            string ext = Path.GetExtension(fn).ToLower();
            if (ext == ".png" || ext == ".jpg")
            {
                Texture2D t2d = new Texture2D(1, 1);
                t2d.LoadImage(File.ReadAllBytes(fn));
                return t2d;
            }
            else
            {
                throw new NotSupportedException($"<color=#00ffffff>[DVModAPI]</color> <color=red>LoadTexture() Error:</color> Texture not supported: {fileName}{Environment.NewLine}");
            }
        }
    }
}
