using System.Collections.Generic;
using D_F_Enum;
using UnityEngine;

public interface IBundleLoaderOwner 
{
    public List<string> BundleLabels { get; }
    void OnBundleSizeAction(long size);
    void OnBundleDownLoadAction(long size);
    void OnLoadMemoryAction(string label, int loadCount, int totalCount);
    void OnBundlerEnterError(E_BUNDLE_DOWNLOAD_ERROR errocde);
    void OnLoadToMemoryComplete();

    public long TotalDownLoadBundleSize { get; set; }

    public E_BUNDLE_DOWNLOAD_STATE AgreeBundleDownLoad { get; }
}
