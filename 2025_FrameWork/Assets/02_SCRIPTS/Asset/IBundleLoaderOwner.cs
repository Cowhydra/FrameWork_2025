using System.Collections.Generic;
using UnityEngine;

public interface IBundleLoaderOwner 
{
    public List<string> BundleLabels { get; }
    void OnBundleSizeAction(long size);
    void OnBundleDownLoadAction(long size);
    void OnLoadMemoryAction(string label, int loadCount, int totalCount);
    void OnBundlerEnterError(int errocde);
    void OnLoadToMemoryComplete();
}
